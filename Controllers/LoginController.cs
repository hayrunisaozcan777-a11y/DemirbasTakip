using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DemirbasTakip.Data;
using DemirbasTakip.Helpers;
using DemirbasTakip.ViewModels;

namespace DemirbasTakip.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("KullaniciId") != null)
                return RedirectToAction("Index", "Home");

            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var hash = PasswordHasher.Hash(model.Sifre);

            var kullanici = await _context.Kullanicilar
                .FirstOrDefaultAsync(k => k.KullaniciAdi == model.KullaniciAdi && k.AktifMi);

            if (kullanici == null || kullanici.SifreHash != hash)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
                return View(model);
            }

            // Session bilgilerini kaydet
            HttpContext.Session.SetInt32("KullaniciId", kullanici.Id);
            HttpContext.Session.SetString("KullaniciAdi", kullanici.KullaniciAdi);
            HttpContext.Session.SetString("KullaniciRol", kullanici.Rol.ToString());

            // ZORUNLU ŞİFRE DEĞİŞTİRME KONTROLÜ
            if (kullanici.SifreDegistirilsinMi)
            {
                TempData["Hata"] = "Geçici şifrenizle giriş yaptınız. Lütfen devam etmek için yeni şifrenizi belirleyin.";
                return RedirectToAction("SifreDegistir");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult SifreDegistir()
        {
            if (HttpContext.Session.GetInt32("KullaniciId") == null)
                return RedirectToAction("Index");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SifreDegistir(string MevcutSifre, string YeniSifre, string YeniSifreTekrar)
        {
            var kullaniciId = HttpContext.Session.GetInt32("KullaniciId");
            if (kullaniciId == null)
                return RedirectToAction("Index");

            if (string.IsNullOrEmpty(MevcutSifre) || string.IsNullOrEmpty(YeniSifre) || string.IsNullOrEmpty(YeniSifreTekrar))
            {
                TempData["Hata"] = "Lütfen tüm alanları doldurun.";
                return View();
            }

            if (YeniSifre != YeniSifreTekrar)
            {
                TempData["Hata"] = "Yeni şifreler birbiriyle eşleşmiyor!";
                return View();
            }

            var kullanici = await _context.Kullanicilar.FindAsync(kullaniciId);

            if (kullanici == null)
            {
                TempData["Hata"] = "Kullanıcı bulunamadı!";
                return View();
            }

            var mevcutHash = PasswordHasher.Hash(MevcutSifre);
            if (kullanici.SifreHash != mevcutHash)
            {
                TempData["Hata"] = "Mevcut şifreniz hatalı!";
                return View();
            }

            // Şifreyi güncelle ve zorunluluk bayrağını kaldır
            kullanici.SifreHash = PasswordHasher.Hash(YeniSifre);
            kullanici.SifreDegistirilsinMi = false;

            await _context.SaveChangesAsync();

            TempData["Basarili"] = "Şifreniz başarıyla değiştirildi.";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Cikis()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult ErisimEngellendi()
        {
            return View();
        }
    }
}