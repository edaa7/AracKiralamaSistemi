using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace AracKiralamaSistemi
{
    public class MainForm : Form
    {
        private readonly TabControl tabs = new TabControl();

        private readonly DataGridView dgvArac = Grid();
        private readonly TextBox txtPlaka = new TextBox();
        private readonly TextBox txtMarka = new TextBox();
        private readonly TextBox txtModel = new TextBox();
        private readonly NumericUpDown numYil = new NumericUpDown();
        private readonly TextBox txtRenk = new TextBox();
        private readonly NumericUpDown numAracUcret = MoneyBox();
        private readonly ComboBox cmbAracDurum = new ComboBox();

        private readonly DataGridView dgvMusteri = Grid();
        private readonly TextBox txtAd = new TextBox();
        private readonly TextBox txtSoyad = new TextBox();
        private readonly TextBox txtTc = new TextBox();
        private readonly TextBox txtTelefon = new TextBox();
        private readonly TextBox txtEmail = new TextBox();
        private readonly TextBox txtAdres = new TextBox();

        private readonly DataGridView dgvKiralama = Grid();
        private readonly ComboBox cmbKiralamaArac = new ComboBox();
        private readonly ComboBox cmbKiralamaMusteri = new ComboBox();
        private readonly DateTimePicker dtBaslangic = new DateTimePicker();
        private readonly DateTimePicker dtBitis = new DateTimePicker();
        private readonly NumericUpDown numKiralamaUcret = MoneyBox();

        private readonly DataGridView dgvOdeme = Grid();
        private readonly ComboBox cmbOdemeKiralama = new ComboBox();
        private readonly DateTimePicker dtOdeme = new DateTimePicker();
        private readonly NumericUpDown numOdemeTutar = MoneyBox();
        private readonly ComboBox cmbOdemeTuru = new ComboBox();
        private readonly TextBox txtAciklama = new TextBox();

        private readonly DataGridView dgvRapor = Grid();

        public MainForm()
        {
            Text = "Araç Kiralama Sistemi";
            WindowState = FormWindowState.Maximized;
            MinimumSize = new Size(1100, 700);

            tabs.Dock = DockStyle.Fill;
            Controls.Add(tabs);

            BuildAracTab();
            BuildMusteriTab();
            BuildKiralamaTab();
            BuildOdemeTab();
            BuildRaporTab();

            LoadAll();
        }

        private static DataGridView Grid()
        {
            return new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false
            };
        }

        private static NumericUpDown MoneyBox()
        {
            return new NumericUpDown { Minimum = 0, Maximum = 1000000, DecimalPlaces = 2, Increment = 50, Width = 160 };
        }

        private void BuildAracTab()
        {
            TabPage page = new TabPage("Araç İşlemleri");
            Panel form = LeftPanel();
            cmbAracDurum.Items.AddRange(new object[] { "Müsait", "Kirada", "Bakımda" });
            cmbAracDurum.SelectedIndex = 0;
            numYil.Minimum = 1990; numYil.Maximum = DateTime.Now.Year + 1; numYil.Value = DateTime.Now.Year;

            AddRow(form, "Plaka", txtPlaka, 20);
            AddRow(form, "Marka", txtMarka, 60);
            AddRow(form, "Model", txtModel, 100);
            AddRow(form, "Yıl", numYil, 140);
            AddRow(form, "Renk", txtRenk, 180);
            AddRow(form, "Günlük Ücret", numAracUcret, 220);
            AddRow(form, "Durum", cmbAracDurum, 260);
            AddButton(form, "Araç Ekle", 310, BtnAracEkle_Click);
            AddButton(form, "Araç Güncelle", 355, BtnAracGuncelle_Click);
            AddButton(form, "Araç Sil", 400, BtnAracSil_Click);
            AddButton(form, "Temizle", 445, (s, e) => ClearAracForm());

            dgvArac.SelectionChanged += DgvArac_SelectionChanged;
            page.Controls.Add(dgvArac); page.Controls.Add(form); tabs.TabPages.Add(page);
        }

        private void BuildMusteriTab()
        {
            TabPage page = new TabPage("Müşteri İşlemleri");
            Panel form = LeftPanel();
            AddRow(form, "Ad", txtAd, 20);
            AddRow(form, "Soyad", txtSoyad, 60);
            AddRow(form, "TC/Pasaport", txtTc, 100);
            AddRow(form, "Telefon", txtTelefon, 140);
            AddRow(form, "E-posta", txtEmail, 180);
            AddRow(form, "Adres", txtAdres, 220);
            AddButton(form, "Müşteri Ekle", 275, BtnMusteriEkle_Click);
            AddButton(form, "Müşteri Güncelle", 320, BtnMusteriGuncelle_Click);
            AddButton(form, "Müşteri Sil", 365, BtnMusteriSil_Click);
            AddButton(form, "Temizle", 410, (s, e) => ClearMusteriForm());
            dgvMusteri.SelectionChanged += DgvMusteri_SelectionChanged;
            page.Controls.Add(dgvMusteri); page.Controls.Add(form); tabs.TabPages.Add(page);
        }

        private void BuildKiralamaTab()
        {
            TabPage page = new TabPage("Kiralama İşlemleri");
            Panel form = LeftPanel();
            cmbKiralamaArac.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbKiralamaMusteri.DropDownStyle = ComboBoxStyle.DropDownList;
            dtBaslangic.Format = DateTimePickerFormat.Short;
            dtBitis.Format = DateTimePickerFormat.Short;
            dtBitis.Value = DateTime.Today.AddDays(1);

            AddRow(form, "Araç", cmbKiralamaArac, 20);
            AddRow(form, "Müşteri", cmbKiralamaMusteri, 60);
            AddRow(form, "Başlangıç", dtBaslangic, 100);
            AddRow(form, "Bitiş", dtBitis, 140);
            AddRow(form, "Günlük Ücret", numKiralamaUcret, 180);
            AddButton(form, "Kiralama Başlat", 230, BtnKiralamaEkle_Click);
            AddButton(form, "Kiralama Tamamla", 275, BtnKiralamaTamamla_Click);
            AddButton(form, "Listeyi Yenile", 320, (s, e) => LoadAll());
            cmbKiralamaArac.SelectedIndexChanged += CmbKiralamaArac_SelectedIndexChanged;
            page.Controls.Add(dgvKiralama); page.Controls.Add(form); tabs.TabPages.Add(page);
        }

        private void BuildOdemeTab()
        {
            TabPage page = new TabPage("Ödeme İşlemleri");
            Panel form = LeftPanel();
            cmbOdemeKiralama.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbOdemeTuru.Items.AddRange(new object[] { "Nakit", "Kart", "Havale" });
            cmbOdemeTuru.SelectedIndex = 0;
            dtOdeme.Format = DateTimePickerFormat.Short;
            AddRow(form, "Kiralama", cmbOdemeKiralama, 20);
            AddRow(form, "Ödeme Tarihi", dtOdeme, 60);
            AddRow(form, "Ödeme Tutarı", numOdemeTutar, 100);
            AddRow(form, "Ödeme Türü", cmbOdemeTuru, 140);
            AddRow(form, "Açıklama", txtAciklama, 180);
            AddButton(form, "Ödeme Kaydet", 235, BtnOdemeEkle_Click);
            AddButton(form, "Listeyi Yenile", 280, (s, e) => LoadAll());
            page.Controls.Add(dgvOdeme); page.Controls.Add(form); tabs.TabPages.Add(page);
        }

        private void BuildRaporTab()
        {
            TabPage page = new TabPage("Raporlar");
            Panel top = new Panel { Dock = DockStyle.Top, Height = 60 };
            Button btnAktif = new Button { Text = "Aktif Kiralamalar ve Kalan Süreleri Göster", Left = 20, Top = 15, Width = 280, Height = 32 };
            Button btnGeciken = new Button { Text = "Geciken Teslimleri Göster", Left = 320, Top = 15, Width = 220, Height = 32 };
            Button btnBorclar = new Button { Text = "Borç Listesini Göster", Left = 560, Top = 15, Width = 180, Height = 32 };
            btnAktif.Click += (s, e) => LoadRaporAktif();
            btnGeciken.Click += (s, e) => LoadRaporGeciken();
            btnBorclar.Click += (s, e) => LoadRaporBorclar();
            top.Controls.AddRange(new Control[] { btnAktif, btnGeciken, btnBorclar });
            page.Controls.Add(dgvRapor); page.Controls.Add(top); tabs.TabPages.Add(page);
        }

        private static Panel LeftPanel() => new Panel { Dock = DockStyle.Left, Width = 330, Padding = new Padding(10) };

        private static void AddRow(Panel panel, string label, Control input, int top)
        {
            panel.Controls.Add(new Label { Text = label, Left = 15, Top = top + 5, Width = 100 });
            input.Left = 125; input.Top = top; input.Width = 170;
            panel.Controls.Add(input);
        }

        private static void AddButton(Panel panel, string text, int top, EventHandler click)
        {
            Button btn = new Button { Text = text, Left = 125, Top = top, Width = 170, Height = 35 };
            btn.Click += click;
            panel.Controls.Add(btn);
        }

        private void LoadAll()
        {
            LoadAraclar(); LoadMusteriler(); LoadKiralamalar(); LoadOdemeler(); LoadCombos(); LoadRaporAktif();
        }

        private void LoadAraclar() => dgvArac.DataSource = DatabaseHelper.GetDataTable("SELECT * FROM Arac ORDER BY arac_id DESC");
        private void LoadMusteriler() => dgvMusteri.DataSource = DatabaseHelper.GetDataTable("SELECT * FROM Musteri ORDER BY musteri_id DESC");
        private void LoadOdemeler() => dgvOdeme.DataSource = DatabaseHelper.GetDataTable(@"SELECT o.odeme_id, o.kiralama_id, m.ad + ' ' + m.soyad AS musteri, a.plaka, o.odeme_tarihi, o.odeme_tutari, o.odeme_turu, o.aciklama FROM Odeme o INNER JOIN Kiralama k ON o.kiralama_id=k.kiralama_id INNER JOIN Musteri m ON k.musteri_id=m.musteri_id INNER JOIN Arac a ON k.arac_id=a.arac_id ORDER BY o.odeme_id DESC");
        private void LoadKiralamalar() => dgvKiralama.DataSource = DatabaseHelper.GetDataTable(@"SELECT k.kiralama_id, a.plaka, a.marka, a.model, m.ad + ' ' + m.soyad AS musteri, k.baslangic_tarihi, k.bitis_tarihi, k.kiralama_suresi_gun, k.gunluk_ucret, k.toplam_borc, k.kalan_borc, k.durum FROM Kiralama k INNER JOIN Arac a ON k.arac_id=a.arac_id INNER JOIN Musteri m ON k.musteri_id=m.musteri_id ORDER BY k.kiralama_id DESC");

        private void LoadCombos()
        {
            DataTable araclar = DatabaseHelper.GetDataTable("SELECT arac_id, plaka + ' - ' + marka + ' ' + model AS arac, gunluk_ucret FROM Arac WHERE durum='Müsait' ORDER BY plaka");
            cmbKiralamaArac.DataSource = araclar; cmbKiralamaArac.DisplayMember = "arac"; cmbKiralamaArac.ValueMember = "arac_id";

            DataTable musteriler = DatabaseHelper.GetDataTable("SELECT musteri_id, ad + ' ' + soyad AS musteri FROM Musteri ORDER BY ad, soyad");
            cmbKiralamaMusteri.DataSource = musteriler; cmbKiralamaMusteri.DisplayMember = "musteri"; cmbKiralamaMusteri.ValueMember = "musteri_id";

            DataTable kiralamalar = DatabaseHelper.GetDataTable(@"SELECT k.kiralama_id, CAST(k.kiralama_id AS VARCHAR) + ' - ' + m.ad + ' ' + m.soyad + ' / ' + a.plaka + ' / Kalan: ' + CAST(k.kalan_borc AS VARCHAR) AS bilgi FROM Kiralama k INNER JOIN Musteri m ON k.musteri_id=m.musteri_id INNER JOIN Arac a ON k.arac_id=a.arac_id WHERE k.durum='Aktif' AND k.kalan_borc > 0 ORDER BY k.kiralama_id DESC");
            cmbOdemeKiralama.DataSource = kiralamalar; cmbOdemeKiralama.DisplayMember = "bilgi"; cmbOdemeKiralama.ValueMember = "kiralama_id";
        }

        private void BtnAracEkle_Click(object sender, EventArgs e)
        {
            if (IsEmpty(txtPlaka, txtMarka, txtModel)) return;
            DatabaseHelper.ExecuteNonQuery("INSERT INTO Arac(plaka, marka, model, yil, renk, gunluk_ucret, durum) VALUES(@p,@ma,@mo,@y,@r,@u,@d)",
                new SqlParameter("@p", txtPlaka.Text.Trim()), new SqlParameter("@ma", txtMarka.Text.Trim()), new SqlParameter("@mo", txtModel.Text.Trim()),
                new SqlParameter("@y", (int)numYil.Value), new SqlParameter("@r", txtRenk.Text.Trim()), new SqlParameter("@u", numAracUcret.Value), new SqlParameter("@d", cmbAracDurum.Text));
            LoadAll(); ClearAracForm();
        }

        private void BtnAracGuncelle_Click(object sender, EventArgs e)
        {
            if (dgvArac.CurrentRow == null) return;
            int id = Convert.ToInt32(dgvArac.CurrentRow.Cells["arac_id"].Value);
            DatabaseHelper.ExecuteNonQuery("UPDATE Arac SET plaka=@p, marka=@ma, model=@mo, yil=@y, renk=@r, gunluk_ucret=@u, durum=@d WHERE arac_id=@id",
                new SqlParameter("@p", txtPlaka.Text.Trim()), new SqlParameter("@ma", txtMarka.Text.Trim()), new SqlParameter("@mo", txtModel.Text.Trim()),
                new SqlParameter("@y", (int)numYil.Value), new SqlParameter("@r", txtRenk.Text.Trim()), new SqlParameter("@u", numAracUcret.Value), new SqlParameter("@d", cmbAracDurum.Text), new SqlParameter("@id", id));
            LoadAll();
        }

        private void BtnAracSil_Click(object sender, EventArgs e)
        {
            if (dgvArac.CurrentRow == null) return;
            int id = Convert.ToInt32(dgvArac.CurrentRow.Cells["arac_id"].Value);
            if (Confirm("Seçili aracı silmek istiyor musunuz?"))
            {
                DatabaseHelper.ExecuteNonQuery("DELETE FROM Arac WHERE arac_id=@id", new SqlParameter("@id", id));
                LoadAll(); ClearAracForm();
            }
        }

        private void BtnMusteriEkle_Click(object sender, EventArgs e)
        {
            if (IsEmpty(txtAd, txtSoyad, txtTc, txtTelefon)) return;
            DatabaseHelper.ExecuteNonQuery("INSERT INTO Musteri(ad, soyad, tc_pasaport_no, telefon, email, adres) VALUES(@a,@s,@tc,@t,@e,@adrs)",
                new SqlParameter("@a", txtAd.Text.Trim()), new SqlParameter("@s", txtSoyad.Text.Trim()), new SqlParameter("@tc", txtTc.Text.Trim()), new SqlParameter("@t", txtTelefon.Text.Trim()), new SqlParameter("@e", txtEmail.Text.Trim()), new SqlParameter("@adrs", txtAdres.Text.Trim()));
            LoadAll(); ClearMusteriForm();
        }

        private void BtnMusteriGuncelle_Click(object sender, EventArgs e)
        {
            if (dgvMusteri.CurrentRow == null) return;
            int id = Convert.ToInt32(dgvMusteri.CurrentRow.Cells["musteri_id"].Value);
            DatabaseHelper.ExecuteNonQuery("UPDATE Musteri SET ad=@a, soyad=@s, tc_pasaport_no=@tc, telefon=@t, email=@e, adres=@adrs WHERE musteri_id=@id",
                new SqlParameter("@a", txtAd.Text.Trim()), new SqlParameter("@s", txtSoyad.Text.Trim()), new SqlParameter("@tc", txtTc.Text.Trim()), new SqlParameter("@t", txtTelefon.Text.Trim()), new SqlParameter("@e", txtEmail.Text.Trim()), new SqlParameter("@adrs", txtAdres.Text.Trim()), new SqlParameter("@id", id));
            LoadAll();
        }

        private void BtnMusteriSil_Click(object sender, EventArgs e)
        {
            if (dgvMusteri.CurrentRow == null) return;
            int id = Convert.ToInt32(dgvMusteri.CurrentRow.Cells["musteri_id"].Value);
            if (Confirm("Seçili müşteriyi silmek istiyor musunuz?"))
            {
                DatabaseHelper.ExecuteNonQuery("DELETE FROM Musteri WHERE musteri_id=@id", new SqlParameter("@id", id));
                LoadAll(); ClearMusteriForm();
            }
        }

        private void BtnKiralamaEkle_Click(object sender, EventArgs e)
        {
            if (cmbKiralamaArac.SelectedValue == null || cmbKiralamaMusteri.SelectedValue == null) { MessageBox.Show("Araç ve müşteri seçiniz."); return; }
            int gun = Math.Max(1, (dtBitis.Value.Date - dtBaslangic.Value.Date).Days);
            decimal toplam = gun * numKiralamaUcret.Value;
            int aracId = Convert.ToInt32(cmbKiralamaArac.SelectedValue);
            DatabaseHelper.ExecuteNonQuery(@"INSERT INTO Kiralama(arac_id, musteri_id, baslangic_tarihi, bitis_tarihi, kiralama_suresi_gun, gunluk_ucret, toplam_borc, kalan_borc, durum) VALUES(@arac,@musteri,@bas,@bit,@gun,@ucret,@toplam,@kalan,'Aktif'); UPDATE Arac SET durum='Kirada' WHERE arac_id=@arac;",
                new SqlParameter("@arac", aracId), new SqlParameter("@musteri", Convert.ToInt32(cmbKiralamaMusteri.SelectedValue)), new SqlParameter("@bas", dtBaslangic.Value.Date), new SqlParameter("@bit", dtBitis.Value.Date), new SqlParameter("@gun", gun), new SqlParameter("@ucret", numKiralamaUcret.Value), new SqlParameter("@toplam", toplam), new SqlParameter("@kalan", toplam));
            LoadAll();
        }

        private void BtnKiralamaTamamla_Click(object sender, EventArgs e)
        {
            if (dgvKiralama.CurrentRow == null) return;
            int kiralamaId = Convert.ToInt32(dgvKiralama.CurrentRow.Cells["kiralama_id"].Value);
            object aracId = DatabaseHelper.ExecuteScalar("SELECT arac_id FROM Kiralama WHERE kiralama_id=@id", new SqlParameter("@id", kiralamaId));
            DatabaseHelper.ExecuteNonQuery("UPDATE Kiralama SET durum='Tamamlandı' WHERE kiralama_id=@id; UPDATE Arac SET durum='Müsait' WHERE arac_id=@arac;", new SqlParameter("@id", kiralamaId), new SqlParameter("@arac", Convert.ToInt32(aracId)));
            LoadAll();
        }

        private void BtnOdemeEkle_Click(object sender, EventArgs e)
        {
            if (cmbOdemeKiralama.SelectedValue == null) { MessageBox.Show("Ödeme için kiralama seçiniz."); return; }
            int kiralamaId = Convert.ToInt32(cmbOdemeKiralama.SelectedValue);
            decimal tutar = numOdemeTutar.Value;
            DatabaseHelper.ExecuteNonQuery(@"INSERT INTO Odeme(kiralama_id, odeme_tarihi, odeme_tutari, odeme_turu, aciklama) VALUES(@k,@tarih,@tutar,@tur,@aciklama); UPDATE Kiralama SET kalan_borc = CASE WHEN kalan_borc - @tutar < 0 THEN 0 ELSE kalan_borc - @tutar END WHERE kiralama_id=@k;",
                new SqlParameter("@k", kiralamaId), new SqlParameter("@tarih", dtOdeme.Value.Date), new SqlParameter("@tutar", tutar), new SqlParameter("@tur", cmbOdemeTuru.Text), new SqlParameter("@aciklama", txtAciklama.Text.Trim()));
            LoadAll(); txtAciklama.Clear(); numOdemeTutar.Value = 0;
        }

        private void LoadRaporAktif()
        {
            dgvRapor.DataSource = DatabaseHelper.GetDataTable(@"SELECT k.kiralama_id, m.ad + ' ' + m.soyad AS musteri, a.plaka, a.marka, a.model, k.bitis_tarihi, DATEDIFF(DAY, CAST(GETDATE() AS DATE), k.bitis_tarihi) AS kalan_gun, k.toplam_borc, k.kalan_borc, CASE WHEN k.bitis_tarihi < CAST(GETDATE() AS DATE) THEN 'Gecikti' ELSE 'Aktif' END AS teslim_durumu FROM Kiralama k INNER JOIN Musteri m ON k.musteri_id=m.musteri_id INNER JOIN Arac a ON k.arac_id=a.arac_id WHERE k.durum='Aktif' ORDER BY k.bitis_tarihi");
        }
        private void LoadRaporGeciken() => dgvRapor.DataSource = DatabaseHelper.GetDataTable(@"SELECT * FROM (SELECT k.kiralama_id, m.ad + ' ' + m.soyad AS musteri, a.plaka, k.bitis_tarihi, DATEDIFF(DAY, k.bitis_tarihi, CAST(GETDATE() AS DATE)) AS geciken_gun, k.kalan_borc FROM Kiralama k INNER JOIN Musteri m ON k.musteri_id=m.musteri_id INNER JOIN Arac a ON k.arac_id=a.arac_id WHERE k.durum='Aktif') x WHERE geciken_gun > 0 ORDER BY geciken_gun DESC");
        private void LoadRaporBorclar() => dgvRapor.DataSource = DatabaseHelper.GetDataTable(@"SELECT m.ad + ' ' + m.soyad AS musteri, a.plaka, k.toplam_borc, k.kalan_borc, k.durum FROM Kiralama k INNER JOIN Musteri m ON k.musteri_id=m.musteri_id INNER JOIN Arac a ON k.arac_id=a.arac_id WHERE k.kalan_borc > 0 ORDER BY k.kalan_borc DESC");

        private void CmbKiralamaArac_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbKiralamaArac.SelectedItem is DataRowView row && row["gunluk_ucret"] != DBNull.Value)
                numKiralamaUcret.Value = Convert.ToDecimal(row["gunluk_ucret"]);
        }

        private void DgvArac_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvArac.CurrentRow == null) return;
            txtPlaka.Text = Convert.ToString(dgvArac.CurrentRow.Cells["plaka"].Value);
            txtMarka.Text = Convert.ToString(dgvArac.CurrentRow.Cells["marka"].Value);
            txtModel.Text = Convert.ToString(dgvArac.CurrentRow.Cells["model"].Value);
            numYil.Value = Convert.ToDecimal(dgvArac.CurrentRow.Cells["yil"].Value);
            txtRenk.Text = Convert.ToString(dgvArac.CurrentRow.Cells["renk"].Value);
            numAracUcret.Value = Convert.ToDecimal(dgvArac.CurrentRow.Cells["gunluk_ucret"].Value);
            cmbAracDurum.Text = Convert.ToString(dgvArac.CurrentRow.Cells["durum"].Value);
        }

        private void DgvMusteri_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMusteri.CurrentRow == null) return;
            txtAd.Text = Convert.ToString(dgvMusteri.CurrentRow.Cells["ad"].Value);
            txtSoyad.Text = Convert.ToString(dgvMusteri.CurrentRow.Cells["soyad"].Value);
            txtTc.Text = Convert.ToString(dgvMusteri.CurrentRow.Cells["tc_pasaport_no"].Value);
            txtTelefon.Text = Convert.ToString(dgvMusteri.CurrentRow.Cells["telefon"].Value);
            txtEmail.Text = Convert.ToString(dgvMusteri.CurrentRow.Cells["email"].Value);
            txtAdres.Text = Convert.ToString(dgvMusteri.CurrentRow.Cells["adres"].Value);
        }

        private static bool IsEmpty(params TextBox[] boxes)
        {
            foreach (TextBox box in boxes)
                if (string.IsNullOrWhiteSpace(box.Text)) { MessageBox.Show("Zorunlu alanları doldurunuz."); return true; }
            return false;
        }

        private static bool Confirm(string message) => MessageBox.Show(message, "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

        private void ClearAracForm() { txtPlaka.Clear(); txtMarka.Clear(); txtModel.Clear(); txtRenk.Clear(); numYil.Value = DateTime.Now.Year; numAracUcret.Value = 0; cmbAracDurum.SelectedIndex = 0; }
        private void ClearMusteriForm() { txtAd.Clear(); txtSoyad.Clear(); txtTc.Clear(); txtTelefon.Clear(); txtEmail.Clear(); txtAdres.Clear(); }
    }
}
