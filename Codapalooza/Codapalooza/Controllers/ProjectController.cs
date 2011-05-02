using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Codapalooza.Models;

namespace Codapalooza.Controllers
{
  public class ProjectController : Controller
  {
    //
    // GET: /Project/

    public ActionResult Index()
    {
      using (var codapaloozaEntities = new CodapaloozaEntities())
      {
        return View(codapaloozaEntities.Projects.ToArray());
      }
    }

    //
    // GET: /Project/Details/5

    public ActionResult Details(Guid id)
    {
      using (var codapaloozaEntities = new CodapaloozaEntities())
      {
        var project = codapaloozaEntities.Projects.Single(p => p.Id == id);
        return View(project);
      }
    }

    //
    // GET: /Project/Create

    public ActionResult Create()
    {
      return View();
    }

    //
    // POST: /Project/Create

    [HttpPost]
    public ActionResult Create(Project project)
    {
      try
      {
        if (project.Id == Guid.Empty)
          project.Id = Guid.NewGuid();

        using (var codapaloozaEntities = new CodapaloozaEntities())
        {
        codapaloozaEntities.AddToProjects(project);
        codapaloozaEntities.SaveChanges();
        }

        return RedirectToAction("Index");
      }
      catch
      {
        return View(project);
      }
    }

    //
    // GET: /Project/Edit/5

    public ActionResult Edit(Guid id)
    {
      using (var codapaloozaEntities = new CodapaloozaEntities())
      {
        var project = codapaloozaEntities.Projects.Single(p => p.Id == id);
        return View(project);
      }
    }

    //
    // POST: /Project/Edit/5

    [HttpPost]
    public ActionResult Edit(Guid id, Project project)
    {
      try
      {
        using (var codapaloozaEntities = new CodapaloozaEntities())
        {
          var projectEntity = codapaloozaEntities.Projects.Single(p => p.Id == id);
          UpdateModel(projectEntity);
          codapaloozaEntities.SaveChanges();
        }

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
      using (var codapaloozaEntities = new CodapaloozaEntities())
      {
        var project = codapaloozaEntities.Projects.Single(p => p.Id == id);
        return View(project);
      }
    }

    //
    // POST: /Project/Delete/5

    [HttpPost]
    public ActionResult Delete(Guid id, Project project)
    {
      try
      {
        using (var codapaloozaEntities = new CodapaloozaEntities())
        {
          var projectEntity = codapaloozaEntities.Projects.Single(p => p.Id == id);
          codapaloozaEntities.DeleteObject(projectEntity);
          codapaloozaEntities.SaveChanges();
        }

        return RedirectToAction("Index");
      }
      catch
      {
        return View(project);
      }
    }
  }
}