using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Loader.Helper
{
    public class SendEmail
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public static string Host { get; set; }

        public static int Port { get; set; }
        public static string FromEmail { get; set; }

        public static string FromPassword { get; set; }

        public static void SendEmailNormal(string To, string subject, string body)
        {
            Loader.Service.EmailService emailService = new Service.EmailService();
            Host = emailService.Host ;
            Port = emailService.Port;
            FromEmail = emailService.FromEmail;
            FromPassword = emailService.FromPassword;

            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.EnableSsl = false;
            client.Host =Host;
            client.Port = Port;


            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(FromEmail, FromPassword);
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;

            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            msg.From = new MailAddress(FromEmail);
            msg.To.Add(new MailAddress(To));

            msg.Subject = subject;
            msg.IsBodyHtml = true;
            msg.Body = body;

            client.Send(msg);
        }
        public static void SendEmailWithHostandPort(string To, string subject, string body, string Host, int Port)
        {
            Loader.Service.EmailService emailService = new Service.EmailService();
            Host = emailService.Host;
            Port = emailService.Port;
            FromEmail = emailService.FromEmail;
            FromPassword = emailService.FromPassword;

            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Host = Host;
            client.Port = Port;


            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(FromEmail, FromPassword);
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;

            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            msg.From = new MailAddress("bishaltesting16@gmail.com");
            msg.To.Add(new MailAddress(To));

            msg.Subject = subject;
            msg.IsBodyHtml = true;
            msg.Body = body;

            client.Send(msg);
        }
        public static void SendFromDynamicEmail(string FromUserEmail, string FromUserPassword, string To, string subject, string body, string Host, int Port)
        {
            Loader.Service.EmailService emailService = new Service.EmailService();
            Host = emailService.Host;
            Port = emailService.Port;
            FromEmail = emailService.FromEmail;
            FromPassword = emailService.FromPassword;
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Host = Host;
            client.Port = Port;


            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(FromUserEmail, FromUserPassword);
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;

            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            msg.From = new MailAddress(FromUserEmail);
            msg.To.Add(new MailAddress(To));

            msg.Subject = subject;
            msg.IsBodyHtml = true;
            msg.Body = body;

            client.Send(msg);
        }

    }

}