using LibrarieLab.Data;
using LibrarieLab.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibrarieLab.Controllers
{
    public class CartiController : Controller
    {
        private readonly LibrarieContext _db;
        private readonly IWebHostEnvironment _env;

        public CartiController(LibrarieContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public IActionResult Index()
        {
            List<Carte> carti = _db.Carti.ToList();
            return View(carti);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Carte carte, IFormFile fisierCoperta)
        {
            if (!ModelState.IsValid)
            {
                return View(carte);
            }

            if (fisierCoperta != null && fisierCoperta.Length > 0)
            {
                string extensie = Path.GetExtension(fisierCoperta.FileName);
                string numeFisier = Guid.NewGuid().ToString() + extensie;
                string caleSalvare = Path.Combine(_env.WebRootPath, "incarcari", numeFisier);

                using (FileStream stream = new FileStream(caleSalvare, FileMode.Create))
                {
                    fisierCoperta.CopyTo(stream);
                }

                carte.Coperta = numeFisier;
            }

            _db.Carti.Add(carte);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}