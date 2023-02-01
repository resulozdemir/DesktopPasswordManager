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


//EPOSTA İLEDE GUNCELLENEMEDİ HATASI


namespace sifreKayitUygulamasi
{
    public partial class Steam : Form
    {
        bool surukle;
        Point start_point = new Point(0, 0);
        OleDbConnection conn;
        OleDbDataAdapter da;
        OleDbCommand cmd;
        DataSet ds;
        public string veri;

        public Steam()
        {
            InitializeComponent();
        }

        private void steamKapat_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void steamKucult_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void steamGeri_Click_1(object sender, EventArgs e)
        {
            Form1 git = new Form1();
            git.veri = label1.Text;
            git.Show();
            this.Hide();
        }

        private void steamPanel_MouseUp_1(object sender, MouseEventArgs e)
        {
            surukle = false;
        }

        private void steamPanel_MouseDown_1(object sender, MouseEventArgs e)
        {
            surukle = true;
            start_point = new Point(e.X, e.Y);
        }

        private void steamPanel_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (surukle)
            {
                Point p = PointToScreen(e.Location);
                this.Location = new Point(p.X - start_point.X, p.Y - start_point.Y);
            }
        }

        void temizle()
        {
            steamSifre.Text = "";
            steamEposta.Text = "";

        }       


        private void steamKaydet_Click(object sender, EventArgs e)
        {
            if (steamEposta.Text == "" || steamEposta.Text == "")
            {
                MessageBox.Show("Boş Alan Hatası");
            }
            else
            {
                string query = "INSERT INTO steam (eposta,parola,anaKullaniciAdi) VALUES" + "(@eposta,@parola,@anaKullaniciAdi)";
                cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@eposta", steamEposta.Text);
                cmd.Parameters.AddWithValue("@parola", steamSifre.Text);
                cmd.Parameters.AddWithValue("@anaKullaniciAdi", label1.Text);
                conn.Open();
                if (cmd.ExecuteNonQuery() > 0)
                    MessageBox.Show("Basariyla Kaydedildi");
                else
                    MessageBox.Show("Kaydedilemedi");
                conn.Close();
                fillGrid();
                temizle();
                steamListele_Click(sender, e);
            }
        }

        private void steamSil_Click(object sender, EventArgs e)
        {
            if (steamEposta.Text == "" || steamSifre.Text == "")
            {
                MessageBox.Show("Boş Alan Hatası");
            }
            else
            {
                cmd = new OleDbCommand();
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "delete from steam where eposta='" + steamEposta.Text + "'";
                if (cmd.ExecuteNonQuery() > 0)
                    MessageBox.Show("Basariyla Silindi");
                else
                    MessageBox.Show("Silinemedi");
                conn.Close();
                fillGrid();
                temizle();
                steamListele_Click(sender, e);
            }
        }

        private void steamListele_Click(object sender, EventArgs e)
        {
            conn = new OleDbConnection("Provider=Microsoft.ACE.Oledb.12.0;Data Source=DataBase.accdb");
            string query = "Select eposta, parola from steam where anaKullaniciAdi = @anaKullaniciAdi";
            cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("@anaKullaniciAdi", label1.Text);
            ds = new DataSet();
            conn.Open();
            using (OleDbDataReader reader = cmd.ExecuteReader())
            {

                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                steamDataGridView.DataSource = dataTable;
            }
            conn.Close();
        }

        private void steamGuncelle_Click(object sender, EventArgs e)
        {
            if (steamEposta.Text == "" || steamSifre.Text == "")
            {
                MessageBox.Show("Boş Alan Hatası");
            }
            else
            {
                cmd = new OleDbCommand();
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "UPDATE steam set parola=@parola where eposta='" + steamEposta.Text + "'";

                cmd.Parameters.AddWithValue("@parola", steamSifre.Text);

                cmd.ExecuteNonQuery(); 
                if (cmd.ExecuteNonQuery() > 0)
                    MessageBox.Show("Basariyla Guncellendi");
                else
                    MessageBox.Show("Guncellenemedi");

                conn.Close();
                fillGrid();
                temizle();
                steamListele_Click(sender, e);

            }
        }

        private void steamRastgeleSifre_Click(object sender, EventArgs e)
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
            steamSifre.Text = sifreStr;
        }

        private void steamDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilenDeger = steamDataGridView.SelectedCells[0].RowIndex;
            steamEposta.Text = steamDataGridView.Rows[secilenDeger].Cells[0].Value.ToString();
            steamSifre.Text = steamDataGridView.Rows[secilenDeger].Cells[1].Value.ToString();
        }

        private void steamSifre_TextChanged(object sender, EventArgs e)
        {
            // Güçlü şifre kriterlerini kontrol et
            bool karakterVeSayiVar = false;
            bool buyukHarfVar = false;
            bool ozelKarakterVar = false;

            foreach (char c in steamSifre.Text)
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

            if (steamSifre.Text.Length >= 6 && karakterVeSayiVar && buyukHarfVar && ozelKarakterVar)
            {
                steamGosterge.BackColor = Color.Green;
            }
            else if (steamSifre.Text.Length >= 6 && karakterVeSayiVar && ozelKarakterVar)
            {
                steamGosterge.BackColor = Color.Yellow;
            }
            else if (steamSifre.Text.Length >= 6 && karakterVeSayiVar && buyukHarfVar)
            {
                steamGosterge.BackColor = Color.Yellow;
            }
            else if (steamSifre.Text.Length >= 6 && karakterVeSayiVar)
            {
                steamGosterge.BackColor = Color.Red;
            }
            else
            {
                steamGosterge.BackColor = Color.Red;
            }
        }

        private void Steam_Load(object sender, EventArgs e)
        {
            label1.Text = veri;
            fillGrid();
        }

        void fillGrid()
        {
            conn = new OleDbConnection("Provider=Microsoft.ACE.Oledb.12.0;Data Source=DataBase.accdb");
            da = new OleDbDataAdapter("Select eposta, parola, anaKullaniciAdi from steam where anaKullaniciAdi = '" + label1.Text + "'", conn);
            DataTable dt = new DataTable();
            conn.Open();
            da.Fill(dt);
            conn.Close();

        }
    }
}
