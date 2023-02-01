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

namespace sifreKayitUygulamasi
{
    public partial class Instagram : Form
    {
        bool surukle;
        Point start_point = new Point(0, 0);
        OleDbConnection conn;
        OleDbDataAdapter da;
        OleDbCommand cmd;
        DataSet ds;

        public Instagram()
        {
            InitializeComponent();
        }

        private void instagramPanel_MouseUp(object sender, MouseEventArgs e)
        {
            surukle = false;
        }

        private void instagramPanel_MouseDown(object sender, MouseEventArgs e)
        {
            surukle = true;
            start_point = new Point(e.X, e.Y);
        }

        private void instagramPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (surukle)
            {
                Point p = PointToScreen(e.Location);
                this.Location = new Point(p.X - start_point.X, p.Y - start_point.Y);
            }
        }

        private void instagramKapat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void instagramKucult_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void instagramGeri_Click(object sender, EventArgs e)
        {
            Form1 git = new Form1();
            git.veri = label1.Text;
            git.Show();
            this.Hide();
        }

        void temizle()
        {
            instagramEposta.Text = "";
            instagramSifre.Text = "";
            instagramKullaniciAdi.Text = "";

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

        private void instgaramKaydet_Click(object sender, EventArgs e)
        {
            if (instagramKullaniciAdi.Text == "" || instagramSifre.Text == "" || instagramEposta.Text == "")
            {
                MessageBox.Show("Boş Alan Hatası");
            }
            else
            {
                string query = "INSERT INTO instagram (kullaniciAdi,parola,eposta,anaKullaniciAdi) VALUES" + "(@kullaniciAdi,@parola,@eposta,@anaKullaniciAdi)";
                cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@kullaniciAdi", instagramKullaniciAdi.Text);
                cmd.Parameters.AddWithValue("@parola", instagramSifre.Text);
                cmd.Parameters.AddWithValue("@eposta", instagramEposta.Text);
                cmd.Parameters.AddWithValue("@anaKullaniciAdi", label1.Text);
                conn.Open();
                if (cmd.ExecuteNonQuery() > 0)
                    MessageBox.Show("Basariyla Kaydedildi");
                else
                    MessageBox.Show("Kaydedilemedi");
                conn.Close();
                fillGrid();
                temizle();
                instagramListele_Click(sender, e);
            }
        }
        
        public string veri;
        private void Instagram_Load(object sender, EventArgs e)
        {
            label1.Text = veri;
            fillGrid();
        }

        private void instagramSil_Click(object sender, EventArgs e)
        {
            if (instagramKullaniciAdi.Text == "" || instagramSifre.Text == ""  || instagramEposta.Text == "")
            {
                MessageBox.Show("Boş Alan Hatası");
            }
            else
            {
                cmd = new OleDbCommand();
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "delete from instagram where kullaniciAdi='" + instagramKullaniciAdi.Text + "'";
                if (cmd.ExecuteNonQuery() > 0)
                    MessageBox.Show("Basariyla Silindi");
                else
                    MessageBox.Show("Silinemedi");
                conn.Close();
                fillGrid();
                temizle();
                instagramListele_Click(sender, e);
            }
        }

        private void instagramListele_Click(object sender, EventArgs e)
        {
            conn = new OleDbConnection("Provider=Microsoft.ACE.Oledb.12.0;Data Source=DataBase.accdb");
            string query = "Select kullaniciAdi, parola, eposta from instagram where anaKullaniciAdi = @anaKullaniciAdi";
            cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("@anaKullaniciAdi", label1.Text);
            ds = new DataSet();
            conn.Open();
            using (OleDbDataReader reader = cmd.ExecuteReader())
            {
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                instagramDataGridView.DataSource = dataTable;
            }
            conn.Close();

           
        }

        private void instagramGuncelle_Click(object sender, EventArgs e)
        {
            if (instagramKullaniciAdi.Text == "" || instagramSifre.Text == "" || instagramEposta.Text == "")
            {
                MessageBox.Show("Boş Alan Hatası");
            }
            else
            {
                cmd = new OleDbCommand();
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "UPDATE instagram set kullaniciAdi=@kullaniciAdi, parola=@parola where eposta='" + instagramEposta.Text + "'"; 
                cmd.Parameters.AddWithValue("@kullaniciAdi", instagramKullaniciAdi.Text);
                cmd.Parameters.AddWithValue("@parola", instagramSifre.Text);
                cmd.ExecuteNonQuery(); 
                if (cmd.ExecuteNonQuery() > 0)
                    MessageBox.Show("Basariyla Guncellendi");
                else
                    MessageBox.Show("Guncellenemedi");

                conn.Close();
                fillGrid();
                temizle();
                instagramListele_Click(sender, e);
            }
        }

        private void instagramSifre_TextChanged(object sender, EventArgs e)
        {
            bool karakterVeSayiVar = false;
            bool buyukHarfVar = false;
            bool ozelKarakterVar = false;

            foreach (char c in instagramSifre.Text)
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

            if (instagramSifre.Text.Length >= 6 && karakterVeSayiVar && buyukHarfVar && ozelKarakterVar)
            {
                instagramGosterge.BackColor = Color.Green;
            }
            else if (instagramSifre.Text.Length >= 6 && karakterVeSayiVar && ozelKarakterVar)
            {
                instagramGosterge.BackColor = Color.Yellow;
            }
            else if (instagramSifre.Text.Length >= 6 && karakterVeSayiVar && buyukHarfVar)
            {
                instagramGosterge.BackColor = Color.Yellow;
            }
            else if (instagramSifre.Text.Length >= 6 && karakterVeSayiVar)
            {
                instagramGosterge.BackColor = Color.Red;
            }
            else
            {
                instagramGosterge.BackColor = Color.Red;
            }
        }

        private void instagramDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilenDeger = instagramDataGridView.SelectedCells[0].RowIndex;
            instagramKullaniciAdi.Text = instagramDataGridView.Rows[secilenDeger].Cells[0].Value.ToString();// Hücreyi 1'den başlatmamızın sebebi 0. değerde ID olmasıdır.
            instagramSifre.Text = instagramDataGridView.Rows[secilenDeger].Cells[1].Value.ToString();
            instagramEposta.Text = instagramDataGridView.Rows[secilenDeger].Cells[2].Value.ToString();
        }

        private void instagramRastgeleSifre_Click(object sender, EventArgs e)
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
            instagramSifre.Text = sifreStr;

        }

        
    }
}
