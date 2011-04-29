using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Codapalooza.Models;

namespace Codapalooza.Controllers
{
  public class ParticipantController : Controller
  {
    private CodapaloozaEntities _db;

    public ParticipantController()
    {
      _db = new Models.CodapaloozaEntities();
    }

    //
    // GET: /Participant/

    public ActionResult Index()
    {
      return View(_db.Participants);
    }

    //
    // GET: /Participant/Details/5

    public ActionResult Details(Guid id)
    {
      var participant = _db.Participants.Single(p => p.Id == id);
      return View(participant);
    }

    //
    // GET: /Participant/Create

    public ActionResult Create()
    {
      return View();
    }

    //
    // POST: /Participant/Create

    [HttpPost]
    public ActionResult Create(Participant participant)
    {
      try
      {
        if (participant.Id == Guid.Empty)
          participant.Id = Guid.NewGuid(); 
        _db.AddToParticipants(participant);
        _db.SaveChanges();

        return RedirectToAction("Index");
      }
      catch
      {
        return View(participant);
      }
    }

    //
    // GET: /Participant/Edit/5

    public ActionResult Edit(Guid id)
    {
      var participant = _db.Participants.Single(p => p.Id == id);
      return View(participant);
    }

    //
    // POST: /Participant/Edit/5

    [HttpPost]
    public ActionResult Edit(Guid id, Participant source)
    {
      try
      {
        var target = _db.Participants.Single(p => p.Id == id);
        UpdateModel(target);
        _db.SaveChanges();

        return RedirectToAction("Index");
      }
      catch
      {
        return View(source);
      }
    }

    //
    // GET: /Participant/Delete/5

    public ActionResult Delete(Guid id)
    {
      var participant = _db.Participants.Single(p => p.Id == id);
      return View(participant);
    }

    //
    // POST: /Participant/Delete/5

    [HttpPost]
    public ActionResult Delete(Guid id, Participant participant)
    {
      try
      {
        var target = _db.Participants.Single(p => p.Id == id);
        _db.DeleteObject(target);
        _db.SaveChanges();

        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }
  }
}