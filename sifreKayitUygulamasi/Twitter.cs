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

namespace sifreKayitUygulamasi
{
    public partial class Twitter : Form
    {

        bool surukle;
        Point start_point = new Point(0, 0);
        OleDbConnection conn;
        OleDbDataAdapter da;
        OleDbCommand cmd;
        DataSet ds;
        public string veri;

        public Twitter()
        {
            InitializeComponent();
        }

        private void twitterKapat_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void twitterKucult_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void twitterGeri_Click_1(object sender, EventArgs e)
        {
            Form1 git = new Form1();
            git.veri = label1.Text;
            git.Show();
            this.Hide();
        }


        void fillGrid()
        {
            conn = new OleDbConnection("Provider=Microsoft.ACE.Oledb.12.0;Data Source=DataBase.accdb");
            da = new OleDbDataAdapter("Select kullaniciAdi, parola, eposta, anaKullaniciAdi from twitter where anaKullaniciAdi = '" + label1.Text + "'", conn);
            DataTable dt = new DataTable();
            conn.Open();
            da.Fill(dt);
            conn.Close();

        }

        private void twitterPanel_MouseUp(object sender, MouseEventArgs e)
        {
            surukle = false;
        }

        private void twitterPanel_MouseDown(object sender, MouseEventArgs e)
        {
            surukle = true;
            start_point = new Point(e.X, e.Y);
        }

        private void twitterPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (surukle)
            {
                Point p = PointToScreen(e.Location);
                this.Location = new Point(p.X - start_point.X, p.Y - start_point.Y);
            }
        }

        void temizle()
        {
            twitterEposta.Text = "";
            twitterKullaniciAdi.Text = "";
            twitterSifre.Text = "";

        }

        private void twitterListele_Click(object sender, EventArgs e)
        {

            conn = new OleDbConnection("Provider=Microsoft.ACE.Oledb.12.0;Data Source=DataBase.accdb");
            string query = "Select kullaniciAdi, parola, eposta from twitter where anaKullaniciAdi = @anaKullaniciAdi";
            cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("@anaKullaniciAdi", label1.Text);
            ds = new DataSet();
            conn.Open();
            using (OleDbDataReader reader = cmd.ExecuteReader())
            {
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                twitterDataGridView.DataSource = dataTable;
            }
            conn.Close();
        }

        private void twitterSil_Click(object sender, EventArgs e)
        {
            if (twitterKullaniciAdi.Text == "" || twitterSifre.Text == "" || twitterEposta.Text == "")
            {
                MessageBox.Show("Boş Alan Hatası");
            }
            else
            {
                cmd = new OleDbCommand();
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "delete from twitter where kullaniciAdi='" + twitterKullaniciAdi.Text + "'";
                if (cmd.ExecuteNonQuery() > 0)
                    MessageBox.Show("Basariyla Silindi");
                else
                    MessageBox.Show("Silinemedi");
                conn.Close();
                fillGrid();
                temizle();
                twitterListele_Click(sender, e);
            }
        }

        private void twitterKaydet_Click(object sender, EventArgs e)
        {
            if (twitterKullaniciAdi.Text == "" || twitterSifre.Text == "" || twitterEposta.Text == "")
            {
                MessageBox.Show("Boş Alan Hatası");
            }
            else
            {
                string query = "INSERT INTO twitter (kullaniciAdi,parola,eposta,anaKullaniciAdi) VALUES" + "(@kullaniciAdi,@parola,@eposta,@anaKullaniciAdi)";
                cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@kullaniciAdi", twitterKullaniciAdi.Text);
                cmd.Parameters.AddWithValue("@parola", twitterSifre.Text);
                cmd.Parameters.AddWithValue("@eposta", twitterEposta.Text);
                cmd.Parameters.AddWithValue("@anaKullaniciAdi", label1.Text);
                conn.Open();
                if (cmd.ExecuteNonQuery() > 0)
                    MessageBox.Show("Basariyla Kaydedildi");
                else
                    MessageBox.Show("Kaydedilemedi");
                conn.Close();
                fillGrid();
                temizle();
                twitterListele_Click(sender, e);
            }
        }

        private void twitterGuncelle_Click(object sender, EventArgs e)
        {
            if (twitterKullaniciAdi.Text == "" || twitterSifre.Text == "" || twitterEposta.Text == "")
            {
                MessageBox.Show("Boş Alan Hatası");
            }
            else
            {
                cmd = new OleDbCommand();
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "UPDATE twitter set kullaniciAdi=@kullaniciAdi, parola=@parola where eposta='" + twitterEposta.Text + "'"; //tek tırnak string yazabilmek icin sql dilinde yoksa numerik olarak algılar
                cmd.Parameters.AddWithValue("@kullaniciAdi", twitterKullaniciAdi.Text);
                cmd.Parameters.AddWithValue("@parola", twitterSifre.Text);
                cmd.ExecuteNonQuery(); 
                if (cmd.ExecuteNonQuery() > 0)
                    MessageBox.Show("Basariyla Guncellendi");
                else
                    MessageBox.Show("Guncellenemedi");

                conn.Close();
                fillGrid();
                temizle();
                twitterListele_Click(sender, e);
            }
        }

        private void twitterRastgeleSifre_Click(object sender, EventArgs e)
        {
            string karakterler = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!$%^&*_-";
            char[] sifre = new char[10];
            Random rdm = new Random();

            for (int i = 0; i < 10; i++)
            {
                sifre[i] = karakterler[rdm.Next(26)];
                i++;

                if (i < 10)
                {
                    sifre[i] = karakterler[26 + rdm.Next(10)];
                    i++;
                }

                if (i < 10)
                {
                    sifre[i] = karakterler[36 + rdm.Next(karakterler.Length - 36)];
                }
            }

            string sifreStr = new string(sifre);
            twitterSifre.Text = sifreStr;

        }
    

        private void twitterSifre_TextChanged(object sender, EventArgs e)
        {
            bool karakterVeSayiVar = false;
            bool buyukHarfVar = false;
            bool ozelKarakterVar = false;

            foreach (char c in twitterSifre.Text)
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

            if (twitterSifre.Text.Length >= 6 && karakterVeSayiVar && buyukHarfVar && ozelKarakterVar)
            {
                twitterGosterge.BackColor = Color.Green;
            }
            else if (twitterSifre.Text.Length >= 6 && karakterVeSayiVar && ozelKarakterVar)
            {
                twitterGosterge.BackColor = Color.Yellow;
            }
            else if (twitterSifre.Text.Length >= 6 && karakterVeSayiVar && buyukHarfVar)
            {
                twitterGosterge.BackColor = Color.Yellow;
            }
            else if (twitterSifre.Text.Length >= 6 && karakterVeSayiVar)
            {
                twitterGosterge.BackColor = Color.Red;
            }
            else
            {
                twitterGosterge.BackColor = Color.Red;
            }
        }

        private void twitterDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilenDeger = twitterDataGridView.SelectedCells[0].RowIndex;
            twitterKullaniciAdi.Text = twitterDataGridView.Rows[secilenDeger].Cells[0].Value.ToString();
            twitterSifre.Text = twitterDataGridView.Rows[secilenDeger].Cells[1].Value.ToString();
            twitterEposta.Text = twitterDataGridView.Rows[secilenDeger].Cells[2].Value.ToString();
        }

        private void Twitter_Load(object sender, EventArgs e)
        {
            label1.Text = veri;
            fillGrid();
        }
    }
}
