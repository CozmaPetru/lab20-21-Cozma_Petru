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
        [HttpPost]
        public IActionResult Noua(CerereForm form)
        {
            if (!ModelState.IsValid)
            {
                PregatesteCamerele();
                return View(form);
            }

            Cerere cerere = new Cerere();
            cerere.CameraId = form.CameraId;
            cerere.NumeStudent = form.Student.Nume;
            cerere.TelefonStudent = form.Student.Telefon;
            cerere.Oras = form.Student.Adresa.Oras;
            cerere.Strada = form.Student.Adresa.Strada;
            cerere.NumeTutore = form.Tutore.Nume;
            cerere.TelefonTutore = form.Tutore.Telefon;

            db.Cereri.Add(cerere);
            db.SaveChanges();

            TempData["Mesaj"] = "Cererea a fost salvată.";
            return RedirectToAction("Index");
        }

        // Sarcina 3: aici scrii acțiunea pentru butonul „Verifică doar tutorele”.
        [HttpPost]
        public IActionResult VerificaTutorele([Bind(Prefix = "Tutore")] Persoana tutore)
        {
            return View("Tutore", tutore);
        }
    }
}
