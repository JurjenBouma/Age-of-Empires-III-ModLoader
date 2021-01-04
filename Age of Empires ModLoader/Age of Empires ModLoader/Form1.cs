using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

struct ModItem
{
    //Name of mod
    public string Name;
    //Mod main folder
    public string DirectoryName;
    //Mod file path
    public string FileName;
}

namespace Age_of_Empires_ModLoader
{
    public enum ImageConvertionType {AlphaMapPng, Tga, Png}
    public partial class Form1 : Form
    {
        //List of known mods + intallFolder
        List<ModItem> modItemList;
        public static string gameFolder = "C:\\Program Files (x86)\\Microsoft Games\\Age of Empires III";
        public static bool ConvertXMBOnExtract = true;
        public static bool ConvertDDTOnExtract = true;
        public static ImageConvertionType imageConvertingSetting = ImageConvertionType.AlphaMapPng;

        //Info about the mod and its files
        string currentModPath;//Mod main folder
        string currentModFile;//Mod file path
        List<string> fileListStartUp;//Lists of the modded filePaths included by mod
        List<string> fileListData;
        List<string> fileListSound;
        List<string> fileListArt;

        //GameProcess
        System.Diagnostics.Process gameProcess;

        public Form1()
        {
            InitializeComponent();
            modItemList = new List<ModItem>();

            //Load Settings
            OpenModFilePaths();
            ReadSettingsFile();

            //Set the processObject
            gameProcess = new System.Diagnostics.Process();
            gameProcess.StartInfo.FileName = gameFolder + "\\age3.exe";
            gameProcess.EnableRaisingEvents = true;
            gameProcess.Exited += this.RestoreGameFiles;


        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialogMod.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            //Read the modfile for mod info
            currentModFile = openFileDialogMod.FileName;
            FileInfo info = new FileInfo(currentModFile);
            currentModPath = info.DirectoryName;

            foreach (string line in File.ReadAllLines(currentModFile))
            {
                ReadMeta(line);
            }

            SaveModFilePaths();
        }  

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //update the buttons and mod discription
            for (int i = 0; i < modItemList.Count; i++)
            {
                if (modItemList[i].Name == comboBox1.SelectedItem.ToString())
                {
                    currentModPath = modItemList[i].DirectoryName;
                    currentModFile = modItemList[i].FileName;
                    foreach (string line in File.ReadAllLines(currentModFile))
                    {
                        ReadMeta(line);
                    }
                    i = modItemList.Count;
                }
            }
        }

        //play button clicked
        private void button1_Click(object sender, EventArgs e)
        {
            //New file lists
            fileListStartUp = new List<string>();
            fileListData = new List<string>();
            fileListSound = new List<string>();
            fileListArt = new List<string>();

            //fetch all files
            foreach (string line in File.ReadAllLines(currentModFile))
            {
                ReadFiles(line);
            }

            FileInfo fileInfo;

            //Place all modded startupFiles(.con)
            foreach (string file in fileListStartUp)
            {
                fileInfo = new FileInfo(file);
                PlaceFile(file, gameFolder + "\\Startup\\" + fileInfo.Name);
            }

            //Place all modded dataFiles(.xml)
            foreach (string file in fileListData)
            {
                fileInfo = new FileInfo(file);
                string placePath = "";
                if (fileInfo.Name.Contains("proto"))
                {
                    placePath = gameFolder + "\\data\\proto.xml";
                }
                else if (fileInfo.Name.Contains("stringtable"))
                {
                    placePath = gameFolder + "\\data\\stringtable.xml";
                }
                else if (fileInfo.Name.Contains("techtree"))
                {
                    placePath = gameFolder + "\\data\\techtree.xml";
                }

                PlaceFile(file, placePath);
            }

            //Place sound files
            foreach (string file in fileListSound)
            {
                //Get local path
                string subPath = file.Substring((currentModPath + "\\").Length, file.Length - (currentModPath + "\\").Length);
                //Check for Sound folder and remove if there
                if (subPath.StartsWith("Sound\\"))
                    subPath = subPath.Substring("Sound\\".Length, subPath.Length - "Sound\\".Length);
                PlaceFile(file, gameFolder + "\\Sound\\" + subPath);
            }

            //Place art files
            foreach (string file in fileListArt)
            {
                //Get local path
                string subPath = file.Substring((currentModPath + "\\").Length, file.Length - (currentModPath + "\\").Length);
                //Check for art folder and remove if there
                if (subPath.StartsWith("art\\"))
                    subPath = subPath.Substring("art\\".Length, subPath.Length - "art\\".Length);
                PlaceFile(file, gameFolder + "\\art\\" + subPath);
            }

            //Start aoe.exe
            gameProcess.Start();
        }

        private void optiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOptions formOptions = new FormOptions();
            formOptions.Show();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormHelp formHelp = new FormHelp();
            formHelp.Show();
        }

        void RestoreGameFiles(object sender, EventArgs e)
        {
            FileInfo fileInfo;

            //Restore all modded startup files (.con)
            foreach (string file in fileListStartUp)
            {
                fileInfo = new FileInfo(file);
                RestoreFile(gameFolder + "\\Startup\\" + fileInfo.Name);
            }
            //Restore all modded data files
            foreach (string file in fileListData)
            {
                fileInfo = new FileInfo(file);
                string restorePath = "";
                if (fileInfo.Name.Contains("proto"))
                {
                    restorePath = gameFolder + "\\data\\proto.xml";
                }
                else if (fileInfo.Name.Contains("stringtable"))
                {
                    restorePath = gameFolder + "\\data\\stringtable.xml";
                }
                else if (fileInfo.Name.Contains("techtree"))
                {
                    restorePath = gameFolder + "\\data\\techtree.xml";
                }

                RestoreFile(restorePath);
            }

            //Restore all modded sound files
            foreach (string file in fileListSound)
            {
                //Get local path
                string subPath = file.Substring((currentModPath + "\\").Length, file.Length - (currentModPath + "\\").Length);
                //Check for art folder and remove if there
                if (subPath.StartsWith("Sound\\"))
                    subPath = subPath.Substring("Sound\\".Length, subPath.Length - "Sound\\".Length);
                RestoreFile(gameFolder + "\\Sound\\" + subPath);
            }

            //Restore all modded art files;
            foreach (string file in fileListArt)
            {
                //Get local path
                string subPath = file.Substring((currentModPath + "\\").Length, file.Length - (currentModPath + "\\").Length);
                //Check for art folder and remove if there
                if (subPath.StartsWith("art\\"))
                    subPath = subPath.Substring("art\\".Length, subPath.Length - "art\\".Length);
                RestoreFile(gameFolder + "\\art\\" + subPath);
            }
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
                    Image buttonIcon = Image.FromFile(currentModPath + "\\" + value);
                    button1.BackgroundImage = buttonIcon;
                }
            }
            if (command == "Name")
            {

                ModItem modItem = new ModItem();
                modItem.Name = value;
                modItem.DirectoryName = currentModPath;
                modItem.FileName = currentModFile;

                if (!modItemList.Contains(modItem))
                {
                    modItemList.Add(modItem);
                    comboBox1.Items.Add(value);
                    comboBox1.SelectedIndex = comboBox1.Items.IndexOf(value);
                }

                comboBox1.SelectedIndex = comboBox1.Items.IndexOf(value);
                
            }
            if (command == "Discription")
            {
                richTextBox1.Text = File.ReadAllText(currentModPath + "\\" + value);
            }
        }

        //function to fetch modded files from modfile(.aoemod)
        void ReadFiles(string line)
        {
            string command = line.Substring(0, line.IndexOf("="));
            string value = line.Substring(line.IndexOf("=") + 1);

            if (command == "Startup")
            {
                fileListStartUp.Add(currentModPath + "\\" + value);
            }
            if (command == "Data")
            {
                fileListData.Add(currentModPath + "\\" + value);
            }
            if (command == "Sound")
            {
                fileListSound.Add(currentModPath + "\\" + value);
            }
            if (command == "Art")
            {
                fileListArt.Add(currentModPath + "\\" + value);
            }
        }

        void PlaceFile(string filePath,string placePath)
        {
            FileInfo fileInfo = new FileInfo(placePath);

            //Check for original file
            if (File.Exists(placePath))
            {
                //Temp rename of original file
                string newFilePath = placePath.Substring(0, placePath.IndexOf(fileInfo.Extension));
                newFilePath += "-Backup" + fileInfo.Extension;
                try
                {
                    File.Move(placePath, newFilePath);
                }
                catch
                {
                }
            }
            if(!Directory.Exists(fileInfo.DirectoryName))
            {
                Directory.CreateDirectory(fileInfo.DirectoryName);
            }
            //place modded file
            File.Copy(filePath, placePath);
        }
        void RestoreFile(string filePath)
        {
            //Get the paths the originalFile
            FileInfo fileInfo = new FileInfo(filePath);
            string originalFilePath = filePath.Substring(0, filePath.IndexOf(fileInfo.Extension));
            originalFilePath += "-Backup" + fileInfo.Extension;

            //Delete the modded file
            try
            {
                File.Delete(filePath);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            //Place original file back
            if (File.Exists(originalFilePath))
            {
                File.Move(originalFilePath, filePath);
            }
        }

        void SaveSettingsFile()
        {
            //DumpOld file
            if (File.Exists(Application.StartupPath + "\\settings.con"))
                File.Delete(Application.StartupPath + "\\settings.con");

            List<string> fileText = new List<string>();
            fileText.Add(gameFolder);
            fileText.Add(ConvertXMBOnExtract.ToString());
            fileText.Add(ConvertDDTOnExtract.ToString());
            fileText.Add(imageConvertingSetting.ToString());

            File.WriteAllLines(Application.StartupPath + "\\settings.con", fileText.ToArray());
        }

        void ReadSettingsFile()
        {
            if (File.Exists(Application.StartupPath + "\\settings.con"))
            {
                string[] fileLines = File.ReadAllLines(Application.StartupPath + "\\settings.con");
                gameFolder = fileLines[0];
                bool.TryParse(fileLines[1], out ConvertXMBOnExtract);
                bool.TryParse(fileLines[2],out ConvertDDTOnExtract);
                imageConvertingSetting = (ImageConvertionType)Enum.Parse(typeof(ImageConvertionType),fileLines[3]);
            }
        }

        void SaveModFilePaths()
        {
            //Make list object 
            List<string> listModFilePaths = new List<string>();
            //Add modfile(.aoemod) paths to list
            foreach( ModItem mod in modItemList)
            {
                listModFilePaths.Add(mod.FileName);
            }
            //dump old list
            if (File.Exists(Application.StartupPath + "\\modlist.txt"))
                File.Delete(Application.StartupPath + "\\modlist.txt");

            //write new list
            File.WriteAllLines(Application.StartupPath + "\\modlist.txt", listModFilePaths.ToArray());
        }

        void OpenModFilePaths()
        {
            //check for listfile
            if (File.Exists(Application.StartupPath + "\\modlist.txt"))
            {
                foreach (string path in File.ReadAllLines(Application.StartupPath + "\\modlist.txt"))
                {
                    if (File.Exists(path))
                    {
                        //Set mod data
                        currentModFile = path;
                        FileInfo info = new FileInfo(currentModFile);
                        currentModPath = info.DirectoryName;
                        //fetch for mod data
                        foreach (string line in File.ReadAllLines(currentModFile))
                        {
                            ReadMeta(line);
                        }
                    }
                }
            }
            else
            {
                //Add vanilla
                currentModFile = Application.StartupPath + "\\Resources\\vanilla.aoemod";
                FileInfo info = new FileInfo(currentModFile);
                currentModPath = info.DirectoryName;
                //fetch for mod data
                foreach (string line in File.ReadAllLines(currentModFile))
                {
                    ReadMeta(line);
                }
            }
        }

        //Delete Mod MenuItem clicked
        private void verwijderModToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Check if not Vanilla
            if (comboBox1.SelectedItem.ToString() != "Vanilla Age of Empires")
            {
                //Search for selected mod
                for (int i = 0; i < modItemList.Count; i++)
                {
                    if (modItemList[i].Name == comboBox1.SelectedItem.ToString())
                    {
                        //Remove mod from combobox1 and modlist
                        modItemList.RemoveAt(i);
                        comboBox1.Items.Remove(comboBox1.SelectedItem);
                        i = modItemList.Count;
                    }
                }

                currentModPath = modItemList[0].DirectoryName;
                currentModFile = modItemList[0].FileName;

                foreach (string line in File.ReadAllLines(currentModFile))
                {
                    ReadMeta(line);
                }

                SaveModFilePaths();
            }
        }

        private void converteerAfbeeldingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialogImage.ShowDialog();
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            //Convert ddt to imagefile or imagefile to ddt
            FileInfo fileInfo = new FileInfo(openFileDialogImage.FileName);
            string savePath = fileInfo.DirectoryName + "\\" + fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length);

            if(fileInfo.Extension == ".ddt")
            {
                Stream stream = new FileStream(openFileDialogImage.FileName, FileMode.Open);
                BinaryReader reader = new BinaryReader(stream);

                DdtImage ddt = new DdtImage(reader.ReadBytes((int)stream.Length));
                RasterImage image = ddt.ToRasterImage();
                stream.Close();

                if (imageConvertingSetting == ImageConvertionType.Tga)
                {

                    savePath += ".tga";
                    if (image != null)
                    {
                        image.SaveTga(savePath);
                    }
                }
                else if (imageConvertingSetting == ImageConvertionType.AlphaMapPng)
                {
                    if (image != null)
                    {
                        image.Save(savePath + "1.png",false);
                        image.SaveAlphaMap(savePath + "A.png");
                    }
                }
                else if (imageConvertingSetting == ImageConvertionType.Png)
                {
                    if (image != null)
                    {
                        image.Save(savePath + "0.png", false);
                    }
                }
            }
            else
            {
                string texturePath = openFileDialogImage.FileName;
                string useAlphaMap = savePath.Substring(savePath.Length-1,1);
                if (useAlphaMap == "1")
                {
                    savePath = savePath.Substring(0, savePath.Length - 1);
                    string alphaPath = savePath + "A" + fileInfo.Extension;
                    savePath += ".ddt";
                    RasterImage image = new RasterImage(texturePath, alphaPath);
                    image.SaveDdt(savePath);
                }
                else
                {
                    savePath = savePath.Substring(0, savePath.Length - 1);
                    savePath += ".ddt";
                    RasterImage image = new RasterImage(texturePath);
                    image.SaveDdt(savePath);
                }
            }
        }

        private void openBARBestandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialogBar.ShowDialog();
        }

        private void openFileDialogBar_FileOk(object sender, CancelEventArgs e)
        {
            //show barExtractorForm
            FileInfo fileInfo = new FileInfo(openFileDialogBar.FileName);
            FormBarExtract formOpenBar = new FormBarExtract(openFileDialogBar.FileName);
            formOpenBar.Text = fileInfo.Name;
            formOpenBar.Show();
        }

        private void converteerXMBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialogXmb.ShowDialog();
        }

        private void openFileDialogXmb_FileOk(object sender, CancelEventArgs e)
        {
            //Get savePath
            FileInfo fileInfo = new FileInfo(openFileDialogXmb.FileName);
            string savePath = fileInfo.DirectoryName + "\\" + fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length);
            
            //Get fileBytes
            Stream readStream = new FileStream(openFileDialogXmb.FileName,FileMode.Open);
            BinaryReader reader = new BinaryReader(readStream);
            byte[] fileBytes = reader.ReadBytes((int)readStream.Length);
            XmbFile xmbFile = new XmbFile(fileBytes);
            readStream.Close();

            //Write Converted file to savePath
            Stream writeStream = new FileStream(savePath, FileMode.Create);
            BinaryWriter writer = new BinaryWriter(writeStream);
            writer.Write(Encoding.UTF8.GetBytes(xmbFile.Read()));
            writeStream.Close();
        }

        private void maakModBestandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormModfileCombiner formModMaker = new FormModfileCombiner();
            formModMaker.Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettingsFile();
        }
    }

}
