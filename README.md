# Araç Kiralama Sistemi

Bu proje, araç kiralama işlemlerini bilgisayar ortamında takip etmek için hazırlanmıştır. Araç kiralama yapan bir işletmede araç, müşteri, kiralama ve ödeme bilgilerinin daha düzenli tutulması amaçlanmıştır.

# Projenin Amacı

Araç kiralama işlemleri bazen defter, Excel veya sözlü takip ile yapılabilmektedir. Bu durum araçların karışmasına, ödeme bilgilerinin unutulmasına veya teslim tarihlerinin takip edilememesine neden olabilir.

Bu projede amaç; araçların durumunu görmek, müşteri bilgilerini kaydetmek, kiralama süresini takip etmek ve ödeme bilgilerini düzenli şekilde tutmaktır.

# Projede Yapılabilen İşlemler
Araç ekleme
Araç silme
Müşteri ekleme
Kiralama işlemi oluşturma
Kiralama süresi ve borç bilgisini görüntüleme
Ödeme bilgisi ekleme
Aktif kiralamaları takip etme
Kalan borç ve kalan süre bilgisini görme

# Kullanılan Teknolojiler
C#
Windows Forms
SQL Server LocalDB
Visual Studio 2022

# Programın Kullanımı

Program çalıştırıldığında ilk olarak giriş ekranı açılır.

Giriş bilgileri:

Kullanıcı adı: admin
Şifre: 1234

Giriş yapıldıktan sonra ana ekrandan araç, müşteri, kiralama ve ödeme işlemleri yapılabilir.

# Proje Dosyaları

Projede bulunan temel dosyalar şunlardır:

Program.cs: Programın başladığı dosyadır.
LoginForm.cs: Giriş ekranı kodlarını içerir.
MainForm.cs: Ana ekran ve işlemlerin yapıldığı dosyadır.
DatabaseHelper.cs: Veritabanı bağlantısı ve tablo işlemleri bu dosyada yer alır.
AracKiralamaSistemi.sln: Visual Studio proje dosyasıdır.
database/create_database.sql: Veritabanı tabloları için SQL dosyasıdır.

# Veritabanı Yapısı

Projede temel olarak şu tablolar kullanılmıştır:

Araç tablosu
Müşteri tablosu
Kiralama tablosu
Ödeme tablosu
Yönetici tablosu

Bu tablolar sayesinde araç, müşteri, kiralama ve ödeme bilgileri kayıt altında tutulmaktadır.

# Ekran Görüntüleri

Proje ekran görüntüleri screenshots klasörü içinde yer almaktadır.

# Projeyi Çalıştırma
Proje klasörü bilgisayara indirilir.
AracKiralamaSistemi.sln dosyası Visual Studio 2022 ile açılır.
Üst menüden Start butonuna basılır veya F5 tuşu kullanılır.
Giriş ekranında kullanıcı adı ve şifre girilir.
Ana ekrandan araç kiralama işlemleri yapılabilir.

<img width="1100" height="650" alt="arac_islemleri" src="https://github.com/user-attachments/assets/8b89fef6-b143-44b4-8e27-5b7e53a04b9d" />

<img width="1100" height="650" alt="kiralama_islemleri" src="https://github.com/user-attachments/assets/0a765d48-7045-4c9d-81f0-0c7c3c1927e5" />

<img width="1100" height="650" alt="login" src="https://github.com/user-attachments/assets/336d2440-1a03-4868-a20c-4ba273604e5e" />

<img width="1100" height="650" alt="raporlar" src="https://github.com/user-attachments/assets/13c0c4c4-3141-4651-a752-5688d60fb823" />

# Hazırlayan

Edanur Çelimli
2024212039
Yönetim Bilişim Sistemleri 2. Sınıf
