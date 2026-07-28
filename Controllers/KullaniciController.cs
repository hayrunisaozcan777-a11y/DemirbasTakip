using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DemirbasTakip.Data;
using DemirbasTakip.Helpers;
using DemirbasTakip.Models;
using DemirbasTakip.ViewModels;

namespace DemirbasTakip.Controllers
{
    public class KullaniciController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KullaniciController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Kullanici
        public async Task<IActionResult> Index()
        {
            var liste = await _context.Kullanicilar
                .OrderBy(k => k.KullaniciAdi)
                .ToListAsync();

            return View(liste);
        }

        // GET: Kullanici/Create
        public IActionResult Create()
        {
            return View(new KullaniciFormViewModel());
        }

        // POST: Kullanici/Create
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
                AktifMi = true
            };

            _context.Kullanicilar.Add(kullanici);
            await _context.SaveChangesAsync();

            TempData["Basarili"] = "Kullanıcı başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Kullanici/PasifYap/5
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
    }
}