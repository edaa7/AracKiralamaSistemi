using System;
using System.Windows.Forms;

namespace AracKiralamaSistemi
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                DatabaseHelper.InitializeDatabase();
                Application.Run(new LoginForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Program başlatılırken hata oluştu:\n" + ex.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
