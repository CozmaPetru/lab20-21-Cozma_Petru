using System.ComponentModel.DataAnnotations;

namespace CaminLab.Models
{
    public class Camera
    {
        public int Id { get; set; }

        [Display(Name = "Camera")]
        [Required(ErrorMessage = "Scrie numărul camerei.")]
        [StringLength(10)]
        public string Numar { get; set; }

        [Display(Name = "Etajul")]
        [Range(1, 9, ErrorMessage = "Etajul este între 1 și 9.")]
        public int Etaj { get; set; }

        [Display(Name = "Locuri")]
        [Range(1, 6, ErrorMessage = "O cameră are între 1 și 6 locuri.")]
        public int Locuri { get; set; }

        [Display(Name = "Tariful, lei/lună")]
        [Range(1, 100000, ErrorMessage = "Tariful trebuie să fie pozitiv.")]
        public decimal Tarif { get; set; }

        [Display(Name = "Liberă")]
        public bool Libera { get; set; }
    }
}
