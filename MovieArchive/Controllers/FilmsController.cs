using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieArchive.Data;
using MovieArchive.DTOs;
using MovieArchive.Models;
using MovieArchive.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieArchive.Controllers
{
    public class FilmsController : Controller, IYorumlanabilir, IPuanlanabilir
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Kullanici> _userManager;

        public FilmsController(ApplicationDbContext context, UserManager<Kullanici> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Films
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["CurrentSort"] = sortOrder;

            var filmler = _context.Filmler.AsQueryable();

            switch (sortOrder)
            {
                case "puan":
                    filmler = filmler.OrderByDescending(f => f.OrtalamaPuan);
                    break;
                case "yorum":
                    filmler = filmler.OrderByDescending(f => f.Yorumlar.Count); 
                    break;
                case "izlenme":
                    filmler = filmler.OrderByDescending(f => f.ToplamPuanVerenKisi);
                    break;
                default:
                    // varsayılan sıralama (ID'ye göre olabilir)
                    filmler = filmler.OrderBy(f => f.Id);
                    break;
            }

            return View(await filmler.ToListAsync());
        }


        // GET: Films/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var film = await _context.Filmler
                .Include(f => f.Yorumlar)
                .ThenInclude(y => y.Uye)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (film == null)
                return NotFound();

            return View(film);
        }

        // GET: Films/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [Authorize(Roles = "Uye")]
        public async Task<IActionResult> AddPoint(int filmId, int puan)
        {
            // Puan 1-10 arasında olmalı
            if (puan < 1 || puan > 10)
            {
                TempData["PuanHatasi"] = "Puan 1 ile 10 arasında olmalıdır.";
                return RedirectToAction(nameof(Details), new { id = filmId });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var film = await _context.Filmler.FirstOrDefaultAsync(f => f.Id == filmId);
            var uye = await _userManager.FindByIdAsync(userId);

            if (film == null || uye == null)
                return NotFound();

            // Daha önce puan verdiyse tekrar vermesini engelle (opsiyonel)
            bool dahaOncePuanVerdiMi = await _context.Puanlar
                .AnyAsync(p => p.Film == film && p.Uye == uye);

            if (dahaOncePuanVerdiMi)
            {
                TempData["PuanHatasi"] = "Bu filme daha önce puan verdiniz.";
                return RedirectToAction(nameof(Details), new { id = filmId });
            }

            var yeniPuan = new Puan
            {
                Film = film,
                Uye = (Uye)uye,
                Deger = puan,
            };

            _context.Puanlar.Add(yeniPuan);

            // Ortalama puanları güncelle (isteğe bağlı)
            film.ToplamPuan += puan;
            film.ToplamPuanVerenKisi += 1;
            film.OrtalamaPuan = (double)film.ToplamPuan / film.ToplamPuanVerenKisi;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = filmId });
        }




        // POST: Films/AddComment
        [HttpPost]
        [Authorize]
        [Authorize(Roles = "Uye")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int filmId, string content)
        {
            // Basit doğrulama: içeriğin dolu ve 1000 karakterden kısa olması gerekir
            if (string.IsNullOrWhiteSpace(content) || content.Length > 1000)
            {
                ModelState.AddModelError(
                    "content",
                    "Yorum içeriği zorunludur ve en fazla 1000 karakter olabilir."
                );
            }

            // ModelState geçersizse, filmi ve yorumlarını tekrar yükle ve Detay sayfasını göster
            if (!ModelState.IsValid)
            {
                var filmHatasi = await _context.Filmler
                    .Include(f => f.Yorumlar)
                    .ThenInclude(y => y.Uye)
                    .FirstOrDefaultAsync(f => f.Id == filmId);

                return View("Details", filmHatasi);
            }

            // Giriş yapan kullanıcının Id'sini al
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            

            // Film ve Uye nesnelerini veritabanından çek
            var film = await _context.Filmler
                .Include(f => f.Yorumlar)          // <— BAŞARILI DAKİKA İÇİN DE EKLİYORUZ
                .ThenInclude(y => y.Uye)           //     Aksi takdirde yeni View’da null olur
                .FirstOrDefaultAsync(f => f.Id == filmId);

            var uye = await _userManager.FindByIdAsync(userId);

            // Güvenlik: Film veya kullanıcı bulunamazsa 404 döndür
            if (film == null || uye == null)
            {
                return NotFound();
            }

            // Yorum nesnesi oluştur
            var yorum = new Yorum
            {
                Icerik = content,
                Film = film,
                Uye = (Uye) uye,
                Tarih = DateTime.UtcNow
            };

            // Veritabanına ekle
            _context.Yorumlar.Add(yorum);
            await _context.SaveChangesAsync();

            // Detay sayfasına yönlendir
            return RedirectToAction(nameof(Details), new { id = filmId });
        }




        // POST: Films/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FilmCreateDTO request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var yeniFilm = new Film
            {
                Ad = request.Ad,
                Tur = request.Tur,
                YayinTarihi = request.YayinTarihi,
                Yoneten = request.Yoneten,
                Aciklama = request.Aciklama,
                OrtalamaPuan = 0,
                ToplamPuanVerenKisi = 0,
                ToplamPuan=0,
                Yorumlar = new List<Yorum>(),
                Puanlar = new List<Puan>()
            };

            _context.Filmler.Add(yeniFilm);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: Films/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Filmler.FindAsync(id);
            if (film == null)
            {
                return NotFound();
            }
            return View(film);
        }

        // POST: Films/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ad,Tur,YayinTarihi,Yoneten,Aciklama")] Film film)
        {
            if (id != film.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var mevcutFilm = await _context.Filmler
                        .Include(f => f.Yorumlar)
                        .Include(f => f.Puanlar)
                        .FirstOrDefaultAsync(f => f.Id == id);

                    if (mevcutFilm == null)
                        return NotFound();

                    // Sadece güncellenmesi gereken alanlar
                    mevcutFilm.Ad = film.Ad;
                    mevcutFilm.Tur = film.Tur;
                    mevcutFilm.YayinTarihi = film.YayinTarihi;
                    mevcutFilm.Yoneten = film.Yoneten;
                    mevcutFilm.Aciklama = film.Aciklama;

                    // OrtalamaPuan, ToplamPuan, Yorumlar gibi alanlara dokunma

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmExists(film.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(film);
        }


        // GET: Films/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Filmler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // POST: Films/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var film = await _context.Filmler.FindAsync(id);
            if (film != null)
            {
                _context.Filmler.Remove(film);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmExists(int id)
        {
            return _context.Filmler.Any(e => e.Id == id);
        }
    }
}
