using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDisplayController : MonoBehaviour {

    public Camera ARCAmera;
    public int hideUIFromDisplayAfter = 2;

	// Use this for initialization
	void Awake () {
        UpdateCameraCount();
	}
	
	// Update is called once per frame
	public void UpdateCameraCount () {
        Debug.Log(Display.displays.Length);
        if (Display.displays.Length > hideUIFromDisplayAfter)
        {
            ARCAmera.targetDisplay = hideUIFromDisplayAfter;
            for (int n = hideUIFromDisplayAfter; n < Display.displays.Length; n++)
            {
                Display.displays[n].Activate(Display.displays[n].renderingWidth, Display.displays[n].renderingHeight, 60);
                
            }
        }
        else
            ARCAmera.targetDisplay = 0;
	}
}
