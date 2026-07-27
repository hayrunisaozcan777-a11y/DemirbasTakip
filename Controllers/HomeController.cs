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
                ToplamKaynakSayisi = await _context.Kaynaklar.CountAsync(k => k.AktifMi),
                AktifRezervasyonSayisi = await _context.Rezervasyonlar.CountAsync(r => r.Durum == RezervasyonDurumu.Aktif),
                BugunkuRezervasyonSayisi = await _context.Rezervasyonlar
                    .CountAsync(r => r.Durum == RezervasyonDurumu.Aktif && r.BaslangicZamani.Date == DateTime.Now.Date),
                SonRezervasyonlar = await _context.Rezervasyonlar
                    .Include(r => r.Personel)
                    .Include(r => r.Kaynak)
                    .OrderByDescending(r => r.OlusturmaTarihi)
                    .Take(5)
                    .ToListAsync()
            };

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