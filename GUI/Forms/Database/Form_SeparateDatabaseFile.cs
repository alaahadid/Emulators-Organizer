// This file is part of Emulators Organizer
// A program that can organize roms and emulators
// 
// Copyright © Alaa Ibrahim Hadid 2009 - 2021
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.If not, see<http://www.gnu.org/licenses/>.
// 
// Author email: mailto:alaahadidfreeware@gmail.com
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Resources;
using System.Reflection;
using System.Windows.Forms;
using System.Security.Cryptography;
using SevenZip;
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;
using MMB;
namespace EmulatorsOrganizer.GUI
{
    public partial class Form_SeparateDatabaseFile : Form
    {
        public Form_SeparateDatabaseFile()
        {
            InitializeComponent();
            comboBox_format.Items.Clear();
            DatabaseFilesManager.DetectSupportedFormats();
            foreach (DatabaseFile db in DatabaseFilesManager.AvailableFormats)
            {
                if (db.Separable)
                    comboBox_format.Items.Add(db);
            }
            if (comboBox_format.Items.Count > 0)
                comboBox_format.SelectedIndex = 0;
            comboBox_fileFormat.SelectedIndex = 0;
        }
        private DatabaseFile selectedFormat;
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");

        // Browse for database
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = DatabaseFilesManager.GetFilter(selectedFormat);
            op.Title = ls["Title_Separate"] + " " + selectedFormat.Name + " " + ls["Word_File"];

            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = op.FileName;
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.Description = ls["Title_BrowseForDestinationFolder"];
            if (fol.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_destinationFolder.Text = fol.SelectedPath;
            }
        }
        // Start
        private void button2_Click(object sender, EventArgs e)
        {
            if (!File.Exists(textBox1.Text))
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseBrowseForTheDatabaseFileFirst"],
                    ls["MessageCaption_SeparateDatabaseFile"]);
                return;
            }
            if (!Directory.Exists(textBox_destinationFolder.Text))
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseBrowseForDestinationFolder"],
                    ls["MessageCaption_SeparateDatabaseFile"]);
                return;
            }
            groupBox1.Enabled = groupBox2.Enabled = groupBox3.Enabled = button1.Enabled =
                button2.Enabled = button3.Enabled = button4.Enabled = false;
            progressBar1.Visible = label_status.Visible = true;
            progressBar1.Refresh();
            label_status.Refresh();
            //load the file into buffer
            if (File.Exists(textBox1.Text))
            {
                //Clear destination folder ?
                if (checkBox_clearFolders.Checked)
                {
                    label_status.Text = ls["Status_ClearingInfoFolder"];
                    label_status.Refresh();
                    string[] files = Directory.GetFiles(HelperTools.GetFullPath(textBox_destinationFolder.Text));
                    progressBar1.Maximum = files.Length;
                    progressBar1.Value = 0;
                    foreach (string file in files)
                    {
                        try
                        { File.Delete(file); }
                        catch
                        {

                        }
                        progressBar1.Value++;
                    }
                }

                SeparateItem[] items = null;
                selectedFormat.ProgressStarted += SMSDatabase_Progress;
                selectedFormat.ProgressFinished += SMSDatabase_Progress;
                selectedFormat.Progress += SMSDatabase_Progress;

                items = selectedFormat.GetSeparate(textBox1.Text);

                progressBar1.Maximum = items.Length;
                progressBar1.Value = 0;
                label_status.Text = ls["Status_SeparatingDatabase"];
                label_status.Refresh();
                // Needed !
                RichTextBox virtualTextBox = new RichTextBox();
                for (int i = 0; i < items.Length; i++)
                {
                    //save the info file
                    bool cancel = false;
                    if (checkBox_dontReplaceInfo.Checked)
                        if (File.Exists(HelperTools.GetFullPath(textBox_destinationFolder.Text) + "\\" + items[i].FileName))
                            cancel = true;
                    items[i].FileName = items[i].FileName.Replace(":", "_");
                    items[i].FileName = items[i].FileName.Replace("|", "_");
                    items[i].FileName = items[i].FileName.Replace(@"\", "_");
                    items[i].FileName = items[i].FileName.Replace("/", "_");
                    items[i].FileName = items[i].FileName.Replace("*", "_");
                    items[i].FileName = items[i].FileName.Replace("?", "_");
                    items[i].FileName = items[i].FileName.Replace("<", "_");
                    items[i].FileName = items[i].FileName.Replace(">", "_");
                    items[i].FileName = items[i].FileName.Replace(@"""", "_");
                    string path = HelperTools.GetFullPath(textBox_destinationFolder.Text) + "\\" + items[i].FileName;
                    if (!cancel)
                    {
                        virtualTextBox.Lines = items[i].Data.Split(new char[] { '\n' });
                        switch (comboBox_fileFormat.SelectedIndex)
                        {
                            case 0:// Text
                                {
                                    File.WriteAllLines(path, virtualTextBox.Lines);
                                    break;
                                }
                            case 1:// RTF
                                {
                                    virtualTextBox.SaveFile(path.Replace(".txt", ".rtf"), RichTextBoxStreamType.RichText);
                                    break;
                                }
                            case 2:// DOC
                                {
                                    virtualTextBox.SaveFile(path.Replace(".txt", ".doc"), RichTextBoxStreamType.RichText);
                                    break;
                                }
                        }

                    }

                    progressBar1.Value = i;
                    progressBar1.Refresh();
                }
                MessageBox.Show(ls["Status_Done"]);
                groupBox1.Enabled = groupBox2.Enabled = groupBox3.Enabled = button1.Enabled =
                button2.Enabled = button3.Enabled = button4.Enabled = true;
                progressBar1.Visible = label_status.Visible = false;
            }
        }
        private void SMSDatabase_Progress(object sender, ProgressArgs e)
        {
            label_status.Text = e.Status + "(" + e.Completed + " %)";
            Application.DoEvents();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void comboBox_format_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";
            selectedFormat = (DatabaseFile)comboBox_format.SelectedItem;
        }
    }
}
