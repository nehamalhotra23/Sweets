using System;
using Microsoft.AspNetCore.Mvc;
using Sweets.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Sweets.Controllers
{
    [Authorize]
    public class FlavourController : Controller
    {
        private readonly SweetsContext _db;
        private readonly UserManager<ApplicationUser> _userManager; 


        public FlavourController(UserManager<ApplicationUser> userManager, SweetsContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public async Task<ActionResult> Index()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            var userFlavours = _db.Flavours.Where(entry => entry.User.Id == currentUser.Id);
            return View(userFlavours);
        }

        public ActionResult Create()
        {
           
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(Flavour flavour, string Treat1, string Treat2, string Treat3, string Treat4)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            flavour.User = currentUser;
            _db.Flavours.Add(flavour);
            _db.SaveChanges();
            
            List<string> treats = new List<string>();
            treats.Add(Treat1);
            treats.Add(Treat2);
            treats.Add(Treat3);
            treats.Add(Treat4);
           
            foreach(string treat in treats)
            {
                if (treat != null)
                {
                   Treat treatObject;
                    int treatId;
                    if(_db.Treats.Contains(new Treat() { TreatName = treat}))
                    {
                        treatObject = _db.Treats.FirstOrDefault(Treat => Treat.TreatName == treat);
                        treatId = treatObject.TreatId;
                    } 
                    else 
                    {
                        var newTreat = new Treat();
                        _db.Treats.Add(new Treat() { TreatName = treat}); // add treat to database
                        _db.SaveChanges();
                        treatObject = _db.Treats.FirstOrDefault(Treat => Treat.TreatName == treat);
                        treatId = treatObject.TreatId;
                    }
                    _db.FlavourTreat.Add(new FlavourTreat() {TreatId = treatId, FlavourId = flavour.FlavourId});                    
                }
            }
            

            _db.SaveChanges();
            return RedirectToAction("Index");
        }
         public ActionResult Details(int id)
        {
            List<FlavourTreat> flavourTreats = new List<FlavourTreat>();
            flavourTreats = _db.FlavourTreat
                .Include(flavourTreat => flavourTreat.Treat)
                .Where(flavourTreat => flavourTreat.FlavourId == id)
                .ToList();
            List<Treat> treats = new List<Treat>();

            foreach (FlavourTreat flavourTreat in flavourTreats)
            {
                treats.Add(flavourTreat.Treat);
            }
            ViewBag.Treats = treats;
            Flavour thisFlavour = _db.Flavours
                .Include(f => f.Treats)
                .FirstOrDefault(f => f.FlavourId == id);
            return View(thisFlavour);
        }
    }
}