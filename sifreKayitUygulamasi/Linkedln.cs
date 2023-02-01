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
    public partial class Linkedln : Form
    {
        bool surukle;
        Point start_point = new Point(0, 0);
        public string veri;
        OleDbConnection conn;
        OleDbDataAdapter da;
        OleDbCommand cmd;
        DataSet ds;

        public Linkedln()
        {
            InitializeComponent();
        }

        private void linkedlnKapat_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void linkedlnKucult_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void linkedlnGeri_Click(object sender, EventArgs e)
        {
            Form1 git = new Form1();
            git.veri = label1.Text;
            git.Show();
            this.Hide();
        }

        private void linkedlnPanel_MouseDown(object sender, MouseEventArgs e)
        {
            surukle = true;
            start_point = new Point(e.X, e.Y);
        }

        private void linkedlnPanel_MouseUp(object sender, MouseEventArgs e)
        {
            surukle = false;
        }

        private void linkedlnPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (surukle)
            {
                Point p = PointToScreen(e.Location);
                this.Location = new Point(p.X - start_point.X, p.Y - start_point.Y);
            }
        }

        void temizle()
        {
            linkedlnEposta.Text = "";
            linkedlnSifre.Text = "";

        }

        private void linkedlnSifre_TextChanged(object sender, EventArgs e)
        {
            bool karakterVeSayiVar = false;
            bool buyukHarfVar = false;
            bool ozelKarakterVar = false;

            foreach (char c in linkedlnSifre.Text)
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
                    ozelKarakterVar = true;  //sayi değilse rakam da değilse özel karakterdir
                }
            }

            if (linkedlnSifre.Text.Length >= 6 && karakterVeSayiVar && buyukHarfVar && ozelKarakterVar)
            {
                linkedlnGosterge.BackColor = Color.Green;
            }
            else if (linkedlnSifre.Text.Length >= 6 && karakterVeSayiVar && ozelKarakterVar)
            {
                linkedlnGosterge.BackColor = Color.Yellow;
            }
            else if (linkedlnSifre.Text.Length >= 6 && karakterVeSayiVar && buyukHarfVar)
            {
                linkedlnGosterge.BackColor = Color.Yellow;
            }
            else if (linkedlnSifre.Text.Length >= 6 && karakterVeSayiVar)
            {
                linkedlnGosterge.BackColor = Color.Red;
            }
            else
            {
                linkedlnGosterge.BackColor = Color.Red;
            }
        }

        private void linkedlnGuncelle_Click(object sender, EventArgs e)
        {
            if (linkedlnEposta.Text == "" || linkedlnSifre.Text == "")
            {
                MessageBox.Show("Boş Alan Hatası");
            }
            else
            {
                cmd = new OleDbCommand();
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "UPDATE linkedln set parola=@parola where eposta='" + linkedlnEposta.Text + "'";
                cmd.Parameters.AddWithValue("@parola", linkedlnSifre.Text);

                cmd.ExecuteNonQuery(); //sonuc doner kac tane islem yaptıysan
                if (cmd.ExecuteNonQuery() > 0)
                    MessageBox.Show("Basariyla Guncellendi");
                else
                    MessageBox.Show("Guncellenemedi");

                conn.Close();
                fillGrid();
                temizle();
                linkedlnListele_Click(sender, e);

            }
        }

        private void linkedlnListele_Click(object sender, EventArgs e)
        {
            conn = new OleDbConnection("Provider=Microsoft.ACE.Oledb.12.0;Data Source=DataBase.accdb");
            string query = "Select eposta, parola from linkedln where anaKullaniciAdi = @anaKullaniciAdi";
            cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("@anaKullaniciAdi", label1.Text);
            ds = new DataSet();
            conn.Open();
            using (OleDbDataReader reader = cmd.ExecuteReader())
            {

                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                linkedlnDataGridView.DataSource = dataTable;
            }
            conn.Close();
        }

        private void linkedlnSil_Click(object sender, EventArgs e)
        {
            if (linkedlnEposta.Text == "" || linkedlnSifre.Text == "")
            {
                MessageBox.Show("Boş Alan Hatası");
            }
            else
            {
                cmd = new OleDbCommand();
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "delete from linkedln where eposta='" + linkedlnEposta.Text + "'";
                if (cmd.ExecuteNonQuery() > 0)
                    MessageBox.Show("Basariyla Silindi");
                else
                    MessageBox.Show("Silinemedi");
                conn.Close();
                fillGrid();
                temizle();
                linkedlnListele_Click(sender, e);
            }
        }

        private void linkedlnKaydet_Click(object sender, EventArgs e)
        {
            if (linkedlnEposta.Text == "" || linkedlnSifre.Text == "")
            {
                MessageBox.Show("Boş Alan Hatası");
            }
            else
            {
                string query = "INSERT INTO linkedln (eposta,parola,anaKullaniciAdi) VALUES" + "(@eposta,@parola,@anaKullaniciAdi)";
                cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@eposta", linkedlnEposta.Text);
                cmd.Parameters.AddWithValue("@parola", linkedlnSifre.Text);
                cmd.Parameters.AddWithValue("@anaKullaniciAdi", label1.Text);
                conn.Open();
                if (cmd.ExecuteNonQuery() > 0)
                    MessageBox.Show("Basariyla Kaydedildi");
                else
                    MessageBox.Show("Kaydedilemedi");
                conn.Close();
                fillGrid();
                temizle();
                linkedlnListele_Click(sender, e);
            }
        }

        private void linkedlnRastgeleSifre_Click(object sender, EventArgs e)
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
            linkedlnSifre.Text = sifreStr;
        }

        private void linkedlnDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilenDeger = linkedlnDataGridView.SelectedCells[0].RowIndex;
            linkedlnEposta.Text = linkedlnDataGridView.Rows[secilenDeger].Cells[0].Value.ToString();// Hücreyi 1'den başlatmamızın sebebi 0. değerde ID olmasıdır.
            linkedlnSifre.Text = linkedlnDataGridView.Rows[secilenDeger].Cells[1].Value.ToString();
        }

        private void Linkedln_Load(object sender, EventArgs e)
        {
            label1.Text = veri;
            fillGrid();
        }

        void fillGrid()
        {
            conn = new OleDbConnection("Provider=Microsoft.ACE.Oledb.12.0;Data Source=DataBase.accdb");
            da = new OleDbDataAdapter("Select eposta, parola, anaKullaniciAdi from linkedln where anaKullaniciAdi = '" + label1.Text + "'", conn);
            DataTable dt = new DataTable();
            conn.Open();
            da.Fill(dt);
            conn.Close();

        }
    }
}
