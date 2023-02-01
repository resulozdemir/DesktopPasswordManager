using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Security.Cryptography;

namespace sifreKayitUygulamasi
{
    public partial class girisPaneli : Form
    {
        bool surukle;
        Point start_point = new Point(0, 0);
        int sayac = 0;
        OleDbConnection conn;
        OleDbDataAdapter da;
        OleDbCommand cmd;
        DataSet ds;

        public girisPaneli()
        {
            InitializeComponent();
        }

        private void girisPanel_MouseUp(object sender, MouseEventArgs e)
        {
            surukle = false;
        }

        private void girisPanel_MouseDown(object sender, MouseEventArgs e)
        {
            surukle = true;
            start_point = new Point(e.X, e.Y);
        }

        private void girisPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (surukle)
            {
                Point p = PointToScreen(e.Location);
                this.Location = new Point(p.X - start_point.X, p.Y - start_point.Y);
            }
        }

        private void sifreyiGosterPictureBox_Click(object sender, EventArgs e)
        {
            sayac++;
            if (sayac % 2 == 0)
            {
                girisPaneliSifreTextBox.UseSystemPasswordChar = true;
            }
            else
            {
                girisPaneliSifreTextBox.UseSystemPasswordChar = false;
            }
        }

        private void girisPanelKapat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void girisPanelKucult_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void sifremiUnuttum_Click(object sender, EventArgs e)
        {
            sifreSifirla git = new sifreSifirla();
            git.veri = girisPaneliKullaniciAdiTextBox.Text;
            git.Show();
            this.Hide();

        }

        private void kayitOlButon_Click(object sender, EventArgs e)
        {
            kayitOl git = new kayitOl();
            git.veri = girisPaneliKullaniciAdiTextBox.Text;
            git.Show();
            this.Hide();
        }

        private void girisButonu_Click(object sender, EventArgs e)
        {

            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Database.accdb";
            OleDbConnection connection = new OleDbConnection(connectionString);
            connection.Open();
            string sql = "SELECT kullaniciAdi, parola FROM kullaniciBilgiler";
            OleDbCommand command = new OleDbCommand(sql, connection);
            OleDbDataReader reader = command.ExecuteReader();
            string vtKullaniciAdi = "";
            string vtSifre = "";
            int flag = 0;
            while (reader.Read())
            {
                vtKullaniciAdi = reader["kullaniciAdi"].ToString();
                vtSifre = reader["parola"].ToString();
                string sifrelenmisParola = MD5Sifrele(girisPaneliSifreTextBox.Text);
                
                if (vtKullaniciAdi == girisPaneliKullaniciAdiTextBox.Text && vtSifre == sifrelenmisParola)
                {
                    Form1 git = new Form1();
                    git.veri = girisPaneliKullaniciAdiTextBox.Text;
                    git.Show();
                    this.Hide();
                    flag++;
                }
            }

            if (flag == 0)
            {
                MessageBox.Show("Kullanici adi veya sifre hatali");
            }
        }

        void temizle()
        {
            girisPaneliKullaniciAdiTextBox.Text = "";
            girisPaneliSifreTextBox.Text = "";

        }

        public static string MD5Sifrele(string veri)
        {
            MD5 md5 = MD5.Create();

            // Girilen veriyi byte dizisine çevirir
            byte[] veriByteDizisi = Encoding.ASCII.GetBytes(veri);

            // byte dizisini MD5 ile şifreler
            byte[] sifrelenmisVeriByteDizisi = md5.ComputeHash(veriByteDizisi);

            // Şifrelenmiş byte dizisini string'e çevirir
            StringBuilder sb = new StringBuilder();         //stringleri birleştirme işlemi yapar
            for (int i = 0; i < sifrelenmisVeriByteDizisi.Length; i++)
            {
                sb.Append(sifrelenmisVeriByteDizisi[i].ToString("x2"));     //"x2" dizenin Onaltılık olarak biçimlendirilmesi gerektiğini belirtir.
            }

           
            return sb.ToString();
        }
    }
}
