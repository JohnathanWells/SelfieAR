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

                destination = Path.Combine(Application.persistentDataPath, folder);

                if (!Directory.Exists(destination))
                    Directory.CreateDirectory(destination);

                destination = Path.Combine(destination, (celebrityManager.celebrityList[celebrityManager.selectedCelebrity].name + "(" + DateTime.Now.ToString(@"MMddyyyy hmmtt") + ")" + extension));
                break;
            case destinationType.applicationDataPath:
                destination = Path.Combine(Application.dataPath, folder);

                if (!Directory.Exists(destination))
                    Directory.CreateDirectory(destination);

                destination = Path.Combine(destination, (celebrityManager.celebrityList[celebrityManager.selectedCelebrity].name + extension));
                break;
            default:
                destination = Application.temporaryCachePath + "/" + celebrityManager.celebrityList[celebrityManager.selectedCelebrity].name + "(" + DateTime.Now.ToString(@"MMddyyyy hmmtt") + ")" + extension;
                break;
        }

        UserInterface.gameObject.SetActive(false);

        //yield return new WaitForSeconds(cooldown);
        
        ScreenCapture.CaptureScreenshot(destination);
        AddPhotoToList(destination);
        Debug.Log(destination);
        yield return new WaitForSeconds(cooldown);
        UserInterface.gameObject.SetActive(true);

        emailManager.SendEmail();

        onPhotoSuccesful.Invoke();
    }

    public void AddPhotoToList(string withPath)
    {
        emailManager.AddAttachment(withPath);
    }
}
