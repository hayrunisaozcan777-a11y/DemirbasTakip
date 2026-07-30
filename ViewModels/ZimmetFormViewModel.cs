using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DemirbasTakip.ViewModels
{
    public class ZimmetFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Personel seçimi zorunludur.")]
        [Display(Name = "Personel")]
        public int PersonelId { get; set; }

        [Required(ErrorMessage = "Demirbaş seçimi zorunludur.")]
        [Display(Name = "Demirbaş")]
        public int DemirbasId { get; set; }

        [Required(ErrorMessage = "Zimmet tarihi zorunludur.")]
        [Display(Name = "Zimmet Tarihi")]
        public DateTime ZimmetTarihi { get; set; } = DateTime.Now;

        [StringLength(250)]
        [Display(Name = "Not")]
        public string? Not { get; set; }

        public List<SelectListItem> PersonelListesi { get; set; } = new();
        public List<SelectListItem> DemirbasListesi { get; set; } = new();
    }
}