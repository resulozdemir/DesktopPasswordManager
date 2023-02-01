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
    public partial class Netflix : Form
    {
        bool surukle;
        Point start_point = new Point(0, 0);
        public string veri;
        OleDbConnection conn;
        OleDbDataAdapter da;
        OleDbCommand cmd;
        DataSet ds;

        public Netflix()
        {
            InitializeComponent();
        }

        private void netflixKapat_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void netflixKucult_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void netflixGeri_Click(object sender, EventArgs e)
        {
            Form1 git = new Form1();
            git.veri = label1.Text;
            git.Show();
            this.Hide();
        }

        private void netflixPanel_MouseUp(object sender, MouseEventArgs e)
        {
            surukle = false;
        }

        private void netflixPanel_MouseDown(object sender, MouseEventArgs e)
        {
            surukle = true;
            start_point = new Point(e.X, e.Y);

        }

        private void netflixPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (surukle)
            {
                Point p = PointToScreen(e.Location);
                this.Location = new Point(p.X - start_point.X, p.Y - start_point.Y);
            }
        }

        void temizle()
        {
            netflixEposta.Text = "";
            netflixSifre.Text = "";

        }

        private void netflixRastgeleSifre_Click(object sender, EventArgs e)
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
            netflixSifre.Text = sifreStr;
        }

        private void netflixGuncelle_Click(object sender, EventArgs e)
        {
            if (netflixEposta.Text == "" || netflixSifre.Text == "")
            {
                MessageBox.Show("Boş Alan Hatası");
            }
            else
            {
                cmd = new OleDbCommand();
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "UPDATE netflix set parola=@parola where eposta='" + netflixEposta.Text + "'";
                cmd.Parameters.AddWithValue("@parola", netflixSifre.Text);


                cmd.ExecuteNonQuery();
                if (cmd.ExecuteNonQuery() > 0)
                    MessageBox.Show("Basariyla Guncellendi");
                else
                    MessageBox.Show("Guncellenemedi");

                conn.Close();
                fillGrid();
                temizle();
                netflixListele_Click(sender, e);

            }
        }

        private void netflixListele_Click(object sender, EventArgs e)
        {
            conn = new OleDbConnection("Provider=Microsoft.ACE.Oledb.12.0;Data Source=DataBase.accdb");
            string query = "Select eposta, parola from netflix where anaKullaniciAdi = @anaKullaniciAdi";
            cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("@anaKullaniciAdi", label1.Text);
            ds = new DataSet();
            conn.Open();
            using (OleDbDataReader reader = cmd.ExecuteReader())
            {

                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                netflixDataGridView.DataSource = dataTable;
            }
            conn.Close();
        }

        private void netflixSil_Click(object sender, EventArgs e)
        {
            if (netflixEposta.Text == "" || netflixSifre.Text == "")
            {
                MessageBox.Show("Boş Alan Hatası");
            }
            else
            {
                cmd = new OleDbCommand();
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "delete from netflix where eposta='" + netflixEposta.Text + "'";
                if (cmd.ExecuteNonQuery() > 0)
                    MessageBox.Show("Basariyla Silindi");
                else
                    MessageBox.Show("Silinemedi");
                conn.Close();
                fillGrid();
                temizle();
                netflixListele_Click(sender, e);
            }
        }

        private void netflixKaydet_Click(object sender, EventArgs e)
        {
            if (netflixEposta.Text == "" || netflixSifre.Text == "")
            {
                MessageBox.Show("Boş Alan Hatası");
            }
            else
            {
                string query = "INSERT INTO netflix (eposta,parola,anaKullaniciAdi) VALUES" + "(@eposta,@parola,@anaKullaniciAdi)";
                cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@eposta", netflixEposta.Text);
                cmd.Parameters.AddWithValue("@parola", netflixSifre.Text);
                cmd.Parameters.AddWithValue("@anaKullaniciAdi", label1.Text);
                conn.Open();
                if (cmd.ExecuteNonQuery() > 0)
                    MessageBox.Show("Basariyla Kaydedildi");
                else
                    MessageBox.Show("Kaydedilemedi");
                conn.Close();
                fillGrid();
                temizle();
                netflixListele_Click(sender, e);
            }
        }

        private void netflixDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilenDeger = netflixDataGridView.SelectedCells[0].RowIndex;
            netflixEposta.Text = netflixDataGridView.Rows[secilenDeger].Cells[0].Value.ToString();
            netflixSifre.Text = netflixDataGridView.Rows[secilenDeger].Cells[1].Value.ToString();
        }

        private void netflixSifre_TextChanged(object sender, EventArgs e)
        {
            bool karakterVeSayiVar = false;
            bool buyukHarfVar = false;
            bool ozelKarakterVar = false;

            foreach (char c in netflixSifre.Text)
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

            if (netflixSifre.Text.Length >= 6 && karakterVeSayiVar && buyukHarfVar && ozelKarakterVar)
            {
                netflixGosterge.BackColor = Color.Green;
            }
            else if (netflixSifre.Text.Length >= 6 && karakterVeSayiVar && ozelKarakterVar)
            {
                netflixGosterge.BackColor = Color.Yellow;
            }
            else if (netflixSifre.Text.Length >= 6 && karakterVeSayiVar && buyukHarfVar)
            {
                netflixGosterge.BackColor = Color.Yellow;
            }
            else if (netflixSifre.Text.Length >= 6 && karakterVeSayiVar)
            {
                netflixGosterge.BackColor = Color.Red;
            }
            else
            {
                netflixGosterge.BackColor = Color.Red;
            }
        }

        private void Netflix_Load(object sender, EventArgs e)
        {
            label1.Text = veri;
            fillGrid();
        }

        void fillGrid()
        {
            conn = new OleDbConnection("Provider=Microsoft.ACE.Oledb.12.0;Data Source=DataBase.accdb");
            da = new OleDbDataAdapter("Select kullaniciAdi, parola, eposta, anaKullaniciAdi from instagram where anaKullaniciAdi = '" + label1.Text + "'", conn);
            DataTable dt = new DataTable();
            conn.Open();
            da.Fill(dt);
            conn.Close();

        }
    }
}
