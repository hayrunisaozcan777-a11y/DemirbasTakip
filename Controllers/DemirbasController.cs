using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DemirbasTakip.Data;
using DemirbasTakip.Filters;
using DemirbasTakip.Models;

namespace DemirbasTakip.Controllers
{
    public class DemirbasController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DemirbasController(ApplicationDbContext context) { _context = context; }

        public async Task<IActionResult> Index(string? arama, DemirbasTuru? kategori, DemirbasDurum? durum, int sayfa = 1)
        {
            const int sayfaBoyutu = 5;

            var query = _context.Demirbaslar.AsQueryable();

            if (!string.IsNullOrWhiteSpace(arama))
                query = query.Where(d => d.Ad.Contains(arama) || d.DemirbasKodu.Contains(arama));

            if (kategori.HasValue)
                query = query.Where(d => d.Kategori == kategori.Value);

            if (durum.HasValue)
                query = query.Where(d => d.Durum == durum.Value);

            ViewBag.Arama = arama;
            ViewBag.Kategori = kategori;
            ViewBag.Durum = durum;

            int toplamKayit = await query.CountAsync();
            int toplamSayfa = (int)Math.Ceiling(toplamKayit / (double)sayfaBoyutu);

            var kayitlar = await query
                .OrderBy(d => d.Ad)
                .Skip((sayfa - 1) * sayfaBoyutu)
                .Take(sayfaBoyutu)
                .ToListAsync();

            var vm = new DemirbasTakip.ViewModels.SayfalanmisListe<Demirbas>
            {
                Kayitlar = kayitlar,
                MevcutSayfa = sayfa,
                ToplamSayfa = Math.Max(toplamSayfa, 1),
                ToplamKayit = toplamKayit
            };

            return View(vm);
        }

        // Demirbaş Detay ve Zimmet Geçmişi (Audit Trail)
        public async Task<IActionResult> Details(int id)
        {
            var demirbas = await _context.Demirbaslar
                .Include(d => d.Zimmetler)
                    .ThenInclude(z => z.Personel)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (demirbas == null) return NotFound();

            demirbas.Zimmetler = demirbas.Zimmetler
                .OrderByDescending(z => z.ZimmetTarihi)
                .ToList();

            return View(demirbas);
        }

        [AdminOnlyFilter]
        public IActionResult Create() => View(new Demirbas());

        [AdminOnlyFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Demirbas demirbas)
        {
            if (!ModelState.IsValid) return View(demirbas);

            bool kodVarMi = await _context.Demirbaslar.AnyAsync(d => d.DemirbasKodu == demirbas.DemirbasKodu);
            if (kodVarMi)
            {
                ModelState.AddModelError(nameof(Demirbas.DemirbasKodu), "Bu demirbaş kodu zaten kayıtlı.");
                return View(demirbas);
            }

            _context.Demirbaslar.Add(demirbas);
            await _context.SaveChangesAsync();
            TempData["Basarili"] = "Demirbaş başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        [AdminOnlyFilter]
        public async Task<IActionResult> Edit(int id)
        {
            var demirbas = await _context.Demirbaslar.FindAsync(id);
            if (demirbas == null) return NotFound();
            return View(demirbas);
        }

        [AdminOnlyFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Demirbas demirbas)
        {
            if (id != demirbas.Id) return NotFound();
            if (!ModelState.IsValid) return View(demirbas);

            try
            {
                _context.Update(demirbas);
                await _context.SaveChangesAsync();
                TempData["Basarili"] = "Demirbaş bilgileri güncellendi.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Demirbaslar.AnyAsync(d => d.Id == id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        [AdminOnlyFilter]
        public async Task<IActionResult> Delete(int id)
        {
            var demirbas = await _context.Demirbaslar.FirstOrDefaultAsync(d => d.Id == id);
            if (demirbas == null) return NotFound();

            bool aktifZimmetVarMi = await _context.Zimmetler
                .AnyAsync(z => z.DemirbasId == id && z.Durum == ZimmetDurumu.Aktif);
            ViewBag.SilinebilirMi = !aktifZimmetVarMi;

            return View(demirbas);
        }

        [AdminOnlyFilter]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var demirbas = await _context.Demirbaslar.FindAsync(id);
            if (demirbas == null) return NotFound();

            bool aktifZimmetVarMi = await _context.Zimmetler
                .AnyAsync(z => z.DemirbasId == id && z.Durum == ZimmetDurumu.Aktif);

            if (aktifZimmetVarMi)
            {
                TempData["Hata"] = "Bu demirbaşın aktif zimmeti bulunduğu için silinemez.";
                return RedirectToAction(nameof(Index));
            }

            _context.Demirbaslar.Remove(demirbas);
            await _context.SaveChangesAsync();
            TempData["Basarili"] = "Demirbaş silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}