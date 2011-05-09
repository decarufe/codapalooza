using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.ApplicationServices;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Codapalooza.Models;
using Codapalooza.Models.Security;

namespace Codapalooza.Controllers
{
  public class AccountController : Controller
  {
    private CodapaloozaEntities _db;

    public IFormsAuthenticationService FormsService { get; set; }
    public IMembershipService MembershipService { get; set; }

    protected override void Initialize(RequestContext requestContext)
    {
      if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
      if (MembershipService == null) { MembershipService = new AccountMembershipService(); }
      if (_db == null) _db = new CodapaloozaEntities();

      base.Initialize(requestContext);
    }

    // **************************************
    // URL: /Account/LogOn
    // **************************************

    public ActionResult LogOn()
    {
      return View();
    }

    [HttpPost]
    public ActionResult LogOn(LogOnModel model, string returnUrl)
    {
      if (ModelState.IsValid)
      {
        if (MembershipService.ValidateUser(model.UserName, model.Password))
        {
          FormsService.SignIn(model.UserName, model.RememberMe);
          if (Url.IsLocalUrl(returnUrl))
          {
            return Redirect(returnUrl);
          }
          else
          {
            return RedirectToAction("Index", "Home");
          }
        }
        else
        {
          ModelState.AddModelError("", "The user name or password provided is incorrect.");
        }
      }

      // If we got this far, something failed, redisplay form
      return View(model);
    }

    // **************************************
    // URL: /Account/LogOff
    // **************************************

    public ActionResult LogOff()
    {
      FormsService.SignOut();

      return RedirectToAction("Index", "Home");
    }

    // **************************************
    // URL: /Account/Register
    // **************************************

    public ActionResult Register()
    {
      ViewBag.PasswordLength = MembershipService.MinPasswordLength;
      return View();
    }

    [HttpPost]
    public ActionResult Register(RegisterModel model)
    {
      if (ModelState.IsValid)
      {
        // Attempt to register the user
        MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserName, model.Password, model.Email);

        if (createStatus == MembershipCreateStatus.Success)
        {
          Roles.AddUserToRole(model.UserName, "Participant");
          FormsService.SignIn(model.UserName, false /* createPersistentCookie */);
          var participant = new Participant()
          {
            Id = Guid.NewGuid(),
            UserName = model.UserName,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email
          };
          _db.Participants.AddObject(participant);
          _db.SaveChanges();

          SendConfirmationEmail(model.Email, participant.Id);

          return RedirectToAction("WaitingForConfirmation", "Account");
        }
        else
        {
          ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
        }
      }

      // If we got this far, something failed, redisplay form
      ViewBag.PasswordLength = MembershipService.MinPasswordLength;
      return View(model);
    }

    private void SendConfirmationEmail(string userEmail, Guid userId)
    {
      MailMessage mail = new MailMessage("postmaster@codapalooza.net", userEmail)
      {
        Subject = "Coonfirmation d'inscription au Codapalooza",
        Body = "Pour confirmer votre inscription vous devez cliquez sur le lien suivant\n\n" +
               Url.Content("~/Account/Confirm/") + userId + "\n\n"
      };
      //send the message 
#if DEBUG
      // vlquhxvm
      SmtpClient smtp = new SmtpClient("relais.videotron.ca");
      //NetworkCredential credentials = new NetworkCredential("postmaster@decarufel.net", "amix0214");
#else
      SmtpClient smtp = new SmtpClient("mail.codapalooza.net");
      NetworkCredential credentials = new NetworkCredential("postmaster@decarufel.net", "amix0214");
      smtp.Credentials = credentials;
#endif
      smtp.Send(mail);
    }

    // **************************************
    // URL: /Account/ChangePassword
    // **************************************

    [Authorize]
    public ActionResult ChangePassword()
    {
      ViewBag.PasswordLength = MembershipService.MinPasswordLength;
      return View();
    }

    [Authorize]
    [HttpPost]
    public ActionResult ChangePassword(ChangePasswordModel model)
    {
      if (ModelState.IsValid)
      {
        if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
        {
          return RedirectToAction("ChangePasswordSuccess");
        }
        else
        {
          ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
        }
      }

      // If we got this far, something failed, redisplay form
      ViewBag.PasswordLength = MembershipService.MinPasswordLength;
      return View(model);
    }

    // **************************************
    // URL: /Account/ChangePasswordSuccess
    // **************************************

    public ActionResult ChangePasswordSuccess()
    {
      return View();
    }

    public ActionResult WaitingForConfirmation()
    {
      var participant = _db.Participants.Single(p => p.UserName == User.Identity.Name);

      return View(participant);
    }

    public ActionResult Confirm(Guid? id)
    {
      try
      {
        var participant = _db.Participants.Single(p => p.Id == id);

        participant.Confirmed = true;
        _db.SaveChanges();

        return View(participant);
      }
      catch (Exception e)
      {
        ModelState.AddModelError("", "Une erreur est surevenu lors de la validation\nContactez l'administrateur");
        return View();
      }      
    }
  }
}
