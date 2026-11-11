using System.ComponentModel.DataAnnotations;

namespace CaminLab.Models
{
    // Un rând din tabelul de tarife. Conține doar ce se modifică pe această pagină.
    public class TarifNou
    {
        public int Id { get; set; }

        public string Numar { get; set; }

        [Range(1, 100000, ErrorMessage = "Tariful trebuie să fie pozitiv.")]
        public decimal Tarif { get; set; }
    }
}
