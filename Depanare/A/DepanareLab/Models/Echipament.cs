using System.ComponentModel.DataAnnotations;

namespace DepanareLab.Models
{
    public class Echipament
    {
        public int Id { get; set; }

        [Display(Name = "Denumirea")]
        [Required(ErrorMessage = "Scrie denumirea.")]
        [StringLength(60)]
        public string Denumire { get; set; }

        [Display(Name = "Seria")]
        [StringLength(30)]
        public string Serie { get; set; }

        [Display(Name = "Prețul, lei")]
        [Range(1, 1000000, ErrorMessage = "Prețul trebuie să fie pozitiv.")]
        public decimal Pret { get; set; }

        [Display(Name = "Anul")]
        public int An { get; set; }

        [StringLength(60)]
        public string Imagine { get; set; }
    }
}
