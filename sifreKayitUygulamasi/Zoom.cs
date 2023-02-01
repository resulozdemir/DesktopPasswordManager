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
    public partial class Zoom : Form
    {
        bool surukle;
        Point start_point = new Point(0, 0);
        public string veri;
        OleDbConnection conn;
        OleDbDataAdapter da;
        OleDbCommand cmd;
        DataSet ds;


        public Zoom()
        {
            InitializeComponent();
        }

        private void zoomKapat_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void zoomKucult_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void zoomGeri_Click_1(object sender, EventArgs e)
        {
            Form1 git = new Form1();
            git.veri = label1.Text;
            git.Show();
            this.Hide();
        }

        private void zoomPanel_MouseUp(object sender, MouseEventArgs e)
        {
            surukle = false;
        }

        private void zoomPanel_MouseDown(object sender, MouseEventArgs e)
        {
            surukle = true;
            start_point = new Point(e.X, e.Y);
        }

        private void zoomPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (surukle)
            {
                Point p = PointToScreen(e.Location);
                this.Location = new Point(p.X - start_point.X, p.Y - start_point.Y);
            }
        }

        void temizle()
        {
            zoomSifre.Text = "";
            zoomEposta.Text = "";

        }

        private void zoomListele_Click(object sender, EventArgs e)
        {
            conn = new OleDbConnection("Provider=Microsoft.ACE.Oledb.12.0;Data Source=DataBase.accdb");
            string query = "Select eposta, parola from zoom where anaKullaniciAdi = @anaKullaniciAdi";
            cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("@anaKullaniciAdi", label1.Text);
            ds = new DataSet();
            conn.Open();
            using (OleDbDataReader reader = cmd.ExecuteReader())
            {

                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                zoomDataGridView.DataSource = dataTable;
            }
            conn.Close();
        }

        private void zoomGuncelle_Click(object sender, EventArgs e)
        {
            if (zoomEposta.Text == "" || zoomSifre.Text == "")
            {
                MessageBox.Show("Boş Alan Hatası");
            }
            else
            {
                cmd = new OleDbCommand();
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "UPDATE zoom set parola=@parola where eposta='" + zoomEposta.Text + "'";
                cmd.Parameters.AddWithValue("@parola", zoomSifre.Text);

                cmd.ExecuteNonQuery(); 
                if (cmd.ExecuteNonQuery() > 0)
                    MessageBox.Show("Basariyla Guncellendi");
                else
                    MessageBox.Show("Guncellenemedi");

                conn.Close();
                fillGrid();
                temizle();
                zoomListele_Click(sender, e);

            }
        }

        private void zoomSil_Click(object sender, EventArgs e)
        {
            if (zoomEposta.Text == "" || zoomEposta.Text == "")
            {
                MessageBox.Show("Boş Alan Hatası");
            }
            else
            {
                cmd = new OleDbCommand();
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "delete from zoom where eposta='" + zoomEposta.Text + "'";
                if (cmd.ExecuteNonQuery() > 0)
                    MessageBox.Show("Basariyla Silindi");
                else
                    MessageBox.Show("Silinemedi");
                conn.Close();
                fillGrid();
                temizle();
                zoomListele_Click(sender, e);
            }
        }

        private void zoomKaydet_Click(object sender, EventArgs e)
        {
            if (zoomEposta.Text == "" || zoomSifre.Text == "")
            {
                MessageBox.Show("Boş Alan Hatası");
            }
            else
            {
                string query = "INSERT INTO zoom (eposta,parola,anaKullaniciAdi) VALUES" + "(@eposta,@parola,@anaKullaniciAdi)";
                cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@eposta", zoomEposta.Text);
                cmd.Parameters.AddWithValue("@parola", zoomSifre.Text);
                cmd.Parameters.AddWithValue("@anaKullaniciAdi", label1.Text);
                conn.Open();
                if (cmd.ExecuteNonQuery() > 0)
                    MessageBox.Show("Basariyla Kaydedildi");
                else
                    MessageBox.Show("Kaydedilemedi");
                conn.Close();
                fillGrid();
                temizle();
                zoomListele_Click(sender, e);
            }
        }

        private void zoomRastgeleSifre_Click(object sender, EventArgs e)
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
            zoomSifre.Text = sifreStr;
        }

        private void zoomSifre_TextChanged(object sender, EventArgs e)
        {
            bool karakterVeSayiVar = false;
            bool buyukHarfVar = false;
            bool ozelKarakterVar = false;

            foreach (char c in zoomSifre.Text)
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

            if (zoomSifre.Text.Length >= 6 && karakterVeSayiVar && buyukHarfVar && ozelKarakterVar)
            {
                zoomGosterge.BackColor = Color.Green;
            }
            else if (zoomSifre.Text.Length >= 6 && karakterVeSayiVar && ozelKarakterVar)
            {
                zoomGosterge.BackColor = Color.Yellow;
            }
            else if (zoomSifre.Text.Length >= 6 && karakterVeSayiVar && buyukHarfVar)
            {
                zoomGosterge.BackColor = Color.Yellow;
            }
            else if (zoomSifre.Text.Length >= 6 && karakterVeSayiVar)
            {
                zoomGosterge.BackColor = Color.Red;
            }
            else
            {
                zoomGosterge.BackColor = Color.Red;
            }
        }

        private void zoomDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilenDeger = zoomDataGridView.SelectedCells[0].RowIndex;
            zoomEposta.Text = zoomDataGridView.Rows[secilenDeger].Cells[0].Value.ToString();
            zoomSifre.Text = zoomDataGridView.Rows[secilenDeger].Cells[1].Value.ToString();
        }

        private void Zoom_Load(object sender, EventArgs e)
        {

            label1.Text = veri;
            fillGrid();
        }

        void fillGrid()
        {
            conn = new OleDbConnection("Provider=Microsoft.ACE.Oledb.12.0;Data Source=DataBase.accdb");
            da = new OleDbDataAdapter("Select eposta, parola, anaKullaniciAdi from zoom where anaKullaniciAdi = '" + label1.Text + "'", conn);
            DataTable dt = new DataTable();
            conn.Open();
            da.Fill(dt);
            conn.Close();

        }

        private void zoomGosterge_Click(object sender, EventArgs e)
        {

        }
    }
}
