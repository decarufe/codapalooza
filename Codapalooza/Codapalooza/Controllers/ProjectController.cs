using System;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using Codapalooza.Models;

namespace Codapalooza.Controllers
{
	public class ProjectController : Controller
	{
		private readonly CodapaloozaEntities _db;

		public ProjectController()
		{
			_db = new CodapaloozaEntities();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
				_db.Dispose();
		}

		public bool CanEdit { get; set; }

		//
		// GET: /Project/
		[Authorize(Roles = "Participant")]
		public ActionResult Index()
		{
			var projects = _db.Projects.OrderByDescending(p => p.Votes.Count).ToArray();
			foreach (var project in projects)
			{
				Validate(project);
			}
			return View(projects);
		}

		//
		// GET: /Project/Details/5

		public ActionResult Details(Guid id)
		{
			var project = _db.Projects.Single(p => p.Id == id);
			Validate(project);
			return View(project);
		}

		private void Validate(Project project)
		{
			project.CanEdit = User.Identity.Name.Equals(project.Proposer.UserName) ||
							  User.IsInRole("Admin");
		}

		//
		// GET: /Project/Create
		[Authorize(Roles = "Participant")]
		public ActionResult Create()
		{
			return View();
		}

		//
		// POST: /Project/Create

		[HttpPost]
		[Authorize(Roles = "Participant")]
		public ActionResult Create(Project project)
		{
			try
			{
				if (project.Id == Guid.Empty)
					project.Id = Guid.NewGuid();

				var participant =
				  _db.Participants.Single(
					p => p.UserName.Equals(User.Identity.Name, StringComparison.InvariantCultureIgnoreCase));
				project.ProposerId = participant.Id;
				_db.AddToProjects(project);
				_db.SaveChanges();

				return RedirectToAction("Index");
			}
			catch
			{
				return View(project);
			}
		}

		//
		// GET: /Project/Edit/5
		[Authorize(Roles = "Participant")]
		public ActionResult Edit(Guid id)
		{
			var project = _db.Projects.Single(p => p.Id == id);
			return View(project);
		}

		//
		// POST: /Project/Edit/5

		[HttpPost]
		[Authorize(Roles = "Participant")]
		public ActionResult Edit(Guid id, Project project)
		{
			try
			{
				var projectEntity = _db.Projects.Single(p => p.Id == id);
				UpdateModel(projectEntity);
				_db.SaveChanges();

				return RedirectToAction("Index");
			}
			catch
			{
				return View(project);
			}
		}

		//
		// GET: /Project/Delete/5
		[Authorize(Roles = "Participant")]
		public ActionResult Delete(Guid id)
		{
			var project = _db.Projects.Single(p => p.Id == id);
			Validate(project);
			return View(project);
		}

		//
		// POST: /Project/Delete/5

		[HttpPost]
		[Authorize(Roles = "Participant")]
		public ActionResult Delete(Guid id, Project project)
		{
			try
			{
				var projectEntity = _db.Projects.Single(p => p.Id == id);
				_db.DeleteObject(projectEntity);
				_db.SaveChanges();

				return RedirectToAction("Index");
			}
			catch
			{
				return View(project);
			}
		}

		[Authorize(Roles = "Participant")]
		public ActionResult Vote(Guid id)
		{
			try
			{
				var projectEntity = _db.Projects.Single(p => p.Id == id);
				var participantEntity = _db.Participants.Single(p => p.UserName == User.Identity.Name);

				var previousVotes = from v in _db.Votes
									where v.ParticipantId == participantEntity.Id
									select v;

				using (var transactionScope = new TransactionScope())
				{
					foreach (var previousVote in previousVotes)
					{
						_db.Votes.DeleteObject(previousVote);
					}

					Vote vote = new Vote()
					{
						Id = Guid.NewGuid(),
						ProjectId = projectEntity.Id,
						ParticipantId = participantEntity.Id
					};
					_db.Votes.AddObject(vote);
					_db.SaveChanges();

					transactionScope.Complete();
				}

				return RedirectToAction("Index");
			}
			catch
			{
				return RedirectToAction("Index");
			}
		}
	}
}