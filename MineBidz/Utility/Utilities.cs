using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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


        public static string GetResponseString(string url, string parameters, string contentType, string method)
        {

            var client = new HttpClient();


            byte[] b;
            Stream s;
            StreamReader sr;
            HttpWebRequest req;
            HttpWebResponse res;
            //ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidationCB;

            req = (HttpWebRequest)HttpWebRequest.Create(url);

            //encode the form data into a byte array
            b = System.Text.Encoding.ASCII.GetBytes(parameters);

            //indicate that you will be posting the data 
            req.Method = "post";
            req.ContentType = contentType;
            //req.KeepAlive = true;

            //CookieContainer cookies = new CookieContainer();
            //if (cookieCollection != null)
            //{
            //    cookies.Add(cookieCollection);
            //}
            //req.CookieContainer = cookies;
            //req.KeepAlive = true;
            //req.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; MRA 4.10 (build 01952); GTB6.4; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729; MS-RTC LM 8)";

            //send the form 
            req.ContentLength = b.Length;
            s = req.GetRequestStream();
            s.Write(b, 0, b.Length);
            s.Close();

            res = (HttpWebResponse)req.GetResponse();

            //read in the page 
            sr = new System.IO.StreamReader(res.GetResponseStream());
            var response = sr.ReadToEnd();
            sr.Close();
            res.Close();

            return response;
        }

        public static string RecaptchaVerificationString(string clientResponse)
        {
             return GetResponseString("https://www.google.com/recaptcha/api/siteverify", "secret=6LfV_0YUAAAAAF0yLvgY1WC1Ioku68YQv8v5vaQk&response=" + clientResponse, "application/x-www-form-urlencoded", "post");
        }


    }


}