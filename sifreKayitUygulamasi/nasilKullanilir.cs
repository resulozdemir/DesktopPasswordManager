using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sifreKayitUygulamasi
{
    public partial class nasilKullanilir : Form
    {
        bool surukle;
        Point start_point = new Point(0, 0);
        public string veri;
        
        public nasilKullanilir()
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

        private void resulozdemir_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/resulozdemir");

        }

        private void nasilKullanilirKapat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void nasilKullanilirKucult_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void nasilKullanilirGeri_Click(object sender, EventArgs e)
        {
            Form1 git = new Form1();
            git.veri = label1.Text;
            git.Show();
            this.Hide();
        }

        private void nasilKullanilir_Load(object sender, EventArgs e)
        {
            label1.Text = veri;
        }
    }
}
