using Microsoft.AspNetCore.Mvc;
using ExpozitieLab.Data;
using ExpozitieLab.Models;

namespace ExpozitieLab.Controllers
{
    public class LucrariController : Controller
    {
        private ExpozitieDbContext db;
        private IWebHostEnvironment mediu;

        public LucrariController(ExpozitieDbContext db, IWebHostEnvironment mediu)
        {
            this.db = db;
            this.mediu = mediu;
        }

        public IActionResult Index()
        {
            List<Lucrare> lucrari = db.Lucrari.ToList();
            return View(lucrari);
        }

        public IActionResult Detalii(int id)
        {
            Lucrare lucrare = db.Lucrari.Find(id);
            if (lucrare == null)
            {
                return NotFound();
            }
            return View(lucrare);
        }

        [HttpGet]
        public IActionResult Incarca(int id)
        {
            Lucrare lucrare = db.Lucrari.Find(id);
            if (lucrare == null)
            {
                return NotFound();
            }
            return View(lucrare);
        }

        // Sarcina 1: aici scrii acțiunea Incarca pentru POST.
    }
}
