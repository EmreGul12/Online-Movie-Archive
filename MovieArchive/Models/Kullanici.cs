using Microsoft.AspNetCore.Identity;

namespace MovieArchive.Models
{
    public class Kullanici : IdentityUser
    {
        //Username
        //Password
        //Id
        //Email özelliklerini Identity User' dan extends ediyor.
        public string KullaniciAdi { get; set; }
        public DateTime CreatedAt { get; set; } 
    }
}
