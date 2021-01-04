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
    public partial class FormOptions : Form
    {
        public FormOptions()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            FileInfo info = new FileInfo(openFileDialog1.FileName);
            textBox1.Text = info.DirectoryName;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Form1.gameFolder = textBox1.Text;
        }

        private void FormOptions_Load(object sender, EventArgs e)
        {
            textBox1.Text = Form1.gameFolder;
            checkBox1.Checked = Form1.ConvertXMBOnExtract;
            checkBox2.Checked = Form1.ConvertDDTOnExtract;
            comboBox1.Items.Add(ImageConvertionType.AlphaMapPng);
            comboBox1.Items.Add(ImageConvertionType.Tga);
            comboBox1.Items.Add(ImageConvertionType.Png);
            comboBox1.SelectedItem = Form1.imageConvertingSetting;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Form1.ConvertXMBOnExtract = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Form1.ConvertDDTOnExtract = checkBox2.Checked;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Form1.imageConvertingSetting = (ImageConvertionType)comboBox1.SelectedItem;
        }
    }
}
