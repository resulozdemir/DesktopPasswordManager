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
    public partial class Discord : Form
    {
        bool surukle;
        Point start_point = new Point(0, 0);
        OleDbConnection conn;
        OleDbDataAdapter da;
        OleDbCommand cmd;
        DataSet ds;
        public string discord_veri;
        String anaKullaniciAdi;

        public Discord()
        {
            InitializeComponent();
        }

        private void discordKapat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void discordKucult_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void discordGeri_Click(object sender, EventArgs e)
        {
            Form1 git = new Form1();
            git.veri = discord_label.Text;
            git.Show();
            this.Hide();
        }

        private void discordPanel_MouseDown(object sender, MouseEventArgs e)
        {
            surukle = true;
            start_point = new Point(e.X, e.Y);
        }

        private void discordPanel_MouseUp(object sender, MouseEventArgs e)
        {
            surukle = false;
        }

        private void discordPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (surukle)
            {
                Point p = PointToScreen(e.Location);
                this.Location = new Point(p.X - start_point.X, p.Y - start_point.Y);
            }
        }

        void temizle()
        {
            discordEposta.Text = "";
            discordSifre.Text = "";

        }

        void fillGrid()
        {
            conn = new OleDbConnection("Provider=Microsoft.ACE.Oledb.12.0;Data Source=DataBase.accdb");
            da = new OleDbDataAdapter("Select kullaniciAdi, parola, eposta, anaKullaniciAdi from instagram where anaKullaniciAdi = '" + anaKullaniciAdi + "'", conn);
            DataTable dt = new DataTable();
            conn.Open();
            da.Fill(dt);
            conn.Close();

        }
        private void discordListele_Click(object sender, EventArgs e)
        {
            conn = new OleDbConnection("Provider=Microsoft.ACE.Oledb.12.0;Data Source=DataBase.accdb");
            string query = "Select eposta, parola from discord where anaKullaniciAdi = @anaKullaniciAdi";
            cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("@anaKullaniciAdi", anaKullaniciAdi);
            ds = new DataSet();
            conn.Open();
            using (OleDbDataReader reader = cmd.ExecuteReader())
            {

                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                discordDataGridView.DataSource = dataTable;
            }
            conn.Close();
        }

        private void discordSil_Click(object sender, EventArgs e)
        {
            if (discordEposta.Text == "" || discordSifre.Text == "" )
            {
                MessageBox.Show("Boş Alan Hatası");
            }
            else
            {
                cmd = new OleDbCommand();
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "delete from discord where eposta='" + discordEposta.Text + "'";
                if (cmd.ExecuteNonQuery() > 0)
                    MessageBox.Show("Basariyla Silindi");
                else
                    MessageBox.Show("Silinemedi");
                conn.Close();
                fillGrid();
                temizle();
                discordListele_Click(sender, e);
            }
        }

        private void discordKaydet_Click(object sender, EventArgs e)
        {
            if (discordEposta.Text == "" || discordSifre.Text == "")
            {
                MessageBox.Show("Boş Alan Hatası");
            }
            else
            {
                string query = "INSERT INTO discord (eposta,parola,anaKullaniciAdi) VALUES" + "(@eposta,@parola,@anaKullaniciAdi)";
                cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@eposta", discordEposta.Text);
                cmd.Parameters.AddWithValue("@parola", discordSifre.Text);
                cmd.Parameters.AddWithValue("@anaKullaniciAdi", anaKullaniciAdi);
                conn.Open();
                if (cmd.ExecuteNonQuery() > 0)
                    MessageBox.Show("Basariyla Kaydedildi");
                else
                    MessageBox.Show("Kaydedilemedi");
                conn.Close();
                fillGrid();
                temizle();
                discordListele_Click(sender, e);
            }
        }

        private void discordGuncelle_Click(object sender, EventArgs e)
        {
            if (discordEposta.Text == "" || discordSifre.Text == "")
            {
                MessageBox.Show("Boş Alan Hatası");
            }
            else
            {
                cmd = new OleDbCommand();
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "UPDATE discord set parola=@parola where eposta='" + discordEposta.Text + "'"; 
                cmd.Parameters.AddWithValue("@parola", discordSifre.Text);
                
                cmd.ExecuteNonQuery();
                if (cmd.ExecuteNonQuery() > 0)
                    MessageBox.Show("Basariyla Guncellendi");
                else
                    MessageBox.Show("Guncellenemedi");

                conn.Close();
                fillGrid();
                temizle();
                discordListele_Click(sender, e);

            }
        }

        private void discordRastgeleSifre_Click(object sender, EventArgs e)
        {
            string karakterler = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!$%^&*_-";
            char[] sifre = new char[10];    //10 haneli olusturmasını istedim
            Random rdm = new Random();

            for (int i = 0; i < 10; i++)
            {
                sifre[i] = karakterler[rdm.Next(26)];
                i++;

                if (i < 10)
                {
                    sifre[i] = karakterler[26 + rdm.Next(10)];      //26 ile 36 arasında sayılar var
                    i++;
                }

                if (i < 10)
                {
                    sifre[i] = karakterler[36 + rdm.Next(karakterler.Length - 36)];     //36 ile 44 arasinda ozel karakterler var
                }
            }

            string sifreStr = new string(sifre);
            discordSifre.Text = sifreStr;
        }

        private void Discord_Load(object sender, EventArgs e)
        {
            discord_label.Text = discord_veri;
            anaKullaniciAdi = discord_veri;
            fillGrid();
        }

        private void discordDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilenDeger = discordDataGridView.SelectedCells[0].RowIndex;
            discordEposta.Text = discordDataGridView.Rows[secilenDeger].Cells[0].Value.ToString();// Hücreyi 1'den başlatmamızın sebebi 0. değerde ID olmasıdır.
            discordSifre.Text = discordDataGridView.Rows[secilenDeger].Cells[1].Value.ToString();
        }

        private void discordSifre_TextChanged(object sender, EventArgs e)
        {
            bool karakterVeSayiVar = false;
            bool buyukHarfVar = false;
            bool ozelKarakterVar = false;

            foreach (char c in discordSifre.Text)
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

            if (discordSifre.Text.Length >= 6 && karakterVeSayiVar && buyukHarfVar && ozelKarakterVar)
            {
                discordGosterge.BackColor = Color.Green;
            }
            else if (discordSifre.Text.Length >= 6 && karakterVeSayiVar && ozelKarakterVar)
            {
                discordGosterge.BackColor = Color.Yellow;
            }
            else if (discordSifre.Text.Length >= 6 && karakterVeSayiVar && buyukHarfVar)
            {
                discordGosterge.BackColor = Color.Yellow;
            }
            else if (discordSifre.Text.Length >= 6 && karakterVeSayiVar)
            {
                discordGosterge.BackColor = Color.Red;
            }
            else
            {
                discordGosterge.BackColor = Color.Red;
            }
        }
    }
}
