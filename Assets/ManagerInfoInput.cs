using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerInfoInput : MonoBehaviour {

    public string LinkToSetting = "https://myaccount.google.com/u/1/lesssecureapps?pli=1&pageId=none";
    public InputField usernameEnterField;
    public InputField passwordEnterField;
    public Transform confirmationText;
    public Transform confirmationButton;
    public EmailSender emailManager;

    public void TestInformation()
    {
        emailManager.TestCredentials(usernameEnterField.text, passwordEnterField.text);
        confirmationText.gameObject.SetActive(true);
        confirmationButton.gameObject.SetActive(true);
    }

    public void ConfirmInformation()
    {
        //emailManager.managerEmail = usernameEnterField.text;
        //emailManager.managerPassword = passwordEnterField.text;
        emailManager.SetCredentials(usernameEnterField.text, passwordEnterField.text);
    }

    public void OpenLink()
    {
        Application.OpenURL(LinkToSetting);
    }
}
