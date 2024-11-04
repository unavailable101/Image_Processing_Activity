using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Image_Processing_Activity
{
    static class BasicDIP
    {
        public static void BinaryThreshold(ref Bitmap loaded, ref Bitmap result, int value)
        {
            result = new Bitmap(loaded.Width, loaded.Height);
            Color pixel;
            int ave;
            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    ave = (int)(pixel.R + pixel.G + pixel.B) / 3;

                    if (ave < value) result.SetPixel(x, y, Color.Black);
                    else result.SetPixel(x, y, Color.White);

                }
            }

        }
        public static void Scale(ref Bitmap loaded, ref Bitmap result, int targetWidth, int targetHeight) { 
            int xTarget, yTarget, xSource, ySource;

            result = new Bitmap(loaded.Width, loaded.Height);

            for (xTarget = 0; xTarget < targetWidth; xTarget++)
            {
                for (yTarget = 0; yTarget < targetHeight; yTarget++) { 
                    xSource = xTarget + loaded.Width / targetWidth;
                    ySource = yTarget + loaded.Height/ targetHeight;

                    result.SetPixel(xTarget,yTarget, loaded.GetPixel(xSource,ySource));
                }
            }
        }
        public static void Rotate(ref Bitmap loaded, ref Bitmap result, int value)
        {
            float angle = (float)(value*Math.PI / 180);
            int xCenter = (int)(loaded.Width / 2);
            int yCenter = (int)(loaded.Height / 2);
            int xs, ys, xp, yp, x0, y0;

            float cosA, sinA;

            cosA = (float)Math.Cos(angle);
            sinA = (float)Math.Sin(angle);

            result = new Bitmap(loaded.Width, loaded.Height);

            for (xp = 0; xp < loaded.Width; xp++) {
                for (yp = 0; yp < loaded.Height; yp++) {
                    x0 = xp - xCenter;  //translate to (0,0)
                    y0 = yp - yCenter;

                    xs = (int)(x0 * cosA + y0 * sinA);  //rotate the origin
                    ys = (int)(-x0 * sinA + y0 * cosA);

                    //transalte back to xCenter yCenter
                    xs = (int)(xs + xCenter);
                    ys = (int)(ys + yCenter);

                    //set bounds
                    xs = Math.Max(0, Math.Min(loaded.Width - 1, xs));
                    ys = Math.Max(0, Math.Min(loaded.Height - 1, ys));

                    result.SetPixel(xp, yp, loaded.GetPixel(xs, ys));
                }
            }
        }
        public static void MirrorVert(ref Bitmap loaded, ref Bitmap result)
        {
            result = new Bitmap(loaded.Width, loaded.Height);
            Color pixel;
            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    result.SetPixel(x, loaded.Height - 1 - y, pixel);
                }
            }
        }
        public static void MirrorHor(ref Bitmap loaded, ref Bitmap result)
        {
            result = new Bitmap(loaded.Width, loaded.Height);
            Color pixel;
            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    result.SetPixel(loaded.Width - 1 - x, y, pixel);
                }
            }
        }

        public static void Equalisation(ref Bitmap loaded, ref Bitmap result, int percent)
        {
            int height = loaded.Height;
            int width = loaded.Width;
            int numSamples, histSum;
            int[] Ymap = new int[256];
            int[] hist = new int[256];

            //Compute the histogram from sub-image
            Color nakuha;
            Color grey;
            Byte greydata;

            // compute greyscale
            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    nakuha = loaded.GetPixel(x, y);
                    greydata = (byte)((nakuha.R + nakuha.G + nakuha.B) / 3);
                    grey = Color.FromArgb(greydata, greydata, greydata);
                    loaded.SetPixel(x, y, grey);
                }
            }

            //histogram 1d data
            for (int x = 0; (x < loaded.Width); x++) { 
                for (int y = 0; y < loaded.Height; y++)
                {
                    nakuha= loaded.GetPixel(x, y);
                    hist[nakuha.B]++;
                }
            }

            //remap the Ys, use the maximum contrast (percent == 100)
            //base on histogram equalisation
            numSamples = (loaded.Height * loaded.Width);        // no. of samples contributed to the histogram
            histSum = 0;
            for (int h = 0; h < 256; h++) { 
                histSum += hist[h];
                Ymap[h] = histSum * 255 / numSamples;
            }

            // if desired contrast is not maximum (percent < 100), then adjust the mapping
            if (percent < 100)
            {
                for (int h = 0; h < 256; h++)
                {
                    Ymap[h] = h + ((int)Ymap[h] - h) * percent / 100;
                }
            }

            result = new Bitmap(loaded.Width, loaded.Height);

            //enhance the region by mapping the intensities
            for (int y = 0; y < loaded.Height; y++)
            {
                for (int x = 0; x < loaded.Width; x++) {
                    // set the new value to greyscale value
                    Color temp = Color.FromArgb(Ymap[loaded.GetPixel(x, y).R], Ymap[loaded.GetPixel(x, y).G], Ymap[loaded.GetPixel(x, y).B]);
                    result.SetPixel(x, y, temp);
                }
            }
        }

        public static void BasicCopy(ref Bitmap loaded, ref Bitmap result)
        {
            result = new Bitmap(loaded.Width, loaded.Height);
            Color pixel;
            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    result.SetPixel(x, y, pixel);
                }
            }
        }
        public static void Greyscale (ref Bitmap loaded, ref  Bitmap result)
        {
            result = new Bitmap(loaded.Width, loaded.Height);
            Color pixel;
            int ave;
            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    ave = (int)(pixel.R + pixel.G + pixel.B) / 3;
                    Color grey = Color.FromArgb(ave, ave, ave);
                    result.SetPixel(x, y, grey);
                }
            }
        }
        public static void ColorInvert(ref Bitmap loaded, ref Bitmap result)
        {
            result = new Bitmap(loaded.Width, loaded.Height);
            Color pixel;
            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    Color invert = Color.FromArgb(255 - pixel.R, 255 - pixel.G, 255 - pixel.B);
                    result.SetPixel(x, y, invert);
                }
            }
        }
        public static void Sepia(ref Bitmap loaded, ref Bitmap result)
        {
            result = new Bitmap(loaded.Width, loaded.Height);
            Color pixel;
            int r, g, b;
            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    r = (int)(pixel.R * 0.393 + pixel.G * 0.769 + pixel.B * 0.189);
                    g = (int)(pixel.R * 0.349 + pixel.G * 0.686 + pixel.B * 0.168);
                    b = (int)(pixel.R * 0.272 + pixel.G * 0.534 + pixel.B * 0.131);
                    Color sep= Color.FromArgb(Math.Min(r, 255), Math.Min(g, 255), Math.Min(b, 255));
                    result.SetPixel(x, y, sep);
                }
            }
        }
        public static void Brightness(ref Bitmap loaded, ref Bitmap result, int value)
        {
            result = new Bitmap(loaded.Width, loaded.Height);

            for (int x = 0; x < loaded.Width; x++) {
                for (int y = 0; y < loaded.Height; y++) { 
                    Color temp = loaded.GetPixel(x, y);
                    Color change;
                    if (value > 0)
                        change = Color.FromArgb( Math.Min(temp.R + value, 255 ), Math.Min(temp.G + value, 255), Math.Min(temp.B + value, 255));
                    else
                        change = Color.FromArgb( Math.Max(temp.R + value, 255 ), Math.Max(temp.G + value, 255), Math.Max(temp.B + value, 255));

                    result.SetPixel(x, y, change);

                }
            }
        }

        public static void Histogram(ref Bitmap loaded, ref Bitmap result)
        {
            //greyscale ni sha na histogram
            Color sample;
            Color grey;
            Byte greydata;

            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    sample = loaded.GetPixel(x, y);
                    greydata = (byte)((sample.R + sample.G + sample.B) / 3);
                    grey = Color.FromArgb(greydata, greydata, greydata);
                    loaded.SetPixel(x, y, grey);
                }
            }


            int[] histo_data = new int[256];

            for (int x = 0; x < loaded.Width; x++) {
                for (int y = 0; y < loaded.Height; y++) { 
                    sample = loaded.GetPixel(x, y);
                    histo_data[sample.R]++;
                }
            }

            result = new Bitmap(256, 800);
            for (int x = 0; x < 256; x++) {
                for (int y = 0; y < 800; y++) { 
                    result.SetPixel(x, y, Color.White);
                }
            }
            //plot sa data
            for (int x = 0; x < 256; x++) {
                for (int y = 0; y < Math.Min(histo_data[x] / 5 , result.Height-1); y++) { 
                    result.SetPixel(x, (result.Height - 1) - y, Color.Black);
                }
            }
        }
    }
}
