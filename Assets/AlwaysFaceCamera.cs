using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysFaceCamera : MonoBehaviour {


    public Camera ARCamera;

	// Use this for initialization
	void Awake () {
		if (!ARCamera)
        {
            if (GameObject.FindGameObjectWithTag("ARCamera"))
                ARCamera = GameObject.FindGameObjectWithTag("ARCamera").GetComponent<Camera>();
            else
                ARCamera = Camera.main;
        }
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.LookAt(ARCamera.transform);	
	}
}
