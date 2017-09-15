using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace MineBidz.Utility
{
    public class Utilities
    {
        public static void SendMail(string to, string subject, string body)
        {
            MailMessage mail = new MailMessage("formhandler@iias.biz", to, subject, body);
            mail.IsBodyHtml = true;
            SmtpClient client = new SmtpClient(" mail.iias.biz");
            // Add credentials if the SMTP server requires them.
            //client.Credentials = CredentialCache.DefaultNetworkCredentials;
            client.Credentials = new NetworkCredential("formhandler", "iiasform20981");
            client.Send(mail);
        }
    }
}