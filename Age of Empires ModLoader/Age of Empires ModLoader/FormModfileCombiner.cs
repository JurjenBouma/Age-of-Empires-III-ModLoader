using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Age_of_Empires_ModLoader
{
    public partial class FormModfileCombiner : Form
    {
        List<string> fileListStartUp = new List<string>();//Lists of the modded filePaths included by mod
        List<string> fileListData = new List<string>();
        List<string> fileListSound = new List<string>();
        List<string> fileListArt = new List<string>();
        string disctiptionFileName ="";
        string iconFileName = "";

        ModItem mod = new ModItem();//mod info

        public FormModfileCombiner()
        {
            InitializeComponent();
        }

        private void FormModfileCombiner_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialogOpenMod.ShowDialog();
        }

        private void openFileDialogOpenMod_FileOk(object sender, CancelEventArgs e)
        {
            mod.FileName = openFileDialogOpenMod.FileName;
            FileInfo info = new FileInfo(mod.FileName);
            mod.DirectoryName = info.DirectoryName;

            fileListStartUp = new List<string>();
            fileListData = new List<string>();
            fileListSound = new List<string>();
            fileListArt = new List<string>();

            foreach (string line in File.ReadAllLines(mod.FileName))
            {
                ReadMeta(line);
                ReadFiles(line);
            }
            UpdateListBox();
        }

        //Function to fetch the mod info from modfile(.aoemod)
        void ReadMeta(string line)
        {
            string command = line.Substring(0, line.IndexOf("="));
            string value = line.Substring(line.IndexOf("=") + 1);
            if (command == "Icon")
            {
                if (value != "null")
                {
                    pictureBox1.ImageLocation = mod.DirectoryName + "\\" + value;
                    iconFileName = "\\" + value;
                }
            }
            if (command == "Name")
            {
                mod.Name = value;
                textBox1.Text = value;
            }
            if (command == "Discription")
            {
                richTextBox1.Text = File.ReadAllText(mod.DirectoryName + "\\" + value);
                disctiptionFileName = "\\" + value;
            }
        }

        //function to fetch modded files from modfile(.aoemod)
        void ReadFiles(string line)
        {
            string command = line.Substring(0, line.IndexOf("="));
            string value = line.Substring(line.IndexOf("=") + 1);

            if (command == "Startup")
            {
                fileListStartUp.Add(mod.DirectoryName + "\\" + value);
            }
            if (command == "Data")
            {
                fileListData.Add(mod.DirectoryName + "\\" + value);
            }
            if (command == "Sound")
            {
                fileListSound.Add(mod.DirectoryName + "\\" + value);
            }
            if (command == "Art")
            {
                fileListArt.Add(mod.DirectoryName + "\\" + value);
            }
        }

        void UpdateListBox()
        {
            listBox1.Items.Clear();
            listBox1.Items.Add("Startup :");
            listBox1.Items.AddRange(fileListStartUp.ToArray());
            listBox1.Items.Add("data:");
            listBox1.Items.AddRange(fileListData.ToArray());
            listBox1.Items.Add("Sound:");
            listBox1.Items.AddRange(fileListSound.ToArray());
            listBox1.Items.Add("art");
            listBox1.Items.AddRange(fileListArt.ToArray());
        }

        void AddFiles(string folderPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
            foreach(string file in Directory.GetFiles(folderPath))
            {
                if(folderPath.Contains("\\Startup"))
                {
                    if(!fileListArt.Contains(file))
                        fileListStartUp.Add(file);
                }
                if(folderPath.Contains("\\data"))
                {
                    if (!fileListData.Contains(file))
                        fileListData.Add(file);
                }
                if(folderPath.Contains("\\Sound"))
                {
                    if (!fileListSound.Contains(file))
                     fileListSound.Add(file);
                }
                if(folderPath.Contains("\\art"))
                {
                    if (!fileListArt.Contains(file))
                        fileListArt.Add(file);
                }
            }
            foreach (string dir in Directory.GetDirectories(folderPath))
            {
                AddFiles(dir);
            }

            UpdateListBox();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialogImage.ShowDialog();
        }

        private void openFileDialogImage_FileOk(object sender, CancelEventArgs e)
        {
            pictureBox1.ImageLocation = openFileDialogImage.FileName;
            FileInfo fileInfo = new FileInfo(openFileDialogImage.FileName);
            iconFileName = "\\"+ fileInfo.Name;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialogAdd.ShowDialog() == DialogResult.OK)
            {
                AddFiles(folderBrowserDialogAdd.SelectedPath);
            }
        }

        private void listBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete)
            {
                try
                {
                    fileListStartUp.Remove(listBox1.SelectedItem.ToString());
                    fileListData.Remove(listBox1.SelectedItem.ToString());
                    fileListSound.Remove(listBox1.SelectedItem.ToString());
                    fileListArt.Remove(listBox1.SelectedItem.ToString());
                    UpdateListBox();
                }
                catch
                {
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            saveFileDialogMod.ShowDialog();
        }

        private void saveFileDialogMod_FileOk(object sender, CancelEventArgs e)
        {
            SaveMod(saveFileDialogMod.FileName);
        }

        void SaveMod(string savePath)
        {
            FileInfo fileInfo = new FileInfo(savePath);
            mod.FileName = fileInfo.Name;
            mod.Name = textBox1.Text;
            mod.DirectoryName = fileInfo.DirectoryName;

            //Write disciption file and set DisciptionFile?Name
            string discriptionFilePath = fileInfo.DirectoryName;
            if (disctiptionFileName.Length == 0)
                disctiptionFileName = "\\discription.txt";
            discriptionFilePath += disctiptionFileName;
            WriteDisctriptionFile(discriptionFilePath);

            CopyIcon(pictureBox1.ImageLocation);
            CopyFiles(fileListStartUp, mod.DirectoryName);
            CopyFiles(fileListData, mod.DirectoryName);
            CopyFiles(fileListSound, mod.DirectoryName);
            CopyFiles(fileListArt, mod.DirectoryName);
            UpdateFileList(ref fileListStartUp);
            UpdateFileList(ref fileListData);
            UpdateFileList(ref fileListSound);
            UpdateFileList(ref fileListArt);
            UpdateListBox();

            WriteModFile(savePath);
        }

        void WriteModFile(string savePath)
        {
            string fileText = "";
            fileText += "Name=" + textBox1.Text;
            fileText += "\nDiscription=" + disctiptionFileName.Substring(1, disctiptionFileName.Length - 1);
            fileText += "\nIcon=" + iconFileName.Substring(1,iconFileName.Length-1);
            foreach (string file in fileListStartUp)
            {
                string localPath = file.Substring(file.IndexOf("\\Startup"), file.Length - file.IndexOf("\\Startup"));
                string commandLine =  "\nStartup=" + localPath.Substring(1, localPath.Length - 1);
                if(!fileText.Contains(commandLine))
                    fileText += commandLine;
            }
            foreach (string file in fileListData)
            {
                string localPath = file.Substring(file.IndexOf("\\data"), file.Length - file.IndexOf("\\data"));
                string commandLine = "\nData=" + localPath.Substring(1, localPath.Length - 1);
                if (!fileText.Contains(commandLine))
                    fileText += commandLine;
            }
            foreach (string file in fileListSound)
            {
                string localPath = file.Substring(file.IndexOf("\\Sound"), file.Length - file.IndexOf("\\Sound"));
                string commandLine = "\nSound=" + localPath.Substring(1, localPath.Length - 1);
                if (!fileText.Contains(commandLine))
                    fileText += commandLine;
            }
            foreach (string file in fileListArt)
            {
                string localPath = file.Substring(file.IndexOf("\\art"), file.Length - file.IndexOf("\\art"));
                string commandLine = "\nArt=" + localPath.Substring(1, localPath.Length - 1);
                if (!fileText.Contains(commandLine))
                    fileText += commandLine;
            }
            File.WriteAllText(savePath,fileText);
        }

        void UpdateFileList(ref List<string> fileList)
        {
            for (int i = 0; i < fileList.Count; i++)
            {
                string file = fileList[i];
                if (file.Contains("\\Startup"))
                {
                    string localPath = file.Substring(file.IndexOf("\\Startup"), file.Length - file.IndexOf("\\Startup"));
                    string newPath = mod.DirectoryName + localPath;
                    fileList[i] = newPath;
                }
                else if (file.Contains("\\data"))
                {
                    string localPath = file.Substring(file.IndexOf("\\data"), file.Length - file.IndexOf("\\data"));
                    string newPath = mod.DirectoryName + localPath;
                    fileList[i] = newPath;
                }
                else if (file.Contains("\\Sound"))
                {
                    string localPath = file.Substring(file.IndexOf("\\Sound"), file.Length - file.IndexOf("\\Sound"));
                    string newPath = mod.DirectoryName + localPath;
                    fileList[i] = newPath;
                }
                else if (file.Contains("\\art"))
                {
                    string localPath = file.Substring(file.IndexOf("\\art"), file.Length - file.IndexOf("\\art"));
                    string newPath = mod.DirectoryName + localPath;
                    fileList[i] = newPath;
                }
            }
        }

        void CopyFiles(List<string> sourcePaths, string saveDirectory)
        {
            foreach (string file in sourcePaths)
            {
                FileInfo fileInfo = new FileInfo(file);
                if (file.Contains("\\Startup"))
                {
                    string localPath = file.Substring(file.IndexOf("\\Startup"), file.Length - file.IndexOf("\\Startup"));
                    string savePath = saveDirectory + localPath;
                    fileInfo = new FileInfo(savePath);
                    if (!Directory.Exists(fileInfo.DirectoryName))
                        Directory.CreateDirectory(fileInfo.DirectoryName);
                    try
                    {
                        File.Copy(file, savePath);
                    }
                    catch { }
                }
                else if (file.Contains("\\data"))
                {
                    string localPath = file.Substring(file.IndexOf("\\data"), file.Length - file.IndexOf("\\data"));
                    string savePath = saveDirectory + localPath;
                    fileInfo = new FileInfo(savePath);
                    if (!Directory.Exists(fileInfo.DirectoryName))
                        Directory.CreateDirectory(fileInfo.DirectoryName);
                    try
                    {
                        File.Copy(file, savePath);
                    }
                    catch { }
                }
                else if (file.Contains("\\Sound"))
                {
                    string localPath = file.Substring(file.IndexOf("\\Sound"), file.Length - file.IndexOf("\\Sound"));
                    string savePath = saveDirectory + localPath;
                    fileInfo = new FileInfo(savePath);
                    if (!Directory.Exists(fileInfo.DirectoryName))
                        Directory.CreateDirectory(fileInfo.DirectoryName);
                    try
                    {
                        File.Copy(file, savePath);
                    }
                    catch { }
                }
                else if (file.Contains("\\art"))
                {
                    string localPath = file.Substring(file.IndexOf("\\art"), file.Length - file.IndexOf("\\art"));
                    string savePath = saveDirectory + localPath;
                    fileInfo = new FileInfo(savePath);
                    if (!Directory.Exists(fileInfo.DirectoryName))
                        Directory.CreateDirectory(fileInfo.DirectoryName);
                    try
                    {
                        File.Copy(file, savePath);
                    }
                    catch { }
                }
            }
        }

        void WriteDisctriptionFile(string savePath)
        {
            File.WriteAllText(savePath,richTextBox1.Text);
        }

        void CopyIcon(string sourcePath)
        {
            try
            {
                File.Copy(sourcePath, mod.DirectoryName + iconFileName);
            }
            catch { }

        }

    }
}
