using Microsoft.EntityFrameworkCore;

namespace DemirbasTakip.ViewModels
{
    public class SayfalanmisListe<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int ToplamKayit { get; set; }
        public int Toplamkayit
        {
            get => ToplamKayit;
            set => ToplamKayit = value;
        }

        // Farklı yazım ihtimallerine karşı tüm varyasyonlar
        public int Mevcutsayfa
        {
            get => CurrentPage;
            set => CurrentPage = value;
        }
        public int MevcutSayfa
        {
            get => CurrentPage;
            set => CurrentPage = value;
        }

        public int Toplamsayfa
        {
            get => TotalPages;
            set => TotalPages = value;
        }
        public int ToplamSayfa
        {
            get => TotalPages;
            set => TotalPages = value;
        }

        public List<T> Kayitlar
        {
            get => this;
            set
            {
                Clear();
                if (value != null) AddRange(value);
            }
        }

        public List<T> Items
        {
            get => this;
            set
            {
                Clear();
                if (value != null) AddRange(value);
            }
        }

        public SayfalanmisListe()
        {
        }

        public SayfalanmisListe(List<T> items, int count, int pageIndex, int pageSize)
        {
            CurrentPage = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            ToplamKayit = count;
            if (items != null)
            {
                AddRange(items);
            }
        }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

        public static async Task<SayfalanmisListe<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new SayfalanmisListe<T>(items, count, pageIndex, pageSize);
        }
    }
}