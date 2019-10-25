using Microsoft.AspNetCore.Mvc;
using Sweets.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Sweets.Controllers
{
  public class TreatController : Controller
  {
    private readonly SweetsContext _db;

    public TreatController(SweetsContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Treat> model = _db.Treats.ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Create(Treat treat)
    {
      _db.Treats.Add(treat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    public ActionResult Details(int id)
    {
      List<FlavourTreat> flavourTreats = new List<FlavourTreat>();
      flavourTreats = _db.FlavourTreat
        .Include(flavourTreat => flavourTreat.Flavour)
        .Where(flavourTreat => flavourTreat.TreatId == id)
        .ToList();

      List<Flavour> flavours = new List<Flavour>();
      foreach(FlavourTreat flavourTreat in flavourTreats)
      {
        flavours.Add(flavourTreat.Flavour);
      }
      ViewBag.Treats = flavours;
     Treat thistreat = _db.Treats
          .FirstOrDefault(a => a.TreatId == id);
      return View(thistreat);
    }

  }
}