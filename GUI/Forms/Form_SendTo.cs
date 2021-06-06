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
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;
using EmulatorsOrganizer.Services.DefaultServices.Settings;
using MMB;
using SevenZip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace EmulatorsOrganizer.GUI
{
    public partial class Form_SendTo : Form
    {
        public Form_SendTo(Rom[] romsToSend)
        {
            this.romsToSend = romsToSend;
            InitializeComponent();

            label_info.Text = romsToSend.Length + " " + ls["Info_ReadyToSend"];

            comboBox_compressFormat.Items.AddRange(Enum.GetNames(typeof(SevenZip.OutArchiveFormat)));
            comboBox_compressLevel.Items.AddRange(Enum.GetNames(typeof(SevenZip.CompressionLevel)));

            comboBox_compressLevel.SelectedIndex = comboBox_compressFormat.SelectedIndex = 0;
        }
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private SettingsService settings = (SettingsService)ServicesManager.GetService("Global Settings");
        private Rom[] romsToSend;
        private int progress1 = 0;
        private string progressText1 = "";
        private int progress2 = 0;
        private string progressText2 = "";
        private Thread mainThread;

        enum CopyMode
        {
            Rom, RomRelated, Related
        }
        //options
        private string _DestinationFolder;
        private bool _ClearDestinationFolder;
        private bool _MoveRoms;
        private CopyMode _CopyMode;
        private bool _OpenDestinationFolder;
        private bool _ExtractRoms;
        private bool _create_related_folders;
        private bool _ExtractCreateFolderForEachRom;
        private bool _ExtractUsePAss;
        private string _ExtractPAss;
        private bool _CompressRoms;
        private string _CompressFormat;
        private string _CompressLevel;
        private bool _CompressUsePass;
        private bool _CompressIgnoreArchive;
        private bool _CompressPreserve;
        private string _CompressPass;
        private bool _MoveRelatedFiles;
        private bool done = false;

        bool IsFileArchive(string fileName)
        {
            List<string> archiveExtensions = new List<string>(new string[] { ".7z", ".rar", ".zip", ".gzip", ".tar", ".bzip2", ".xz" });

            return archiveExtensions.Contains(Path.GetExtension(fileName).ToLower());
        }
        private void StartProgress()
        {
            List<string> log = new List<string>();
            #region Empty the destination folder
            if (_ClearDestinationFolder)
            {
                string[] files = Directory.GetFiles(_DestinationFolder, "*", SearchOption.AllDirectories);
                for (int j = 0; j < files.Length; j++)
                {
                    progress1 = (100 * j) / files.Length;
                    progressText1 = ls["Status_ClearingTheDestinationFolder"] + " " + progress1 + " %";
                    try
                    {
                        FileInfo inf = new FileInfo(files[j]);
                        inf.IsReadOnly = false;
                        File.Delete(files[j]);
                    }
                    catch { }
                }
                files = Directory.GetDirectories(_DestinationFolder, "*", SearchOption.AllDirectories);
                for (int j = 0; j < files.Length; j++)
                {
                    progress1 = (100 * j) / files.Length;
                    progressText1 = ls["Status_ClearingTheDestinationFolder"] + " " + progress1 + " %";
                    try
                    {
                        Directory.Delete(files[j]);
                    }
                    catch { }
                }
            }
            #endregion
            //Create folders if we need to
            string _romsDir = _DestinationFolder + "\\Roms\\";
            Directory.CreateDirectory(_romsDir);

            #region Roms copy/move
            SevenZipCompressor comper = new SevenZipCompressor();
            comper.ArchiveFormat = (OutArchiveFormat)Enum.Parse(typeof(OutArchiveFormat), _CompressFormat);
            comper.CompressionLevel = (CompressionLevel)Enum.Parse(typeof(CompressionLevel), _CompressLevel);
            comper.Compressing += comper_Compressing;
            comper.FileCompressionStarted += comper_FileCompressionStarted;
            comper.PreserveDirectoryRoot = _CompressPreserve;
            string _ArchiveExtension = "." + _CompressFormat.ToLower().Replace("sevenzip", "7z");

            int i = 0;
            foreach (Rom rom in romsToSend)
            {
                string romPath = "";
                bool deleteRomFile = false;
                bool _CopyRelatedFiles = (_CopyMode == CopyMode.Related) || (_CopyMode == CopyMode.RomRelated);
                bool _CopyRomFile = (_CopyMode == CopyMode.Rom) || (_CopyMode == CopyMode.RomRelated);
                // AI path ?
                if (HelperTools.IsAIPath(rom.Path))
                {
                    int index = HelperTools.GetIndexFromAIPath(rom.Path);
                    string path = HelperTools.GetFullPath(HelperTools.GetPathFromAIPath(rom.Path));
                    SevenZipExtractor extractor;
                    if (_ExtractUsePAss)
                    {
                        extractor = new SevenZipExtractor(path, _ExtractPAss);
                    }
                    else
                    {
                        extractor = new SevenZipExtractor(path);
                    }
                    extractor.Extracting += extractor_Extracting;
                    extractor.FileExtractionStarted += extractor_FileExtractionStarted;
                    string tempFolder = HelperTools.GetFullPath((string)settings.GetValue(DefaultProfileSettings.TempFolder_Key,
                    true, DefaultProfileSettings.TempFolder));
                    if (_CopyRomFile)
                        extractor.ExtractFiles(tempFolder + "\\" + extractor.ArchiveFileData[index].FileName, new int[] { index });
                    romPath = tempFolder + "\\" + extractor.ArchiveFileData[index].FileName;
                    deleteRomFile = true;
                }
                else
                {
                    romPath = HelperTools.GetFullPath(rom.Path);
                    deleteRomFile = false;
                }
                if (_CopyRomFile)
                {
                    if (_CompressRoms)
                    {
                        progressText1 = ls["Status_Compressing"] + " " + progress1 + " %";
                        if (File.Exists(romPath))
                        {
                            bool cancelCompress = false;
                            if (_CompressIgnoreArchive)
                                if (IsFileArchive(romPath))
                                    cancelCompress = true;
                            if (!cancelCompress)
                            {
                                if (!_CompressUsePass)
                                    comper.CompressFiles(_romsDir + "\\" + Path.GetFileNameWithoutExtension(romPath) + _ArchiveExtension,
                                        new string[] { romPath });
                                else
                                    comper.CompressFilesEncrypted(_romsDir + "\\" + Path.GetFileNameWithoutExtension(romPath) +
                                        _ArchiveExtension, _CompressPass, new string[] { romPath });
                            }
                            else
                            {
                                //normal copy
                                try
                                {
                                    File.Copy(romPath, _romsDir + "\\" + Path.GetFileName(romPath));
                                }
                                catch (Exception ex)
                                {
                                    log.Add(ls["Log_UnableToCopyRomFile"] +
                                        ": " + romPath + " [" + ex.Message + "]");
                                }
                            }
                        }
                        else
                        {
                            log.Add(ls["Log_UnableToCompressRomFile"] +
                                ": " + romPath + ", " + ls["Log_FileNotExist"]);
                        }
                    }
                    else if (_ExtractRoms)
                    {
                        progressText1 = ls["Status_Extracting"] + " " + progress1 + " %";
                        string _extractDir = _romsDir;
                        try
                        {
                            if (IsFileArchive(romPath))
                            {
                                SevenZipExtractor extractor;
                                if (_ExtractUsePAss)
                                {
                                    extractor = new SevenZipExtractor(romPath, _ExtractPAss);
                                }
                                else
                                {
                                    extractor = new SevenZipExtractor(romPath);
                                }
                                extractor.Extracting += extractor_Extracting;
                                extractor.FileExtractionStarted += extractor_FileExtractionStarted;
                                if (_ExtractCreateFolderForEachRom)
                                {
                                    _extractDir = _romsDir + "\\" + Path.GetFileNameWithoutExtension(romPath) + "\\";
                                    Directory.CreateDirectory(_extractDir);
                                }
                                extractor.ExtractArchive(_extractDir);
                            }
                            else
                            {
                                //normal copy
                                try
                                {
                                    File.Copy(romPath, _romsDir + "\\" + Path.GetFileName(romPath));
                                }
                                catch (Exception ex)
                                {
                                    log.Add(ls["Log_UnableToCopyRomFile"] +
                                      ": " + romPath + " [" + ex.Message + "]");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Add(ls["Log_UnableToExtractRomFile"] +
                                ": " + romPath + " [" + ex.Message + "]");
                        }
                    }
                    else//Normal copy
                    {
                        progressText1 = ls["Status_Copying"] + " " + progress1 + " %";
                        try
                        {
                            File.Copy(romPath, _romsDir + "\\" + Path.GetFileName(romPath));
                        }
                        catch (Exception ex)
                        {
                            log.Add(ls["Log_UnableToCopyRomFile"] +
                            ": " + romPath + " [" + ex.Message + "]");
                        }
                    }
                }
                if (_CopyRelatedFiles)
                {
                    // Get parent console
                    EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[rom.ParentConsoleID];
                    if (console != null)
                    {
                        // Create folders

                        foreach (InformationContainer cont in console.InformationContainers)
                        {
                            if (cont is InformationContainerFiles)
                            {
                                string cc_dir = _DestinationFolder + "\\" + cont.DisplayName + "\\";
                                Directory.CreateDirectory(cc_dir);
                                string _containerDir = cc_dir + Path.GetFileNameWithoutExtension(romPath) + "\\";

                                //get the item from the rom
                                InformationContainerItemFiles item = (InformationContainerItemFiles)rom.GetInformationContainerItem(cont.ID);
                                if (item != null)
                                {
                                    if (item.Files != null)
                                    {
                                        if (_create_related_folders)
                                            Directory.CreateDirectory(_containerDir);
                                        for (int o = 0; o < item.Files.Count; o++)
                                        {
                                            try
                                            {
                                                if (_create_related_folders)
                                                    File.Copy(HelperTools.GetFullPath(item.Files[o]), _containerDir + Path.GetFileName(item.Files[o]));
                                                else
                                                {
                                                    string ff = cc_dir + Path.GetFileName(item.Files[o]);
                                                    int h = 0;
                                                    while (File.Exists(ff))
                                                    {
                                                        ff = cc_dir + Path.GetFileNameWithoutExtension(item.Files[o]) + "_" + h + Path.GetExtension(item.Files[o]);
                                                        h++;
                                                    }
                                                    File.Copy(HelperTools.GetFullPath(item.Files[o]), ff);
                                                }
                                                if (_MoveRelatedFiles)
                                                {
                                                    File.Delete(HelperTools.GetFullPath(item.Files[o]));
                                                    item.Files.RemoveAt(o);
                                                    o--;
                                                }
                                            }
                                            catch (Exception ex)
                                            { log.Add(ls["Log_UnableToCopyRomRelatedFile"] + ": " + item.Files[o] + " [" + ex.Message + "]"); }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (_CopyRomFile)// Roms can be deleted only if they are copied !!
                {
                    if (deleteRomFile)
                    {
                        try
                        {
                            File.Delete(HelperTools.GetFullPath(romPath));
                        }
                        catch
                        {
                        }
                    }
                    else if (_MoveRoms)
                    {
                        try
                        {
                            File.Delete(HelperTools.GetFullPath(romPath));
                        }
                        catch (Exception ex)
                        {
                            log.Add(ls["Log_UnableToDeleteRomFile"] +
                                ": " + HelperTools.GetFullPath(romPath) + " [" + ex.Message + "]");
                        }
                    }
                }
                i++;
                progress1 = (100 * i) / romsToSend.Length;
            }
            #endregion
            //done !!
            progress1 = progress2 = 100;
            progressText1 = progressText2 = ls["Status_Done"];
            if (_OpenDestinationFolder)
                System.Diagnostics.Process.Start(_DestinationFolder);
            if (log.Count > 0)
            {
                File.WriteAllLines(_DestinationFolder + "\\log.txt", log.ToArray());
                System.Diagnostics.Process.Start(_DestinationFolder + "\\log.txt");
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
        private void extractor_FileExtractionStarted(object sender, FileInfoEventArgs e)
        {
            progress2 = e.PercentDone;
            progressText2 = ls["Status_Extracting"] + " " + e.PercentDone + " %";
        }
        private void extractor_Extracting(object sender, ProgressEventArgs e)
        {
            progress2 = e.PercentDone;
            progressText2 = ls["Status_Extracting"] + " " + e.PercentDone + " %";
        }
        private void comper_FileCompressionStarted(object sender, FileNameEventArgs e)
        {
            progress2 = e.PercentDone;
            progressText2 = ls["Status_Compressing"] + " " + e.PercentDone + " %";
        }
        private void comper_Compressing(object sender, ProgressEventArgs e)
        {
            progress2 = e.PercentDone;
            progressText2 = ls["Status_Compressing"] + " " + e.PercentDone + " %";
        }
        private void checkBox_ExtractRomsIfArchive_CheckedChanged(object sender, EventArgs e)
        {
            checkBox_extractCreateFolders.Enabled =
                checkBox_ExtractRomsIfArchive.Checked;
            if (checkBox_ExtractRomsIfArchive.Checked)
                checkBox_compressRoms.Checked = false;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox_ignoreArchiveInCompress.Enabled = groupBox4.Enabled = checkBox_compressRoms.Checked;
            if (checkBox_compressRoms.Checked)
                checkBox_ExtractRomsIfArchive.Checked = false;
        }
        //browse for destination folder
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.SelectedPath = textBox_destinationFolder.Text;
            if (textBox_destinationFolder.Text.Length > 0)
                fol.SelectedPath = textBox_destinationFolder.Text;
            if (fol.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_destinationFolder.Text = fol.SelectedPath;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
        //start
        private void button2_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(textBox_destinationFolder.Text))
            {
                ManagedMessageBox.ShowMessage(ls["Message_PleaseBrowseForDestinationFolderFirst"],
                   ls["MessageCaption_SendTo"]);
                return;
            }
            if (checkBox_compressRoms.Checked)
            {
                if (checkBox_CompressPassword.Checked)
                {
                    if (textBox_compressPassword.Text.Length == 0)
                    {
                        ManagedMessageBox.ShowMessage(ls["Message_PleaseEnterACompressPassword"],
                            ls["MessageCaption_SendTo"]);
                        return;
                    }
                }
            }
            if (checkBox_ExtractRomsIfArchive.Checked)
            {
                if (checkBox_extractPassword.Checked)
                {
                    if (textBox_compressPassword.Text.Length == 0)
                    {
                        ManagedMessageBox.ShowMessage(ls["Message_PleaseEnterExtractPassword"],
                           ls["MessageCaption_SendTo"]);
                        return;
                    }
                }
            }
            //Disable things
            button2.Enabled = groupBox_destinationFolder.Enabled = groupBox_options.Enabled = false;
            progressBar2.Visible = label_progress2.Visible = progressBar1.Visible = label_progress.Visible = true;
            //Get options
            _DestinationFolder = textBox_destinationFolder.Text;
            _ClearDestinationFolder = checkBox_empty.Checked;
            _MoveRoms = checkBox_moveRoms.Checked;

            if (radioButton_copy_mode_rom.Checked)
                _CopyMode = CopyMode.Rom;
            if (radioButton_copy_mode_roms_related.Checked)
                _CopyMode = CopyMode.RomRelated;
            if (radioButton1.Checked)
                _CopyMode = CopyMode.Related;

            _OpenDestinationFolder = checkBox_OpenDestinationFolderWhenDone.Checked;
            _ExtractRoms = checkBox_ExtractRomsIfArchive.Checked;
            _ExtractCreateFolderForEachRom = checkBox_extractCreateFolders.Checked;
            _ExtractUsePAss = checkBox_extractPassword.Checked;
            _ExtractPAss = textBox_extractPass.Text;
            _CompressRoms = checkBox_compressRoms.Checked;
            _CompressFormat = comboBox_compressFormat.SelectedItem.ToString();
            _CompressLevel = comboBox_compressLevel.SelectedItem.ToString();
            _CompressUsePass = checkBox_CompressPassword.Checked;
            _CompressPass = textBox_compressPassword.Text;
            _CompressIgnoreArchive = checkBox_ignoreArchiveInCompress.Checked;
            _CompressPreserve = checkBox_compreesPreserve.Checked;
            _MoveRelatedFiles = checkBox_moveRelatedFiles.Checked;
            _create_related_folders = checkBox_create_related_folders.Checked;
            //start
            timer1.Start();
            mainThread = new Thread(new ThreadStart(StartProgress));
            mainThread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
            mainThread.Start();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Value = progress1;
            label_progress.Text = progressText1;
            progressBar2.Value = progress2;
            label_progress2.Text = progressText2;
        }
        private void Frm_SendTo_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!done)
            {
                if (mainThread != null)
                {
                    if (mainThread.IsAlive)
                    {
                        ManagedMessageBoxResult result =
                            ManagedMessageBox.ShowQuestionMessage(ls["Message_AreYouSureYouWantRoStopCurrentProgress"],
                            ls["MessageCaption_SendTo"]);
                        if (result.ClickedButtonIndex == 0)
                        {
                            mainThread.Abort();
                            mainThread = null;
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }
                }
            }
        }
        private void checkBox_copyRelatedFiles_CheckedChanged(object sender, EventArgs e)
        {
            checkBox_moveRelatedFiles.Enabled = !radioButton_copy_mode_rom.Checked;
        }
        private void radioButton_copy_mode_rom_CheckedChanged(object sender, EventArgs e)
        {
            checkBox_moveRelatedFiles.Enabled = !radioButton_copy_mode_rom.Checked;
            groupBox4.Enabled = checkBox_compressRoms.Enabled = textBox_extractPass.Enabled =
        checkBox_extractPassword.Enabled = checkBox_extractCreateFolders.Enabled =
        checkBox_ExtractRomsIfArchive.Enabled = checkBox_moveRoms.Enabled =
        !radioButton1.Checked;
        }
        private void radioButton_copy_mode_roms_related_CheckedChanged(object sender, EventArgs e)
        {
            checkBox_moveRelatedFiles.Enabled = !radioButton_copy_mode_rom.Checked;
            groupBox4.Enabled = checkBox_compressRoms.Enabled = textBox_extractPass.Enabled =
          checkBox_extractPassword.Enabled = checkBox_extractCreateFolders.Enabled =
          checkBox_ExtractRomsIfArchive.Enabled = checkBox_moveRoms.Enabled = checkBox_create_related_folders.Enabled =
          !radioButton1.Checked;
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox_moveRelatedFiles.Enabled = !radioButton_copy_mode_rom.Checked;
            groupBox4.Enabled = checkBox_compressRoms.Enabled = textBox_extractPass.Enabled =
                checkBox_extractPassword.Enabled = checkBox_extractCreateFolders.Enabled =
                checkBox_ExtractRomsIfArchive.Enabled = checkBox_moveRoms.Enabled =
                !radioButton1.Checked;
        }
    }
}
