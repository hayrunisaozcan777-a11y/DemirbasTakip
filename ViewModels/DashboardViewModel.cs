namespace DemirbasTakip.ViewModels
{
    public class DashboardViewModel
    {
        public int ToplamPersonelSayisi { get; set; }
        public int ToplamKaynakSayisi { get; set; }
        public int AktifRezervasyonSayisi { get; set; }
        public int BugunkuRezervasyonSayisi { get; set; }

        public List<DemirbasTakip.Models.Rezervasyon> SonRezervasyonlar { get; set; } = new();
    }
}