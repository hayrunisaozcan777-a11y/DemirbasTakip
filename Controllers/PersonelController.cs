using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DemirbasTakip.Data;
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

        // GET: Personel
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

        // GET: Personel/Create
        public IActionResult Create()
        {
            return View(new Personel());
        }

        // POST: Personel/Create
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

        // GET: Personel/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var personel = await _context.Personeller.FindAsync(id);
            if (personel == null) return NotFound();
            return View(personel);
        }

        // POST: Personel/Edit/5
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

        // GET: Personel/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var personel = await _context.Personeller.FirstOrDefaultAsync(p => p.Id == id);
            if (personel == null) return NotFound();

            bool aktifRezervasyonVarMi = await _context.Rezervasyonlar
                .AnyAsync(r => r.PersonelId == id && r.Durum == RezervasyonDurumu.Aktif);

            ViewBag.SilinebilirMi = !aktifRezervasyonVarMi;

            return View(personel);
        }

        // POST: Personel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var personel = await _context.Personeller.FindAsync(id);
            if (personel == null) return NotFound();

            bool aktifRezervasyonVarMi = await _context.Rezervasyonlar
                .AnyAsync(r => r.PersonelId == id && r.Durum == RezervasyonDurumu.Aktif);

            if (aktifRezervasyonVarMi)
            {
                TempData["Hata"] = "Bu personelin aktif rezervasyonu bulunduğu için silinemez.";
                return RedirectToAction(nameof(Index));
            }

            _context.Personeller.Remove(personel);
            await _context.SaveChangesAsync();

            TempData["Basarili"] = "Personel silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}