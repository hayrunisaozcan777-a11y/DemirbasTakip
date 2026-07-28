using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DemirbasTakip.Data;
using DemirbasTakip.Models;

namespace DemirbasTakip.Controllers
{
    public class RaporController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RaporController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? personelId, int? kaynakId, DateTime? baslangic, DateTime? bitis)
        {
            var query = _context.Rezervasyonlar
                .Include(r => r.Personel)
                .Include(r => r.Kaynak)
                .AsQueryable();

            if (personelId.HasValue)
                query = query.Where(r => r.PersonelId == personelId.Value);

            if (kaynakId.HasValue)
                query = query.Where(r => r.KaynakId == kaynakId.Value);

            if (baslangic.HasValue)
                query = query.Where(r => r.BaslangicZamani.Date >= baslangic.Value.Date);

            if (bitis.HasValue)
                query = query.Where(r => r.BaslangicZamani.Date <= bitis.Value.Date);

            var sonuclar = await query
                .OrderByDescending(r => r.BaslangicZamani)
                .ToListAsync();

            ViewBag.Personeller = new SelectList(await _context.Personeller.OrderBy(p => p.AdSoyad).ToListAsync(), "Id", "AdSoyad", personelId);
            ViewBag.Kaynaklar = new SelectList(await _context.Kaynaklar.OrderBy(k => k.Ad).ToListAsync(), "Id", "Ad", kaynakId);
            ViewBag.Baslangic = baslangic?.ToString("yyyy-MM-dd");
            ViewBag.Bitis = bitis?.ToString("yyyy-MM-dd");

            ViewBag.ToplamKayit = sonuclar.Count;
            ViewBag.AktifSayisi = sonuclar.Count(r => r.Durum == RezervasyonDurumu.Aktif);
            ViewBag.IptalSayisi = sonuclar.Count(r => r.Durum == RezervasyonDurumu.IptalEdildi);

            return View(sonuclar);
        }
    }
}