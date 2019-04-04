using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerInfoInput : MonoBehaviour {

    public string LinkToSetting = "https://myaccount.google.com/u/1/lesssecureapps?pli=1&pageId=none";
    public InputField usernameEnterField;
    public InputField passwordEnterField;
    public Transform confirmationText;
    public Transform errorMessageTransform;
    public Transform confirmationButton;
    public EmailSender emailManager;
    public InputField subjectEnterField;
    public InputField bodyEnterField;

    public void TestInformation()
    {
        errorMessageTransform.gameObject.SetActive(true);
        emailManager.TestCredentials(usernameEnterField.text, passwordEnterField.text);
        errorMessageTransform.gameObject.SetActive(false);
        confirmationText.gameObject.SetActive(true);
        confirmationButton.gameObject.SetActive(true);
    }

    public void ConfirmInformation()
    {
        //emailManager.managerEmail = usernameEnterField.text;
        //emailManager.managerPassword = passwordEnterField.text;
        emailManager.SetCredentials(usernameEnterField.text, passwordEnterField.text);
    }

    public void ConfirmEmailText()
    {
        emailManager.subjectText = subjectEnterField.text;
        emailManager.bodyText = bodyEnterField.text;
    }

    public void OpenLink()
    {
        Application.OpenURL(LinkToSetting);
    }
}
