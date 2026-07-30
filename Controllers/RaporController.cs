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
        public RaporController(ApplicationDbContext context) { _context = context; }

        public async Task<IActionResult> Index(int? personelId, int? demirbasId, DateTime? baslangic, DateTime? bitis)
        {
            var query = _context.Zimmetler.Include(z => z.Personel).Include(z => z.Demirbas).AsQueryable();

            if (personelId.HasValue) query = query.Where(z => z.PersonelId == personelId.Value);
            if (demirbasId.HasValue) query = query.Where(z => z.DemirbasId == demirbasId.Value);
            if (baslangic.HasValue) query = query.Where(z => z.ZimmetTarihi.Date >= baslangic.Value.Date);
            if (bitis.HasValue) query = query.Where(z => z.ZimmetTarihi.Date <= bitis.Value.Date);

            var sonuclar = await query.OrderByDescending(z => z.ZimmetTarihi).ToListAsync();

            ViewBag.Personeller = new SelectList(await _context.Personeller.OrderBy(p => p.AdSoyad).ToListAsync(), "Id", "AdSoyad", personelId);
            ViewBag.Demirbaslar = new SelectList(await _context.Demirbaslar.OrderBy(d => d.Ad).ToListAsync(), "Id", "Ad", demirbasId);
            ViewBag.Baslangic = baslangic?.ToString("yyyy-MM-dd");
            ViewBag.Bitis = bitis?.ToString("yyyy-MM-dd");
            ViewBag.ToplamKayit = sonuclar.Count;
            ViewBag.AktifSayisi = sonuclar.Count(z => z.Durum == ZimmetDurumu.Aktif);
            ViewBag.IadeSayisi = sonuclar.Count(z => z.Durum == ZimmetDurumu.IadeEdildi);

            return View(sonuclar);
        }
    }
}