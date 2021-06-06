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
    public partial class Frm_GetInfoFromDatabase : Form
    {
        public Frm_GetInfoFromDatabase(string consoleID, string containerID)
        {
            this.containerID = containerID;
            this.consoleID = consoleID;
            InitializeComponent();
        }
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private string consoleID;
        private string containerID;

        //browse for dat file
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "MAME info dat (*.dat)|*.dat";
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = op.FileName;
            }
        }
        //Browse for destination folder
        private void button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.Description = ls["Message_PleaseBrowseForDestinationFolder"];
            if (fol.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_destinationFolder.Text = fol.SelectedPath;
            }
        }
        //start
        private void button2_Click(object sender, EventArgs e)
        {
            if (!File.Exists(textBox1.Text))
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseBrowseForInfoDatFileFirst"],
                    ls["MessageCaption_GetInfoFromDatFile"]);
                return;
            }
            if (!Directory.Exists(textBox_destinationFolder.Text))
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseBrowseForDestinationFolder"],
                   ls["MessageCaption_GetInfoFromDatFile"]);
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
                List<int> RomIndexes = new List<int>();
                Rom[] roms = profileManager.Profile.Roms[consoleID, false];
                int o = 0;
                foreach (Rom rom in roms)
                {
                    RomIndexes.Add(o);
                    o++;
                }
                List<string> FileBuffer = new List<string>(File.ReadAllLines(textBox1.Text));
                progressBar1.Maximum = FileBuffer.Count;
                progressBar1.Value = 0;
                label_status.Text = ls["Status_CreatingInfoFiles"];
                label_status.Refresh();
                //read the info file database, build text files
                for (int i = 0; i < FileBuffer.Count; i++)
                {
                    if (FileBuffer[i].StartsWith("#") || FileBuffer[i] == "")
                        continue;
                    //if (FileBuffer[i].StartsWith("$info"))
                    if (FileBuffer[i].StartsWith("$bio"))
                    {
                        i--;
                        string[] sp = FileBuffer[i].Split('=');
                        if (sp.Length < 2) continue;
                        string romName = sp[1];
                        string[] romNames = romName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        //get info
                        List<string> infoLines = new List<string>();
                        while (FileBuffer[i] != "$end")
                        {
                            if (!FileBuffer[i].StartsWith("$"))
                            {
                                infoLines.Add(FileBuffer[i]);
                            }
                            i++;
                        }
                        foreach (string romN in romNames)
                        {
                            if (romN == "")
                                continue;
                            bool found = false;
                            //search for the rom
                            for (int j = 0; j < RomIndexes.Count; j++)
                            {
                                if (romN.ToLower() ==
                                    Path.GetFileNameWithoutExtension(
                                    HelperTools.GetPathFromAIPath(roms[RomIndexes[j]].Path)).ToLower())
                                //if (romN.ToLower() == roms[RomIndexes[j]].Name.ToLower())
                                {
                                    //this is it !!
                                    //save the info file
                                    bool cancel = false;
                                    if (checkBox_dontReplaceInfo.Checked)
                                        if (File.Exists(HelperTools.GetFullPath(textBox_destinationFolder.Text) + "\\" + romN + ".txt"))
                                            cancel = true;

                                    if (!cancel)
                                        File.WriteAllLines(HelperTools.GetFullPath(textBox_destinationFolder.Text) + "\\" + romN + ".txt", infoLines.ToArray());

                                    //Set the rom to the info file, this is the hardest part :D
                                    InformationContainerItemFiles item;
                                    if (roms[RomIndexes[j]].IsInformationContainerItemExist(containerID))
                                    {
                                        item = ((InformationContainerItemFiles)roms[RomIndexes[j]].GetInformationContainerItem(containerID));
                                    }
                                    else
                                    {
                                        item = new InformationContainerItemFiles(profileManager.Profile.GenerateID(), containerID);
                                        // To make sure, get the rom like this
                                        profileManager.Profile.Roms[roms[RomIndexes[j]].ID].RomInfoItems.Add(item);
                                        profileManager.Profile.Roms[roms[RomIndexes[j]].ID].Modified = true;
                                    }
                                    if (item.Files == null)
                                        item.Files = new List<string>();
                                    string pathOfInf = HelperTools.GetDotPath(HelperTools.GetFullPath(textBox_destinationFolder.Text) + "\\" + romN + ".txt");
                                    if (!item.Files.Contains(pathOfInf))
                                        item.Files.Add(pathOfInf);

                                    //we've done with this rom, remove it !
                                    RomIndexes.RemoveAt(j);
                                    found = true;
                                    break;

                                }
                            }
                            if (!found)
                            {
                                if (checkBox_matchrom.Checked)
                                {
                                    // No need for the match, just save
                                    //save the info file
                                    bool cancel = false;
                                    if (checkBox_dontReplaceInfo.Checked)
                                        if (File.Exists(HelperTools.GetFullPath(textBox_destinationFolder.Text) + "\\" + romN + ".txt"))
                                            cancel = true;

                                    if (!cancel)
                                        File.WriteAllLines(HelperTools.GetFullPath(textBox_destinationFolder.Text) + "\\" + romN + ".txt",
                                            infoLines.ToArray());
                                }
                            }
                        }
                    }
                    progressBar1.Value = i;
                }
            }
            profileManager.Profile.OnInformationContainerItemsDetected();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
