CREATE DATABASE AracKiralamaDB;
GO
USE AracKiralamaDB;
GO

CREATE TABLE Arac (
    arac_id INT IDENTITY(1,1) PRIMARY KEY,
    plaka VARCHAR(20) NOT NULL UNIQUE,
    marka NVARCHAR(50) NOT NULL,
    model NVARCHAR(50) NOT NULL,
    yil INT NOT NULL,
    renk NVARCHAR(30),
    gunluk_ucret DECIMAL(10,2) NOT NULL,
    durum VARCHAR(20) NOT NULL DEFAULT 'Müsait'
);

CREATE TABLE Musteri (
    musteri_id INT IDENTITY(1,1) PRIMARY KEY,
    ad NVARCHAR(50) NOT NULL,
    soyad NVARCHAR(50) NOT NULL,
    tc_pasaport_no VARCHAR(20) NOT NULL,
    telefon VARCHAR(20) NOT NULL,
    email NVARCHAR(100),
    adres NVARCHAR(250)
);

CREATE TABLE Kiralama (
    kiralama_id INT IDENTITY(1,1) PRIMARY KEY,
    arac_id INT NOT NULL FOREIGN KEY REFERENCES Arac(arac_id),
    musteri_id INT NOT NULL FOREIGN KEY REFERENCES Musteri(musteri_id),
    baslangic_tarihi DATE NOT NULL,
    bitis_tarihi DATE NOT NULL,
    kiralama_suresi_gun INT NOT NULL,
    gunluk_ucret DECIMAL(10,2) NOT NULL,
    toplam_borc DECIMAL(10,2) NOT NULL,
    kalan_borc DECIMAL(10,2) NOT NULL,
    durum VARCHAR(20) NOT NULL DEFAULT 'Aktif'
);

CREATE TABLE Odeme (
    odeme_id INT IDENTITY(1,1) PRIMARY KEY,
    kiralama_id INT NOT NULL FOREIGN KEY REFERENCES Kiralama(kiralama_id),
    odeme_tarihi DATE NOT NULL,
    odeme_tutari DECIMAL(10,2) NOT NULL,
    odeme_turu VARCHAR(20) NOT NULL,
    aciklama NVARCHAR(250)
);

CREATE TABLE Yonetici (
    yonetici_id INT IDENTITY(1,1) PRIMARY KEY,
    kullanici_adi VARCHAR(50) NOT NULL UNIQUE,
    sifre VARCHAR(100) NOT NULL,
    ad_soyad NVARCHAR(100) NOT NULL
);

INSERT INTO Yonetici(kullanici_adi, sifre, ad_soyad)
VALUES('admin', '1234', N'Sistem Yöneticisi');
GO
