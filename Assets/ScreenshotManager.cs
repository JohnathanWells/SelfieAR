using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.Events;
using UnityEngine.UI;
public enum destinationType { persistentDataPath, applicationDataPath };

public class ScreenshotManager : MonoBehaviour {


    public destinationType savingLocation;
    public string extension = ".png";
    public string folder = "Screenshots";
    public Transform UserInterface;
    public CelebrityManager celebrityManager;
    public float cooldown;
    public UnityEvent onPhotoSuccesful;
    public UnityEvent onPhotoSent;
    public Image confirmationPhoto;
    string lastPhotoTaken;

    [Header("Email Manager")]
    public EmailSender emailManager;

	
    public void TakeScreenshot()
    {
        StartCoroutine(TakeScreenshotCoroutine());
    }

    IEnumerator TakeScreenshotCoroutine()
    {
        switch (savingLocation)
        {
            case destinationType.persistentDataPath:
                DebugScript.instance.Log("A");
                lastPhotoTaken = Path.Combine(Application.persistentDataPath, folder);

                if (!Directory.Exists(lastPhotoTaken))
                    Directory.CreateDirectory(lastPhotoTaken);
                DebugScript.instance.Log("B");

                //destination = Path.Combine(destination, (celebrityManager.celebrityList[celebrityManager.selectedCelebrity].name + "(" + DateTime.Now.ToString(@"MMddyyyy hmmtt") + ")" + extension));
                lastPhotoTaken = lastPhotoTaken + "/" + (celebrityManager.celebrityList[celebrityManager.selectedCelebrity].name + "(" + DateTime.Now.ToString(@"MMddyyyy hmmtt") + ")" + extension);
                DebugScript.instance.Log("C");
                break;
            case destinationType.applicationDataPath:
                lastPhotoTaken = Path.Combine(Application.dataPath, folder);

                if (!Directory.Exists(lastPhotoTaken))
                    Directory.CreateDirectory(lastPhotoTaken);

                lastPhotoTaken = Path.Combine(lastPhotoTaken, (celebrityManager.celebrityList[celebrityManager.selectedCelebrity].name + "(" + DateTime.Now.ToString(@"MMddyyyy hmmtt") + ")" + extension));
                break;
            default:
                lastPhotoTaken = Application.temporaryCachePath + "/" + celebrityManager.celebrityList[celebrityManager.selectedCelebrity].name + "(" + DateTime.Now.ToString(@"MMddyyyy hmmtt") + ")" + extension;
                break;
        }
        DebugScript.instance.Log("D");

        UserInterface.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();

        //yield return new WaitForSeconds(cooldown);
        
#if UNITY_EDITOR
        ScreenCapture.CaptureScreenshot(lastPhotoTaken);
#else
        Texture2D screencap = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
        screencap.ReadPixels(new Rect(0.0f, 0.0f, Screen.width, Screen.height), 0, 0, true);
        screencap.Apply();

        Debug.Log(Screen.width + " " + Screen.height);

        yield return null;

        byte[] screenshotEncodedinBytes = screencap.EncodeToPNG();
        File.WriteAllBytes(destination, screenshotEncodedinBytes);
#endif

        yield return null;
        DebugScript.instance.Log("E");
        //AddPhotoToList(lastPhotoTaken);
        Debug.Log(lastPhotoTaken);
        DebugScript.instance.Log("F");
        yield return new WaitForSeconds(cooldown);
        UserInterface.gameObject.SetActive(true);

        Debug.Log("G " + lastPhotoTaken + " " + (File.Exists(lastPhotoTaken)).ToString());
        //emailManager.SendEmail();

        //DebugScript.instance.Log("H");
        onPhotoSuccesful.Invoke();
    }

    public void SendPhoto()
    {
        AddPhotoToList(lastPhotoTaken);
        emailManager.SendEmail();
        onPhotoSent.Invoke();
        //onPhotoSuccesful.Invoke();
    }

    public void AddPhotoToList(string withPath)
    {
        emailManager.AddAttachment(withPath);
    }

    public void ShowPictureTaken()
    {
        StartCoroutine(LoadPictureFromFolder(lastPhotoTaken));
    }

    public IEnumerator LoadPictureFromFolder(string path)
    {
        Texture2D tex;
        tex = new Texture2D(4, 4, TextureFormat.DXT5, false);
        //WWW www = new WWW("file:" + file);
        //yield return www;
        byte[] fileData = File.ReadAllBytes(path);
        yield return null;
        tex.LoadImage(fileData);

        //www.LoadImageIntoTexture(tex);
        //GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        Sprite tempS = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        tempS.name = Path.GetFileName(path);

        confirmationPhoto.sprite = tempS;
    }
}
