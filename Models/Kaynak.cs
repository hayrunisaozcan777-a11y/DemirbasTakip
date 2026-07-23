using System.ComponentModel.DataAnnotations;

namespace DemirbasTakip.Models
{
    public enum KaynakTuru
    {
        ToplantiOdasi = 1,
        Projeksiyon = 2,
        Arac = 3,
        DigerEkipman = 4
    }

    public class Kaynak
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kaynak adı zorunludur.")]
        [StringLength(100)]
        [Display(Name = "Kaynak Adı")]
        public string Ad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kaynak türü zorunludur.")]
        [Display(Name = "Tür")]
        public KaynakTuru Tur { get; set; }

        [Range(0, 1000, ErrorMessage = "Kapasite 0-1000 arasında olmalıdır.")]
        [Display(Name = "Kapasite")]
        public int Kapasite { get; set; }

        [StringLength(100)]
        [Display(Name = "Konum")]
        public string? Konum { get; set; }

        [Display(Name = "Aktif mi?")]
        public bool AktifMi { get; set; } = true;

        public ICollection<Rezervasyon> Rezervasyonlar { get; set; } = new List<Rezervasyon>();
    }
}