using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.Events;
public enum destinationType { persistentDataPath, applicationDataPath };

public class ScreenshotManager : MonoBehaviour {


    public destinationType savingLocation;
    public string extension = ".png";
    public string folder = "Screenshots";
    public Transform UserInterface;
    public CelebrityManager celebrityManager;
    public float cooldown;
    public UnityEvent onPhotoSuccesful;

    [Header("Email Manager")]
    public EmailSender emailManager;

	
    public void TakeScreenshot()
    {
        StartCoroutine(TakeScreenshotCoroutine());
    }

    IEnumerator TakeScreenshotCoroutine()
    {
        string destination; 

        switch (savingLocation)
        {
            case destinationType.persistentDataPath:
                DebugScript.instance.Log("A");
                destination = Path.Combine(Application.persistentDataPath, folder);

                if (!Directory.Exists(destination))
                    Directory.CreateDirectory(destination);
                DebugScript.instance.Log("B");

                //destination = Path.Combine(destination, (celebrityManager.celebrityList[celebrityManager.selectedCelebrity].name + "(" + DateTime.Now.ToString(@"MMddyyyy hmmtt") + ")" + extension));
                destination = destination + "/" + (celebrityManager.celebrityList[celebrityManager.selectedCelebrity].name + "(" + DateTime.Now.ToString(@"MMddyyyy hmmtt") + ")" + extension);
                DebugScript.instance.Log("C");
                break;
            case destinationType.applicationDataPath:
                destination = Path.Combine(Application.dataPath, folder);

                if (!Directory.Exists(destination))
                    Directory.CreateDirectory(destination);

                destination = Path.Combine(destination, (celebrityManager.celebrityList[celebrityManager.selectedCelebrity].name + "(" + DateTime.Now.ToString(@"MMddyyyy hmmtt") + ")" + extension));
                break;
            default:
                destination = Application.temporaryCachePath + "/" + celebrityManager.celebrityList[celebrityManager.selectedCelebrity].name + "(" + DateTime.Now.ToString(@"MMddyyyy hmmtt") + ")" + extension;
                break;
        }
        DebugScript.instance.Log("D");

        UserInterface.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();

        //yield return new WaitForSeconds(cooldown);
        
#if UNITY_EDITOR
        ScreenCapture.CaptureScreenshot(destination);
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
        AddPhotoToList(destination);
        Debug.Log(destination);
        DebugScript.instance.Log("F");
        yield return new WaitForSeconds(cooldown);
        UserInterface.gameObject.SetActive(true);

        DebugScript.instance.Log("G" + (File.Exists(destination)).ToString());
        emailManager.SendEmail();

        DebugScript.instance.Log("H");
        onPhotoSuccesful.Invoke();
    }

    public void AddPhotoToList(string withPath)
    {
        emailManager.AddAttachment(withPath);
    }
}
