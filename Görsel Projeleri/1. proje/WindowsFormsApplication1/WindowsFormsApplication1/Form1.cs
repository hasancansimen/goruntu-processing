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

namespace WindowsFormsApplication1
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
            Bitmap reelimage = (Bitmap)eventArgs.Frame.Clone();
            image.RotateFlip(RotateFlipType.Rotate180FlipY);
            reelimage.RotateFlip(RotateFlipType.Rotate180FlipY);

            EuclideanColorFiltering filter = new EuclideanColorFiltering();
            filter.CenterColor = new RGB(Color.FromArgb(215, 0, 0)); // Algılanacak Renk ve merkez noktası bulunur.
            filter.Radius = 120;
            filter.ApplyInPlace(image);//Filitre Çalıştırılır.             

            cevreal(image);// Algilanan rengi Çevrçevelemek veya hedeflemek için gerekli Method.
                           // cevreal(reelimage);

            pictureBox2.Image = reelimage;
            pictureBox1.Image = image;
        }
        private void cevreal(Bitmap image)// Algilanan rengi Çevrçevelemek veya hedeflemek için gerekli Method.
        {
            BlobCounter blobCounter = new BlobCounter();
            blobCounter.MinWidth = 20;
            blobCounter.MinHeight = 20;
            blobCounter.FilterBlobs = true;
            blobCounter.ObjectsOrder = ObjectsOrder.Size;

            Grayscale grayFilter = new Grayscale(0.2125, 0.7154, 0.0721);
            Bitmap grayImage = grayFilter.Apply(image);

            blobCounter.ProcessImage(grayImage);
            Rectangle[] rects = blobCounter.GetObjectsRectangles();
            foreach (Rectangle recs in rects)
            {

                if (rects.Length > 0)
                {
                    Rectangle nesneRect = rects[0];
                    //Graphics g = pictureBox2.CreateGraphics();
                    Graphics g = Graphics.FromImage(image);
                    using (Pen pen = new Pen(Color.FromArgb(252, 3, 26), 2))
                    {
                        g.DrawRectangle(pen, nesneRect);
                    }

                    int nesneX = nesneRect.X + (nesneRect.Width / 2); //Dikdörtgenin Koordinatlari alınır.
                    int nesneY = nesneRect.Y + (nesneRect.Height / 2);//Dikdörtgenin Koordinatlari alınır.
                    g.DrawString(nesneX.ToString() + "X" + nesneY.ToString(), new Font("Arial", 50), Brushes.White, new System.Drawing.Point(0, 0));
                    g.Dispose();

                    if (nesneX > 0 && nesneX <= 426 && nesneY > 0 && nesneY <= 240)
                    {
                        led = 2;
                    }
                    else if (nesneX > 426 && nesneX <= 852 && nesneY > 0 && nesneY <= 240)
                    {
                        led = 3;
                    }
                    else if (nesneX > 852 && nesneX <= 1280 && nesneY > 0 && nesneY <= 240)
                    {
                        led = 4;
                    }
                    else if (nesneX > 0 && nesneX <= 426 && nesneY > 240 && nesneY <= 480)
                    {
                        led = 5;
                    }
                    else if (nesneX > 426 && nesneX <= 852 && nesneY > 240 && nesneY <= 480)
                    {
                        led = 6;
                    }
                    else if (nesneX > 852 && nesneX <= 1280 && nesneY > 240 && nesneY <= 480)
                    {
                        led = 7;
                    }
                    else if (nesneX > 0 && nesneX <= 426 && nesneY > 480 && nesneY <= 720)
                    {
                        led = 8;
                    }
                    else if (nesneX > 426 && nesneX <= 852 && nesneY > 480 && nesneY <= 720)
                    {
                        led = 9;
                    }
                    else if (nesneX > 852 && nesneX <= 1280 && nesneY > 480 && nesneY <= 720)
                    {
                        led = 10;
                    }
                    if( gecici != led)
                    {
                        gecici = led;
                        if(serialPort1.IsOpen)
                        {
                            serialPort1.Write(led.ToString());
                        }
                    }
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
           
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
