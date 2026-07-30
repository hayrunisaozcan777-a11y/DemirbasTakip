using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DemirbasTakip.Data;
using DemirbasTakip.Filters;
using DemirbasTakip.Models;

namespace DemirbasTakip.Controllers
{
    public class PersonelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PersonelController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? arama)
        {
            var query = _context.Personeller.AsQueryable();

            if (!string.IsNullOrWhiteSpace(arama))
            {
                query = query.Where(p => p.AdSoyad.Contains(arama) || p.Departman.Contains(arama));
            }

            ViewBag.Arama = arama;
            var liste = await query.OrderBy(p => p.AdSoyad).ToListAsync();
            return View(liste);
        }

        [AdminOnlyFilter]
        public IActionResult Create()
        {
            return View(new Personel());
        }

        [AdminOnlyFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Personel personel)
        {
            if (!ModelState.IsValid)
            {
                return View(personel);
            }

            bool epostaVarMi = await _context.Personeller.AnyAsync(p => p.Eposta == personel.Eposta);
            if (epostaVarMi)
            {
                ModelState.AddModelError(nameof(Personel.Eposta), "Bu e-posta adresi zaten kayıtlı.");
                return View(personel);
            }

            _context.Personeller.Add(personel);
            await _context.SaveChangesAsync();

            TempData["Basarili"] = "Personel başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        [AdminOnlyFilter]
        public async Task<IActionResult> Edit(int id)
        {
            var personel = await _context.Personeller.FindAsync(id);
            if (personel == null) return NotFound();
            return View(personel);
        }

        [AdminOnlyFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Personel personel)
        {
            if (id != personel.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                return View(personel);
            }

            bool epostaVarMi = await _context.Personeller
                .AnyAsync(p => p.Eposta == personel.Eposta && p.Id != personel.Id);
            if (epostaVarMi)
            {
                ModelState.AddModelError(nameof(Personel.Eposta), "Bu e-posta adresi başka bir personelde kayıtlı.");
                return View(personel);
            }

            try
            {
                _context.Update(personel);
                await _context.SaveChangesAsync();
                TempData["Basarili"] = "Personel bilgileri güncellendi.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Personeller.AnyAsync(p => p.Id == id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        [AdminOnlyFilter]
        public async Task<IActionResult> Delete(int id)
        {
            var personel = await _context.Personeller.FirstOrDefaultAsync(p => p.Id == id);
            if (personel == null) return NotFound();

            bool aktifZimmetVarMi = await _context.Zimmetler
                .AnyAsync(z => z.PersonelId == id && z.Durum == ZimmetDurumu.Aktif);

            ViewBag.SilinebilirMi = !aktifZimmetVarMi;

            return View(personel);
        }

        [AdminOnlyFilter]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var personel = await _context.Personeller.FindAsync(id);
            if (personel == null) return NotFound();

            bool aktifZimmetVarMi = await _context.Zimmetler
                .AnyAsync(z => z.PersonelId == id && z.Durum == ZimmetDurumu.Aktif);

            if (aktifZimmetVarMi)
            {
                TempData["Hata"] = "Bu personelin aktif zimmeti bulunduğu için silinemez.";
                return RedirectToAction(nameof(Index));
            }

            _context.Personeller.Remove(personel);
            await _context.SaveChangesAsync();

            TempData["Basarili"] = "Personel silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}