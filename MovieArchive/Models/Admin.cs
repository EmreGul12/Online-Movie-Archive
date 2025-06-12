namespace MovieArchive.Models
{
    public class Admin : Kullanici
    {
        // Admine özel ek özellik aklıma gelmdei belki eklerim.
        public List<Film> EklenenFilmler { get; set; } 
    }
}
