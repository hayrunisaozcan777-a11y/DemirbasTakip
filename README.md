# Demirbaş Takip Sistemi (Rezervasyon Sistemi)

Şirket içi toplantı odası ve ekipman rezervasyon takip sistemi. ASP.NET Core MVC ile geliştirilmiştir.

## Özellikler

- **Session tabanlı giriş/çıkış** sistemi
- **Personel Yönetimi** (CRUD)
- **Kaynak Yönetimi** (Toplantı Odası / Ekipman) (CRUD)
- **Rezervasyon İşlemleri** (çakışma kontrolü ile)
- **Dashboard** (özet istatistikler)
- **Raporlar** (filtrelenebilir rezervasyon geçmişi)

## Kullanılan Teknolojiler

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core
- SQL Server (LocalDB)
- Bootstrap 5

## İş Kuralları

1. Aynı kaynak için çakışan zaman aralığında aktif rezervasyon oluşturulamaz.
2. İptal edilmeden aynı kaynağa aynı saat aralığında yeni rezervasyon yapılamaz.
3. Aktif rezervasyonu bulunan personel veya kaynak silinemez.

## Kurulum

1. Depoyu klonlayın.
2. `appsettings.json` dosyasındaki connection string'i kendi ortamınıza göre düzenleyin.
3. Package Manager Console'da `Update-Database` komutunu çalıştırın.
4. Projeyi çalıştırın (`Ctrl+F5`).

## Varsayılan Giriş Bilgileri

- Kullanıcı Adı: `admin`
- Şifre: `Admin123!`

## Veritabanı Tabloları

- Personel
- Kaynak
- Rezervasyon
- Kullanici