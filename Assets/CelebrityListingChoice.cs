using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CelebrityListingChoice : MonoBehaviour {

    public CelebrityManager celebrityManager;
    public Transform prefab;
    public Transform parent;
    public UnityEvent onSelected;
    List<Transform> spawnedButtons = new List<Transform>();

	public void SpawnButtons()
    {
        for (int x = spawnedButtons.Count - 1; x >= 0; x--)
        {
            Destroy(spawnedButtons[x].gameObject);
        }

        spawnedButtons.Clear();

        int n = 0;
        Transform temp;
        foreach (CelebrityManager.celebrityOption c in celebrityManager.celebrityList)
        {
            if (c.included)
            {
                if (c.variants != null && c.variants.Count > 0)
                {
                    for (int i = 0; i < c.variants.Count; i++)
                    {
                        if (c.variants[i].included)
                        {
                            temp = Instantiate(prefab, parent);
                            temp.GetComponentInChildren<ButtonGeneric>().SetButton(new ButtonProperty(this.transform, "SelectCeleb", n.ToString() + "_" + i, c.variants[i].thumbnail));
                            temp.GetComponentInChildren<Text>().text = "";
                            spawnedButtons.Add(temp);
                        }
                    }
                }
                else
                {
                    temp = Instantiate(prefab, parent);
                    temp.GetComponentInChildren<ButtonGeneric>().SetButton(new ButtonProperty(this.transform, "SelectCeleb", n.ToString() + "_-1", c.thumbnail));
                    temp.GetComponentInChildren<Text>().text = "";
                    spawnedButtons.Add(temp);
                }
            }
            n++;
        }
    }

    public void SelectCeleb(string withID)
    {
        int n, n2;
        string[] str = withID.Split('_');

        if (int.TryParse(str[0], out n) && int.TryParse(str[1], out n2))
        {
            celebrityManager.selectedCelebrity = n;
            celebrityManager.selectedVariant = n2;
            onSelected.Invoke();
        }
    }
}
