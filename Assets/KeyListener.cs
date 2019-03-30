using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyListener : MonoBehaviour {

    public bool status = false;
    public KeyCode key;
    public UnityEvent onPress;

    public void Enable()
    {
        status = true;
    }

    public void Disable()
    {
        status = false;
    }

	void Update () {
        if (status && Input.GetKeyDown(key))
        {
            onPress.Invoke();
        }
		
	}
}
