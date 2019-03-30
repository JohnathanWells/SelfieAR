using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Events;

public class LogoManager : MonoBehaviour {

    public destinationType folderLocation;
    public string folderWithLogos = "Logos";
    string completeDirectory;

    [Header("Instruction Screen")]
    public Text directoryDisplay;

    [Header("List")]
    public Transform thumbnailPrefab;
    public Transform thumbnailParent;
    public Transform thumbnailList;
    public List<Sprite> loadedLogos = new List<Sprite>();
    List<Transform> thumbnails = new List<Transform>();

    [Header("Manipulation")]
    public Vector2 defaultDimensions;
    public Transform logoParent;
    public Transform logoPrefab;
    public Transform selectedLogo;
    List<Transform> spawnedLogos = new List<Transform>();

    public UnityEvent onFinish;


	// Use this for initialization
	void Start () {
        PrepareFolder();
	}

    public void PrepareFolder()
    {
        completeDirectory = "";

        if (folderLocation == destinationType.applicationDataPath)
            completeDirectory = Application.dataPath;
        else
            completeDirectory = Application.persistentDataPath;

        completeDirectory += "/" + folderWithLogos;

        if (!Directory.Exists(completeDirectory))
            Directory.CreateDirectory(completeDirectory);

        directoryDisplay.text = completeDirectory;
    }

    public void LoadSprites()
    {
        StartCoroutine(LoadLogosFromFolder());
    }

    public void SelectLogo(string withIndex)
    {
        int n;
        if (int.TryParse(withIndex, out n))
            selectedLogo = spawnedLogos[n];
    }

    public void DestroySelectedLogo()
    {
        if (selectedLogo)
        {
            Destroy(selectedLogo.gameObject);
        }
    }

    public void RotateObject(int by)
    {
        selectedLogo.Rotate(Vector3.forward, by);
    }

    public void SpawnThumbnails()
    {
        for (int x = thumbnails.Count - 1; x >= 0; x--)
        {
            Destroy(thumbnails[x].gameObject);
        }

        thumbnails.Clear();

        int n = 0;
        Transform temp;
        foreach (Sprite s in loadedLogos)
        {
            temp = Instantiate(thumbnailPrefab, thumbnailParent);
            temp.GetComponentInChildren<ButtonGeneric>().SetButton(new ButtonProperty(this.transform, "SpawnLogo", n.ToString(), s));
            temp.GetComponentInChildren<Text>().text = "";
            thumbnails.Add(temp);
            n++;
        }
    }

    public void SpawnLogo(string withID)
    {
        int n;

        if (int.TryParse(withID, out n))
        {
            Transform temp = Instantiate(logoPrefab, new Vector2(Screen.width / 2, Screen.height / 2), Quaternion.identity, logoParent);
            temp.GetComponentInChildren<ButtonGeneric>().SetButton(new ButtonProperty(this.transform, "SelectLogo", spawnedLogos.Count.ToString(), loadedLogos[n]));
            temp.GetComponent<RectTransform>().sizeDelta = defaultDimensions;

            spawnedLogos.Add(temp);

            selectedLogo = temp;

            thumbnailList.gameObject.SetActive(false);
        }
    }

    public IEnumerator LoadLogosFromFolder()
    {
        string[] Files = Directory.GetFiles(@completeDirectory, "*.png");

        foreach (string file in Files)
        {
            Texture2D tex;
            tex = new Texture2D(4, 4, TextureFormat.DXT5, false);
            WWW www = new WWW("file:" + file);
            yield return www;
            www.LoadImageIntoTexture(tex);
            //GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
            loadedLogos.Add(Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero));

        }
    }

    public void Finish()
    {
        onFinish.Invoke();
    }
}
