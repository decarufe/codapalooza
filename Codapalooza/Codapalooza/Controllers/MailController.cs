using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Codapalooza.Controllers
{
  public class MailController : Controller
  {
    //
    // GET: /Mail/

    public ActionResult Index()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Index(string dummy)
    {
      MailMessage mail = new MailMessage("postmaster@codapalooza.net", "eric.decarufel@gmail.com")
      {
        Subject = "This is an email",
        Body = "This is from system.net.mail using C sharp with smtp authentication."
      };
      //send the message 
      SmtpClient smtp = new SmtpClient("mail.codapalooza.net");

      NetworkCredential credentials = new NetworkCredential("postmaster@decarufel.net", "amix0214");
      smtp.Credentials = credentials;
      smtp.Send(mail);

      return View();
    }
  }
}