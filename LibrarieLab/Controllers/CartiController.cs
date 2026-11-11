using LibrarieLab.Data;
using LibrarieLab.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibrarieLab.Controllers
{
    public class CartiController : Controller
    {
        private readonly LibrarieContext _db;
        public CartiController(LibrarieContext db)
        {
            _db = db;
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
        public IActionResult Create(Carte carte)
        {
            if (!ModelState.IsValid)
            {
                return View(carte);
            }

            _db.Carti.Add(carte);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
