using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace AracKiralamaSistemi
{
    public static class DatabaseHelper
    {
        public static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["AracKiralamaDb"].ConnectionString;

        public static void InitializeDatabase()
        {
            CreateDatabaseIfNotExists();
            ExecuteNonQuery(GetTableScript());
            SeedDefaultAdmin();
        }

        private static void CreateDatabaseIfNotExists()
        {
            var builder = new SqlConnectionStringBuilder(ConnectionString);
            string dbName = builder.InitialCatalog;
            builder.InitialCatalog = "master";

            using (SqlConnection con = new SqlConnection(builder.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("IF DB_ID(@db) IS NULL EXEC('CREATE DATABASE [' + @db + ']')", con))
            {
                cmd.Parameters.AddWithValue("@db", dbName);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private static string GetTableScript()
        {
            return @"
IF OBJECT_ID('dbo.Odeme', 'U') IS NULL
CREATE TABLE dbo.Odeme (
    odeme_id INT IDENTITY(1,1) PRIMARY KEY,
    kiralama_id INT NOT NULL,
    odeme_tarihi DATE NOT NULL,
    odeme_tutari DECIMAL(10,2) NOT NULL,
    odeme_turu VARCHAR(20) NOT NULL,
    aciklama NVARCHAR(250) NULL
);

IF OBJECT_ID('dbo.Kiralama', 'U') IS NULL
CREATE TABLE dbo.Kiralama (
    kiralama_id INT IDENTITY(1,1) PRIMARY KEY,
    arac_id INT NOT NULL,
    musteri_id INT NOT NULL,
    baslangic_tarihi DATE NOT NULL,
    bitis_tarihi DATE NOT NULL,
    kiralama_suresi_gun INT NOT NULL,
    gunluk_ucret DECIMAL(10,2) NOT NULL,
    toplam_borc DECIMAL(10,2) NOT NULL,
    kalan_borc DECIMAL(10,2) NOT NULL,
    durum VARCHAR(20) NOT NULL DEFAULT 'Aktif'
);

IF OBJECT_ID('dbo.Arac', 'U') IS NULL
CREATE TABLE dbo.Arac (
    arac_id INT IDENTITY(1,1) PRIMARY KEY,
    plaka VARCHAR(20) NOT NULL UNIQUE,
    marka NVARCHAR(50) NOT NULL,
    model NVARCHAR(50) NOT NULL,
    yil INT NOT NULL,
    renk NVARCHAR(30) NULL,
    gunluk_ucret DECIMAL(10,2) NOT NULL,
    durum VARCHAR(20) NOT NULL DEFAULT 'Müsait'
);

IF OBJECT_ID('dbo.Musteri', 'U') IS NULL
CREATE TABLE dbo.Musteri (
    musteri_id INT IDENTITY(1,1) PRIMARY KEY,
    ad NVARCHAR(50) NOT NULL,
    soyad NVARCHAR(50) NOT NULL,
    tc_pasaport_no VARCHAR(20) NOT NULL,
    telefon VARCHAR(20) NOT NULL,
    email NVARCHAR(100) NULL,
    adres NVARCHAR(250) NULL
);

IF OBJECT_ID('dbo.Yonetici', 'U') IS NULL
CREATE TABLE dbo.Yonetici (
    yonetici_id INT IDENTITY(1,1) PRIMARY KEY,
    kullanici_adi VARCHAR(50) NOT NULL UNIQUE,
    sifre VARCHAR(100) NOT NULL,
    ad_soyad NVARCHAR(100) NOT NULL
);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Kiralama_Arac')
ALTER TABLE dbo.Kiralama ADD CONSTRAINT FK_Kiralama_Arac FOREIGN KEY (arac_id) REFERENCES dbo.Arac(arac_id);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Kiralama_Musteri')
ALTER TABLE dbo.Kiralama ADD CONSTRAINT FK_Kiralama_Musteri FOREIGN KEY (musteri_id) REFERENCES dbo.Musteri(musteri_id);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Odeme_Kiralama')
ALTER TABLE dbo.Odeme ADD CONSTRAINT FK_Odeme_Kiralama FOREIGN KEY (kiralama_id) REFERENCES dbo.Kiralama(kiralama_id);
";
        }

        private static void SeedDefaultAdmin()
        {
            string sql = @"
IF NOT EXISTS (SELECT 1 FROM Yonetici WHERE kullanici_adi='admin')
INSERT INTO Yonetici(kullanici_adi, sifre, ad_soyad) VALUES('admin', '1234', N'Sistem Yöneticisi');";
            ExecuteNonQuery(sql);
        }

        public static DataTable GetDataTable(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public static int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                con.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public static object ExecuteScalar(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                con.Open();
                return cmd.ExecuteScalar();
            }
        }
    }
}
