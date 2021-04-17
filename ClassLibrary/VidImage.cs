using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using Emgu.CV;

namespace ClassLibrary
{
    public class VidImage
    {
        private string[] asciiString = new string[400];

        private Image vidImage;
        private FrameDimension dimension;
        private int numFrames;
        private int currentFrame;

        public VidImage()
        {
            vidImage = Image.FromFile(@"..\..\..\..\Repos\VideoSource\BadApple.gif");
            dimension = new FrameDimension(vidImage.FrameDimensionsList[0]);
            numFrames = 3000;
        }

        public VidImage(int currentFrame)
        {
            vidImage = Image.FromFile(@"..\..\..\..\Repos\VideoSource\BadApple.gif");
            dimension = new FrameDimension(vidImage.FrameDimensionsList[0]);
            numFrames = vidImage.GetFrameCount(dimension);
            this.currentFrame = currentFrame;
        }

      
        //GET & SETS
        public string[] GetAsciiString()
        {
            return this.asciiString;
        }
        public void SetAsciiString(string[] asciiString)
        {
            this.asciiString = asciiString;
        }

        public int GetNumFrames()
        {
            return this.numFrames;
        }

        public int GetCurrentFrame()
        {
            return this.currentFrame;
        }
        public void SetCurrentFrame(int currentFrame)
        {
            this.currentFrame = currentFrame;
        }


        //METHODS
        public void ThreadCallBack(object newBactchAmmount)
        {
            int batchAmmount = (int)newBactchAmmount;

            for (int i = 0; i < batchAmmount; i++)
            {
                asciiString[i] = GrayscaleImageToASCII(NextFrame());

                Console.WriteLine(i);
            }
            Console.WriteLine($"thread finished!");
        }

        public Image NextFrame()
        {
            currentFrame++;
            vidImage.SelectActiveFrame(dimension, currentFrame); //find the frame
            return (Image)vidImage.Clone(); //return a copy of it
        }

        public string GrayscaleImageToASCII(Image img)
        {
            StringBuilder asciiString = new StringBuilder();
            Bitmap bmp = null;
            Size size = new Size(170, 64);

            // Create a bitmap from the image

            bmp = new Bitmap(img, size);

            // Loop through each pixel in the bitmap

            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color col = bmp.GetPixel(x, y);

                    int gValue = col.R; //between 0-255.

                    // Append the "color" using various darknesses of ASCII character.

                    asciiString.Append(GetGrayShade(gValue));

                    // If we're at the width, insert a line break

                    if (x == bmp.Width - 1)
                        asciiString.Append("\n");
                }
            }
            return asciiString.ToString();
        }   

        public string GetGrayShade(int color)
        {
            string asciival = " ";

            //BLACK
            if (color <= 50) { asciival = " "; } 

            else if (color <= 70) { asciival = "."; }

            else if (color <= 100) { asciival = "*"; }

            else if (color <= 130) { asciival = ":"; }

            else if (color <= 160) { asciival = "o"; }

            else if (color <= 180) { asciival = "&"; }

            else if (color <= 200) { asciival = "8"; }

            else if (color <= 230) { asciival = "#"; }

            else { asciival = "@"; }
            //WHITE

            return asciival;
        }

        public void SaveAscii(string path, List<VidImage> vidImageList)
        {
            StreamWriter w = new StreamWriter(path);

            for (int i = 0; i < vidImageList.Count; i++)
            {
                string[] temp = vidImageList[i].GetAsciiString();

                for (int j = 0; j < temp.Length; j++)
                {
                    Console.SetCursorPosition(0, 0);
                    w.WriteLine(temp[j]);
                    Thread.Sleep(30);
                }
            }
            w.Close();
        }

        public void LoadAscii(string path, VidImage vidImage)
        {
            StreamReader rLine = new StreamReader(path);
            string[] ascii = new string[GetNumFrames()];
            int lines = 0;

            while (rLine.ReadLine() != null)
            {
                for (int i = 0; i < 64; i++)
                {
                    ascii[lines] += rLine.ReadLine();
                    ascii[lines] += "\n";
                }
                lines++;
            }
            rLine.Close();

            for (int i = 0; i < GetNumFrames(); i++)
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine(ascii[i]);
                Thread.Sleep(28);
            }
        }
    }
}
