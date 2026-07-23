using Microsoft.EntityFrameworkCore;
using DemirbasTakip.Models;

namespace DemirbasTakip.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Kullanici> Kullanicilar => Set<Kullanici>();
        public DbSet<Personel> Personeller => Set<Personel>();
        public DbSet<Kaynak> Kaynaklar => Set<Kaynak>();
        public DbSet<Rezervasyon> Rezervasyonlar => Set<Rezervasyon>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Kullanici>()
                .HasOne(k => k.Personel)
                .WithMany()
                .HasForeignKey(k => k.PersonelId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Rezervasyon>()
                .HasOne(r => r.Personel)
                .WithMany(p => p.Rezervasyonlar)
                .HasForeignKey(r => r.PersonelId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rezervasyon>()
                .HasOne(r => r.Kaynak)
                .WithMany(k => k.Rezervasyonlar)
                .HasForeignKey(r => r.KaynakId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Kullanici>().Property(k => k.Rol).HasConversion<string>();
            modelBuilder.Entity<Kaynak>().Property(k => k.Tur).HasConversion<string>();
            modelBuilder.Entity<Rezervasyon>().Property(r => r.Durum).HasConversion<string>();

            // Seed - varsayılan admin kullanıcısı: admin / Admin123!
            modelBuilder.Entity<Kullanici>().HasData(new Kullanici
            {
                Id = 1,
                KullaniciAdi = "admin",
                SifreHash = "3eb3fe66b31e3b4d10fa70b5cad49c7112294af6ae4e476a1c405155d45aa121",
                Rol = KullaniciRol.Admin,
                AktifMi = true
            });
        }
    }
}