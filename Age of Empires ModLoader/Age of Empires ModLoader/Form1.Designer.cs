namespace Age_of_Empires_ModLoader
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.openFileDialogMod = new System.Windows.Forms.OpenFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.bestandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openBARBestandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.converteerAfbeeldingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.converteerXMBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.maakModBestandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verwijderModToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialogImage = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialogBar = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialogXmb = new System.Windows.Forms.OpenFileDialog();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialogMod
            // 
            this.openFileDialogMod.Filter = "\"Mod\"|*.aoemod";
            this.openFileDialogMod.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::Age_of_Empires_ModLoader.Properties.Resources.splashimage;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.richTextBox1);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(634, 430);
            this.panel1.TabIndex = 9;
            // 
            // comboBox1
            // 
            this.comboBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.comboBox1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.comboBox1.Location = new System.Drawing.Point(91, 13);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(443, 21);
            this.comboBox1.TabIndex = 6;
            this.comboBox1.TabStop = false;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // button2
            // 
            this.button2.BackgroundImage = global::Age_of_Empires_ModLoader.Properties.Resources.ico;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button2.ForeColor = System.Drawing.Color.Gainsboro;
            this.button2.Location = new System.Drawing.Point(538, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(90, 46);
            this.button2.TabIndex = 5;
            this.button2.Text = "Voeg Mod toe";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(12, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Beschrijving : ";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Font = new System.Drawing.Font("Lucida Calligraphy", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(91, 40);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(441, 136);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // button1
            // 
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.Font = new System.Drawing.Font("Lucida Calligraphy", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Gainsboro;
            this.button1.Location = new System.Drawing.Point(461, 345);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(128, 64);
            this.button1.TabIndex = 2;
            this.button1.Text = "Spelen";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(48, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Mod : ";
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.White;
            this.menuStrip1.BackgroundImage = global::Age_of_Empires_ModLoader.Properties.Resources.Menu;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bestandToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(634, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // bestandToolStripMenuItem
            // 
            this.bestandToolStripMenuItem.BackColor = System.Drawing.Color.Transparent;
            this.bestandToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openBARBestandToolStripMenuItem,
            this.converteerAfbeeldingToolStripMenuItem,
            this.converteerXMBToolStripMenuItem,
            this.maakModBestandToolStripMenuItem,
            this.verwijderModToolStripMenuItem,
            this.optiesToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.bestandToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.bestandToolStripMenuItem.Name = "bestandToolStripMenuItem";
            this.bestandToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.bestandToolStripMenuItem.Text = "Bestand";
            // 
            // openBARBestandToolStripMenuItem
            // 
            this.openBARBestandToolStripMenuItem.Name = "openBARBestandToolStripMenuItem";
            this.openBARBestandToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.openBARBestandToolStripMenuItem.Text = "Open BAR bestand";
            this.openBARBestandToolStripMenuItem.Click += new System.EventHandler(this.openBARBestandToolStripMenuItem_Click);
            // 
            // converteerAfbeeldingToolStripMenuItem
            // 
            this.converteerAfbeeldingToolStripMenuItem.Name = "converteerAfbeeldingToolStripMenuItem";
            this.converteerAfbeeldingToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.converteerAfbeeldingToolStripMenuItem.Text = "Converteer afbeelding";
            this.converteerAfbeeldingToolStripMenuItem.Click += new System.EventHandler(this.converteerAfbeeldingToolStripMenuItem_Click);
            // 
            // converteerXMBToolStripMenuItem
            // 
            this.converteerXMBToolStripMenuItem.Name = "converteerXMBToolStripMenuItem";
            this.converteerXMBToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.converteerXMBToolStripMenuItem.Text = "Converteer XMB";
            this.converteerXMBToolStripMenuItem.Click += new System.EventHandler(this.converteerXMBToolStripMenuItem_Click);
            // 
            // maakModBestandToolStripMenuItem
            // 
            this.maakModBestandToolStripMenuItem.Name = "maakModBestandToolStripMenuItem";
            this.maakModBestandToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.maakModBestandToolStripMenuItem.Text = "Maak mod bestand";
            this.maakModBestandToolStripMenuItem.Click += new System.EventHandler(this.maakModBestandToolStripMenuItem_Click);
            // 
            // verwijderModToolStripMenuItem
            // 
            this.verwijderModToolStripMenuItem.Name = "verwijderModToolStripMenuItem";
            this.verwijderModToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.verwijderModToolStripMenuItem.Text = "Verwijder Mod";
            this.verwijderModToolStripMenuItem.Click += new System.EventHandler(this.verwijderModToolStripMenuItem_Click);
            // 
            // optiesToolStripMenuItem
            // 
            this.optiesToolStripMenuItem.BackColor = System.Drawing.Color.Transparent;
            this.optiesToolStripMenuItem.Name = "optiesToolStripMenuItem";
            this.optiesToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.optiesToolStripMenuItem.Text = "Opties";
            this.optiesToolStripMenuItem.Click += new System.EventHandler(this.optiesToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // openFileDialogImage
            // 
            this.openFileDialogImage.Filter = "\"Afbeelding\"|*.png;*.jpg;*.bmp;*.tga;*.ddt|\"All Files\"|*.*";
            this.openFileDialogImage.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog2_FileOk);
            // 
            // openFileDialogBar
            // 
            this.openFileDialogBar.Filter = "\"BAR\"|*.bar";
            this.openFileDialogBar.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialogBar_FileOk);
            // 
            // openFileDialogXmb
            // 
            this.openFileDialogXmb.Filter = "\"XMB\"|*.xmb";
            this.openFileDialogXmb.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialogXmb_FileOk);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(634, 454);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ModLoader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialogMod;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem bestandToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verwijderModToolStripMenuItem;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ToolStripMenuItem openBARBestandToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem converteerAfbeeldingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem converteerXMBToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialogImage;
        private System.Windows.Forms.ToolStripMenuItem maakModBestandToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialogBar;
        private System.Windows.Forms.OpenFileDialog openFileDialogXmb;
    }
}

