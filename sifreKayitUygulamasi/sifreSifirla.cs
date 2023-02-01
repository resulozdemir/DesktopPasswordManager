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
    public partial class sifreSifirla : Form
    {
        bool surukle;
        Point start_point = new Point(0, 0);
        OleDbConnection conn;
        OleDbDataAdapter da;
        OleDbCommand cmd;
        DataSet ds;
        public string veri;

        public sifreSifirla()
        {
            InitializeComponent();
        }

        private void sifreSifirla_MouseUp(object sender, MouseEventArgs e)
        {
            surukle = false;
        }

        private void sifreSifirla_MouseDown(object sender, MouseEventArgs e)
        {
            surukle = true;
            start_point = new Point(e.X, e.Y);
        }

        private void sifreSifirla_MouseMove(object sender, MouseEventArgs e)
        {
            if (surukle)
            {
                Point p = PointToScreen(e.Location);
                this.Location = new Point(p.X - start_point.X, p.Y - start_point.Y);
            }
        }

        private void sifreSifirlaKapat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void sifreSifirlaKucult_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void sifreSifirlaGeri_Click(object sender, EventArgs e)
        {
            girisPaneli geri = new girisPaneli();
            geri.Show();
            this.Hide();
        }

        void temizle()
        {
            sifreSifirlaCevapTextBox.Text = "";
            sifreSifirlaYeniSifreTextBox.Text = "";
            sifreSifirlaKullaniciAdiTextBox.Text = "";
            sifreSifirlaGuvenlikComboBox.Text = "";

        }

        private void sifreSifirlaSifirlaButon_Click(object sender, EventArgs e)
        {
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Database.accdb";
            OleDbConnection connection = new OleDbConnection(connectionString);
            connection.Open();
            string sql = "SELECT kullaniciAdi, parola, guvenlikSorusu, cevap FROM kullaniciBilgiler";
            OleDbCommand command = new OleDbCommand(sql, connection);
            OleDbDataReader reader = command.ExecuteReader();
            string vtKullaniciAdi = "";
            string vtSifre = "";
            string vtSoru = "";
            string vtCevap = "";
            int flag = 0;
            while (reader.Read())
            {
                vtKullaniciAdi = reader["kullaniciAdi"].ToString();
                vtSifre = reader["parola"].ToString();
                vtSoru = reader["guvenlikSorusu"].ToString();
                vtCevap = reader["cevap"].ToString();
                string sifrelenmisParola = MD5Sifrele(sifreSifirlaYeniSifreTextBox.Text);
                string secilenSoru = sifreSifirlaGuvenlikComboBox.SelectedIndex.ToString();

                if (vtKullaniciAdi == sifreSifirlaKullaniciAdiTextBox.Text && vtSifre != sifrelenmisParola && secilenSoru == vtSoru && vtCevap == sifreSifirlaCevapTextBox.Text)
                {
                    string queryString = "DELETE FROM kullaniciBilgiler WHERE kullaniciAdi=@anaKullaniciAdi";

                    using (command = new OleDbCommand(queryString, connection))
                    {

                        command.Parameters.AddWithValue("@anaKullaniciAdi", vtKullaniciAdi);
                        command.ExecuteNonQuery();  //veritabanından eski şifrenin yazılı olduğu sütunu siliyoruz
                        
                    }

                    conn = new OleDbConnection("Provider=Microsoft.ACE.Oledb.12.0;Data Source=DataBase.accdb");

                    string query = "INSERT INTO kullaniciBilgiler (kullaniciAdi, parola, guvenlikSorusu, cevap) VALUES (@kullaniciAdi,@parola,@guvenlikSorusu,@cevap)";
                    cmd = new OleDbCommand(query, conn);
                    cmd.Parameters.AddWithValue("@kullaniciAdi", vtKullaniciAdi);
                    string md5Parola = MD5Sifrele(sifreSifirlaYeniSifreTextBox.Text);
                    cmd.Parameters.AddWithValue("@parola", md5Parola);      //veritabanına yeni şifrenin md5 ile şifrelenmiş halini veritabanına kaydediyoruz.
                    cmd.Parameters.AddWithValue("@guvenlikSorusu", vtSoru);
                    cmd.Parameters.AddWithValue("@cevap", vtCevap);
                    conn.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                        MessageBox.Show("Basariyla Kaydedildi");
                    else
                        MessageBox.Show("Kaydedilemedi");
                    conn.Close();
                    temizle();

                    flag++;


                }
            }

            if (flag == 0) {
                MessageBox.Show("Kullanici adi,guvenlik sorusu veya cevap hatali");
            }
        }

        public static string MD5Sifrele(string veri)
        {
            MD5 md5 = MD5.Create();

            byte[] veriByteDizisi = Encoding.ASCII.GetBytes(veri);

            byte[] sifrelenmisVeriByteDizisi = md5.ComputeHash(veriByteDizisi);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < sifrelenmisVeriByteDizisi.Length; i++)
            {
                sb.Append(sifrelenmisVeriByteDizisi[i].ToString("x2"));
            }

            return sb.ToString();
        }

        private void sifreSifirla_Load(object sender, EventArgs e)
        {

            label1.Text = veri;

            OleDbConnection baglanti = new OleDbConnection();
            baglanti.ConnectionString = @"Provider=Microsoft.ACE.Oledb.12.0;Data Source=DataBase.accdb";
            OleDbCommand komut = new OleDbCommand();
            komut.CommandText = "SELECT * from guvenlikSorulari";
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;
            OleDbDataReader dr;
            baglanti.Open();
            dr = komut.ExecuteReader();

            while (dr.Read())
            {
                sifreSifirlaGuvenlikComboBox.Items.Add(dr["guvenlikSorusu"]);
            }

            baglanti.Close();
        }

        void fillGrid()
        {
            conn = new OleDbConnection("Provider=Microsoft.ACE.Oledb.12.0;Data Source=DataBase.accdb");
            da = new OleDbDataAdapter("Select kullaniciAdi, parola, guvenlikSorusu, cevap from kullaniciBilgiler where kullaniciAdi = '" + label1.Text + "'", conn);
            DataTable dt = new DataTable();
            conn.Open();
            da.Fill(dt);
            conn.Close();

        }

        private void sifreSifirlaYeniSifreTextBox_TextChanged(object sender, EventArgs e)
        {
            bool karakterVeSayiVar = false;
            bool buyukHarfVar = false;
            bool ozelKarakterVar = false;

            foreach (char c in sifreSifirlaYeniSifreTextBox.Text)
            {
                if (char.IsDigit(c))
                {
                    karakterVeSayiVar = true;
                }
                else if (char.IsLetter(c))
                {
                    if (char.IsUpper(c))
                    {
                        buyukHarfVar = true;
                    }
                }
                else
                {
                    ozelKarakterVar = true;  
                }
            }

            if (sifreSifirlaYeniSifreTextBox.Text.Length >= 6 && karakterVeSayiVar && buyukHarfVar && ozelKarakterVar)
            {
                sifresSifirlaGosterge.BackColor = Color.Green;
            }
            else if (sifreSifirlaYeniSifreTextBox.Text.Length >= 6 && karakterVeSayiVar && buyukHarfVar)
            {
                sifresSifirlaGosterge.BackColor = Color.Yellow;
            }
            else if (sifreSifirlaYeniSifreTextBox.Text.Length >= 6 && karakterVeSayiVar)
            {
                sifresSifirlaGosterge.BackColor = Color.Red;
            }
            else
            {
                sifresSifirlaGosterge.BackColor = Color.Red;
            }
        }
    }
}
