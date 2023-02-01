using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace sifreKayitUygulamasi
{
    public partial class kayitOl : Form
    {
        bool surukle;
        Point start_point = new Point(0, 0);
        OleDbConnection conn;
        OleDbDataAdapter da;
        OleDbCommand cmd;
        DataSet ds;
        String guvenlikSorusu;
        String guvenlikSorusuSecimi;
        public string veri; 

        public kayitOl()
        {
            InitializeComponent();
        }

        private void kayitOlPanel_MouseUp(object sender, MouseEventArgs e)
        {
            surukle = false;
        }

        private void kayitOlPanel_MouseDown(object sender, MouseEventArgs e)
        {
            surukle = true;
            start_point = new Point(e.X, e.Y);
        }

        private void kayitOlPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (surukle)
            {
                Point p = PointToScreen(e.Location);
                this.Location = new Point(p.X - start_point.X, p.Y - start_point.Y);
            }
        }

        private void kayitOlKapat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void kayitOlKucult_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void kayitOlGeri_Click(object sender, EventArgs e)
        {
            girisPaneli geri = new girisPaneli();            
            geri.Show();
            this.Hide();
        }

        void temizle()
        {
            kayitOlKullaniciAdiTextBox.Text = "";
            kayitOlSifreTextBox.Text = "";
            kayitOlComboBox.Text = "";
            kayitOlCevap.Text = "";

        }

        private void kayitOlComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            guvenlikSorusu = kayitOlComboBox.Text;
            
            if(guvenlikSorusu== "Dogum tarihiniz ?")
            {
                guvenlikSorusuSecimi = "0";
            }
            else if(guvenlikSorusu== "Tuttugunuz takim ?")
            {
                guvenlikSorusuSecimi = "1";
            }
            else if(guvenlikSorusu== "En sevdiğiniz renk?")
            {
                guvenlikSorusuSecimi = "2";
            }
        }

        private void kayitOlCevap_Click(object sender, EventArgs e)
        {

        }

        private void kayitOlButon_Click(object sender, EventArgs e)
        {
            int flag = 0;
            if (kayitOlKullaniciAdiTextBox.Text == "" || kayitOlSifreTextBox.Text == "" || kayitOlCevap.Text == "" || kayitOlComboBox.SelectedItem == null)      
            {
                MessageBox.Show("Boş Alan Hatası");
            }
            else 
            {
                string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Database.accdb";
                OleDbConnection connection = new OleDbConnection(connectionString);
                connection.Open();
                string sql = "SELECT kullaniciAdi FROM kullaniciBilgiler";
                OleDbCommand command = new OleDbCommand(sql, connection);
                OleDbDataReader reader = command.ExecuteReader();
                string vtKullaniciAdi = "";
                while (reader.Read())
                {
                    vtKullaniciAdi = reader["kullaniciAdi"].ToString();
                    if (vtKullaniciAdi == kayitOlKullaniciAdiTextBox.Text)
                    {
                        MessageBox.Show("Bu kullanici adina sahip baska bir kullanici vardir bu kullanici adiyla kayit olamazsiniz");
                        flag++;
                    }
                }


                
                if (flag == 0)
                {
                    conn = new OleDbConnection("Provider=Microsoft.ACE.Oledb.12.0;Data Source=DataBase.accdb");

                    string query = "INSERT INTO kullaniciBilgiler (kullaniciAdi, parola, guvenlikSorusu, cevap) VALUES (@kullaniciAdi,@parola,@guvenlikSorusu,@cevap)";
                    cmd = new OleDbCommand(query, conn);
                    cmd.Parameters.AddWithValue("@kullaniciAdi", kayitOlKullaniciAdiTextBox.Text);
                    string md5Parola = MD5Sifrele(kayitOlSifreTextBox.Text);
                    cmd.Parameters.AddWithValue("@parola", md5Parola);
                    cmd.Parameters.AddWithValue("@guvenlikSorusu", guvenlikSorusuSecimi);
                    cmd.Parameters.AddWithValue("@cevap", kayitOlCevap.Text);
                    conn.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                        MessageBox.Show("Basariyla Kaydedildi");
                    else
                        MessageBox.Show("Kaydedilemedi");
                    conn.Close();
                    temizle();
                }
            }
        }


        private void gucluSifreGostergesi_Click(object sender, EventArgs e)
        {

        }

        private void kayitOlSifreTextBox_TextChanged(object sender, EventArgs e)
        {
            
            bool karakterVeSayiVar = false;
            bool buyukHarfVar = false;
            bool ozelKarakterVar = false;

            //lllll66666*

            foreach (char c in kayitOlSifreTextBox.Text)
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


            if (kayitOlSifreTextBox.Text.Length >= 6 && karakterVeSayiVar && buyukHarfVar && ozelKarakterVar)
            {
                gucluSifreGostergesi.BackColor = Color.Green;
            }
            else if (kayitOlSifreTextBox.Text.Length >= 6 && karakterVeSayiVar && ozelKarakterVar)
            {
                gucluSifreGostergesi.BackColor = Color.Yellow;
            }
            else if (kayitOlSifreTextBox.Text.Length >= 6 && karakterVeSayiVar && buyukHarfVar)
            {
                gucluSifreGostergesi.BackColor = Color.Yellow;
            }
            else if (kayitOlSifreTextBox.Text.Length >= 6 && karakterVeSayiVar)
            {
                gucluSifreGostergesi.BackColor = Color.Red;
            }
            else
            {
                gucluSifreGostergesi.BackColor = Color.Red;
            }
        }

        private void kayitOl_Load(object sender, EventArgs e)
        {
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
                kayitOlComboBox.Items.Add(dr["guvenlikSorusu"]);
            }

            baglanti.Close();


            label1.Text = veri;
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
    }
}
