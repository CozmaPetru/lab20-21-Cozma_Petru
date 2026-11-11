using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CaminLab.Data;
using CaminLab.Models;

namespace CaminLab.Controllers
{
    public class CereriController : Controller
    {
        private CaminDbContext db;

        public CereriController(CaminDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            List<Cerere> cereri = db.Cereri.ToList();
            return View(cereri);
        }

        // Pune în ViewBag camerele pentru lista derulantă.
        private void PregatesteCamerele()
        {
            ViewBag.Camere = new SelectList(db.Camere.ToList(), "Id", "Numar");
        }

        [HttpGet]
        public IActionResult Noua()
        {
            PregatesteCamerele();
            return View(new CerereForm());
        }

        // Sarcina 2: aici scrii acțiunea Noua pentru POST.

        // Sarcina 3: aici scrii acțiunea pentru butonul „Verifică doar tutorele”.
    }
}
