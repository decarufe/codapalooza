using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Codapalooza.Models;
using Codapalooza.Models.Security;

namespace Codapalooza.Controllers
{
	[Authorize(Roles = "Admin")]
	public class UserController : Controller
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


		//
		// GET: /User/
		public ActionResult Index()
		{
			var users = MembershipService.GetAllUsers();


			return View(users.ToList());
		}

		public ActionResult Delete(string userName)
		{
			var user = MembershipService.GetUserByName(userName);
			return View(user);
		}

		[HttpPost]
		public ActionResult Delete(string userName, string dummy)
		{
			MembershipUser user = null;
			try
			{
				//using (var tran = new TransactionScope())
				//{
				user = MembershipService.GetUserByName(userName);
				MembershipService.DeleteUser(userName);
				var participant = _db.Participants.Single(p => p.UserName == userName);
				foreach (var vote in participant.Votes.ToArray())
				{
					participant.Votes.Remove(vote);
				}

				foreach (var project in _db.Projects.Where(p => p.Proposer.UserName == participant.UserName))
				{
					foreach (var vote in project.Votes)
					{
						project.Votes.Remove(vote);
					}
					_db.Projects.DeleteObject(project);
				}
				_db.Participants.DeleteObject(participant);
				_db.SaveChanges();
				//  tran.Complete();
				//}
				return RedirectToAction("Index");
			}
			catch (Exception e)
			{
				ModelState.AddModelError("", e);

				return View(user);
			}
		}
	}
}