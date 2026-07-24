using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DemirbasTakip.ViewModels
{
    public class RezervasyonFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Personel seçimi zorunludur.")]
        [Display(Name = "Personel")]
        public int PersonelId { get; set; }

        [Required(ErrorMessage = "Kaynak seçimi zorunludur.")]
        [Display(Name = "Kaynak")]
        public int KaynakId { get; set; }

        [Required(ErrorMessage = "Başlangıç zamanı zorunludur.")]
        [Display(Name = "Başlangıç Zamanı")]
        [DataType(DataType.DateTime)]
        public DateTime BaslangicZamani { get; set; } = DateTime.Now.AddMinutes(30);

        [Required(ErrorMessage = "Bitiş zamanı zorunludur.")]
        [Display(Name = "Bitiş Zamanı")]
        [DataType(DataType.DateTime)]
        public DateTime BitisZamani { get; set; } = DateTime.Now.AddHours(1);

        [StringLength(250)]
        [Display(Name = "Açıklama")]
        public string? Aciklama { get; set; }

        public List<SelectListItem> PersonelListesi { get; set; } = new();
        public List<SelectListItem> KaynakListesi { get; set; } = new();
    }
}