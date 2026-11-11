using System.ComponentModel.DataAnnotations;

namespace ExpozitieLab.Models
{
    public class Lucrare
    {
        public int Id { get; set; }

        [Display(Name = "Titlul")]
        [Required(ErrorMessage = "Scrie titlul.")]
        [StringLength(60)]
        public string Titlu { get; set; }

        [Display(Name = "Autorul")]
        [Required(ErrorMessage = "Scrie autorul.")]
        [StringLength(60)]
        public string Autor { get; set; }

        [Display(Name = "Grupa")]
        [StringLength(20)]
        public string Grupa { get; set; }

        [Display(Name = "Anul")]
        public int An { get; set; }

        // Sarcina 2: aici adaugi proprietatea pentru numele fișierului.
    }
}
