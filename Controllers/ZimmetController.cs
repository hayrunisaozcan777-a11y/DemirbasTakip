using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DemirbasTakip.Data;
using DemirbasTakip.Models;
using DemirbasTakip.ViewModels;

namespace DemirbasTakip.Controllers
{
    public class ZimmetController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ZimmetController(ApplicationDbContext context) { _context = context; }

        public async Task<IActionResult> Index(int? demirbasId, int? personelId)
        {
            var query = _context.Zimmetler
                .Include(z => z.Personel)
                .Include(z => z.Demirbas)
                .AsQueryable();

            if (demirbasId.HasValue) query = query.Where(z => z.DemirbasId == demirbasId.Value);
            if (personelId.HasValue) query = query.Where(z => z.PersonelId == personelId.Value);

            ViewBag.Demirbaslar = new SelectList(await _context.Demirbaslar.OrderBy(d => d.Ad).ToListAsync(), "Id", "Ad", demirbasId);
            ViewBag.Personeller = new SelectList(await _context.Personeller.OrderBy(p => p.AdSoyad).ToListAsync(), "Id", "AdSoyad", personelId);

            var liste = await query.OrderByDescending(z => z.ZimmetTarihi).ToListAsync();
            return View(liste);
        }

        public async Task<IActionResult> Create()
        {
            var vm = new ZimmetFormViewModel();
            await DoldurListeler(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ZimmetFormViewModel vm)
        {
            // İş kuralı: Aktif zimmetli demirbaş tekrar zimmetlenemez
            bool aktifZimmetVarMi = await _context.Zimmetler
                .AnyAsync(z => z.DemirbasId == vm.DemirbasId && z.Durum == ZimmetDurumu.Aktif);

            if (aktifZimmetVarMi)
            {
                ModelState.AddModelError(string.Empty, "Bu demirbaş zaten başka bir personele zimmetli. Önce iade edilmesi gerekir.");
            }

            if (!ModelState.IsValid)
            {
                await DoldurListeler(vm);
                return View(vm);
            }

            var zimmet = new Zimmet
            {
                PersonelId = vm.PersonelId,
                DemirbasId = vm.DemirbasId,
                ZimmetTarihi = vm.ZimmetTarihi,
                Not = vm.Not,
                Durum = ZimmetDurumu.Aktif
            };

            _context.Zimmetler.Add(zimmet);
            await _context.SaveChangesAsync();
            TempData["Basarili"] = "Zimmet işlemi başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Iade(int id)
        {
            var zimmet = await _context.Zimmetler
                .Include(z => z.Personel)
                .Include(z => z.Demirbas)
                .FirstOrDefaultAsync(z => z.Id == id);
            if (zimmet == null) return NotFound();
            return View(zimmet);
        }

        [HttpPost, ActionName("Iade")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IadeOnay(int id)
        {
            var zimmet = await _context.Zimmetler.FindAsync(id);
            if (zimmet == null) return NotFound();

            if (zimmet.Durum == ZimmetDurumu.IadeEdildi)
            {
                TempData["Hata"] = "Bu zimmet zaten iade edilmiş.";
                return RedirectToAction(nameof(Index));
            }

            zimmet.Durum = ZimmetDurumu.IadeEdildi;
            zimmet.IadeTarihi = DateTime.Now;
            await _context.SaveChangesAsync();
            TempData["Basarili"] = "İade işlemi tamamlandı.";
            return RedirectToAction(nameof(Index));
        }

        private async Task DoldurListeler(ZimmetFormViewModel vm)
        {
            vm.PersonelListesi = await _context.Personeller
                .Where(p => p.AktifMi).OrderBy(p => p.AdSoyad)
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.AdSoyad })
                .ToListAsync();

            // Sadece aktif zimmeti olmayan demirbaşlar listelensin
            var zimmetliIdler = await _context.Zimmetler
                .Where(z => z.Durum == ZimmetDurumu.Aktif)
                .Select(z => z.DemirbasId)
                .ToListAsync();

            vm.DemirbasListesi = await _context.Demirbaslar
                .Where(d => d.AktifMi && !zimmetliIdler.Contains(d.Id))
                .OrderBy(d => d.Ad)
                .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Ad + " (" + d.DemirbasKodu + ")" })
                .ToListAsync();
        }
    }
}