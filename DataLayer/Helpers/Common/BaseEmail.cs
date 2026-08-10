using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer.Base
{
    public class BaseEmail
    {
        static string SiteEmailAddress = "info@kalagram.com";
        static string SiteEmailPassword = "Abtin123@";
        static string SiteEmailHost = "mail.kalagram.com";

        private static string GetEmailTemplate(string Title, string Body)
        {
            string email = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/App_Data/Kalagram/Email.html"));
            return email.
                Replace("${TITLE}", Title).
                Replace("${BODY}", Body);
        }

        public static void SendSimpleEmail(string Email, string Title, string Body)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                NetworkCredential basicCredential = new NetworkCredential(SiteEmailAddress, SiteEmailPassword);
                MailMessage message = new MailMessage();
                MailAddress fromAddress = new MailAddress(SiteEmailAddress);
                smtpClient.Host = SiteEmailHost;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = basicCredential;
                message.From = fromAddress;
                message.Subject = Title;
                message.IsBodyHtml = true;
                message.Body = GetEmailTemplate(Title, Body);
                message.To.Add(Email);
                smtpClient.Send(message);
            }
            catch (Exception) { }
        }
    }
}
