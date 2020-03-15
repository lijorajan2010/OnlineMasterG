using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace OnlineMasterG.CommonFramework
{
    public static class EmailManager
    {

        public static void AppSettings(out string FromAddress, out string Password, out string SMTPPort, out string Host)
        {
            FromAddress = ConfigurationManager.AppSettings.Get("FromAddress");
            Password = ConfigurationManager.AppSettings.Get("Password");
            SMTPPort = ConfigurationManager.AppSettings.Get("SMTPPort");
            Host = ConfigurationManager.AppSettings.Get("Host");
        }
        public static void SendEmail(string ToemailAddress, string body, string subject, string FromAddress, string Password, string SMTPPort, string Host)
        {
            using (MailMessage mm = new MailMessage(FromAddress, ToemailAddress))
            {
                mm.Subject = subject;
                mm.Body = body;

                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = Host;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Port = Convert.ToInt32(SMTPPort);
                NetworkCredential NetworkCred = new NetworkCredential(FromAddress, Password);
                smtp.Credentials = NetworkCred;
              
                smtp.Send(mm);

            }
        }
    }
}