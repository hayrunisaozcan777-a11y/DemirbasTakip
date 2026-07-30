namespace DemirbasTakip.ViewModels
{
    public class DashboardViewModel
    {
        public int ToplamPersonelSayisi { get; set; }
        public int ToplamDemirbasSayisi { get; set; }
        public int AktifZimmetSayisi { get; set; }
        public int BugunkuZimmetSayisi { get; set; }

        public List<DemirbasTakip.Models.Zimmet> SonZimmetler { get; set; } = new();
    }
}