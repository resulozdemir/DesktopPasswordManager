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
    public partial class profil : Form
    {
        bool surukle;
        Point start_point = new Point(0, 0);
        int sayac = 0;
        public string veri;
        OleDbConnection conn;
        OleDbDataAdapter da;
        OleDbCommand cmd;
        DataSet ds;

        public profil()
        {
            InitializeComponent();
        }

        private void profilPanel_MouseUp(object sender, MouseEventArgs e)
        {
            surukle = false;
        }

        private void profilPanel_MouseDown(object sender, MouseEventArgs e)
        {
            surukle = true;
            start_point = new Point(e.X, e.Y);
        }

        private void profilPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (surukle)
            {
                Point p = PointToScreen(e.Location);
                this.Location = new Point(p.X - start_point.X, p.Y - start_point.Y);
            }
        }

        private void sifreyiGosterPictureBox1_Click(object sender, EventArgs e)
        {
            sayac++;
            if (sayac % 2 == 0)
            {
                profilSifreTextBox.UseSystemPasswordChar = true;
            }
            else
            {
                profilSifreTextBox.UseSystemPasswordChar = false;
            }
        }

        private void profilKapat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void profilKucult_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void profilGeri_Click(object sender, EventArgs e)
        {
            Form1 git = new Form1();
            git.veri = label1.Text;
            git.Show();
            this.Hide();
        }

        void temizle()
        {
            profilCevapTextBox.Text = "";
            profilGuvenlikComboBox.Text = "";
            profilSifreTextBox.Text = "";

        }

        private void profilGuncelleButon_Click(object sender, EventArgs e)
        {
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Database.accdb";
            OleDbConnection connection = new OleDbConnection(connectionString);
            connection.Open();
            string sql = "SELECT kullaniciAdi, parola, guvenlikSorusu, cevap FROM kullaniciBilgiler where anaKullaniciAdi=@anaKullaniciAdi";
            OleDbCommand command = new OleDbCommand(sql, connection);
            command.Parameters.AddWithValue("@anaKullaniciAdi", label1.Text);
            OleDbDataReader reader = command.ExecuteReader();
            string vtKullaniciAdi = "";
            string vtSifre = "";
            string vtSoru = "";
            string vtCevap = "";
            int flag = 0;
            while (reader.Read())
            {
                vtKullaniciAdi = reader["kullaniciAdi"].ToString();
                if (vtKullaniciAdi == label1.Text)
                {
                    vtSifre = reader["parola"].ToString();
                    vtSoru = reader["guvenlikSorusu"].ToString();
                    vtCevap = reader["cevap"].ToString();
                    string sifrelenmisParola = MD5Sifrele(profilSifreTextBox.Text);
                    string secilenSoru = profilGuvenlikComboBox.SelectedIndex.ToString();

                    if (vtSifre != sifrelenmisParola && secilenSoru == vtSoru && vtCevap == profilCevapTextBox.Text) {
                        
                        connection = new OleDbConnection(connectionString);
                        string md5Parola = MD5Sifrele(profilSifreTextBox.Text);
                        sql = "UPDATE kullaniciBilgiler set parola='" + md5Parola + "' where kullaniciAdi='" + label1.Text + "'";
                        command = new OleDbCommand(sql, connection);
                        connection.Open();
                       
                        command.ExecuteNonQuery();
                        if (command.ExecuteNonQuery() > 0)
                            MessageBox.Show("Basariyla Guncellendi");
                        else
                            MessageBox.Show("Guncellenemedi");

                        connection.Close();
                        fillGrid();
                        temizle();

                        flag++;

                    }


                    else if (vtKullaniciAdi == label2.Text && vtSifre == sifrelenmisParola && secilenSoru != vtSoru || vtCevap != profilCevapTextBox.Text)
                    {
                        connection = new OleDbConnection(connectionString);
                        sql = "UPDATE kullaniciBilgiler set guvenlikSorusu='" + profilGuvenlikComboBox.SelectedIndex + "',cevap='" + profilCevapTextBox.Text + "' where kullaniciAdi='" + label1.Text + "'";
                        command = new OleDbCommand(sql, connection);
                        connection.Open();

                        command.ExecuteNonQuery();
                        if (command.ExecuteNonQuery() > 0)
                            MessageBox.Show("Basariyla Guncellendi");
                        else
                            MessageBox.Show("Guncellenemedi");

                        connection.Close();
                        fillGrid();
                        temizle();

                        flag++;
                    }
                }
                
            }

            if (flag == 0)
            {
                MessageBox.Show("Kullanici adi,guvenlik sorusu veya cevap hatali");
            }

            


        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void profil_Load(object sender, EventArgs e)
        {
            label1.Text = veri;
            label2.Text = label1.Text;

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
                profilGuvenlikComboBox.Items.Add(dr["guvenlikSorusu"]);
            }

            baglanti.Close();
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

        void fillGrid()
        {
            conn = new OleDbConnection("Provider=Microsoft.ACE.Oledb.12.0;Data Source=DataBase.accdb");
            da = new OleDbDataAdapter("Select kullaniciAdi, parola, guvenlikSorusu, cevap from kullaniciBilgiler where kullaniciAdi = '" + label1.Text + "'", conn);
            DataTable dt = new DataTable();
            conn.Open();
            da.Fill(dt);
            conn.Close();

        }


    }
}
