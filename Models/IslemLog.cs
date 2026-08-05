namespace DemirbasTakip.Models
{
    public class IslemLog
    {
        public int Id { get; set; }
        public string Kullanici { get; set; } = string.Empty;
        public string IslemTuru { get; set; } = string.Empty;
        public string Aciklama { get; set; } = string.Empty;
        public DateTime Tarih { get; set; } = DateTime.Now;
    }
}