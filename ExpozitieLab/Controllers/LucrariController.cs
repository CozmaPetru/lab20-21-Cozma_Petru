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
        [HttpPost]
        public IActionResult Incarca(int id, IFormFile fisier)
        {
            Lucrare lucrare = db.Lucrari.Find(id);
            if (lucrare == null)
            {
                return NotFound();
            }

            if (fisier == null || fisier.Length == 0)
            {
                ModelState.AddModelError("fisier", "Alege un fișier.");
                return View(lucrare);
            }

            // Numele îl alege aplicația prin GUID pentru unicitate
            string extensie = Path.GetExtension(fisier.FileName).ToLower();
            string numeNou = Guid.NewGuid().ToString() + extensie;
            string folder = Path.Combine(mediu.WebRootPath, "incarcari");

            Directory.CreateDirectory(folder);

            string cale = Path.Combine(folder, numeNou);
            using (FileStream flux = new FileStream(cale, FileMode.Create))
            {
                fisier.CopyTo(flux);
            }

            TempData["Mesaj"] = "Fișierul a fost salvat: " + numeNou;
            return RedirectToAction("Detalii", new { id = id });
        }
    }
}
