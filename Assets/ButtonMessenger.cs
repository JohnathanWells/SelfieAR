using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMessenger : MonoBehaviour {

    public Transform target;
    public string function;
    public string message;

    public void Set(Transform t, string f, string m)
    {
        target = t;
        function = f;
        message = m;
    }

    public void Execute()
    {
        target.SendMessage(function, message);
    }
}
