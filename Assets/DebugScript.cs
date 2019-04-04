using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugScript : MonoBehaviour {

    public static DebugScript instance;
    public bool enabled = true;
    public Text text;

    void Awake()
    {
        instance = this;

        if (!enabled)
        {
            text.text = "";
        }
    }

    public void Log(string str)
    {
        if (enabled)
        {
            text.text = str;
        }
    }
}
