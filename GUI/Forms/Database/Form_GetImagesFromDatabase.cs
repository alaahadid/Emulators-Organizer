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
using System.Windows.Forms;
using System.Threading;
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;
using MMB;
namespace EmulatorsOrganizer.GUI
{
    public partial class Frm_GetImagesFromDatabase : Form
    {
        public Frm_GetImagesFromDatabase(string consoleID, string containerID)
        {
            this.containerID = containerID;
            this.consoleID = consoleID;
            InitializeComponent();
        }
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private string consoleID;
        private string containerID;
        private Thread mainThread;

        private string status;
        private int progress = 0;
        /*options*/
        private string _databaseFile;
        private string _folderPath;
        private bool _clearRomOldCollection;
        private bool done = false;

        private void Progress()
        {
            status = ls["Status_ReadingDatabaseFile"];
            string[] lines = File.ReadAllLines(_databaseFile);
            List<string> images = new List<string>();
            int i = 0;
            foreach (string line in lines)
            {
                if (line.Contains("rom"))
                {
                    string[] codes = line.Split(new char[] { ' ' });
                    //images.Add(_folderPath + "\\" + codes[3]);
                    if (codes.Length > 1)
                        images.Add(_folderPath + "\\" + codes[1].Replace("name=", "").Replace(@"""", ""));
                }
                progress = (i * 100) / lines.Length;
                status = ls["Status_ReadingDatabaseFile"] + " (" + progress + " %)";
                i++;
            }
            status = ls["Status_ApplyingImages"];
            Rom[] roms = profileManager.Profile.Roms[consoleID, false];
            i = 0;
            foreach (Rom rom in roms)
            {
                foreach (string image in images)
                {
                    if (Path.GetFileNameWithoutExtension(HelperTools.GetPathFromAIPath(rom.Path)).ToLower()
                        == Path.GetFileNameWithoutExtension(image).ToLower())
                    //if (rom.Name.ToLower() == Path.GetFileNameWithoutExtension(image).ToLower())
                    {
                        InformationContainerItemFiles item = (InformationContainerItemFiles)rom.GetInformationContainerItem(containerID);
                        if (item == null)
                        {
                            item = new InformationContainerItemFiles(profileManager.Profile.GenerateID(), containerID);
                            item.Files = new List<string>();
                            item.Files.Add(HelperTools.GetDotPath(image));
                            // To make sure, just do get the rom like this
                            profileManager.Profile.Roms[rom.ID].RomInfoItems.Add(item);
                            profileManager.Profile.Roms[rom.ID].Modified = true;
                        }
                        else
                        {
                            if (item.Files != null)
                            {
                                if (_clearRomOldCollection)
                                    item.Files = new List<string>();
                                item.Files.Add(HelperTools.GetDotPath(image));
                            }
                            else
                            {
                                item.Files = new List<string>();
                                item.Files.Add(HelperTools.GetDotPath(image));
                            }
                        }
                        images.Remove(image);
                        break;
                    }
                }
                progress = (i * 100) / roms.Length;
                status = ls["Status_ApplyingImages"] + " (" + progress + " %)";
                i++;
            }
            if (!this.InvokeRequired)
                Close();
            else
                this.Invoke(new Action(Done));
        }
        private void Done()
        {
            done = true;
            profileManager.Profile.OnInformationContainerItemsDetected();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "MameUI file (*.dat)|*.dat";
            op.FileName = textBox_databaseFile.Text;
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_databaseFile.Text = op.FileName;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.SelectedPath = textBox_ImagesFolder.Text;
            if (fol.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                textBox_ImagesFolder.Text = fol.SelectedPath;
        }
        //start
        private void button3_Click(object sender, EventArgs e)
        {
            if (!File.Exists(textBox_databaseFile.Text))
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseBrowseForTheDatabaseFileFirst"],
                    ls["MessageCaption_GetImagesFromDatabase"]);
                return;
            }
            if (!Directory.Exists(textBox_ImagesFolder.Text))
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseBrowseForTheImagesFolderFirst"],
                    ls["MessageCaption_GetImagesFromDatabase"]);
                return;
            }
            //get options
            _clearRomOldCollection = checkBox_clearRomImagesCollection.Checked;
            _databaseFile = textBox_databaseFile.Text;
            _folderPath = textBox_ImagesFolder.Text;

            //disable things
            button3.Enabled = groupBox1.Enabled = groupBox2.Enabled = groupBox3.Enabled = false;
            //show things
            label_status.Visible = progressBar1.Visible = true;
            timer1.Start();

            mainThread = new Thread(new ThreadStart(Progress));
            mainThread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
            mainThread.Start();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void Frm_GetImagesFromDatabase_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!done)
            {
                if (mainThread != null)
                {
                    if (mainThread.IsAlive)
                    {
                        ManagedMessageBoxResult result =
                            ManagedMessageBox.ShowQuestionMessage(ls["Message_AreYouSureYouWantRoStopCurrentProgress"],
                            ls["MessageCaption_GetImagesFromDatabase"]);
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
            progressBar1.Value = progress;
        }
    }
}
