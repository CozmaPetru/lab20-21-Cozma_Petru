using Microsoft.AspNetCore.Mvc;
using DepanareLab.Data;
using DepanareLab.Models;

namespace DepanareLab.Controllers
{
    public class EchipamenteController : Controller
    {
        private InventarDbContext db;
        private IWebHostEnvironment mediu;

        public EchipamenteController(InventarDbContext db, IWebHostEnvironment mediu)
        {
            this.db = db;
            this.mediu = mediu;
        }

        public IActionResult Index()
        {
            List<Echipament> lista = db.Echipamente.ToList();
            decimal total = 0;
            foreach (Echipament echipament in lista)
            {
                total = total + echipament.Pret;
            }
            ViewBag.Total = total;
            return View(lista);
        }

        // GET /Echipamente/Cauta?an=2022&pretMaxim=5000
        public IActionResult Cauta(int? an, decimal pretMaxim)
        {
            List<Echipament> gasite = new List<Echipament>();
            foreach (Echipament echipament in db.Echipamente)
            {
                bool potrivit = true;
                if (an != null && echipament.An != an)
                {
                    potrivit = false;
                }
                if (pretMaxim != null && echipament.Pret > pretMaxim)
                {
                    potrivit = false;
                }
                if (potrivit)
                {
                    gasite.Add(echipament);
                }
            }
            return View(gasite);
        }

        public IActionResult Detalii(int id)
        {
            Echipament echipament = db.Echipamente.Find(id);
            if (echipament == null)
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult Adauga()
        {
            Echipament echipament = new Echipament();
            echipament.An = DateTime.Now.Year;
            return View(echipament);
        }

        [HttpPost]
        public IActionResult Adauga(Echipament echipament)
        {
            if (!ModelState.IsValid)
            {
                return View(echipament);
            }
            db.Echipamente.Add(echipament);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Editeaza(int id)
        {
            Echipament echipament = db.Echipamente.Find(id);
            return View(echipament);
        }

        [HttpPost]
        public IActionResult Editeaza(Echipament echipament)
        {
            if (!ModelState.IsValid)
            {
                return View(echipament);
            }
            db.Echipamente.Update(echipament);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Sterge(int id)
        {
            Echipament echipament = db.Echipamente.Find(id);
            if (echipament != null)
            {
                db.Echipamente.Remove(echipament);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Preturi()
        {
            List<PretNou> randuri = new List<PretNou>();
            foreach (Echipament echipament in db.Echipamente)
            {
                PretNou rand = new PretNou();
                rand.Id = echipament.Id;
                rand.Denumire = echipament.Denumire;
                rand.Pret = echipament.Pret;
                randuri.Add(rand);
            }
            return View(randuri);
        }

        [HttpPost]
        public IActionResult Preturi(List<PretNou> preturi)
        {
            if (!ModelState.IsValid)
            {
                return View(preturi);
            }
            foreach (PretNou rand in preturi)
            {
                Echipament echipament = db.Echipamente.Find(rand.Id);
                if (echipament != null)
                {
                    echipament.Pret = rand.Pret;
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Imagine(int id)
        {
            Echipament echipament = db.Echipamente.Find(id);
            if (echipament == null)
            {
                return NotFound();
            }
            return View(echipament);
        }

        [HttpPost]
        public IActionResult Imagine(int id, IFormFile fisier)
        {
            Echipament echipament = db.Echipamente.Find(id);
            if (echipament == null)
            {
                return NotFound();
            }
            if (fisier == null || fisier.Length == 0)
            {
                ModelState.AddModelError("fisier", "Alege un fișier.");
                return View(echipament);
            }
            string numeNou = Guid.NewGuid().ToString() + Path.GetExtension(fisier.FileName).ToLower();
            string folder = Path.Combine(mediu.WebRootPath, "incarcari");
            Directory.CreateDirectory(folder);
            using (FileStream flux = new FileStream(Path.Combine(folder, numeNou), FileMode.Create))
            {
                fisier.CopyTo(flux);
            }
            echipament.Imagine = numeNou;
            db.SaveChanges();
            return RedirectToAction("Detalii", new { id = id });
        }
    }
}
