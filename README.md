🔐 IdentityApp

IdentityApp, modern kimlik yönetimi için geliştirilmiş, güvenli ve esnek bir .NET tabanlı kimlik doğrulama ve yetkilendirme uygulamasıdır.
Proje; ASP.NET Core Identity, JWT Authentication, Policy & Claim Based Authorization ve sosyal giriş (Google, Facebook) entegrasyonlarını bir araya getirerek profesyonel düzeyde kullanıcı yönetimi sunar.

🧠 Kazanımlar / Proje Amacı

Bu proje ile amaç:

ASP.NET Core Identity altyapısının derinlemesine öğrenilmesi,

Farklı authentication stratejilerinin (JWT, OAuth, Cookie, Claims) uygulanması,

Gerçek senaryolarda kullanılabilecek güvenli kimlik doğrulama sisteminin oluşturulmasıdır.

🚀 Öne Çıkan Özellikler

✅ Kullanıcı Kayıt, Giriş ve Oturum Yönetimi

🔑 “Şifremi Unuttum” E-posta Doğrulama Sistemi – Kullanıcı e-posta adresine gelen bağlantı üzerinden şifresini yenileyebilir.

🌍 Facebook ve Google ile Sosyal Giriş (OAuth2)

🧩 Policy-Based ve Claim-Based Authentication – Yetki kontrolleri, kullanıcı rollerinin ötesine geçerek özel kurallara ve claim’lere göre yönetilir.

🧑‍💻 Kullanıcı Bilgilerini Güncelleme Özelliği – Kullanıcı profil bilgilerini düzenleyebilir.

🔒 JWT (JSON Web Token) ile Güvenli Kimlik Doğrulama

⚙️ ASP.NET Core Identity Entegrasyonu

📬 SMTP veya MailKit ile E-posta Gönderimi

🧱 Katmanlı Mimari Yapı (Clean Architecture Yaklaşımı)

🧾 Asenkron (async/await) destekli EF Core işlemleri


🧱 Kullanılan Teknolojiler
Katman / Bileşen	Teknoloji
Backend	.NET 8, ASP.NET Core Identity, Entity Framework Core
Authentication	JWT, Cookie Auth, Facebook & Google OAuth2
Authorization	Policy-Based, Claim-Based, Role-Based
Database	SQL Server
Mail	SMTP / MailKit
Mimari	Clean Architecture, Dependency Injection, Asynchronous Programming

📁 Proje Yapısı (Örnek)
IdentityApp/
├── IdentityApp.API/           # API katmanı (Controllers, Auth endpoints)
├── IdentityApp.Application/   # Servisler, Policy tanımları, handler’lar
├── IdentityApp.Domain/        # Entity’ler, Identity modelleri, Claims
├── IdentityApp.Infrastructure/# Mail servisleri, veri erişimi, config
└── IdentityApp.Persistence/   # DbContext, migration, seed verileri

📧 Şifre Yenileme Akışı

Kullanıcı “Şifremi Unuttum” seçeneğini seçer.

Sistem, kullanıcının e-posta adresine tek kullanımlık bir doğrulama bağlantısı (token) gönderir.

Kullanıcı bu bağlantı aracılığıyla yeni şifresini belirler.

Şifre güncellenir ve e-posta doğrulaması tamamlanır.

🌐 Sosyal Girişler

Uygulama, Google ve Facebook OAuth2 servisleriyle entegredir.
Kullanıcılar bu hesaplarıyla güvenli şekilde oturum açabilir.

builder.Services.AddAuthentication()
    .AddGoogle(options => { ... })
    .AddFacebook(options => { ... });

🔑 Policy ve Claim Bazlı Yetkilendirme

Rol tabanlı yetkilendirmeye ek olarak özel erişim kuralları tanımlanabilir:

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("CanEditProfile", policy => policy.RequireClaim("Permission", "EditProfile"));
});


Bu sayede kullanıcılar claim’lerine veya özel politikalara göre yetkilendirilir.

🧾 Örnek API Endpoint’leri
Metot	Endpoint	Açıklama
POST	/api/auth/signup	Yeni kullanıcı kaydı
POST	/api/auth/signin	Token tabanlı giriş
POST	/api/auth/forgot-password	Şifre sıfırlama e-postası gönderme
POST	/api/auth/reset-password	Yeni şifre belirleme
GET	/api/profile	Kullanıcı bilgilerini getir
PUT	/api/profile/edituser	Kullanıcı bilgilerini güncelle

💬 Sonuç

IdentityApp, modern kimlik yönetimi için gereken tüm bileşenleri bir araya getiren güçlü bir örnek projedir.
E-posta doğrulama, sosyal girişler, JWT güvenliği ve claim bazlı yetkilendirme gibi konularda .NET geliştiricilerine kurumsal düzeyde bir temel sunar.


⚙️ Kurulum ve Çalıştırma

Projeyi klonlayın:

git clone https://github.com/omeruren/IdentityApp.git
cd IdentityApp


Bağımlılıkları yükleyin:

dotnet restore


Veritabanını oluşturun:

dotnet ef database update


Projeyi çalıştırın:

dotnet run --project IdentityApp.API

👨‍💻 Geliştirici

Ömer Üren
🔗 GitHub Profilim
