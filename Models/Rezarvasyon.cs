using System.ComponentModel.DataAnnotations;

namespace DemirbasTakip.Models
{
    public enum RezervasyonDurumu
    {
        Aktif = 1,
        IptalEdildi = 2
    }

    public class Rezervasyon : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Personel seçimi zorunludur.")]
        [Display(Name = "Personel")]
        public int PersonelId { get; set; }
        public Personel? Personel { get; set; }

        [Required(ErrorMessage = "Kaynak seçimi zorunludur.")]
        [Display(Name = "Kaynak")]
        public int KaynakId { get; set; }
        public Kaynak? Kaynak { get; set; }

        [Required(ErrorMessage = "Başlangıç zamanı zorunludur.")]
        [Display(Name = "Başlangıç Zamanı")]
        public DateTime BaslangicZamani { get; set; }

        [Required(ErrorMessage = "Bitiş zamanı zorunludur.")]
        [Display(Name = "Bitiş Zamanı")]
        public DateTime BitisZamani { get; set; }

        [StringLength(250)]
        [Display(Name = "Açıklama")]
        public string? Aciklama { get; set; }

        [Display(Name = "Durum")]
        public RezervasyonDurumu Durum { get; set; } = RezervasyonDurumu.Aktif;

        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (BitisZamani <= BaslangicZamani)
            {
                yield return new ValidationResult(
                    "Bitiş zamanı, başlangıç zamanından sonra olmalıdır.",
                    new[] { nameof(BitisZamani) });
            }

            if (BaslangicZamani < DateTime.Now.AddMinutes(-1))
            {
                yield return new ValidationResult(
                    "Geçmiş bir tarihe rezervasyon oluşturulamaz.",
                    new[] { nameof(BaslangicZamani) });
            }
        }
    }
}