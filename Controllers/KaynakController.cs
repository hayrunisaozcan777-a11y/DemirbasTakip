using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DemirbasTakip.Data;
using DemirbasTakip.Models;

namespace DemirbasTakip.Controllers
{
    public class KaynakController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KaynakController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? arama, KaynakTuru? tur)
        {
            var query = _context.Kaynaklar.AsQueryable();

            if (!string.IsNullOrWhiteSpace(arama))
            {
                query = query.Where(k => k.Ad.Contains(arama));
            }

            if (tur.HasValue)
            {
                query = query.Where(k => k.Tur == tur.Value);
            }

            ViewBag.Arama = arama;
            ViewBag.Tur = tur;

            var liste = await query.OrderBy(k => k.Ad).ToListAsync();
            return View(liste);
        }

        public IActionResult Create() => View(new Kaynak());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Kaynak kaynak)
        {
            if (!ModelState.IsValid) return View(kaynak);

            _context.Kaynaklar.Add(kaynak);
            await _context.SaveChangesAsync();

            TempData["Basarili"] = "Kaynak başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var kaynak = await _context.Kaynaklar.FindAsync(id);
            if (kaynak == null) return NotFound();
            return View(kaynak);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Kaynak kaynak)
        {
            if (id != kaynak.Id) return NotFound();
            if (!ModelState.IsValid) return View(kaynak);

            try
            {
                _context.Update(kaynak);
                await _context.SaveChangesAsync();
                TempData["Basarili"] = "Kaynak bilgileri güncellendi.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Kaynaklar.AnyAsync(k => k.Id == id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var kaynak = await _context.Kaynaklar.FirstOrDefaultAsync(k => k.Id == id);
            if (kaynak == null) return NotFound();

            bool aktifRezervasyonVarMi = await _context.Rezervasyonlar
                .AnyAsync(r => r.KaynakId == id && r.Durum == RezervasyonDurumu.Aktif);

            ViewBag.SilinebilirMi = !aktifRezervasyonVarMi;

            return View(kaynak);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kaynak = await _context.Kaynaklar.FindAsync(id);
            if (kaynak == null) return NotFound();

            bool aktifRezervasyonVarMi = await _context.Rezervasyonlar
                .AnyAsync(r => r.KaynakId == id && r.Durum == RezervasyonDurumu.Aktif);

            if (aktifRezervasyonVarMi)
            {
                TempData["Hata"] = "Bu kaynağın aktif rezervasyonu bulunduğu için silinemez.";
                return RedirectToAction(nameof(Index));
            }

            _context.Kaynaklar.Remove(kaynak);
            await _context.SaveChangesAsync();

            TempData["Basarili"] = "Kaynak silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}