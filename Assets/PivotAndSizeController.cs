using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.IO;

public class PivotAndSizeController : MonoBehaviour {

    public CelebrityManager celebrityManager;
    public string QRCodeFilename;
    public string QRCodeFolderName;
    public Slider sizeController;
    public Slider positionController;
    public UnityEvent onFinished;


    public void ReadSliderAndUpdate()
    {
        celebrityManager.SetMovementFloat(positionController.normalizedValue);
        celebrityManager.SetSizeFloat(sizeController.normalizedValue);
    }

    public void Finish()
    {
        onFinished.Invoke();
    }

    public void OpenImage()
    {
        string str = Path.Combine(Application.dataPath, QRCodeFolderName);

        if (!Directory.Exists(str))
        {
            Directory.CreateDirectory(str);
        }
        Debug.Log(str);

        str = Path.Combine(str, QRCodeFilename);

        if (File.Exists(str))
        {
            Application.OpenURL("file:///" + str);
            return;
        }
        Debug.Log(str);
    }
}
