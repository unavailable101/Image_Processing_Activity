using WebCamLib;
using ImageProcess2;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Image_Processing_Activity
{
    public partial class Form1 : Form
    {
        Bitmap loaded, processed, result;
        // for subtract:
        // loaded = imageB, processed = imageA, result = colorgreen

        Device[] camera;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            camera = DeviceManager.GetAllDevices();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void digitalImageProcessingToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            loaded = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = loaded;
        }

        private void basicCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (loaded == null) return;
            BasicDIP.BasicCopy(ref loaded, ref processed);
            pictureBox2.Image = processed;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            processed.Save(saveFileDialog1.FileName);
        }

        private void greyscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (loaded == null) return;
            //BasicDIP.Greyscale(ref loaded, ref processed);
            processed = new Bitmap(loaded);
            BitmapFilter.GrayScale(processed);
            pictureBox2.Image = processed;
        }

        private void colorInversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (loaded == null) return;
            //BasicDIP.ColorInvert(ref loaded, ref processed);
            processed = new Bitmap(loaded);
            BitmapFilter.Invert(processed);
            pictureBox2.Image = processed;
        }

        private void histogramToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (loaded == null) return;
            BasicDIP.Histogram(ref loaded, ref processed);
            pictureBox2.Image = processed;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (loaded == null) return;
            //BasicDIP.Brightness(ref loaded, ref processed, trackBar1.Value);
            processed = new Bitmap(loaded);
            BitmapFilter.Brightness(processed, trackBar1.Value);
            pictureBox2.Image = processed;
        }

        private void sepiaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (loaded == null) return;
            BasicDIP.Sepia(ref loaded, ref processed);
            pictureBox2.Image = processed;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            if (loaded == null) return;
            //BasicDIP.Equalisation(ref loaded, ref processed, trackBar2.Value / 100);
            processed = new Bitmap(loaded);
            BitmapFilter.Contrast(processed, (SByte)(trackBar2.Value/100));
            pictureBox2.Image = processed;
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (loaded == null) return;
            BasicDIP.MirrorHor(ref loaded, ref processed);
            pictureBox2.Image = processed;
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (loaded == null) return;
            BasicDIP.MirrorVert(ref loaded, ref processed);
            pictureBox2.Image = processed;
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            if (loaded == null) return;
            BasicDIP.Rotate(ref loaded, ref processed, trackBar3.Value);
            pictureBox2.Image = processed;
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            if (loaded == null) return;
            BasicDIP.Scale(ref loaded, ref processed, trackBar4.Value * (loaded.Width / 100), trackBar4.Value * (loaded.Height / 100));
            pictureBox2.Image = processed;
        }

        private void binaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (loaded == null) return;
            BasicDIP.BinaryThreshold(ref loaded, ref processed, 180);
            pictureBox2.Image = processed;
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            if (loaded == null) return;
            BasicDIP.BinaryThreshold(ref loaded, ref processed, trackBar5.Value);
            pictureBox2.Image = processed;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
        }

        private void openFileDialog2_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            loaded = new Bitmap(openFileDialog2.FileName);
            pictureBox1.Image = loaded;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog3.ShowDialog();
        }

        private void openFileDialog3_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            processed = new Bitmap(openFileDialog3.FileName);
            pictureBox2.Image = processed;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (loaded == null || processed == null) return;

            int greygreen = 255 / 3;
            int threshold = 5;

            result = new Bitmap(loaded.Width, loaded.Height);

            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {

                    Color pixel = loaded.GetPixel(x, y);
                    Color bgpixel = processed.GetPixel(x, y);

                    int grey = (pixel.R + pixel.G + pixel.B) / 3;

                    int subtractval = Math.Abs(grey - greygreen);

                    if (subtractval < threshold) result.SetPixel(x, y, bgpixel);
                    else result.SetPixel(x, y, pixel);
                }
            }
            pictureBox3.Image = result;
        }

        private void onToolStripMenuItem_Click(object sender, EventArgs e)
        {
            camera[0].ShowWindow(pictureBox1);
        }

        private void offToolStripMenuItem_Click(object sender, EventArgs e)
        {
            camera[0].Stop();
        }

        private void grayscaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer6.Enabled = false;
            timer2.Enabled = false;
            timer3.Enabled = false;
            timer4.Enabled = false;
            timer5.Enabled = false;

            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //get 1 frame
            IDataObject data;
            Image bmap;
            camera[0].Sendmessage();
            data = Clipboard.GetDataObject();
            bmap = (Image)(data.GetData("System.Drawing.Bitmap", true));
            Bitmap video = new Bitmap(bmap);

            processed = new Bitmap(video.Width, video.Height);
            Color pixel;
            int ave;
            for (int x = 0; x < video.Width; x++)
            {
                for (int y = 0; y < video.Height; y++)
                {
                    pixel = video.GetPixel(x, y);
                    ave = (int)(pixel.R + pixel.G + pixel.B) / 3;
                    Color grey = Color.FromArgb(ave, ave, ave);
                    processed.SetPixel(x, y, grey);
                }
            }
            pictureBox2.Image = processed;
        }

        private void greyscaleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            timer6.Enabled = false;
            timer3.Enabled = false;
            timer4.Enabled = false;
            timer5.Enabled = false;

            timer2.Enabled = true;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            //get 1 frame
            IDataObject data;
            Image bmap;
            camera[0].Sendmessage();
            data = Clipboard.GetDataObject();
            bmap = (Image)(data.GetData("System.Drawing.Bitmap", true));
            Bitmap video = new Bitmap(bmap);

            processed = new Bitmap(video.Width, video.Height);

            Color pixel;

            for (int x = 0; x < video.Width; x++)
            {
                for (int y = 0; y < video.Height; y++)
                {
                    pixel = video.GetPixel(x, y);
                    processed.SetPixel(x, y, pixel);
                }
            }
            pictureBox2.Image = processed;
        }

        private void inversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            timer2.Enabled = false;
            timer6.Enabled = false;
            timer4.Enabled = false;
            timer5.Enabled = false;

            timer3.Enabled = true;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            IDataObject data;
            Image bmap;
            camera[0].Sendmessage();
            data = Clipboard.GetDataObject();
            bmap = (Image)(data.GetData("System.Drawing.Bitmap", true));
            Bitmap video = new Bitmap(bmap);

            processed = new Bitmap(video.Width, video.Height);

            Color pixel;

            for (int x = 0; x < video.Width; x++)
            {
                for (int y = 0; y < video.Height; y++)
                {
                    pixel = video.GetPixel(x, y);
                    Color invert = Color.FromArgb(255 - pixel.R, 255 - pixel.G, 255 - pixel.B);
                    processed.SetPixel(x, y, invert);
                }
            }
            pictureBox2.Image = processed;
        }

        private void sepiaToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            timer2.Enabled = false;
            timer3.Enabled = false;
            timer6.Enabled = false;
            timer5.Enabled = false;

            timer4.Enabled = true;
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            IDataObject data;
            Image bmap;
            camera[0].Sendmessage();
            data = Clipboard.GetDataObject();
            bmap = (Image)(data.GetData("System.Drawing.Bitmap", true));
            Bitmap video = new Bitmap(bmap);

            processed = new Bitmap(video.Width, video.Height);

            Color pixel;
            int r, g, b;

            for (int x = 0; x < video.Width; x++)
            {
                for (int y = 0; y < video.Height; y++)
                {
                    pixel = video.GetPixel(x, y);
                    r = (int)(pixel.R * 0.393 + pixel.G * 0.769 + pixel.B * 0.189);
                    g = (int)(pixel.R * 0.349 + pixel.G * 0.686 + pixel.B * 0.168);
                    b = (int)(pixel.R * 0.272 + pixel.G * 0.534 + pixel.B * 0.131);
                    Color sep = Color.FromArgb(Math.Min(r, 255), Math.Min(g, 255), Math.Min(b, 255));
                    processed.SetPixel(x, y, sep);
                }
            }
            pictureBox2.Image = processed;
        }

        private void mirrorHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void mirrorHorizontalToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            timer2.Enabled = false;
            timer3.Enabled = false;
            timer4.Enabled = false;
            timer6.Enabled = false;

            timer5.Enabled = true;
        }

        private void mirrorVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            timer2.Enabled = false;
            timer3.Enabled = false;
            timer4.Enabled = false;
            timer5.Enabled = false;

            timer6.Enabled = true;
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            IDataObject data;
            Image bmap;
            camera[0].Sendmessage();
            data = Clipboard.GetDataObject();
            bmap = (Image)(data.GetData("System.Drawing.Bitmap", true));
            Bitmap video = new Bitmap(bmap);

            processed = new Bitmap(video.Width, video.Height);

            Color pixel;

            for (int x = 0; x < video.Width; x++)
            {
                for (int y = 0; y < video.Height; y++)
                {
                    pixel = video.GetPixel(x, y);
                    processed.SetPixel(video.Width - 1 - x, y, pixel);
                }
            }
            pictureBox2.Image = processed;
        }

        private void timer6_Tick(object sender, EventArgs e)
        {
            IDataObject data;
            Image bmap;
            camera[0].Sendmessage();
            data = Clipboard.GetDataObject();
            bmap = (Image)(data.GetData("System.Drawing.Bitmap", true));
            Bitmap video = new Bitmap(bmap);

            processed = new Bitmap(video.Width, video.Height);

            Color pixel;

            for (int x = 0; x < video.Width; x++)
            {
                for (int y = 0; y < video.Height; y++)
                {
                    pixel = video.GetPixel(x, y);
                    processed.SetPixel( x, video.Height - 1 - y, pixel);
                }
            }
            pictureBox2.Image = processed;
        }
    }
}
