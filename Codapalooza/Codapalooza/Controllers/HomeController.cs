using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Codapalooza.Models;
using Codapalooza.Models.Security;

namespace Codapalooza.Controllers
{
	public class HomeController : Controller
	{
		private CodapaloozaEntities _db;
		public IMembershipService MembershipService { get; set; }

		protected override void Initialize(RequestContext requestContext)
		{
			if (MembershipService == null) { MembershipService = new AccountMembershipService(); }
			if (_db == null) _db = new CodapaloozaEntities();
			base.Initialize(requestContext);
		}

		public ActionResult Index()
		{
			ViewBag.Message = "Welcome to ASP.NET MVC!";

			return View();
		}

		public ActionResult About()
		{
			return View();
		}

		public ActionResult MustPay(Guid? id)
		{
			var participant = _db.Participants.Single(p => p.UserName == User.Identity.Name);
			return PartialView("_MustPayButton", participant);
		}
	}
}
