# Online-Movie-Archive

Proje youtube videosu: https://youtu.be/ecbBZ18X8QM


# 🎬 Movie Archive and Rating System

Bu proje, kullanıcıların filmleri görüntüleyebildiği, yorum yapabildiği ve puan verebildiği bir ASP.NET Core MVC uygulamasıdır.

## 🚀 Özellikler

- 🎥 Film listesi görüntüleme
- ⭐ Filme puan verme
- 💬 Yorum yapma
- 👤 Kullanıcı girişi ve roller
- 🛡️ Admin paneli ile içerik yönetimi

Kullanılan Teknolojiler
ASP.NET Core MVC
Entity Framework Core
Identity
PostgreSQL / SQL Server
Bootstrap 5

![image](https://github.com/user-attachments/assets/9be045ef-cd26-4710-9147-6f834405943e)
FilmsController sınıfı Controller, IYorumlanabilir ve IPuanlanabilir sınıflarından kalıtım aldı
![image](https://github.com/user-attachments/assets/08b18096-621a-4154-9302-d402ada2d176)
Filmlerin listesini gösteren Index metodu ve Her bir filmin detayını gösteren Details metodu authorize edilmedi.
![image](https://github.com/user-attachments/assets/96a06be4-a6a4-4408-97fe-756eff604d25)
Puan ve yorum eklemeyi sağlayan IPuanlanabilir ve IYorumlanabilir classlarını implement eden metodlar sadece “Uye” rolünün ulaşabileceği şekilde authorize edildi.
![image](https://github.com/user-attachments/assets/642d3bd5-2594-4be7-8a75-f17905d8b31c)
Create, Edit ve Delete metodları sadece “Admin” rolünün erişebileceği şekilde authorize edildi.

![Screenshot 2025-06-12 204205](https://github.com/user-attachments/assets/596b8139-bfc8-4460-b3f1-f2d90b6683ef)
Kullanıcı sınıfı IdentityUser sınıfını kalıtımla alıyor.
![Screenshot 2025-06-12 204329](https://github.com/user-attachments/assets/bd7c1a14-12c8-4845-b1d9-c8de81b77fd0)
![Screenshot 2025-06-12 204402](https://github.com/user-attachments/assets/5b34d1b2-9517-499f-941a-a6c5f37f03cf)
Kullanici ve Admin sınıfları Kullanici sınıfını kalıtım aldı.
![Screenshot 2025-06-12 204516](https://github.com/user-attachments/assets/16017502-f5e1-443c-9bfe-f1bf2121f9b0)
Tablosu oluşturulacak entity class’larımız Models klasöründe tutuldu. Gerekli Servicelerimiz ise (Role Based Authentication sağlayan DataSeeder gibi.)Service klasöründe tutuldu.
