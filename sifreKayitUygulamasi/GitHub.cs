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

//FORM1 DE GİTHUBA GİRMİYOR


namespace sifreKayitUygulamasi
{
    public partial class GitHub : Form
    {
        bool surukle;
        Point start_point = new Point(0, 0);
        public string veri;
        OleDbConnection conn;
        OleDbDataAdapter da;
        OleDbCommand cmd;
        DataSet ds;

        public GitHub()
        {
            InitializeComponent();
        }

        private void githubKapat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void githubKucult_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void githubGeri_Click(object sender, EventArgs e)
        {
            Form1 git = new Form1();
            git.veri = label1.Text;
            git.Show();
            this.Hide();
        }

        private void githubPanel_MouseDown(object sender, MouseEventArgs e)
        {
            surukle = true;
            start_point = new Point(e.X, e.Y);
        }

        private void githubPanel_MouseUp(object sender, MouseEventArgs e)
        {
            surukle = false;
        }

        private void githubPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (surukle)
            {
                Point p = PointToScreen(e.Location);
                this.Location = new Point(p.X - start_point.X, p.Y - start_point.Y);
            }
        }

        void temizle()
        {
            githubEposta.Text = "";
            githubSifre.Text = "";

        }

        void fillGrid()
        {
            conn = new OleDbConnection("Provider=Microsoft.ACE.Oledb.12.0;Data Source=DataBase.accdb");
            da = new OleDbDataAdapter("Select eposta, parola, anaKullaniciAdi from github where anaKullaniciAdi = '" + label1.Text + "'", conn);
            DataTable dt = new DataTable();
            conn.Open();
            da.Fill(dt);
            conn.Close();

        }

        private void githubRastgeleSifre_Click(object sender, EventArgs e)
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
            githubSifre.Text = sifreStr;
        }

        private void githubListele_Click(object sender, EventArgs e)
        {
            conn = new OleDbConnection("Provider=Microsoft.ACE.Oledb.12.0;Data Source=DataBase.accdb");
            string query = "Select eposta, parola from github where anaKullaniciAdi = @anaKullaniciAdi";
            cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("@anaKullaniciAdi", label1.Text);
            ds = new DataSet();
            conn.Open();
            using (OleDbDataReader reader = cmd.ExecuteReader())
            {

                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                githubDataGridView.DataSource = dataTable;
            }
            conn.Close();
        }

        private void githubSil_Click(object sender, EventArgs e)
        {
            if (githubEposta.Text == "" || githubSifre.Text == "")
            {
                MessageBox.Show("Boş Alan Hatası");
            }
            else
            {
                cmd = new OleDbCommand();
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "delete from github where eposta='" + githubEposta.Text + "'";
                if (cmd.ExecuteNonQuery() > 0)
                    MessageBox.Show("Basariyla Silindi");
                else
                    MessageBox.Show("Silinemedi");
                conn.Close();
                fillGrid();
                temizle();
                githubListele_Click(sender, e);
            }
        }

        private void githubKaydet_Click(object sender, EventArgs e)
        {
            if (githubEposta.Text == "" || githubSifre.Text == "")
            {
                MessageBox.Show("Boş Alan Hatası");
            }
            else
            {
                string query = "INSERT INTO github (eposta,parola,anaKullaniciAdi) VALUES" + "(@eposta,@parola,@anaKullaniciAdi)";
                cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@eposta", githubEposta.Text);
                cmd.Parameters.AddWithValue("@parola", githubSifre.Text);
                cmd.Parameters.AddWithValue("@anaKullaniciAdi", label1.Text);
                conn.Open();
                if (cmd.ExecuteNonQuery() > 0)
                    MessageBox.Show("Basariyla Kaydedildi");
                else
                    MessageBox.Show("Kaydedilemedi");
                conn.Close();
                fillGrid();
                temizle();
                githubListele_Click(sender, e);
            }
        }

        private void githubGuncelle_Click(object sender, EventArgs e)
        {
            if (githubEposta.Text == "" || githubSifre.Text == "")
            {
                MessageBox.Show("Boş Alan Hatası");
            }
            else
            {
                cmd = new OleDbCommand();
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "UPDATE github set parola=@parola where eposta='" + githubEposta.Text + "'";
                cmd.Parameters.AddWithValue("@eposta", githubEposta.Text);
                cmd.Parameters.AddWithValue("@parola", githubSifre.Text);

                cmd.ExecuteNonQuery(); //sonuc doner kac tane islem yaptıysan
                if (cmd.ExecuteNonQuery() > 0)
                    MessageBox.Show("Basariyla Guncellendi");
                else
                    MessageBox.Show("Guncellenemedi");

                conn.Close();
                fillGrid();
                temizle();
                githubListele_Click(sender, e);

            }
        }

        private void githubSifre_TextChanged(object sender, EventArgs e)
        {
            bool karakterVeSayiVar = false;
            bool buyukHarfVar = false;
            bool ozelKarakterVar = false;

            foreach (char c in githubSifre.Text)
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

            if (githubSifre.Text.Length >= 6 && karakterVeSayiVar && buyukHarfVar && ozelKarakterVar)
            {
                githubGosterge.BackColor = Color.Green;
            }
            else if (githubSifre.Text.Length >= 6 && karakterVeSayiVar && ozelKarakterVar)
            {
                githubGosterge.BackColor = Color.Yellow;
            }
            else if (githubSifre.Text.Length >= 6 && karakterVeSayiVar && buyukHarfVar)
            {
                githubGosterge.BackColor = Color.Yellow;
            }
            else if (githubSifre.Text.Length >= 6 && karakterVeSayiVar)
            {
                githubGosterge.BackColor = Color.Red;
            }
            else
            {
                githubGosterge.BackColor = Color.Red;
            }
        }

        private void githubDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilenDeger = githubDataGridView.SelectedCells[0].RowIndex;
            githubEposta.Text = githubDataGridView.Rows[secilenDeger].Cells[0].Value.ToString();// Hücreyi 1'den başlatmamızın sebebi 0. değerde ID olmasıdır.
            githubSifre.Text = githubDataGridView.Rows[secilenDeger].Cells[1].Value.ToString();
        }

        private void GitHub_Load(object sender, EventArgs e)
        {
            label1.Text = veri;
            fillGrid();
        }

    }
}
