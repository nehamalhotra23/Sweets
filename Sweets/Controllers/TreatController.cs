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
        public ActionResult Create(Treat treat, string Flavour1, string Flavour2, string Flavour3, string Flavour4)
        {
           
            _db.Treats.Add(treat);
            _db.SaveChanges();
            
            List<string> flavours = new List<string>();
            flavours.Add(Flavour1);
            flavours.Add(Flavour2);
            flavours.Add(Flavour3);
            flavours.Add(Flavour4);
           
            foreach(string flavour in flavours)
            {
                if (flavour != null)
                {
                   Flavour flavourObject;
                    int flavourId;
                    if(_db.Flavours.Contains(new Flavour() { FlavourName = flavour}))
                    {
                        flavourObject = _db.Flavours.FirstOrDefault(Flavour => Flavour.FlavourName == flavour);
                        flavourId = flavourObject.FlavourId;
                    } 
                    else 
                    {
                        var newflavour = new Flavour();
                        _db.Flavours.Add(new Flavour() { FlavourName = flavour}); // add treat to database
                        _db.SaveChanges();
                        flavourObject = _db.Flavours.FirstOrDefault(Flavour => Flavour.FlavourName == flavour);
                        flavourId = flavourObject.FlavourId;
                    }
                    _db.FlavourTreat.Add(new FlavourTreat() {FlavourId = flavourId, TreatId = treat.TreatId});                    
                }
            }
            
            

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
      ViewBag.Flavours = flavours;
     Treat thistreat = _db.Treats
     .Include(t => t.Flavours)
          .FirstOrDefault(t => t.TreatId == id);
      return View(thistreat);
    }

  }
}