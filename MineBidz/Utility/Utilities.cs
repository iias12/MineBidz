using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

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
#if DEBUG
            //client.Send(mail);
#else
            client.Send(mail);
#endif
        }

        public static bool SaveDocument(HttpPostedFileBase file, string path)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    file.SaveAs(path);
                    return true;
                }
                return false;
            }
            catch (Exception fileExc)
            {
                return false;
            }
        }

    }
}