using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Imaging.Filters;
using AForge.Imaging;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Vision;
using AForge.Vision.Motion;
using System.IO;
using System.Collections;
using System.IO.Ports;

namespace step_motorla_nesne_takibi
{
    public partial class Form1 : Form
    {
        int led;
        int gecici;

        private FilterInfoCollection cihazlar;
        private VideoCaptureDevice PCwebcam;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cihazlar = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            foreach (FilterInfo cihaz in cihazlar)
            {
                comboBox1.Items.Add(cihaz.Name);
            }

            PCwebcam = new VideoCaptureDevice();
            button3.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (PCwebcam.IsRunning)
            {
                PCwebcam.Stop();
                pictureBox1.Image = null;
                pictureBox1.Invalidate();
                button1.Text = "Başlat";
            }
            else
            {
                PCwebcam = new VideoCaptureDevice(cihazlar[comboBox1.SelectedIndex].MonikerString);
                PCwebcam.NewFrame += kamera_NewFrame;
                PCwebcam.Start();
                button1.Text = "Durdur";
            }
        }

        private void kamera_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap image = (Bitmap)eventArgs.Frame.Clone();
            Bitmap reelimage= (Bitmap)eventArgs.Frame.Clone();
            image.RotateFlip(RotateFlipType.Rotate180FlipY);
            reelimage.RotateFlip(RotateFlipType.Rotate180FlipY);

            EuclideanColorFiltering filter = new EuclideanColorFiltering();
            filter.CenterColor = new RGB(Color.FromArgb(215, 0, 0));
            filter.Radius = 120;
            filter.ApplyInPlace(image);

            cevreal(image);

            pictureBox2.Image = reelimage;
            pictureBox1.Image = image;

        }

        private void cevreal(Bitmap image)
        {
            BlobCounter blobcounter = new BlobCounter();
            blobcounter.MinWidth = 20;
            blobcounter.MinHeight = 20;
            blobcounter.FilterBlobs = true;
            blobcounter.ObjectsOrder = ObjectsOrder.Size;

            Grayscale grı = new Grayscale(0.2125, 0.7154, 0.0721);
            Bitmap grıimage = grı.Apply(image);

            blobcounter.ProcessImage(grıimage);
            Rectangle[] rects = blobcounter.GetObjectsRectangles();

            foreach (Rectangle recs in rects)
            {
                if (rects.Length > 0)
                {
                    Rectangle nesneRect = rects[0];
                    Graphics g = Graphics.FromImage(image);
                        using (Pen pen = new Pen(Color.FromArgb(252, 3, 26), 2))
                        {
                            g.DrawRectangle(pen, nesneRect);
                        }

                    int nesneX = nesneRect.X + (nesneRect.Width / 2);
                    int nesneY = nesneRect.Y + (nesneRect.Height / 2);
                    g.DrawString(nesneX.ToString() + "X" + nesneY.ToString(), new Font("Arial", 50), Brushes.White, new System.Drawing.Point(0, 0));
                    g.Dispose();


                    if (nesneX > 0 && nesneX <= 213)
                    {
                        led = 1;
                    }
                    else if (nesneX> 213 && nesneX <= 426)
                    {
                        led = 2;
                    }
                    else if (nesneX > 426 && nesneX <= 640
                        )
                    {
                        led = 3;
                    }
                    
                    if (gecici!= led)
                    {
                        gecici = led;
                        if (serialPort1.IsOpen)
                        {
                            serialPort1.Write(led.ToString());
                        }
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string[] portlar = SerialPort.GetPortNames();
            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(portlar);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text != "")
            {
                serialPort1.PortName = comboBox2.Text;
                serialPort1.BaudRate = 9600;
                serialPort1.Parity = Parity.None;
                serialPort1.StopBits = StopBits.One;
                serialPort1.Open();
                button4.Enabled = false;
                button3.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }
            button4.Enabled = true;
            button3.Enabled = false;
        }
    }
}
