using System.ComponentModel.DataAnnotations;

namespace DemirbasTakip.Models
{
    public class Personel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [StringLength(100, ErrorMessage = "Ad Soyad en fazla 100 karakter olabilir.")]
        [Display(Name = "Ad Soyad")]
        public string AdSoyad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Departman zorunludur.")]
        [StringLength(50)]
        [Display(Name = "Departman")]
        public string Departman { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [Display(Name = "E-posta")]
        public string Eposta { get; set; } = string.Empty;

        [Display(Name = "Aktif mi?")]
        public bool AktifMi { get; set; } = true;

        public ICollection<Rezervasyon> Rezervasyonlar { get; set; } = new List<Rezervasyon>();
    }
}