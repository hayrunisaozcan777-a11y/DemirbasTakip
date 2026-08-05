using DemirbasTakip.Data;
using DemirbasTakip.Models;

namespace DemirbasTakip.Helpers
{
    public static class LogHelper
    {
        public static async Task KaydetAsync(ApplicationDbContext context, string kullanici, string islemTuru, string aciklama)
        {
            var log = new IslemLog
            {
                Kullanici = kullanici,
                IslemTuru = islemTuru,
                Aciklama = aciklama,
                Tarih = DateTime.Now
            };

            context.IslemLoglari.Add(log);
            await context.SaveChangesAsync();
        }
    }
}