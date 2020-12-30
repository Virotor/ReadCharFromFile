using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReadCharFromFile
{
    class FileAnalizator
    {

        private static EventWaitHandle UseFileToSave { get;set; } = new AutoResetEvent(true);
        
        private static int sizeOfBuffer = short.MaxValue;

        public static void StartAnaliz(object parametrs)
        {
            (string, string) pathCortege = ((string, string))parametrs;
            long[] numberOfOccurrences = new long[256];
            using (FileStream fileStream = File.OpenRead(pathCortege.Item1))
            {
               
                long countOfReadBytes = 0;
                long sizeOfFile = fileStream.Length;
                while (countOfReadBytes < sizeOfFile)
                {
                    byte[] buffer = new byte[sizeOfBuffer];
                    fileStream.Read(buffer, 0, sizeOfBuffer);
                    for(int i=0; i<sizeOfBuffer && countOfReadBytes < sizeOfFile; i++, countOfReadBytes++)
                    {
                        numberOfOccurrences[buffer[i]]++;
                    }
                }
            }
            UseFileToSave.WaitOne();
            using (FileStream fileStream = new FileStream(pathCortege.Item2, FileMode.Append))
            {
                byte[] path = Encoding.Default.GetBytes(pathCortege.Item1 + "\n");
                fileStream.Write(path, 0, path.Length);

                for (int i = 0; i < 256; i++)
                {
                    byte[] temp =Encoding.Default.GetBytes(String.Format("{0}-{1} ", i, numberOfOccurrences[i]));
                    fileStream.Write(temp,0,temp.Length);
                }

                fileStream.Write(Encoding.Default.GetBytes("\n"),0,1);
            }
            UseFileToSave.Set();
            
        }
    }
}
