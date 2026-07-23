using System.ComponentModel.DataAnnotations;

namespace DemirbasTakip.Models
{
    public enum KullaniciRol
    {
        Admin = 1,
        Personel = 2
    }

    public class Kullanici
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        [StringLength(50)]
        [Display(Name = "Kullanıcı Adı")]
        public string KullaniciAdi { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre zorunludur.")]
        public string SifreHash { get; set; } = string.Empty;

        [Required]
        public KullaniciRol Rol { get; set; } = KullaniciRol.Personel;

        public bool AktifMi { get; set; } = true;

        public int? PersonelId { get; set; }
        public Personel? Personel { get; set; }
    }
}