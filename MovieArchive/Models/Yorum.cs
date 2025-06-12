using System.ComponentModel.DataAnnotations;

namespace MovieArchive.Models
{
    public class Yorum
    {
        public int Id { get; set; }
        public Film Film { get; set; }
        public Uye Uye { get; set; }
        [Required(ErrorMessage = "Yorum başlığı zorunludur.")]
        [StringLength(200)]
        public string Baslik { get; set; } = string.Empty;
        [Required(ErrorMessage = "Yorum içeriği zorunludur.")]
        [StringLength(1000)]
        public string Icerik { get; set; } = string.Empty;
        public DateTime Tarih { get; set; } = DateTime.Now;
    }
}
