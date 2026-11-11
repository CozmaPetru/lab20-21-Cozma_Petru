using System.ComponentModel.DataAnnotations;

namespace DepanareLab.Models
{
    public class PretNou
    {
        public int Id { get; set; }

        public string Denumire { get; set; }

        [Range(1, 1000000, ErrorMessage = "Prețul trebuie să fie pozitiv.")]
        public decimal Pret { get; set; }
    }
}
