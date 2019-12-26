using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Loader.Controllers
{
    [MyAuthorize]
    public class SendEmailController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        // GET: SendEmail
        [HttpPost]
        public ActionResult Index(Helper.SendEmail objModelMail, HttpPostedFileBase fileUploader)
        {
            if (ModelState.IsValid)
            {
                string from = Helper.SendEmail.FromEmail;
                using (MailMessage mail = new MailMessage(from, objModelMail.To))
                {
                    mail.Subject = objModelMail.Subject;
                    mail.Body = objModelMail.Body;
                    if (fileUploader != null)
                    {
                        string fileName = Path.GetFileName(fileUploader.FileName);
                        mail.Attachments.Add(new Attachment(fileUploader.InputStream, fileName));
                    }
                    mail.IsBodyHtml = false;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential networkCredential = new NetworkCredential(from, Helper.SendEmail.FromPassword);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = networkCredential;
                    smtp.Port = Helper.SendEmail.Port ;
                    smtp.Send(mail);
                    ViewBag.Message = "Sent";
                    return View("Index", objModelMail);
                }
            }
            else
            {
                return View();
            }
        }
    }
}