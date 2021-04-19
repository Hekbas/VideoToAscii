using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using Emgu.CV;
using Emgu.CV.Structure;

namespace VideoToAscii
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> asciiStringList = new List<string>();

            Program program = new Program();

            Console.CursorVisible = false;
            ConsoleKeyInfo op;

            do
            {
                Console.Clear();

                Console.WriteLine("MENU PRINCIPAL\n" +
                    "1 - Convert Video to Ascii (And play it)\n" +
                    "2 - Save Ascii to .txt\n" +
                    "3 - Play Ascii from .txt\n" +
                    "4 - *Secret feature*");

                op = Console.ReadKey(true);

                if (op.Key == ConsoleKey.D1)    //Convert video to Ascii + play it
                {
                    Console.Clear();
                    program.VideoToAScii(asciiStringList);
                    Console.WriteLine("Done lol");
                    Console.ReadLine();                
                }
                else if (op.Key == ConsoleKey.D2)   //Save asci into .txt
                {                   
                    Console.WriteLine("Saving video...");

                    program.SaveAscii(@"..\..\..\..\Repos\SavedAscii\Ascii.txt", asciiStringList);

                    Console.WriteLine("Video saved!");
                    Console.ReadLine();
                }
                else if (op.Key == ConsoleKey.D3)   //Play Ascii from.txt
                {                    
                    program.LoadAscii(@"..\..\..\..\Repos\SavedAscii\Ascii.txt", asciiStringList);
                    Console.WriteLine("No more video to play :O");
                    Console.ReadLine();
                }
                else if (op.Key == ConsoleKey.D3)
                {
                    Console.WriteLine("*Awesome secret feature appears*");
                }
                else if (op.Key == ConsoleKey.Escape)
                {
                    Environment.Exit(0);
                }
            } while (op.Key != ConsoleKey.Escape);
        }

        public void VideoToAScii(List<string> asciiStringList)
        {
            VideoCapture vc = new VideoCapture(@"..\..\..\..\Repos\VideoSource\BadApple.mp4");
            int numFrames = (int)vc.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount);
            int fps = (int)vc.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps);

            int frameTime = 1000 / fps;
            long processTime = 0;
            bool reachedEnd = false;

            Stopwatch sw = new Stopwatch(); ;
            Console.CursorVisible = false;

            while (reachedEnd == false)
            {
                sw.Start();
                Console.SetCursorPosition(0, 0);
                Mat frame = vc.QueryFrame();

                if (frame != null)
                {
                    Bitmap bmp = frame.ToImage<Bgr, int>().Resize(170, 64, Emgu.CV.CvEnum.Inter.Nearest).ToBitmap<Bgr, int>();
                    Console.WriteLine(PixelToAscii(bmp, asciiStringList));

                    processTime = sw.ElapsedMilliseconds;
                    Thread.Sleep(Math.Max(frameTime - (int)processTime, 0));

                    frame.Dispose();
                }
                else
                {
                    vc.Dispose();
                    reachedEnd = true;
                }
                sw.Reset();
            }
        }

        public string PixelToAscii(Bitmap bmp, List<string> asciiStringList)
        {
            StringBuilder asciiString = new StringBuilder();

            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color rgba = bmp.GetPixel(x, y);   
                    int color = (rgba.R + rgba.G  +rgba.B) / 3;  //Greyscale between 0-255

                    asciiString.Append(AsciiGrayScale(color));

                    if (x == bmp.Width - 1)
                        asciiString.Append("\n");
                }
            }
            asciiStringList.Add(asciiString.ToString());
            return asciiString.ToString();
        }

        public string AsciiGrayScale(int color)
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

        public void SaveAscii(string path, List<string> asciiString)
        {
            StreamWriter w = new StreamWriter(path);

            for (int i = 0; i < asciiString.Count; i++)
            {
                w.WriteLine(asciiString[i]);
            }
            w.Close();
        }

        public void LoadAscii(string path, List<string> asciiStringList)
        {
            asciiStringList.Clear();

            StreamReader rLine = new StreamReader(path);

            while (rLine.ReadLine() != null)
            {
                string frame = "";

                for (int i = 0; i < 64; i++)
                {
                    frame += rLine.ReadLine();
                    frame += "\n";
                }
                asciiStringList.Add(frame);
            }
            rLine.Close();

            for (int i = 0; i < asciiStringList.Count; i++)
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine(asciiStringList[i]);
                Thread.Sleep(31);
            }
        }
    }
}