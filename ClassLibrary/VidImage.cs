using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading;


namespace ClassLibrary
{
    public class VidImage
    {
    
        //GET & SETS
        public string[] GetAsciiString()
        {
            return this.asciiString;
        }
        public void SetAsciiString(string[] asciiString)
        {
            this.asciiString = asciiString;
        }


        //NEW
        

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
