using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DemirbasTakip.Data;
using DemirbasTakip.Filters;
using DemirbasTakip.Helpers;
using DemirbasTakip.Models;
using DemirbasTakip.ViewModels;

namespace DemirbasTakip.Controllers
{
    [AdminOnlyFilter]
    public class KullaniciController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KullaniciController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var liste = await _context.Kullanicilar
                .Include(k => k.Personel)
                .OrderBy(k => k.KullaniciAdi)
                .ToListAsync();

            return View(liste);
        }

        public IActionResult Create()
        {
            return View(new KullaniciFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KullaniciFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            bool kullaniciAdiVarMi = await _context.Kullanicilar
                .AnyAsync(k => k.KullaniciAdi == vm.KullaniciAdi);

            if (kullaniciAdiVarMi)
            {
                ModelState.AddModelError(nameof(vm.KullaniciAdi), "Bu kullanıcı adı zaten kayıtlı.");
                return View(vm);
            }

            var kullanici = new Kullanici
            {
                KullaniciAdi = vm.KullaniciAdi,
                SifreHash = PasswordHasher.Hash(vm.Sifre),
                Rol = vm.Rol,
                AktifMi = true,
                SifreDegistirilsinMi = false,
                OnaylandiMi = (vm.Rol == KullaniciRol.Admin) // Admin ise direkt onaylı, Personel ise onay bekler
            };

            _context.Kullanicilar.Add(kullanici);
            await _context.SaveChangesAsync();

            TempData["Basarili"] = kullanici.OnaylandiMi
                ? "Admin kullanıcısı başarıyla oluşturuldu."
                : "Kullanıcı başarıyla oluşturuldu, onay bekliyor.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Onayla(int id)
        {
            var kullanici = await _context.Kullanicilar
                .Include(k => k.Personel)
                .FirstOrDefaultAsync(k => k.Id == id);

            if (kullanici == null) return NotFound();

            kullanici.OnaylandiMi = true;

            if (kullanici.PersonelId == null)
            {
                var yeniPersonel = new Personel
                {
                    AdSoyad = kullanici.KullaniciAdi,
                    Eposta = $"{kullanici.KullaniciAdi.ToLower()}@test.com",
                    Departman = "Atanmadı",
                    AktifMi = true
                };

                _context.Personeller.Add(yeniPersonel);
                await _context.SaveChangesAsync();

                kullanici.PersonelId = yeniPersonel.Id;
            }

            await _context.SaveChangesAsync();

            TempData["Basarili"] = $"{kullanici.KullaniciAdi} başarıyla onaylandı ve personele eklendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PasifYap(int id)
        {
            var kullanici = await _context.Kullanicilar.FindAsync(id);
            if (kullanici == null) return NotFound();

            kullanici.AktifMi = !kullanici.AktifMi;
            await _context.SaveChangesAsync();

            TempData["Basarili"] = kullanici.AktifMi ? "Kullanıcı aktifleştirildi." : "Kullanıcı pasifleştirildi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SifreSifirla(int id)
        {
            var kullanici = await _context.Kullanicilar.FindAsync(id);
            if (kullanici == null) return NotFound();

            kullanici.SifreHash = PasswordHasher.Hash("123456");
            kullanici.SifreDegistirilsinMi = true;

            await _context.SaveChangesAsync();

            TempData["Basarili"] = $"{kullanici.KullaniciAdi} kullanıcısının şifresi '123456' olarak sıfırlandı.";
            return RedirectToAction(nameof(Index));
        }
    }
}