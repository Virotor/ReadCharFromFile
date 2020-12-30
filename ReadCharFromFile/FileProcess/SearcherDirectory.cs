using ReadCharFromFile.FileProcess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReadCharFromFile
{
    class SearcherDirectory
    {
        public string PathToRootDirectory { private get; set;}

        public string PathToSaveDirectory { private get; set;}

        public int NumberOfThread { private get; set; }


        public SearcherDirectory(string pathToSaveDirectory, string pathToRootDirectory)
        {
            PathToRootDirectory = pathToRootDirectory;
            PathToSaveDirectory = pathToSaveDirectory;
        }
        public  void SearchDirectory()
        {
            
            OwnThreadPool ownThreadPool = new OwnThreadPool(FileAnalizator.StartAnaliz);
            ownThreadPool.NumberOfThread = 4;
            OpenDir(PathToRootDirectory, PathToSaveDirectory + "result.txt", ownThreadPool);
        }
       
        private void OpenDir(string pathToDir, string pathToSave, OwnThreadPool ownThreadPool)
        {
            if (Directory.Exists(pathToDir))
            {

                string[] directories = Directory.GetDirectories(pathToDir);
                if (directories.Length != 0)
                {
                    foreach (var elem in directories)
                    {
                        OpenDir(elem, pathToSave, ownThreadPool);
                    }
                }
                string[] files = Directory.GetFiles(pathToDir);
                if (files.Length != 0)
                {
                    foreach(var elem in files)
                    {

                        do
                        {
                            Thread.Sleep(10);
                        } while (!ownThreadPool.IsHavingFreeThread());

                       ownThreadPool.StartNewTask(FileAnalizator.StartAnaliz, (elem, PathToSaveDirectory+"result.txt"));
                    }
                }
            }
       


        }



    }
}
