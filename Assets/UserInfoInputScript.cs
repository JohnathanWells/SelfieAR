using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UserInfoInputScript : MonoBehaviour {

    public EmailSender emailManager;
    public InputField emailAdressField;
    public char separator = ',';
    public Text errorDisplay;
    public UnityEvent onSuccess;

    public void ReadEmailsIntoList()
    {
        if (!string.IsNullOrEmpty(emailAdressField.text))
        {
            DisplayError("");
            string[] addresses = emailAdressField.text.Split(separator);

            emailManager.ClearEmails();
            emailManager.ClearAttachments();

            foreach (string s in addresses)
            {
                emailManager.AddEmail(s);
            }

            onSuccess.Invoke();
        }
        else
        {
            DisplayError("Please enter your email in the box!");
        }
    }

    public void DisplayError(string str)
    {
        errorDisplay.text = str;
    }

    public void ClearField()
    {
        emailAdressField.text = "";
        DisplayError("");
    }
}
