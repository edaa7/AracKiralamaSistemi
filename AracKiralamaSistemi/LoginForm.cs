using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace AracKiralamaSistemi
{
    public class LoginForm : Form
    {
        private readonly TextBox txtKullaniciAdi = new TextBox();
        private readonly TextBox txtSifre = new TextBox();

        public LoginForm()
        {
            Text = "Araç Kiralama Sistemi - Giriş";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(420, 280);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            Label lblTitle = new Label
            {
                Text = "Araç Kiralama Sistemi",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(80, 25)
            };

            Label lblUser = new Label { Text = "Kullanıcı Adı", Location = new Point(55, 90), AutoSize = true };
            txtKullaniciAdi.Location = new Point(160, 86);
            txtKullaniciAdi.Width = 170;
            txtKullaniciAdi.Text = "admin";

            Label lblPass = new Label { Text = "Şifre", Location = new Point(55, 130), AutoSize = true };
            txtSifre.Location = new Point(160, 126);
            txtSifre.Width = 170;
            txtSifre.PasswordChar = '*';
            txtSifre.Text = "1234";

            Button btnGiris = new Button { Text = "Giriş Yap", Location = new Point(160, 170), Width = 170, Height = 35 };
            btnGiris.Click += BtnGiris_Click;
            AcceptButton = btnGiris;

            Controls.AddRange(new Control[] { lblTitle, lblUser, txtKullaniciAdi, lblPass, txtSifre, btnGiris });
        }

        private void BtnGiris_Click(object sender, EventArgs e)
        {
            string sql = "SELECT COUNT(*) FROM Yonetici WHERE kullanici_adi=@k AND sifre=@s";
            object result = DatabaseHelper.ExecuteScalar(sql,
                new SqlParameter("@k", txtKullaniciAdi.Text.Trim()),
                new SqlParameter("@s", txtSifre.Text.Trim()));

            if (Convert.ToInt32(result) > 0)
            {
                Hide();
                MainForm main = new MainForm();
                main.FormClosed += (s, args) => Close();
                main.Show();
            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya şifre hatalı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
