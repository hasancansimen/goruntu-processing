using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GRİ_metotları
{
    public partial class Form1 : Form
    {
    Bitmap  a, picturesource;
        private int s;
        private int ı;
        private int r;

        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "PNG|*.png";
            DialogResult result = saveFileDialog1.ShowDialog();
            ImageFormat format = ImageFormat.Png;
                if (result == DialogResult.OK)
                {
                    a.Save(saveFileDialog1.FileName, format);
                }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            panel1.Width = panel1.Width + 5;
                if (panel1.Width == 550)
                    timer1.Stop();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            panel1.Width = panel1.Width - 5;
                if (panel1.Width == 100)
                    timer2.Stop();
        }

        private void button3_Click(object sender, EventArgs e)
        {
                if (panel1.Width == 100)
                    timer1.Start();
                else timer2.Start();          
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = null;
            if ( pictureBox1.Image != null )
            {
                int gen = pictureBox1.Width;
                int uz = pictureBox1.Height;
                a = new Bitmap(gen, uz);
                for (int z = 0; z < uz; z++)
                {
                    for (int ac = 0; ac < gen; ac++)
                    {
                        Color aq = picturesource.GetPixel(ac, z);
                        double ez = (aq.R+aq.G+aq.B)/3;
                        int ec = Convert.ToInt32(ez);
                        Color gri = Color.FromArgb(ec, ec, ec);
                        a.SetPixel(ac, z, gri);

                    }
                }
                pictureBox2.Image = a;
            }
            else MessageBox.Show("RESİM SEÇMEDİNİZ...");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult image = openFileDialog1.ShowDialog();
                if( image == DialogResult.OK)
                {
                    picturesource = new Bitmap(openFileDialog1.FileName);
                    pictureBox1.Image = picturesource;
                }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = null;
            if(pictureBox1.Image!= null)
            {
                int gen = pictureBox1.Width;
                int uz = pictureBox1.Height;
                a = new Bitmap(gen, uz);
                for (int s = 0; s < uz; s++)
                {
                    for (int g = 0; g < gen; g++)
                    {
                        Color gc = picturesource.GetPixel(g,s);
                        double lm = (gc.R * 0.3) + (gc.G * 0.59) + (gc.B * 0.11);
                        int lma = Convert.ToInt32(lm);
                        Color gri = Color.FromArgb(lma, lma, lma);
                        a.SetPixel(g, s , gri);
                    }
                }
                pictureBox2.Image = a;
            }
            else MessageBox.Show("RESİM SEÇMEDİNİZ...");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = null;
            if (pictureBox1.Image != null)
            {
                int gen = pictureBox1.Width;
                int uz = pictureBox1.Height;
                a = new Bitmap(gen, uz);
                for(int d = 0; d < uz; d++)
                {
                    for (int b = 0; b < gen; b++)
                    {
                        Color gc = picturesource.GetPixel(b, d);
                        int[] array0 = { gc.R, gc.G, gc.B };
                        double acık = (array0.Max() + array0.Min()) / 2;
                        int acıklık = Convert.ToInt32(acık);
                        Color gri = Color.FromArgb(acıklık, acıklık, acıklık);
                        a.SetPixel(b, d, gri);
                    }
                }
                pictureBox2.Image = a;
            }
            else MessageBox.Show("RESİM SEÇMEDİNİZ...");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = null;
            if (pictureBox1.Image != null)
            {
                int gen = pictureBox1.Width;
                int uz = pictureBox1.Height;
                a = new Bitmap(gen, uz);
                for (int ı = 0; ı < uz; ı++)
                {
                    for (int r = 0; r < gen; r++)
                    {
                        Color gc = picturesource.GetPixel(r, ı);
                        double kl = gc.G;
                        int kla = Convert.ToInt32(kl);
                        Color gri = Color.FromArgb(kla, kla, kla);
                        a.SetPixel(r, ı, gri);
                    }
                }
                pictureBox2.Image = a;
            }
            else MessageBox.Show("RESİM SEÇMEDİNİZ...");
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click_2(object sender, EventArgs e)
        {
            pictureBox2.Image = null;
            if (pictureBox1.Image != null)
            {
                int gen = pictureBox1.Width;
                int uz = pictureBox1.Height;
                a = new Bitmap(gen, uz);
                for (int k = 0; k < uz; k++)
                {
                    for (int f = 0; f < gen; f++)
                    {
                        Color gc = picturesource.GetPixel(f, k);
                        double bt = (gc.R * 0.2125) + (gc.G * 0.7154) + (gc.B * 0.072);
                        int bta = Convert.ToInt32(bt);
                        Color gri = Color.FromArgb(bta, bta, bta);
                        a.SetPixel(f, k, gri);
                    }
                }
                pictureBox2.Image = a;
            }
            else MessageBox.Show("RESİM SEÇMEDİNİZ...");

        }
    }
}