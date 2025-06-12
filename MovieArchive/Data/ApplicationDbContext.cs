using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieArchive.Models;

namespace MovieArchive.Data
{
    public class ApplicationDbContext : IdentityDbContext<Kullanici>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        

        public DbSet<Film> Filmler { get; set; }
        public DbSet<Admin> Adminler { get; set; }

        public DbSet<Uye> Uyeler{ get; set; }

        public DbSet<Yorum> Yorumlar{ get; set; }

        public DbSet<Puan> Puanlar { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Örneğin aşağıda “Admin” rolü için sabit bir GUID kullanıldı
            var adminRoleId = "3f29a0c2-1d5f-4e3b-8a1b-2dbb4b503e4d";
            var userRoleId = "a7d55a5d-7c77-4ae4-b4f3-6db3462e5f12";

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "d1c5a6f2-8e0a-4c15-b8c7-5fa2c8e798ab" 
                },
                new IdentityRole
                {
                    Id = userRoleId,
                    Name = "Uye",
                    NormalizedName = "UYE",
                    ConcurrencyStamp = "f7b2f3a0-4c18-46e0-9c2f-90bb0f4e1a7c" 
                }
            );
        }

    }
}
