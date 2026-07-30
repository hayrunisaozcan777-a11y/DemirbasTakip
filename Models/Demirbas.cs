using System.ComponentModel.DataAnnotations;

namespace DemirbasTakip.Models
{
    public enum DemirbasTuru
    {
        [Display(Name = "Bilgisayar")]
        Bilgisayar = 1,

        [Display(Name = "Telefon")]
        Telefon = 2,

        [Display(Name = "Mobilya")]
        Mobilya = 3,

        [Display(Name = "Diğer")]
        Diger = 4
    }

    public class Demirbas
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Demirbaş kodu zorunludur.")]
        [StringLength(50)]
        [Display(Name = "Demirbaş Kodu")]
        public string DemirbasKodu { get; set; } = string.Empty;

        [Required(ErrorMessage = "Demirbaş adı zorunludur.")]
        [StringLength(100)]
        [Display(Name = "Ad")]
        public string Ad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kategori zorunludur.")]
        [Display(Name = "Kategori")]
        public DemirbasTuru Kategori { get; set; }

        [StringLength(50)]
        [Display(Name = "Seri No")]
        public string? SeriNo { get; set; }

        [Display(Name = "Aktif mi?")]
        public bool AktifMi { get; set; } = true;

        public ICollection<Zimmet> Zimmetler { get; set; } = new List<Zimmet>();
    }
}