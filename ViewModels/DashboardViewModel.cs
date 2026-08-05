namespace DemirbasTakip.ViewModels
{
    public class DashboardViewModel
    {
        public int ToplamPersonelSayisi { get; set; }
        public int ToplamDemirbasSayisi { get; set; }
        public int AktifZimmetSayisi { get; set; }
        public int BugunkuZimmetSayisi { get; set; }

        public List<DemirbasTakip.Models.Zimmet> SonZimmetler { get; set; } = new();

        // Grafik verileri
        public List<string> KategoriEtiketleri { get; set; } = new();
        public List<int> KategoriSayilari { get; set; } = new();

        public int ZimmetliSayisi { get; set; }
        public int IadeEdilmisSayisi { get; set; }
    }
}