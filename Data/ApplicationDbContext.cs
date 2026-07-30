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
        public DbSet<Demirbas> Demirbaslar => Set<Demirbas>();
        public DbSet<Zimmet> Zimmetler => Set<Zimmet>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Kullanici>()
                .HasOne(k => k.Personel)
                .WithMany()
                .HasForeignKey(k => k.PersonelId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Zimmet>()
                .HasOne(z => z.Personel)
                .WithMany(p => p.Zimmetler)
                .HasForeignKey(z => z.PersonelId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Zimmet>()
                .HasOne(z => z.Demirbas)
                .WithMany(d => d.Zimmetler)
                .HasForeignKey(z => z.DemirbasId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Kullanici>().Property(k => k.Rol).HasConversion<string>();
            modelBuilder.Entity<Demirbas>().Property(d => d.Kategori).HasConversion<string>();
            modelBuilder.Entity<Zimmet>().Property(z => z.Durum).HasConversion<string>();

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