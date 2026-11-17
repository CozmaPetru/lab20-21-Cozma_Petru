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

            // 1. Verificare extensie (case-insensitive prin ToLower)
            string extensie = Path.GetExtension(fisier.FileName).ToLower();
            string extensieExtra = Personalizare.ExtensieSuplimentara.ToLower();

            bool extensieValida = false;
            if (extensie == ".jpg" || extensie == ".png" || extensie == extensieExtra)
            {
                extensieValida = true;
            }

            if (!extensieValida)
            {
                ModelState.AddModelError("fisier", "Extensia " + extensie + " nu este permisă.");
                return View(lucrare);
            }

            // 2. Verificare dimensiune (Length este în octeți)
            long limitaOcteti = (long)Personalizare.LimitaMb * 1024 * 1024;
            if (fisier.Length > limitaOcteti)
            {
                ModelState.AddModelError("fisier", "Fișierul depășește limita de " + Personalizare.LimitaMb + " MB.");
                return View(lucrare);
            }

            // --- Salvarea efectivă pe disc (doar dacă a trecut de verificări) ---
            string numeNou = Guid.NewGuid().ToString() + extensie;
            string folder = Path.Combine(mediu.WebRootPath, "incarcari");
            Directory.CreateDirectory(folder);

            string cale = Path.Combine(folder, numeNou);
            using (FileStream flux = new FileStream(cale, FileMode.Create))
            {
                fisier.CopyTo(flux);
            }

            lucrare.Imagine = numeNou;
            db.SaveChanges();

            TempData["Mesaj"] = "Fișierul a fost salvat: " + numeNou;
            return RedirectToAction("Detalii", new { id = id });
        }
    }
}
