using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleScreenshotTaker : MonoBehaviour {

    public KeyCode screenshotKey;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(screenshotKey))
        {
            ScreenCapture.CaptureScreenshot(Application.dataPath + "/Screenshots/" + Time.time + ".png");
        }
	}
}
