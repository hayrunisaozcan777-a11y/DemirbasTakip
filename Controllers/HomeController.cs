using DemirbasTakip.Data;
using DemirbasTakip.Models;
using DemirbasTakip.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DemirbasTakip.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new DashboardViewModel
            {
                ToplamPersonelSayisi = await _context.Personeller.CountAsync(p => p.AktifMi),
                ToplamDemirbasSayisi = await _context.Demirbaslar.CountAsync(d => d.AktifMi),
                AktifZimmetSayisi = await _context.Zimmetler.CountAsync(z => z.Durum == ZimmetDurumu.Aktif),
                BugunkuZimmetSayisi = await _context.Zimmetler
                    .CountAsync(z => z.Durum == ZimmetDurumu.Aktif && z.ZimmetTarihi.Date == DateTime.Now.Date),
                SonZimmetler = await _context.Zimmetler
                    .Include(z => z.Personel)
                    .Include(z => z.Demirbas)
                    .OrderByDescending(z => z.ZimmetTarihi)
                    .Take(5)
                    .ToListAsync(),
                ZimmetliSayisi = await _context.Zimmetler.CountAsync(z => z.Durum == ZimmetDurumu.Aktif),
                IadeEdilmisSayisi = await _context.Zimmetler.CountAsync(z => z.Durum == ZimmetDurumu.IadeEdildi)
            };

            var kategoriGruplari = await _context.Demirbaslar
                .GroupBy(d => d.Kategori)
                .Select(g => new { Kategori = g.Key, Sayi = g.Count() })
                .ToListAsync();

            vm.KategoriEtiketleri = kategoriGruplari.Select(k => k.Kategori.ToString()).ToList();
            vm.KategoriSayilari = kategoriGruplari.Select(k => k.Sayi).ToList();

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}