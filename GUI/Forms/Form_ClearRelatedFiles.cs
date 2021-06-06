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
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;
using EmulatorsOrganizer.Services.DefaultServices.Settings;
using MMB;

namespace EmulatorsOrganizer.GUI
{
    public partial class Form_ClearRelatedFiles : Form
    {
        public Form_ClearRelatedFiles(string[] ids)
        {
            InitializeComponent();
            this.ids = ids;
        }
        private string[] ids;
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private SettingsService settings = (SettingsService)ServicesManager.GetService("Global Settings");
        private TextWriterTraceListener listner;
        private string logPath;
        private Thread mainThread;
        private string status = "";
        private int progressValue = 0;
        private bool finished;
        private bool deleteRelatedFromDisk;

        private void PROCESS()
        {
            ServicesManager.OnDisableWindowListner();
            // Add listener
            logPath = ".\\Logs\\" + DateTime.Now.ToLocalTime().ToString() + "-clear related files.txt";
            logPath = logPath.Replace(":", "");
            logPath = logPath.Replace("/", "-");
            listner = new TextWriterTraceListener(HelperTools.GetFullPath(logPath));
            Trace.Listeners.Add(listner);
            // Start
            Trace.WriteLine("Clear Related Files started at " + DateTime.Now.ToLocalTime(), "Clear Related Files");
            int i = 0;
            foreach (string romID in ids)
            {
                Rom rom = profileManager.Profile.Roms[romID];
                Trace.WriteLine(">Removing rom related files for '" + rom.Name + "'... ", "Clear Related Files");
                if (rom.RomInfoItems != null)
                {
                    for (int r = 0; r < rom.RomInfoItems.Count; r++)
                    {
                        if (rom.RomInfoItems[r] is InformationContainerItemFiles)
                        {
                            if (deleteRelatedFromDisk)
                            {
                                if (((InformationContainerItemFiles)rom.RomInfoItems[r]).Files != null)
                                {
                                    foreach (string rr in ((InformationContainerItemFiles)rom.RomInfoItems[r]).Files)
                                    {
                                        try
                                        {
                                            File.Delete(HelperTools.GetFullPath(rr));
                                            Trace.WriteLine(">>file removed: " + rr, "Clear Related Files");
                                        }
                                        catch (Exception ex)
                                        {
                                            Trace.WriteLine(">>UNABLE to remove file: " + rr, "Clear Related Files");
                                            Trace.WriteLine(">>" + ex.Message, "Clear Related Files");
                                        }
                                    }
                                }
                            }
                            // Remove this related
                            rom.RomInfoItems.RemoveAt(r);
                            rom.Modified = true;
                            r--;
                        }
                    }
                }
                else
                {
                    Trace.WriteLine(">Rom has no related file.", "Roms browser");
                }
                progressValue = (i * 100) / ids.Length;
                status = progressValue + "%";
                i++;
            }
            ServicesManager.OnEnableWindowListner();
            Trace.WriteLine("Clear Related Files finished at " + DateTime.Now.ToLocalTime(), "Clear Related Files");
            listner.Flush();
            Trace.Listeners.Remove(listner);
            CloseAfterFinish();
        }
        private void CloseAfterFinish()
        {
            if (!this.InvokeRequired)
                CloseAfterFinish1();
            else
                this.Invoke(new Action(CloseAfterFinish1));
        }
        private void CloseAfterFinish1()
        {
            finished = true;
            timer1.Stop();
            ManagedMessageBoxResult res = ManagedMessageBox.ShowMessage(ls["Message_Done"] + ", " + ls["Message_LogFileSavedTo"] + " '" + logPath + "'",
          ls["MessageCaption_ClearRelatedFiles"], new string[] { ls["Button_Ok"], ls["Button_OpenLog"] },
          0, null, ManagedMessageBoxIcon.Info);
            if (res.ClickedButtonIndex == 1) { try { Process.Start(HelperTools.GetFullPath(logPath)); } catch { } }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
        // Cancel
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
        // Start
        private void button1_Click(object sender, EventArgs e)
        {
            // Poke options
            deleteRelatedFromDisk = checkBox_deleteFilesFromDiskToo.Checked;
            // Enable/Disable things
            checkBox_deleteFilesFromDiskToo.Enabled = button1.Enabled = false;
            timer1.Start();
            // Start thread !
            mainThread = new Thread(new ThreadStart(PROCESS));
            mainThread.CurrentUICulture = ls.CultureInfo;
            mainThread.Start();
        }

        private void Form_ClearRelatedFiles_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!finished)
            {
                if (mainThread != null)
                {
                    if (mainThread.IsAlive)
                    {
                        ManagedMessageBoxResult result =
                            ManagedMessageBox.ShowQuestionMessage(ls["Message_AreYouSureYouWantRoStopCurrentProgress"],
                            ls["MessageCaption_ImportCSVDB"]);
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
        private void timer1_Tick(object sender, EventArgs e)
        {
            label_status.Text = status;
            progressBar1.Value = progressValue;
        }
    }
}
