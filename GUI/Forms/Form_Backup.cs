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
using System.Threading;
using System.Resources;
using System.Reflection;
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;
using System.Security.Cryptography;
using MMB;
using SevenZip;
namespace EmulatorsOrganizer.GUI
{
    public partial class Form_Backup : Form
    {
        public Form_Backup(string consoleID, string icID)
        {
            InitializeComponent();
            this.consoleID = consoleID;
            this.icID = icID;
            SaveFileDialog sav = new SaveFileDialog();
            sav.Filter = ls["Filter_BackupFile"];
            if (sav.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                filepath = sav.FileName;
                timer1.Start();
                mainThread = new Thread(new ThreadStart(Progress));
                mainThread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
                mainThread.Start();
            }
            else
            {
                Close();
            }
        }
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private string consoleID;
        private string filepath = "";
        private string icID;
        private Thread mainThread;
        private bool done;

        private int progress;
        private void Progress()
        {
            List<string> lines = new List<string>();
            List<string> folders = new List<string>();
            lines.Add("# Emulators Organizer information backup");
            lines.Add("#");
            lines.Add("");
            int i = 0;
            Rom[] roms = profileManager.Profile.Roms[consoleID, false];
            foreach (Rom rom in roms)
            {
                InformationContainerItemFiles icitem = (InformationContainerItemFiles)rom.GetInformationContainerItem(icID);
                if (icitem != null)
                {
                    if (icitem.Files != null)
                    {
                        if (icitem.Files.Count > 0)
                        {
                            lines.Add("$Rom");
                            lines.Add("Name=" + rom.Name);
                            if (HelperTools.IsAIPath(roms[i].Path))
                            {
                                lines.Add("FileName=" + Path.GetFileName(HelperTools.GetPathFromAIPath(rom.Path)));
                                // Extract the content of the archive first
                                SevenZipExtractor extractor = new SevenZipExtractor(HelperTools.GetFullPath(HelperTools.GetPathFromAIPath(roms[i].Path)));
                                int index = HelperTools.GetIndexFromAIPath(roms[i].Path);
                                Stream mstream = null;

                                // Try to extract and get data
                                try
                                {
                                    mstream = new FileStream(Path.GetTempPath() + "\\test.tst", FileMode.Create, FileAccess.Write);
                                    extractor.ExtractFile(index, mstream);
                                    mstream.Close();
                                    mstream.Dispose();

                                    lines.Add("Sha1=" + CalculateSHA1(Path.GetTempPath() + "\\test.tst"));
                                }
                                catch { lines.Add("Sha1=00000000"); }
                            }
                            else
                            {
                                lines.Add("FileName=" + Path.GetFileName(rom.Path));
                                lines.Add("Sha1=" + HelperTools.CalculateSHA1(rom.Path));
                            }
                            lines.Add("$Files");
                            foreach (string file in icitem.Files)
                            {
                                lines.Add("FileName=" + Path.GetFileName(file));
                                lines.Add("Sha1=" + HelperTools.CalculateSHA1(file));

                                if (!folders.Contains(Path.GetDirectoryName(HelperTools.GetFullPath(file))))
                                {
                                    folders.Add(Path.GetDirectoryName(HelperTools.GetFullPath(file)));
                                }
                            }
                            lines.Add("");
                        }
                    }
                }
                progress = (i * 100) / roms.Length;
                i++;
            }
            //add folders
            lines.Add("$Folders");
            foreach (string folder in folders)
            {
                lines.Add("Path=" + folder);
            }
            //save
            File.WriteAllLines(filepath, lines.ToArray());
            done = true;
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

        private void Frm_Backup_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!done)
            {
                if (mainThread != null)
                {
                    if (mainThread.IsAlive)
                    {
                        ManagedMessageBoxResult result =
                            ManagedMessageBox.ShowQuestionMessage(ls["Message_AreYouSureYouWantRoStopCurrentProgress"]);
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

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Value = progress;
        }

        private byte[] GetBuffer(string filePath)
        {
            byte[] fileBuffer;

            Stream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            fileBuffer = new byte[fileStream.Length];
            fileStream.Read(fileBuffer, 0, (int)fileStream.Length);
            fileStream.Close();

            return fileBuffer;
        }

        private string CalculateSHA1(string filePath)
        {
            if (File.Exists(filePath))
            {
                byte[] fileBuffer = GetBuffer(filePath);

                string Sha1 = "";
                SHA1Managed managedSHA1 = new SHA1Managed();
                byte[] shaBuffer = managedSHA1.ComputeHash(fileBuffer);

                foreach (byte b in shaBuffer)
                    Sha1 += b.ToString("x2").ToLower();

                return Sha1;
            }
            return "";
        }
    }
}
