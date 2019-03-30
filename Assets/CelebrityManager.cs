using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CelebrityManager : MonoBehaviour {

    [System.Serializable]
    public class celebrityOption
    {
        public string name;
        public string description;
        public Sprite thumbnail;
        public List<celebrityOption> variants;
        public Transform prefab;
    }

    public celebrityOption[] celebrityList;
    public int selectedCelebrity;
    public int selectedVariant;
    public Transform MarkerOffsetObject;
    public Transform spawnpointPrefab;
    public bool dontRefreshWhenTargetIsLost = false;
    Transform currentCelebrity;
    ScaleKeeper currentCelebrityScaler;
    float scalerSizeFloat = 0;
    float scalerPositionFloat = 1;
    Transform celebritySpawnpoint;
    public bool ARenabled = true;
    bool setDetected = false;
    public UnityEvent onSpawn;

    public void EnableAR()
    {
        ARenabled = true;
        SpawnSelectedCelebrity();
    }

    public void DisableAR()
    {
        ARenabled = false;
        if (currentCelebrity)
        {
            Destroy(currentCelebrity.gameObject);
        }
    }

    public void SetDetected()
    {
        setDetected = true;
    }

    public void SetUndetected()
    {
        setDetected = false;
    }

    public void SpawnSelectedCelebrity()
    {
        if (ARenabled && setDetected)
        {
            if (dontRefreshWhenTargetIsLost)
            {
                if (!celebritySpawnpoint)
                {
                    celebritySpawnpoint = Instantiate(spawnpointPrefab, MarkerOffsetObject.position, Quaternion.identity);
                    celebritySpawnpoint.parent = null;
                }

                if (currentCelebrity)
                {
                    Destroy(currentCelebrity.gameObject);
                }
                Debug.Log("Calling spawn at " + celebritySpawnpoint.position);

                if (selectedVariant < 0)
                    currentCelebrity = Instantiate(celebrityList[selectedCelebrity].prefab, celebritySpawnpoint);
                else
                    currentCelebrity = Instantiate(celebrityList[selectedCelebrity].variants[selectedVariant].prefab, celebritySpawnpoint);

                currentCelebrityScaler = currentCelebrity.GetComponentInChildren<ScaleKeeper>();
            }
            else if (!currentCelebrity)
            {
                if (selectedVariant < 0)
                    currentCelebrity = Instantiate(celebrityList[selectedCelebrity].prefab, MarkerOffsetObject.position, Quaternion.identity, MarkerOffsetObject);
                else
                    currentCelebrity = Instantiate(celebrityList[selectedCelebrity].variants[selectedVariant].prefab, MarkerOffsetObject.position, Quaternion.identity, MarkerOffsetObject);

                currentCelebrityScaler = currentCelebrity.GetComponentInChildren<ScaleKeeper>();
                SetMovementFloat(scalerPositionFloat);
                SetSizeFloat(scalerSizeFloat);

                onSpawn.Invoke();
            }
        }
    }

    public void DeleteSelectedCelebrity()
    {
        Destroy(currentCelebrity.gameObject);
    }

    public void SelectCelebrityAndVariant(string indexSeparatedByUnderscore)
    {
        int a, b;
        string[] split = indexSeparatedByUnderscore.Split('_');

        if (split.Length > 1 && int.TryParse(split[0], out a) && int.TryParse(split[1], out b))
        {
            selectedCelebrity = a;
            selectedVariant = b;
        }
    }

    public void SetMovementFloat(float to)
    {
        scalerPositionFloat = to;
        if (currentCelebrityScaler)
        {
            currentCelebrityScaler.MoveVideoToPoint(to);
        }
    }
    public void SetSizeFloat(float to)
    {
        scalerSizeFloat = to;
        if (currentCelebrityScaler)
        {
            currentCelebrityScaler.ScaleVideo(to);
        }
    }

    public Transform GetCurrentlySelectedCelebrity()
    {
        return currentCelebrity;
    }
}
