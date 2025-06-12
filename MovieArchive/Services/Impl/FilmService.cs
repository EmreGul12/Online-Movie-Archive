using Microsoft.EntityFrameworkCore;
using MovieArchive.Data;
using MovieArchive.Models;

namespace MovieArchive.Services.Impl
{
    public class FilmService
    {
        private readonly ApplicationDbContext _context;

        public FilmService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void PuanVer(int filmId, int deger)
        {
            var film = _context.Filmler
                        .Include(f => f.Puanlar)
                        .FirstOrDefault(f => f.Id == filmId);

            if (film is IPuanlanabilir puanlanabilir)
            {
                puanlanabilir.AddPoint(filmId, deger);
                _context.SaveChanges();
            }
        }

        public void YorumYap(int filmId, int uyeId, string yorum)
        {
            var film = _context.Filmler
                        .Include(f => f.Yorumlar)
                        .FirstOrDefault(f => f.Id == filmId);

            if (film is IYorumlanabilir yorumlanabilir)
            {
                yorumlanabilir.AddComment(uyeId, yorum);
                _context.SaveChanges();
            }
        }
    }
}
