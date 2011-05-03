using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
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
      {
        _db.Dispose();
      }
    }

    //
    // GET: /Project/
    [Authorize(Roles = "Participant")]
    public ActionResult Index()
    {
      return View(_db.Projects.ToArray());
    }

    //
    // GET: /Project/Details/5

    public ActionResult Details(Guid id)
    {
      var project = _db.Projects.Single(p => p.Id == id);
      return View(project);
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

        var participant = _db.Participants.Single(p => p.UserName == User.Identity.Name);
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

    public ActionResult Delete(Guid id)
    {
      var project = _db.Projects.Single(p => p.Id == id);
      return View(project);
    }

    //
    // POST: /Project/Delete/5

    [HttpPost]
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
  }
}