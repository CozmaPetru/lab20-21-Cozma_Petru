using System.ComponentModel.DataAnnotations;

namespace CaminLab.Models
{
    public class Adresa
    {
        [Display(Name = "Orașul")]
        [Required(ErrorMessage = "Scrie orașul.")]
        public string Oras { get; set; }

        [Display(Name = "Strada")]
        public string Strada { get; set; }
    }

    public class Persoana
    {
        [Display(Name = "Numele")]
        [Required(ErrorMessage = "Scrie numele.")]
        public string Nume { get; set; }

        [Display(Name = "Telefonul")]
        public string Telefon { get; set; }

        public Adresa Adresa { get; set; }
    }

    // Ce trimite formularul „Cerere nouă”: două persoane, cu aceleași câmpuri.
    public class CerereForm
    {
        [Display(Name = "Camera")]
        public int CameraId { get; set; }

        public Persoana Student { get; set; }

        public Persoana Tutore { get; set; }
    }
}
