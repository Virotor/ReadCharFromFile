using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReadCharFromFile
{
    public partial class Form1 : Form
    {

        private string pathToRootDirectory = "";
        private string pathToSaveDirectory = "";
        public Form1()
        {
            InitializeComponent();
            ToolTip toolTip = new ToolTip();
            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 1000;
            toolTip.ReshowDelay = 500;

            toolTip.ShowAlways = true;


            toolTip.SetToolTip(this.label2, "Если не задан, используется путь корневой директории");
        }

        private void openFile_Click(object sender, EventArgs e)
        {
            pathToRootDirectory = OpenFile();
            pathToFile.Text = pathToRootDirectory;
        }

       

        private void button_ChooseSaveDir(object sender, EventArgs e)
        {
            pathToSaveDirectory = OpenFile();
            pathToSaveDir.Text = pathToSaveDirectory;
        }



        private string OpenFile()
        {
            string dirPath = "";
            using (var openFileDialog = new FolderBrowserDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    dirPath = openFileDialog.SelectedPath;
                }
            }

            return dirPath;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            pathToSaveDirectory = pathToSaveDir.Text;
            pathToRootDirectory = pathToFile.Text;

            if (pathToSaveDirectory.Equals(string.Empty))
            {
                pathToSaveDirectory = pathToRootDirectory;
                pathToSaveDir.Text = pathToSaveDirectory;
            }
            uint countOfThread;
            if(uint.TryParse(textBox1.Text,out countOfThread))
            {
                label4.Text = "";
                SearcherDirectory searcherDirectory = new SearcherDirectory(pathToSaveDirectory,pathToRootDirectory);
                searcherDirectory.NumberOfThread =(int) countOfThread;
                await Task.Run( ()=> searcherDirectory.SearchDirectory());
                label4.Text = "Ready";
            }
            else
            {
                ToolTip toolTip = new ToolTip();
                toolTip.Active = true;
       
                toolTip.ToolTipIcon = ToolTipIcon.Error;
                toolTip.ShowAlways = true;
                toolTip.SetToolTip(label3, "Введенно неверное число потоков");
            }


        }
    }
}
