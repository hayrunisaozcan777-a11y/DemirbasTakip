using System.ComponentModel.DataAnnotations;

namespace DemirbasTakip.Models
{
    public enum ZimmetDurumu
    {
        [Display(Name = "Zimmetli")]
        Aktif = 1,

        [Display(Name = "İade Edildi")]
        IadeEdildi = 2
    }

    public class Zimmet
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Personel seçimi zorunludur.")]
        [Display(Name = "Personel")]
        public int PersonelId { get; set; }
        public Personel? Personel { get; set; }

        [Required(ErrorMessage = "Demirbaş seçimi zorunludur.")]
        [Display(Name = "Demirbaş")]
        public int DemirbasId { get; set; }
        public Demirbas? Demirbas { get; set; }

        [Required(ErrorMessage = "Zimmet tarihi zorunludur.")]
        [Display(Name = "Zimmet Tarihi")]
        public DateTime ZimmetTarihi { get; set; } = DateTime.Now;

        [Display(Name = "İade Tarihi")]
        public DateTime? IadeTarihi { get; set; }

        [StringLength(250)]
        [Display(Name = "Not")]
        public string? Not { get; set; }

        [Display(Name = "Durum")]
        public ZimmetDurumu Durum { get; set; } = ZimmetDurumu.Aktif;
    }
}