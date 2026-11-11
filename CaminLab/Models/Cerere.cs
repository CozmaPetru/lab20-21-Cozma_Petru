using System.ComponentModel.DataAnnotations;

namespace CaminLab.Models
{
    // Rândul din tabelul Cereri.
    public class Cerere
    {
        public int Id { get; set; }

        public int CameraId { get; set; }

        [StringLength(60)]
        public string NumeStudent { get; set; }

        [StringLength(20)]
        public string TelefonStudent { get; set; }

        [StringLength(40)]
        public string Oras { get; set; }

        [StringLength(60)]
        public string Strada { get; set; }

        [StringLength(60)]
        public string NumeTutore { get; set; }

        [StringLength(20)]
        public string TelefonTutore { get; set; }
    }
}
