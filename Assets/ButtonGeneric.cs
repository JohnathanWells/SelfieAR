using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ButtonProperty
{
    public Transform targetTransform;
    public string function;
    public string message;
    public Sprite sprite;

    public ButtonProperty(Transform t, string f, string m, Sprite s)
    {
        targetTransform = t;
        function = f;
        message = m;
        sprite = s;
    }
}

public class ButtonGeneric : MonoBehaviour {

    public Button button;
    public ButtonProperty property;

    public void SetButton(ButtonProperty to)
    {
        property = to;
        button.image.sprite = to.sprite;
    }

    public void Execute()
    {
        if (property != null)
        {
            property.targetTransform.SendMessage(property.function, property.message);
        }
    }
}
