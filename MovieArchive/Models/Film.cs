using System.ComponentModel.DataAnnotations;

namespace MovieArchive.Models
{
    public class Film
    {
        public int Id { get; set; }
        [Required, StringLength(200)]
        public string Ad { get; set; }
        [Required]
        public string Tur { get; set; }
        [Required]
        public int YayinTarihi { get; set; }
        [Required]
        public string Yoneten { get; set; }
        [Required,StringLength(1000)]
        public string Aciklama { get; set; }
        public double OrtalamaPuan { get; set; }
        public long ToplamPuanVerenKisi {  get; set; }
        public long ToplamPuan { get; set; }
        [Required]
        public List<Yorum> Yorumlar { get; set; } = new List<Yorum>();
        public List<Puan> Puanlar { get; set; }= new List<Puan>();

    }


}
