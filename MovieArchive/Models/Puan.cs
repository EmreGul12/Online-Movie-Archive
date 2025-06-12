namespace MovieArchive.Models
{   

//Hocaya sor: Film ve kullanıcı kullandığım için @ManyToOne gibi anatasyonlar kullanamadığım için bu şekilde
//yorum ve puan oluşturmak zorunda kaldım bu sorunu nasıl çözebilirim.
    public class Puan
    {
        public int Id { get; set; }
        public Film Film { get; set; }
        public Uye Uye { get; set; }
         public int Deger { get; set; }  // 1 ile 10 arası
    }
}
