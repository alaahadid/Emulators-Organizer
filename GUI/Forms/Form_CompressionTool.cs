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
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Resources;
using System.Reflection;
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;
using EmulatorsOrganizer.Services.DefaultServices.Settings;
using SevenZip;
using MMB;
namespace EmulatorsOrganizer.GUI
{
    public partial class Form_CompressionTool : Form
    {
        public Form_CompressionTool()
        {
            extensions = new string[] { ".7z", ".rar", ".zip", ".gzip", ".tar", ".bzip2", ".xz" };

            InitializeComponent();
            comboBox_compressFormat.Items.AddRange(Enum.GetNames(typeof(SevenZip.OutArchiveFormat)));
            comboBox_compressLevel.Items.AddRange(Enum.GetNames(typeof(SevenZip.CompressionLevel)));

            comboBox_compressLevel.SelectedIndex = comboBox_compressFormat.SelectedIndex = 0;
        }

        private string[] extensions;
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private SettingsService settings = (SettingsService)ServicesManager.GetService("Global Settings");
        private int progress1 = 0;
        private string progressText1 = "";
        private int progress2 = 0;
        private string progressText2 = "";
        private Thread mainThread;
        //options
        private string _inputFolder;
        private string _outputFolder;
        private bool _ClearOutputFolder;
        private bool _ExtractCreateFolderForEachRom;
        private bool _ExtractUsePAss;
        private string _ExtractPAss;
        private bool _Compress;
        private string _CompressFormat;
        private string _CompressLevel;
        private bool _CompressUsePass;
        private bool _CompressIgnoreArchive;
        private bool _CompressPreserve;
        private string _CompressPass;
        private bool done;

        private bool IsFileArchive(string fileName)
        {
            return extensions.Contains(Path.GetExtension(fileName).ToLower());
        }
        private void TheProgress()
        {
            //get the files
            List<string> log = new List<string>();
            #region Empty the output folder
            if (_ClearOutputFolder)
            {
                string[] files = Directory.GetFiles(_outputFolder, "*", SearchOption.AllDirectories);
                for (int j = 0; j < files.Length; j++)
                {
                    progress1 = (100 * j) / files.Length;
                    progressText1 = ls["Status_ClearingTheOutputFolder"] + " " + progress1 + " %";
                    try
                    {
                        FileInfo inf = new FileInfo(files[j]);
                        inf.IsReadOnly = false;
                        File.Delete(files[j]);
                    }
                    catch { }
                }
                files = Directory.GetDirectories(_outputFolder, "*", SearchOption.AllDirectories);
                for (int j = 0; j < files.Length; j++)
                {
                    progress1 = (100 * j) / files.Length;
                    progressText1 = ls["Status_ClearingTheOutputFolder"] + " " + progress1 + " %";
                    try
                    {
                        Directory.Delete(files[j]);
                    }
                    catch { }
                }
            }
            #endregion
            string[] inputFiles = Directory.GetFiles(_inputFolder);
            string[] inputDirs = Directory.GetDirectories(_inputFolder);
            SevenZipCompressor comper = new SevenZipCompressor();
            comper.ArchiveFormat = (OutArchiveFormat)Enum.Parse(typeof(OutArchiveFormat), _CompressFormat);
            comper.CompressionLevel = (CompressionLevel)Enum.Parse(typeof(CompressionLevel), _CompressLevel);
            comper.Compressing += comper_Compressing;
            comper.FileCompressionStarted += comper_FileCompressionStarted;
            comper.PreserveDirectoryRoot = _CompressPreserve;
            string _ArchiveExtension = "." + _CompressFormat.ToLower().Replace("sevenzip", "7z");
            int i = 0;
            //Files
            foreach (string file in inputFiles)
            {
                if (_Compress)
                {
                    progressText1 = ls["Status_Compressing"] + " " + progress1 + " %";
                    if (File.Exists(file))
                    {
                        bool cancel = false;
                        if (_CompressIgnoreArchive)
                            if (IsFileArchive(file))
                                cancel = true;
                        if (!cancel)
                        {
                            if (!_CompressUsePass)
                                comper.CompressFiles(_outputFolder + "\\" + Path.GetFileNameWithoutExtension(file) + _ArchiveExtension,
                                    new string[] { file });
                            else
                                comper.CompressFilesEncrypted(_outputFolder + "\\" + Path.GetFileNameWithoutExtension(file) +
                                    _ArchiveExtension, _CompressPass, new string[] { file });
                        }
                    }
                    else
                    {
                        log.Add(ls["Log_UnableToCompressRomFile"] + ": " + file + ", " +
                            ls["Log_FileIsNotExist"]);
                    }
                }
                else//extract
                {
                    progressText1 = ls["Status_Extracting"] + " " + progress1 + " %";
                    string _extractDir = _outputFolder;
                    if (_ExtractCreateFolderForEachRom)
                    {
                        _extractDir = _outputFolder + "\\" + Path.GetFileNameWithoutExtension(file) + "\\";
                        Directory.CreateDirectory(_extractDir);
                    }
                    try
                    {
                        if (IsFileArchive(file))
                        {
                            SevenZipExtractor extractor;
                            if (_ExtractUsePAss)
                            {
                                extractor = new SevenZipExtractor(file, _ExtractPAss);
                            }
                            else
                            {
                                extractor = new SevenZipExtractor(file);
                            }
                            extractor.Extracting += extractor_Extracting;
                            extractor.FileExtractionStarted += extractor_FileExtractionStarted;
                            extractor.ExtractArchive(_extractDir);
                        }
                        else
                        {
                            log.Add(ls["Log_File"] + ": " + file + " " +
                                ls["Log_IsNotArchiveIgnored"]);
                        }
                    }
                    catch (Exception ex) { log.Add(ls["Log_UnableToExtractFile"] + ": " + file + " [" + ex.Message + "]"); }
                }
                i++;
                progress1 = (100 * i) / inputFiles.Length;
            }
            i = 0;
            //Folders
            foreach (string dir in inputDirs)
            {
                if (_Compress)
                {
                    progressText1 = ls["Status_Compressing"] + " " + progress1 + " %";

                    if (!_CompressUsePass)
                        comper.CompressDirectory(dir, _outputFolder + "\\" + Path.GetFileNameWithoutExtension(dir) +
                            _ArchiveExtension);
                    else
                        comper.CompressDirectory(dir, _outputFolder + "\\" + Path.GetFileNameWithoutExtension(dir) +
                            _ArchiveExtension, _CompressPass);
                }
                i++;
                progress1 = (100 * i) / inputDirs.Length;
            }
            //done !!
            progress1 = progress2 = 100;
            progressText1 = progressText2 = ls["Status_Done"];
            if (log.Count > 0)
            {
                File.WriteAllLines(_outputFolder + "\\log.txt", log.ToArray());
                System.Diagnostics.Process.Start(_outputFolder + "\\log.txt");
            }
            if (!this.InvokeRequired)
                Close();
            else
                this.Invoke(new Action(Done));
        }
        private void Done()
        {
            done = true;
            Close();
        }

        void extractor_FileExtractionStarted(object sender, FileInfoEventArgs e)
        {
            progress2 = e.PercentDone;
            progressText2 = ls["Status_Extracting"] + " " + e.PercentDone + " %";
        }
        void extractor_Extracting(object sender, ProgressEventArgs e)
        {
            progress2 = e.PercentDone;
            progressText2 = ls["Status_Extracting"] + " " + e.PercentDone + " %";
        }
        void comper_FileCompressionStarted(object sender, FileNameEventArgs e)
        {
            progress2 = e.PercentDone;
            progressText2 = ls["Status_Extracting"] + " " + e.PercentDone + " %";
        }
        void comper_Compressing(object sender, ProgressEventArgs e)
        {
            progress2 = e.PercentDone;
            progressText2 = ls["Status_Compressing"] + " " + e.PercentDone + " %";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.SelectedPath = textBox_input.Text;
            if (fol.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                textBox_input.Text = fol.SelectedPath;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.SelectedPath = textBox_output.Text;
            if (fol.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                textBox_output.Text = fol.SelectedPath;
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            groupBox_compress.Enabled = radioButton1.Checked;
            groupBox_extract.Enabled = !radioButton1.Checked;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Value = progress1;
            label_progrss1.Text = progressText1;
            progressBar2.Value = progress2;
            label_progress2.Text = progressText2;
        }
        //start
        private void button3_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(textBox_input.Text))
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseBrwoseForInputFolder"],
                   ls["MessageCaption_CompressionTool"]);
                return;
            }
            if (!Directory.Exists(textBox_output.Text))
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseBrowseForOutputFolder"],
                   ls["MessageCaption_CompressionTool"]);
                return;
            }
            if (radioButton1.Checked)//compress
            {
                if (checkBox_CompressPassword.Checked)
                {
                    if (textBox_compressPassword.Text.Length == 0)
                    {
                        ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseEnterACompressPassword"],
                            ls["MessageCaption_CompressionTool"]);
                        return;
                    }
                }
            }
            else//extract
            {
                if (checkBox_extractPassword.Checked)
                {
                    if (textBox_compressPassword.Text.Length == 0)
                    {
                        ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseEnterExtractPassword"],
                            ls["MessageCaption_CompressionTool"]);
                        return;
                    }
                }
            }
            //disable things
            radioButton1.Enabled = radioButton2.Enabled = groupBox_compress.Enabled = groupBox_extract.Enabled
                = groupBox1.Enabled = groupBox2.Enabled = false;
            timer1.Start();
            // Peek settings
            _ClearOutputFolder = checkBox_clearOutputFolder.Checked;
            _Compress = radioButton1.Checked;
            _ExtractCreateFolderForEachRom = checkBox_extractCreateFolders.Checked;
            _ExtractUsePAss = checkBox_extractPassword.Checked;
            _ExtractPAss = textBox_extractPass.Text;
            _CompressFormat = comboBox_compressFormat.SelectedItem.ToString();
            _CompressLevel = comboBox_compressLevel.SelectedItem.ToString();
            _CompressUsePass = checkBox_CompressPassword.Checked;
            _CompressPass = textBox_compressPassword.Text;
            _CompressIgnoreArchive = checkBox_ignoreArchiveInCompress.Checked;
            _CompressPreserve = checkBox_compreesPreserve.Checked;
            _inputFolder = textBox_input.Text;
            _outputFolder = textBox_output.Text;
            // Start thread
            mainThread = new Thread(new ThreadStart(TheProgress));
            mainThread.CurrentUICulture = ls.CultureInfo;
            mainThread.Start();
        }
        private void Frm_CompressionTool_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!done)
            {
                if (mainThread != null)
                {
                    if (mainThread.IsAlive)
                    {
                        ManagedMessageBoxResult result =
                        ManagedMessageBox.ShowMessage(
                       ls["Message_AreYouSureYouWantRoStopCurrentProgress"],
                       ls["MessageCaption_ClearProfile"],
                       new string[] { 
                           ls["Button_Yes"],
                           ls["Button_No"] },
                           0, ManagedMessageBoxIcon.Question);
                        if (result.ClickedButtonIndex == 1)// No not sure
                        {
                            e.Cancel = true;
                        }
                        else
                        {
                            mainThread.Abort();
                        }
                    }
                }
            }
        }
    }
}
