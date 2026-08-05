namespace DemirbasTakip.ViewModels
{
    public class SayfalanmisListe<T>
    {
        public List<T> Kayitlar { get; set; } = new();
        public int MevcutSayfa { get; set; }
        public int ToplamSayfa { get; set; }
        public int ToplamKayit { get; set; }

        public bool OncekiVarMi => MevcutSayfa > 1;
        public bool SonrakiVarMi => MevcutSayfa < ToplamSayfa;
    }
}