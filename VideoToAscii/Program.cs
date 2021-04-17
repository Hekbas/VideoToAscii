using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using ClassLibrary;

namespace VideoToAscii
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo op;
            Console.CursorVisible = false;

            Program program = new Program();
            VidImage vidInfo = new VidImage();
            List<VidImage> vidImageList = new List<VidImage>();
                      
            do
            {
                Console.Clear();
                Console.WriteLine("MENU PRINCIPAL\n" +
                    "1 - Convert Video to Ascii (Save to .txt file)\n" +
                    "2 - Play Ascii from array\n" +
                    "3 - Play Ascii from .txt");

                op = Console.ReadKey(true);

                if (op.Key == ConsoleKey.D1)
                {
                    Console.Clear();
                    //Convert vid to ascii
                    program.ConvertVidToAscii(vidInfo, vidImageList);

                    Console.ReadLine();
                    Console.Clear();
                    Console.WriteLine("Saving video...");

                    //Save asci into .txt
                    vidInfo.SaveAscii(@"..\..\..\..\Repos\SavedAscii\Ascii.txt", vidImageList);

                    Console.WriteLine("Video to Ascii succesful!");
                    Console.ReadLine();
                }
                else if (op.Key == ConsoleKey.D2)
                {
                    //Get ascii from .txt and play it
                    program.PlayAsciiFromFile(vidImageList);
                }
                else if (op.Key == ConsoleKey.D3)
                {
                    vidInfo.LoadAscii(@"..\..\..\..\Repos\SavedAscii\Ascii.txt", vidInfo);
                    Console.WriteLine("Reached end of video");
                    Console.ReadLine();
                }
                else if (op.Key == ConsoleKey.Escape)
                {
                    Environment.Exit(0);
                }
            } while (op.Key != ConsoleKey.Escape);
          
            Console.ReadLine();
            Console.Clear();

            //CARGAR ASCII DEL .TXT A CONSOLE
            //VidImage.LoadAscii(@"C:\Users\hekba\Downloads\Ascii.txt", vidImage);
        }

        public void ConvertVidToAscii(VidImage vidInfo, List<VidImage> vidImageList)
        {
            int batchAmmount = 400;
            int numThreads;
            int numFrames = vidInfo.GetNumFrames();

            //CONVERT VID TO ASCII
            if (numFrames % 400 == 0)
                numThreads = numFrames / 400;
            else
                numThreads = numFrames / 400 + 1;

            numThreads = 4;

            for (int i = 0; i < numThreads; i++)
            {
                int newBatchAmmount;

                VidImage tmp = new VidImage(batchAmmount * i);
                
                if (i == numThreads-1)
                {
                    newBatchAmmount = numFrames - i * batchAmmount;
                }
                else
                {
                    newBatchAmmount = batchAmmount;
                }

                vidImageList.Add(tmp);
                ThreadPool.QueueUserWorkItem(tmp.ThreadCallBack, newBatchAmmount);
                Console.WriteLine($"Threads created: {i+1}");
            }
            //WaitHandle.WaitAll(doneEvents);
        }

        public void PlayAsciiFromFile(List<VidImage> vidImageList)
        {
            for (int i = 0; i < vidImageList.Count; i++)
            {
                string[] temp = vidImageList[i].GetAsciiString();

                for (int j = 0; j < temp.Length; j++)
                {
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine(temp[j]);
                    Thread.Sleep(30);
                }
            }
        }
    }
}
