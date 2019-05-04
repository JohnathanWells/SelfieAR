using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Runtime.Serialization.Formatters.Binary;

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
    public ButtonGeneric selectedLogo;
    string selectedLogoID;
    public float sizeChangeRate = 2;
    List<ButtonGeneric> spawnedLogos = new List<ButtonGeneric>();
    List<string> spawnedLogoNames = new List<string>();

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

        completeDirectory = Path.Combine(completeDirectory, folderWithLogos);

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
        {
            selectedLogo = spawnedLogos[n];
            selectedLogoID = withIndex;
        }
    }

    public void DestroySelectedLogo()
    {
        int minParse;
        if (selectedLogo && int.TryParse(selectedLogo.property.message, out minParse))
        {
            for (int n =minParse + 1; n < spawnedLogos.Count; n++)
            {
                spawnedLogos[n].property.message = (n - 1).ToString();
            }

            spawnedLogos.RemoveAt(minParse);
            spawnedLogoNames.RemoveAt(minParse);

            Destroy(selectedLogo.gameObject);

        }
    }

    public void RotateObject(int by)
    {
        selectedLogo.transform.Rotate(Vector3.forward, by);
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
            ButtonGeneric temp = Instantiate(logoPrefab, new Vector2(Screen.width / 2, Screen.height / 2), Quaternion.identity, logoParent).GetComponentInChildren<ButtonGeneric>();
            temp.SetButton(new ButtonProperty(this.transform, "SelectLogo", spawnedLogos.Count.ToString(), loadedLogos[n]));
            temp.transform.GetComponent<RectTransform>().sizeDelta = defaultDimensions;

            //currentLogoSetup.spriteNames.Add(loadedLogos[n].name);
            spawnedLogoNames.Add(loadedLogos[n].name);

            spawnedLogos.Add(temp);

            selectedLogo = temp;

            thumbnailList.gameObject.SetActive(false);
        }
    }

    public bool SpawnLogoByName(string withID)
    {
        int n;
        Debug.Log(withID);
        if ((n = loadedLogos.FindIndex(x=>x.name == withID)) >= 0)
        {
            Debug.Log("Spawening " + withID);
            ButtonGeneric temp = Instantiate(logoPrefab, new Vector2(Screen.width / 2, Screen.height / 2), Quaternion.identity, logoParent).GetComponentInChildren<ButtonGeneric>();
            temp.SetButton(new ButtonProperty(this.transform, "SelectLogo", spawnedLogos.Count.ToString(), loadedLogos[n]));
            temp.transform.GetComponent<RectTransform>().sizeDelta = defaultDimensions;

            //currentLogoSetup.spriteNames.Add(loadedLogos[n].name);
            spawnedLogoNames.Add(loadedLogos[n].name);

            spawnedLogos.Add(temp);

            selectedLogo = temp;

            thumbnailList.gameObject.SetActive(false);

            return true;
        }

        return false;
    }

    public IEnumerator LoadLogosFromFolder()
    {
        string[] Files = Directory.GetFiles(@completeDirectory, "*.png");

        loadedLogos.Clear();

        foreach (string file in Files)
        {
            Texture2D tex;
            tex = new Texture2D(4, 4, TextureFormat.DXT5, false);
            //WWW www = new WWW("file:" + file);
            //yield return www;
            byte[] fileData = File.ReadAllBytes(file);
            yield return null;
            tex.LoadImage(fileData);

            //www.LoadImageIntoTexture(tex);
            //GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
            Sprite tempS = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
            tempS.name = Path.GetFileName(file);
            loadedLogos.Add(tempS);
            DebugScript.instance.Log("D");

        }

        LoadLogoPlacement();
    }

    public void Finish()
    {
        onFinish.Invoke();
    }

    public void SaveLogoPlacement()
    {
        LogoConfiguration saveFile = new LogoConfiguration();

        for (int n = 0; n < Mathf.Min(spawnedLogos.Count, spawnedLogoNames.Count); n++)
        {
            saveFile.spriteNames.Add(spawnedLogoNames[n]);
            Vector3 temp = spawnedLogos[n].transform.position;
            saveFile.positions.Add(new float[3]{ temp.x, temp.y, temp.z});
            temp = spawnedLogos[n].transform.eulerAngles;
            saveFile.rotations.Add(new float[3] { temp.x, temp.y, temp.z });
            temp = spawnedLogos[n].GetComponent<RectTransform>().sizeDelta;
            saveFile.scales.Add(new float[3] { temp.x, temp.y, temp.z});
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(completeDirectory + "/savedLogoConfiguration.gd");
        bf.Serialize(file, saveFile);
        file.Close();
    }

    public void LoadLogoPlacement()
    {
        if (File.Exists(completeDirectory + "/savedLogoConfiguration.gd"))
        {
            LogoConfiguration saveFile = new LogoConfiguration();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(completeDirectory + "/savedLogoConfiguration.gd", FileMode.Open);
            saveFile = (LogoConfiguration)bf.Deserialize(file);
            file.Close();

            for (int n = spawnedLogos.Count - 1; n >= 0; n--)
            {
                Destroy(spawnedLogos[n].gameObject);
            }

            spawnedLogos.Clear();

            for (int n = 0; n < Mathf.Min(saveFile.positions.Count, saveFile.spriteNames.Count); n++)
            {
                if (SpawnLogoByName(saveFile.spriteNames[n]))
                {
                    float[] temp = saveFile.positions[n];
                    selectedLogo.transform.position = new Vector3(temp[0], temp[1], temp[2]);
                    temp = saveFile.rotations[n];
                    selectedLogo.transform.eulerAngles = new Vector3(temp[0], temp[1], temp[2]);
                    temp = saveFile.scales[n];
                    selectedLogo.GetComponent<RectTransform>().sizeDelta = new Vector3(temp[0], temp[1], temp[2]);
                }
            }
        }
    }

    public void SizeUp()
    {
        if (selectedLogo)
        {
            selectedLogo.gameObject.BroadcastMessage("ModifySize", sizeChangeRate);
        }
    }

    public void SizeDown()
    {
        if (selectedLogo)
        {
            selectedLogo.gameObject.BroadcastMessage("ModifySize", -sizeChangeRate);
        }
    }

    [System.Serializable]
    public class LogoConfiguration
    {
        public List<string> spriteNames;
        public List<float[]> positions;
        public List<float[]> rotations;
        public List<float[]> scales;

        public LogoConfiguration()
        {
            spriteNames = new List<string>();
            positions = new List<float[]>();
            rotations = new List<float[]>();
            scales = new List<float[]>();
        }
    }
}
