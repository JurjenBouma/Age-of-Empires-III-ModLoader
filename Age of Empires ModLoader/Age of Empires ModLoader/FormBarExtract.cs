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
    public partial class FormBarExtract : Form
    {
        BarFile barfile; 
        string filePath;

        public FormBarExtract(string barFilePath)
        {
            InitializeComponent();
            filePath = barFilePath;
        }

        private void FormBarExtract_Load(object sender, EventArgs e)
        {
            barfile = new BarFile(filePath);
            foreach (AoEFile file in barfile.Files)
            {
                listBox1.Items.Add(file.fileName);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            foreach (AoEFile file in barfile.Files)
            {
                if (file.fileName == listBox1.SelectedItem.ToString())
                {
                    string fileExtension = file.fileName.Substring(file.fileName.Length - 4, 4);
                    if (fileExtension == ".xmb")
                    {
                        XmbFile xmbFile = new XmbFile(barfile.ReadFile(file));
                        richTextBox1.Text = xmbFile.Read();
                    }
                    else if (fileExtension == ".ddt")
                    {
                        DdtImage ddt = new DdtImage(barfile.ReadFile(file));
                        RasterImage image = ddt.ToRasterImage();
                        if (image != null)
                        {
                            pictureBox1.Visible = true;
                            pictureBox1.Image = image.ToBitmap(image.Width, image.Height);
                        }
                        else 
                        {
                            richTextBox1.Text = "Deze \".ddt\" indeling kan niet worden gelezen.";
                        }
                    }
                    else if (fileExtension == ".xml")
                        richTextBox1.Text = Encoding.UTF8.GetString(barfile.ReadFile(file));
                    else
                        richTextBox1.Text = "Dubbel klik op het bestand om het uit te pakken.";
                }
            }
        }
         
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            foreach (AoEFile file in barfile.Files)
            {
                if (file.fileName == listBox1.SelectedItem.ToString())
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    string savePath = fileInfo.DirectoryName + "\\" + file.fileName;
                    FileInfo fileInfoNewFile = new FileInfo(savePath);
                    if(!Directory.Exists(fileInfoNewFile.DirectoryName))
                    {
                        Directory.CreateDirectory(fileInfoNewFile.DirectoryName);
                    }
                    if (fileInfoNewFile.Extension == ".xmb" && Form1.ConvertXMBOnExtract)
                    {
                        savePath = savePath.Substring(0, savePath.Length - fileInfoNewFile.Extension.Length);
                        savePath += ".xml";
                        byte[] fileBytes = barfile.ReadFile(file);
                        XmbFile xmbFile = new XmbFile(fileBytes);

                        Stream writeStream = new FileStream(savePath, FileMode.Create);
                        BinaryWriter writer = new BinaryWriter(writeStream);
                        writer.Write(Encoding.UTF8.GetBytes(xmbFile.Read()));
                        writeStream.Close();
                    }
                    else if (fileInfoNewFile.Extension == ".ddt" && Form1.ConvertDDTOnExtract)
                    {
                        savePath = savePath.Substring(0, savePath.Length - fileInfoNewFile.Extension.Length);
                        byte[] fileBytes = barfile.ReadFile(file);
                        DdtImage ddt = new DdtImage(fileBytes);
                        RasterImage image = ddt.ToRasterImage();

                        if (Form1.imageConvertingSetting == ImageConvertionType.Tga)
                        {

                            savePath += ".tga";
                            if (image != null)
                            {
                                image.SaveTga(savePath);
                            }
                        }
                        else if (Form1.imageConvertingSetting == ImageConvertionType.AlphaMapPng)
                        {
                            if (image != null)
                            {
                                image.Save(savePath + "1.png", false);
                                image.SaveAlphaMap(savePath + "A.png");
                            }
                        }
                        else if (Form1.imageConvertingSetting == ImageConvertionType.Png)
                        {
                            if (image != null)
                            {
                                image.Save(savePath + "0.png", false);
                            }
                        }
                    }
                    else
                    {
                        Stream stream = new FileStream(savePath, FileMode.Create);
                        BinaryWriter writer = new BinaryWriter(stream);
                        writer.Write(barfile.ReadFile(file));
                        stream.Close();
                    }
                }
            }
        }
    }
}
