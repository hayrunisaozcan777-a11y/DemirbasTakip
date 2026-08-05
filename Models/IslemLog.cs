using System.ComponentModel.DataAnnotations;

namespace DemirbasTakip.Models
{
    public class IslemLog
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string KullaniciAdi { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Islem { get; set; } = string.Empty;

        [StringLength(250)]
        public string? Detay { get; set; }

        public DateTime Tarih { get; set; } = DateTime.Now;
    }
}