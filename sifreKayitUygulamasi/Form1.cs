using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//instagrama tıklandığında direk internet sitesi açılsın 
//kayıt ol ekranındaki sifre gostergesini hallet



namespace sifreKayitUygulamasi
{
    public partial class Form1 : Form
    {
        bool surukle;
        Point start_point = new Point(0, 0);
        
        public Form1()
        {
            InitializeComponent();
        }

        private void anaEkranPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (surukle)
            {
                Point p = PointToScreen(e.Location);
                this.Location = new Point(p.X - start_point.X, p.Y - start_point.Y);
            }
        }

        private void anaEkranPanel_MouseDown(object sender, MouseEventArgs e)
        {
            surukle = true;
            start_point = new Point(e.X, e.Y);
        }

        private void anaEkranPanel_MouseUp(object sender, MouseEventArgs e)
        {
            surukle = false;
        }

        private void form1Kapat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void form1Kucult_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void form1Geri_Click(object sender, EventArgs e)
        {
            girisPaneli git = new girisPaneli();
            git.Show();
            this.Hide();
        }

        private void profil_Click(object sender, EventArgs e)
        {
            profil git = new profil();
            git.veri = label2.Text;
            git.Show();
            this.Hide();
        }

        private void nasilKullanilirButon_Click(object sender, EventArgs e)
        {
            nasilKullanilir git = new nasilKullanilir();
            git.veri = label2.Text;
            git.Show();
            this.Hide();
        }

        private void gmailBilgileriGetir_Click(object sender, EventArgs e)
        {
            Gmail git = new Gmail();
            git.veri = label2.Text;
            git.Show();
            this.Hide();
        }

        private void twitterBilgileriGetir_Click(object sender, EventArgs e)
        {
            Twitter git = new Twitter();
            git.veri = label2.Text;
            git.Show();
            this.Hide();
        }

        private void instagramBilgileriGetir_Click(object sender, EventArgs e)
        {
            Instagram git = new Instagram();
            git.veri = label2.Text;
            git.Show();
            this.Hide();
        }

        private void linkedlnBilgileriGetir_Click(object sender, EventArgs e)
        {
            Linkedln git = new Linkedln();
            git.veri = label2.Text;
            git.Show();
            this.Hide();
        }

        private void githubBilgileriGetir_Click(object sender, EventArgs e)
        {
            GitHub git = new GitHub();
            git.veri = label2.Text;
            git.Show();
            this.Hide();
        }

        private void zoomBilgileriGetir_Click(object sender, EventArgs e)
        {
            Zoom git = new Zoom();
            git.veri = label2.Text;
            git.Show();
            this.Hide();
        }

        private void steamBilgileriGetir_Click(object sender, EventArgs e)
        {
            Steam git = new Steam();
            git.veri = label2.Text;
            git.Show();
            this.Hide();
        }

        private void outlookBilgileriGetir_Click(object sender, EventArgs e)
        {
            Outlook git = new Outlook();
            git.veri = label2.Text;
            git.Show();
            this.Hide();
        }

        private void discordBilgileriGetir_Click(object sender, EventArgs e)
        {
            Discord git = new Discord();
            git.discord_veri = label2.Text;
            git.Show();
            this.Hide();
        }

        private void netflixBilgileriGetir_Click(object sender, EventArgs e)
        {
            Netflix git = new Netflix();
            git.veri = label2.Text;
            git.Show();
            this.Hide();
        }

        public string veri;
        private void Form1_Load(object sender, EventArgs e)
        {
            label2.Text = veri;
            
        }
    }
}
