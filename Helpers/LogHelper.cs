using DemirbasTakip.Data;
using DemirbasTakip.Models;

namespace DemirbasTakip.Helpers
{
    public static class LogHelper
    {
        public static async Task KaydetAsync(ApplicationDbContext context, string kullaniciAdi, string islem, string? detay = null)
        {
            context.IslemLoglari.Add(new IslemLog
            {
                KullaniciAdi = kullaniciAdi,
                Islem = islem,
                Detay = detay,
                Tarih = DateTime.Now
            });
            await context.SaveChangesAsync();
        }
    }
}