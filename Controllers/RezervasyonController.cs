using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DemirbasTakip.Data;
using DemirbasTakip.Models;
using DemirbasTakip.ViewModels;

namespace DemirbasTakip.Controllers
{
    public class RezervasyonController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RezervasyonController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rezervasyon
        public async Task<IActionResult> Index(int? kaynakId, int? personelId, DateTime? tarih)
        {
            var query = _context.Rezervasyonlar
                .Include(r => r.Personel)
                .Include(r => r.Kaynak)
                .AsQueryable();

            if (kaynakId.HasValue)
                query = query.Where(r => r.KaynakId == kaynakId.Value);

            if (personelId.HasValue)
                query = query.Where(r => r.PersonelId == personelId.Value);

            if (tarih.HasValue)
                query = query.Where(r => r.BaslangicZamani.Date == tarih.Value.Date);

            ViewBag.Kaynaklar = new SelectList(await _context.Kaynaklar.OrderBy(k => k.Ad).ToListAsync(), "Id", "Ad", kaynakId);
            ViewBag.Personeller = new SelectList(await _context.Personeller.OrderBy(p => p.AdSoyad).ToListAsync(), "Id", "AdSoyad", personelId);
            ViewBag.Tarih = tarih?.ToString("yyyy-MM-dd");

            var liste = await query
                .OrderByDescending(r => r.BaslangicZamani)
                .ToListAsync();

            return View(liste);
        }

        // GET: Rezervasyon/Create
        public async Task<IActionResult> Create()
        {
            var vm = new RezervasyonFormViewModel();
            await DoldurListeler(vm);
            return View(vm);
        }

        // POST: Rezervasyon/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RezervasyonFormViewModel vm)
        {
            if (vm.BitisZamani <= vm.BaslangicZamani)
            {
                ModelState.AddModelError(nameof(vm.BitisZamani), "Bitiş zamanı, başlangıç zamanından sonra olmalıdır.");
            }

            if (vm.BaslangicZamani < DateTime.Now.AddMinutes(-1))
            {
                ModelState.AddModelError(nameof(vm.BaslangicZamani), "Geçmiş bir tarihe rezervasyon oluşturulamaz.");
            }

            if (ModelState.IsValid)
            {
                // İş kuralı: Aynı kaynak için çakışan zaman aralığında aktif rezervasyon var mı?
                bool cakisiyorMu = await _context.Rezervasyonlar
                    .Where(r => r.KaynakId == vm.KaynakId && r.Durum == RezervasyonDurumu.Aktif)
                    .AnyAsync(r => r.BaslangicZamani < vm.BitisZamani && r.BitisZamani > vm.BaslangicZamani);

                if (cakisiyorMu)
                {
                    ModelState.AddModelError(string.Empty,
                        "Seçilen kaynak, bu zaman aralığında zaten rezerve edilmiş. Lütfen başka bir saat veya kaynak seçin.");
                }
            }

            if (!ModelState.IsValid)
            {
                await DoldurListeler(vm);
                return View(vm);
            }

            var rezervasyon = new Rezervasyon
            {
                PersonelId = vm.PersonelId,
                KaynakId = vm.KaynakId,
                BaslangicZamani = vm.BaslangicZamani,
                BitisZamani = vm.BitisZamani,
                Aciklama = vm.Aciklama,
                Durum = RezervasyonDurumu.Aktif,
                OlusturmaTarihi = DateTime.Now
            };

            _context.Rezervasyonlar.Add(rezervasyon);
            await _context.SaveChangesAsync();

            TempData["Basarili"] = "Rezervasyon başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Rezervasyon/Iptal/5
        public async Task<IActionResult> Iptal(int id)
        {
            var rezervasyon = await _context.Rezervasyonlar
                .Include(r => r.Personel)
                .Include(r => r.Kaynak)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rezervasyon == null) return NotFound();
            return View(rezervasyon);
        }

        // POST: Rezervasyon/Iptal/5
        [HttpPost, ActionName("Iptal")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IptalOnay(int id)
        {
            var rezervasyon = await _context.Rezervasyonlar.FindAsync(id);
            if (rezervasyon == null) return NotFound();

            if (rezervasyon.Durum == RezervasyonDurumu.IptalEdildi)
            {
                TempData["Hata"] = "Bu rezervasyon zaten iptal edilmiş.";
                return RedirectToAction(nameof(Index));
            }

            rezervasyon.Durum = RezervasyonDurumu.IptalEdildi;
            await _context.SaveChangesAsync();

            TempData["Basarili"] = "Rezervasyon iptal edildi.";
            return RedirectToAction(nameof(Index));
        }

        private async Task DoldurListeler(RezervasyonFormViewModel vm)
        {
            vm.PersonelListesi = await _context.Personeller
                .Where(p => p.AktifMi)
                .OrderBy(p => p.AdSoyad)
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.AdSoyad })
                .ToListAsync();

            vm.KaynakListesi = await _context.Kaynaklar
                .Where(k => k.AktifMi)
                .OrderBy(k => k.Ad)
                .Select(k => new SelectListItem { Value = k.Id.ToString(), Text = k.Ad + " (" + k.Tur + ")" })
                .ToListAsync();
        }
    }
}