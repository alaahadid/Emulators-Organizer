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
using System.Threading;
using System.IO;
using System.Diagnostics;
using SevenZip;
using EmulatorsOrganizer.Services;

namespace EmulatorsOrganizer.Core
{
    public partial class Form_ExtractArchive : Form
    {
        public Form_ExtractArchive(string romPath, string tempFolder, string[] extensions,
            bool selectFirstFileIfMoreThanOneFound, bool extractAll)
        {
            this.romPath = romPath;
            this.tempFolder = tempFolder;
            this.selectFirstFileIfMoreThanOneFound = selectFirstFileIfMoreThanOneFound;
            this.extensions = extensions;
            this.extractAll = extractAll;
            this.AIPathMode = false;
            InitializeComponent();
        }
        public Form_ExtractArchive(string romAIPath, string tempFolder, bool extractAll)
        {
            this.romPath = romAIPath;
            this.tempFolder = tempFolder;
            this.extractAll = extractAll;
            this.AIPathMode = true;
            InitializeComponent();
        }
        private bool AIPathMode;
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private SevenZip.SevenZipExtractor extractor;
        private Thread mainThread = null;
        private string[] extensions;
        private bool extractAll;
        private string outFilePath;
        private string status = "";
        private int progress = 0;
        private bool selectFirstFileIfMoreThanOneFound;
        private string romPath;
        private string tempFolder;

        public string OutFilePath
        { get { return outFilePath; } }
        private void Form_ExtractArchive_Shown(object sender, EventArgs e)
        {
            //mainThread = new Thread(new ThreadStart(PROGRESS));
            //mainThread.Start();
            PROGRESS();
        }

        private void CloseWithCancel()
        {
            if (!this.InvokeRequired)
                CloseWithCancel1();
            else
                this.Invoke(new Action(CloseWithCancel1));
        }
        private void CloseWithCancel1()
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
        private void CloseWithSuccess()
        {
            if (!this.InvokeRequired)
                CloseWithSuccess1();
            else
                this.Invoke(new Action(CloseWithSuccess1));
        }
        private void CloseWithSuccess1()
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void PROGRESS()
        {
            if (!File.Exists("7z.dll"))
            {
                MessageBox.Show(ls["Message_7ZNotFound"]);
                CloseWithCancel();
                return;
            }
            #region Normal mode
            if (!AIPathMode)
            {
                try
                {
                    extractor = new SevenZipExtractor(romPath);
                    if (extractor.ArchiveFileData.Count == 0)
                    {
                        CloseWithCancel(); MessageBox.Show(ls["Message_ThisArchiveIsEmpty"]); return;
                    }
                    if (extractor.ArchiveFileData.Count == 1)
                    {
                        #region One file detected
                        if (extractor.ArchiveFileData[0].Encrypted)
                        {
                            // This file is encrypted, demand password
                            Form_EnterPassword frm = new Form_EnterPassword();
                            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                            {
                                try
                                {
                                    extractor = new SevenZipExtractor(romPath, frm.EnteredPassword);
                                    timer1.Start();
                                    extractor.Extracting += extractor_Extracting;
                                    extractor.ExtractionFinished += extractor_ExtractionFinished;

                                    extractor.ExtractArchive(tempFolder);
                                    outFilePath = tempFolder + "\\" + extractor.ArchiveFileData[0].FileName;
                                    CloseWithSuccess();
                                }
                                catch
                                {
                                    timer1.Stop();
                                    CloseWithCancel();
                                    MessageBox.Show(ls["Message_WrongPasswordOrFileIsDamaged"]);
                                }
                            }
                            else
                            {
                                timer1.Stop();
                                CloseWithCancel();
                            }
                        }
                        else
                        {
                            // Not encrybted, just extract.
                            try
                            {
                                timer1.Start();
                                extractor.Extracting += extractor_Extracting;
                                extractor.ExtractionFinished += extractor_ExtractionFinished;

                                extractor.ExtractArchive(tempFolder);
                                outFilePath = tempFolder + "\\" + extractor.ArchiveFileData[0].FileName;
                                CloseWithSuccess();
                            }
                            catch (Exception ex)
                            {
                                timer1.Stop();
                                CloseWithCancel();
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                        #endregion
                    else// more than one file
                    {
                        List<string> filenames = new List<string>();
                        foreach (SevenZip.ArchiveFileInfo file in extractor.ArchiveFileData)
                        {
                            if (extensions.Length > 0)
                            {
                                // Only add the files that match the extensions list.
                                if (extensions.Contains(Path.GetExtension(file.FileName).ToLower()))
                                    filenames.Add(file.FileName);
                            }
                            else// Just add that file.
                                filenames.Add(file.FileName);
                        }
                        Frm_ArchiveFiles ar = new Frm_ArchiveFiles(filenames.ToArray(), selectFirstFileIfMoreThanOneFound);
                        int fileIndex = 0;
                        if (!selectFirstFileIfMoreThanOneFound)
                        {
                            if (ar.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                            {
                                fileIndex = ar.SelectedRomIndex;
                            }
                            else
                            {
                                CloseWithCancel(); return;
                            }
                        }
                        if (extractor.ArchiveFileData[fileIndex].Encrypted)
                        {
                            Form_EnterPassword frm = new Form_EnterPassword();
                            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                            {
                                try
                                {
                                    extractor = new SevenZipExtractor(romPath, frm.EnteredPassword);
                                    timer1.Start();
                                    extractor.Extracting += extractor_Extracting;
                                    extractor.ExtractionFinished += extractor_ExtractionFinished;

                                    if (!extractAll)
                                    {
                                        extractor.ExtractFiles(tempFolder,
                                            new string[] { extractor.ArchiveFileData[fileIndex].FileName });
                                    }
                                    else
                                    {
                                        // Extract all, send selected to emu
                                        extractor.ExtractFiles(tempFolder, filenames.ToArray());
                                    }
                                    outFilePath = tempFolder + "\\" + extractor.ArchiveFileData[fileIndex].FileName;
                                    CloseWithSuccess();
                                }
                                catch
                                {
                                    timer1.Stop();
                                    CloseWithCancel();
                                    MessageBox.Show(ls["Message_WrongPasswordOrFileIsDamaged"]);
                                }
                            }
                            else
                            {
                                timer1.Stop();
                                CloseWithCancel();
                            }
                        }
                        else
                        {
                            try
                            {
                                timer1.Start();
                                extractor.Extracting += extractor_Extracting;
                                extractor.ExtractionFinished += extractor_ExtractionFinished;
                                if (!extractAll)
                                {
                                    extractor.ExtractFiles(tempFolder,
                                        new string[] { extractor.ArchiveFileData[fileIndex].FileName });
                                }
                                else
                                {
                                    // Extract all, send selected to emu
                                    extractor.ExtractFiles(tempFolder, filenames.ToArray());
                                }
                                outFilePath = tempFolder + "\\" + extractor.ArchiveFileData[fileIndex].FileName;
                                CloseWithSuccess();
                            }
                            catch (Exception ex)
                            {
                                timer1.Stop();
                                CloseWithCancel();
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = System.Windows.Forms.DialogResult.Abort;
                    Close();
                }
            }
            #endregion
            #region AI mode
            else
            {
                try
                {
                    extractor = new SevenZipExtractor(HelperTools.GetPathFromAIPath(romPath));
                    if (extractor.ArchiveFileData.Count == 0)
                    {
                        CloseWithCancel(); MessageBox.Show(ls["Message_ThisArchiveIsEmpty"]); return;
                    }

                    int fileIndex = HelperTools.GetIndexFromAIPath(romPath);
                    Trace.WriteLine(">Index = " + fileIndex);
                    Trace.WriteLine(">Path = " + HelperTools.GetPathFromAIPath(romPath));
                    if (extractor.ArchiveFileData[fileIndex].Encrypted)
                    {
                        Form_EnterPassword frm = new Form_EnterPassword();
                        if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                        {
                            try
                            {
                                extractor = new SevenZipExtractor(HelperTools.GetPathFromAIPath(romPath),
                                    frm.EnteredPassword);
                                timer1.Start();
                                extractor.Extracting += extractor_Extracting;
                                extractor.ExtractionFinished += extractor_ExtractionFinished;

                                if (!extractAll)
                                {
                                    extractor.ExtractFiles(tempFolder,
                                        new string[] { extractor.ArchiveFileData[fileIndex].FileName });
                                }
                                else
                                {
                                    // Extract all, send selected to emu
                                    extractor.ExtractFiles(tempFolder, extractor.ArchiveFileNames.ToArray());
                                }
                                outFilePath = tempFolder + "\\" + extractor.ArchiveFileData[fileIndex].FileName;
                                CloseWithSuccess();
                            }
                            catch
                            {
                                timer1.Stop();
                                CloseWithCancel();
                                MessageBox.Show(ls["Message_WrongPasswordOrFileIsDamaged"]);
                            }
                        }
                        else
                        {
                            timer1.Stop();
                            CloseWithCancel();
                        }
                    }
                    else
                    {
                        try
                        {
                            timer1.Start();
                            extractor.Extracting += extractor_Extracting;
                            extractor.ExtractionFinished += extractor_ExtractionFinished;
                            if (!extractAll)
                            {
                                extractor.ExtractFiles(tempFolder,
                                    new string[] { extractor.ArchiveFileData[fileIndex].FileName });
                            }
                            else
                            {
                                // Extract all, send selected to emu
                                extractor.ExtractFiles(tempFolder, extractor.ArchiveFileNames.ToArray());
                            }
                            outFilePath = tempFolder + "\\" + extractor.ArchiveFileData[fileIndex].FileName;
                            CloseWithSuccess();
                        }
                        catch (Exception ex)
                        {
                            timer1.Stop();
                            CloseWithCancel();
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = System.Windows.Forms.DialogResult.Abort;
                    Close();
                }
            }
            #endregion
        }

        private void extractor_ExtractionFinished(object sender, EventArgs e)
        {
            CloseWithSuccess();
        }
        private void extractor_Extracting(object sender, ProgressEventArgs e)
        {
            status = ls["Status_Extracting"] + " " + e.PercentDone + "%";
            progress = e.PercentDone;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            label_info.Text = status;
            progressBar1.Value = progress;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (mainThread != null)
                if (mainThread.IsAlive)
                    mainThread.Abort();
            CloseWithCancel();
        }
    }
}
