namespace MovieArchive.Models
{
    public class Uye : Kullanici
    {
        public List<Yorum> Yorumlar { get; set; } 

        public List<Puan> Puanlar { get; set; } 
    }
}
