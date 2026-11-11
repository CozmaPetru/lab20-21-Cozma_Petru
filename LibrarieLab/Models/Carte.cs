using System.ComponentModel.DataAnnotations;
namespace LibrarieLab.Models
{
    public class Carte
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Introduceți titlul")]
        [StringLength(150)]
        public string Titlu { get; set; }

        [Required(ErrorMessage = "Introduceți autorul")]
        [StringLength(100)]
        public string Autor { get; set; }

        [Display(Name = "Anul apariției")]
        [Required(ErrorMessage = "Introduceți anul")]
        [Range(1500, 2100, ErrorMessage = "An invalid")]
        public int AnAparitie { get; set; }

        [Display(Name = "Preț (lei)")]
        [Required(ErrorMessage = "Introduceți prețul")]
        [Range(0, 10000, ErrorMessage = "Preț invalid")]
        public decimal Pret { get; set; }

        public string Descriere { get; set; }

        // Numele fișierului din wwwroot/incarcari (îl generează aplicația)
        [Display(Name = "Copertă")]
        public string Coperta { get; set; }

    }
}
