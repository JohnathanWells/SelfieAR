using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class EmailSender : MonoBehaviour
{

    public string managerEmail;
    public string managerPassword;
    public List<string> targetEmails = new List<string>();

    public string subjectText;
    public string bodyText;
    public List<string> photoPaths = new List<string>();

    public string server = "smtp.gmail.com";

    public string subjectConfirmation = "Your email works with SelfieAR!";
    public string bodyConfirmation = "If you are receiving this email, you just logged in successfully using SelfieAR! Pictures taken throughough the event will be sent to your costumers from this email.\nDISCLAIMER: SelfieAR does NOT save your email or that of your costumers in any way.";



    public void SendEmail()
    {
        MailMessage mail = new MailMessage();

        mail.From = new MailAddress(managerEmail);

        foreach (string s in targetEmails)
        {
            mail.To.Add(s);
        }
        
        mail.Subject = subjectText;
        mail.Body = bodyText;

        foreach (string s in photoPaths)
        {
            mail.Attachments.Add(new Attachment(s));
        }

        SmtpClient smtpServer = new SmtpClient(server);
        smtpServer.Port = 587;
        smtpServer.Credentials = new System.Net.NetworkCredential(managerEmail, managerPassword) as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };
        smtpServer.Send(mail);
        Debug.Log("success");

    }

    public void SetCredentials(string username, string password)
    {
        managerEmail = username;
        managerPassword = password;
    }

    public void TestCredentials(string username, string password)
    {
        MailMessage mail = new MailMessage();

        mail.From = new MailAddress(managerEmail);

        mail.To.Add(username);

        mail.Subject = subjectConfirmation;
        mail.Body = bodyConfirmation;

        SmtpClient smtpServer = new SmtpClient(server);
        smtpServer.Port = 587;
        smtpServer.Credentials = new System.Net.NetworkCredential(username, password) as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        Debug.Log(mail.To[0] + " " + mail.Body);
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };
        smtpServer.Send(mail);
    }

    public void AddEmail(string str)
    {
        targetEmails.Add(str);
    }

    public void AddAttachment(string path)
    {
        photoPaths.Add(path);
    }

    public void ClearEmails()
    {
        targetEmails.Clear();
    }

    public void ClearAttachments()
    {
        photoPaths.Clear();
    }

}