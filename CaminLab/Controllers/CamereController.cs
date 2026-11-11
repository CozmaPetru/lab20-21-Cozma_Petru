using Microsoft.AspNetCore.Mvc;
using CaminLab.Data;
using CaminLab.Models;

namespace CaminLab.Controllers
{
    public class CamereController : Controller
    {
        private CaminDbContext db;

        public CamereController(CaminDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            List<Camera> camere = db.Camere.ToList();
            return View(camere);
        }

        [HttpGet]
        public IActionResult Editeaza(int id)
        {
            Camera camera = db.Camere.Find(id);
            if (camera == null)
            {
                return NotFound();
            }
            return View(camera);
        }

        [HttpPost]
        public IActionResult Editeaza(Camera camera)
        {
            if (!ModelState.IsValid)
            {
                return View(camera);
            }
            db.Camere.Update(camera);
            db.SaveChanges();
            TempData["Mesaj"] = "Camera " + camera.Numar + " a fost modificată.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Sterge(int id)
        {
            Camera camera = db.Camere.Find(id);
            if (camera != null)
            {
                db.Camere.Remove(camera);
                db.SaveChanges();
                TempData["Mesaj"] = "Camera " + camera.Numar + " a fost ștearsă.";
            }
            return RedirectToAction("Index");
        }

        // Sarcina 1: aici scrii acțiunea Cauta.
        // GET /Camere/Cauta?etaj=2&tarifMaxim=600&doarLibere=true
        public IActionResult Cauta(int? etaj, decimal? tarifMaxim, bool doarLibere)
        {
            List<Camera> gasite = new List<Camera>();
            foreach (Camera camera in db.Camere)
            {
                bool potrivit = true;
                if (etaj != null && camera.Etaj != etaj)
                {
                    potrivit = false;
                }
                if (tarifMaxim != null && camera.Tarif > tarifMaxim)
                {
                    potrivit = false;
                }
                if (doarLibere && !camera.Libera)
                {
                    potrivit = false;
                }
                if (potrivit)
                {
                    gasite.Add(camera);
                }
            }
            ViewBag.Etaj = etaj;
            ViewBag.TarifMaxim = tarifMaxim;
            ViewBag.DoarLibere = doarLibere;
            return View(gasite);
        }

        // GET /Camere/Tarife → câte un rând editabil pentru fiecare cameră
        [HttpGet]
        public IActionResult Tarife()
        {
            List<TarifNou> randuri = new List<TarifNou>();
            foreach (Camera camera in db.Camere)
            {
                TarifNou rand = new TarifNou();
                rand.Id = camera.Id;
                rand.Numar = camera.Numar;
                rand.Tarif = camera.Tarif;
                randuri.Add(rand);
            }
            return View(randuri);
        }

        // Sarcina 4: aici scrii acțiunea care primește tabelul de tarife.

        // Pentru sarcina suplimentară E1
        public IActionResult Alege()
        {
            List<Camera> camere = db.Camere.ToList();
            return View(camere);
        }



        // Pentru sarcina suplimentară E3: parametrul bloc vine din cookie, după ce înregistrezi furnizorul.
        public IActionResult Bloc(string bloc)
        {
            ViewBag.Bloc = bloc;
            return View();
        }

        [HttpPost]
        public IActionResult AlegeBloc(string litera)
        {
            Response.Cookies.Append("bloc", litera);
            return RedirectToAction("Bloc");
        }
    }
}
