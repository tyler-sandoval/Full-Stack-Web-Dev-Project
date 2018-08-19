using FSDP.UI.MVC.Models;
using System;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace FSDP.UI.MVC.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactViewModel contact)
        {
            if (ModelState.IsValid)
            {
                contact.DateSent = DateTime.Now;
                contact.Subject = "FROM WEBSITE: " + contact.Subject;

                string MessageContent = $"Name: {contact.Name}<br/>Email: {contact.Email}<br/>Subject: {contact.Subject}<br/><h4>Message</h4> {contact.Message}<br/>Date Sent: {contact.DateSent}";

                MailMessage m = new MailMessage("no-reply@tylersandoval.com", "tyler.sandoval@outlook.com", contact.Subject, MessageContent);

                m.ReplyToList.Add(contact.Email);

                SmtpClient client = new SmtpClient("mail.tylersandoval.com");
                client.Credentials = new NetworkCredential("no-reply@tylersandoval.com", "Vivian0221!");

                using (client)
                {
                    try
                    {
                        client.Send(m);
                    }
                    catch
                    {
                        ViewBag.ErrorMessage = "There was an error sending your message. Please Try Again!";

                        return View(contact);
                    }
                }

                return View("ContactConfirm", contact);
            }
            return View(contact);
        }
    }
}
