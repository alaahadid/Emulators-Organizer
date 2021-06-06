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
using MMB;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
namespace EmulatorsOrganizer.GUI
{
    public partial class Form_ExportToDatabase : Form
    {
        private bool aToZ = true;
        public Form_ExportToDatabase(string[] romIDS)
        {
            InitializeComponent();
            // Fill up formats
            DatabaseFilesManager.DetectSupportedFormats();
            comboBox_format.Items.Clear();
            foreach (DatabaseFile f in DatabaseFilesManager.AvailableFormats)
                if (f.Exportable)
                    comboBox_format.Items.Add(f);
            if (comboBox_format.Items.Count > 0)
                comboBox_format.SelectedIndex = 0;
            //refresh roms
            foreach (string romID in romIDS)
            {
                Rom rom = profileManager.Profile.Roms[romID];

                listView1.Items.Add(rom.Name);
                listView1.Items[listView1.Items.Count - 1].Checked = true;
                listView1.Items[listView1.Items.Count - 1].Tag = rom;
                listView1.Items[listView1.Items.Count - 1].SubItems.Add(Path.GetExtension(HelperTools.GetPathFromAIPath(rom.Path)));

                listView1.Items[listView1.Items.Count - 1].SubItems.Add(rom.SizeLable);

                listView1.Items[listView1.Items.Count - 1].SubItems.Add(rom.Path);
                listView1.Items[listView1.Items.Count - 1].Tag = romID;
            }
        }
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private List<DBEntry> dbFiles = new List<DBEntry>();
        private DatabaseFile selectedFormat;
        private string targetPath = "";
        private string status;
        private int progress;
        private bool finished;
        private Thread mainThread;

        private void PROGRESS()
        {
            // Save database
            selectedFormat.SaveFile(targetPath, dbFiles);

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
            button7.Enabled = groupBox1.Enabled = groupBox2.Enabled = groupBox3.Enabled = true;
            label_progress.Visible = progressBar1.Visible = false;
            timer1.Stop();
            ManagedMessageBox.ShowMessage(ls["Message_Done"], ls["Title_ExportNoIntroDatabaseFile"]);
        }
        private void button8_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //select all
        private void button1_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem it in listView1.Items)
                it.Checked = true;
        }
        //select none
        private void button2_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem it in listView1.Items)
                it.Checked = false;
        }
        //sort by ..
        private void button3_Click(object sender, EventArgs e)
        {
            listView1.ListViewItemSorter = new ListViewItemComparer(0, aToZ);
            aToZ = !aToZ;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            listView1.ListViewItemSorter = new ListViewItemComparer(1, aToZ);
            aToZ = !aToZ;
        }
        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            listView1.ListViewItemSorter = new ListViewItemComparer(e.Column, aToZ);
            aToZ = !aToZ;
        }
        //move up
        private void button5_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;
            //get selected item
            ListViewItem selected = listView1.SelectedItems[0];
            int index = selected.Index;
            //remove it !!
            listView1.Items.Remove(listView1.SelectedItems[0]);
            //move up
            index--;
            if (index < 0)
                index = 0;
            //insert
            listView1.Items.Insert(index, selected);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
                return;
            //get selected item
            ListViewItem selected = listView1.SelectedItems[0];
            int index = selected.Index;
            //remove it !!
            listView1.Items.Remove(listView1.SelectedItems[0]);
            //move down
            index++;
            if (index >= listView1.Items.Count)
                index = listView1.Items.Count;
            //insert
            listView1.Items.Insert(index, selected);
        }
        private void listView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;
            DoDragDrop(listView1.SelectedItems, DragDropEffects.Move);
        }
        private void listView1_DragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
                e.Effect = DragDropEffects.None;
        }
        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            Point targetPoint = listView1.PointToClient(new Point(e.X, e.Y));
            ListViewItem targetItem = listView1.GetItemAt(targetPoint.X, targetPoint.Y);
            int index = targetItem.Index;

            for (int i = 0; i < listView1.SelectedItems.Count; i++)
            {
                ListViewItem dragedItem = listView1.SelectedItems[i];
                listView1.Items.Remove(listView1.SelectedItems[i]);
                dragedItem.Selected = false;
                i = -1;

                listView1.Items.Insert(index, dragedItem);
                index++;
            }
        }
        //Export
        private void button7_Click(object sender, EventArgs e)
        {
            if (selectedFormat == null)
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseSelectDatabaseFileTypeFirst"]);
                return;
            }
            SaveFileDialog sav = new SaveFileDialog();
            sav.Title = ls["Title_ExportTo"] + " " + selectedFormat.Name;
            sav.Filter = DatabaseFilesManager.GetFilter(selectedFormat);
            if (sav.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                // Convert roms into dbfiles
                dbFiles = new List<DBEntry>();
                foreach (ListViewItem item in listView1.Items)
                {
                    if (item.Checked)
                    {
                        DBEntry dbFile = DBEntry.Empty;
                        Rom realRom = profileManager.Profile.Roms[item.Tag.ToString()];
                        EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[realRom.ParentConsoleID];
                        // Add name
                        dbFile.Properties.Add(new PropertyStruct("Name", item.Text));
                        // Add data elements
                        foreach (RomData romd in console.RomDataInfoElements)
                        {
                            object d = realRom.GetDataItemValue(romd.ID);
                            if (d == null)
                                d = "";
                            dbFile.Properties.Add(new PropertyStruct(romd.Name, d.ToString()));
                        }
                        // Categories
                        if (realRom.Categories != null)
                            if (realRom.Categories.Count > 0)
                            {
                                foreach (string cat in realRom.Categories)
                                    dbFile.Categories.Add(cat);
                            }
                        // Set the temp path
                        dbFile.TemporaryPathForExport = realRom.Path;

                        dbFiles.Add(dbFile);
                    }
                }
                button7.Enabled = groupBox1.Enabled = groupBox2.Enabled = groupBox3.Enabled = false;
                label_progress.Visible = progressBar1.Visible = true;
                timer1.Start();
                targetPath = sav.FileName;
                selectedFormat.ProgressStarted += NoIntroDB_Progress;
                selectedFormat.ProgressFinished += NoIntroDB_Progress;
                selectedFormat.Progress += NoIntroDB_Progress;
                selectedFormat.ShowExportOptions();
                // Start the thread
                mainThread = new Thread(new ThreadStart(PROGRESS));
                mainThread.CurrentUICulture = ls.CultureInfo;
                mainThread.Start();
            }
        }
        private void NoIntroDB_Progress(object sender, ProgressArgs e)
        {
            status = e.Status;
            progress = e.Completed;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            label_progress.Text = status;
            progressBar1.Value = progress;
        }
        private void Form_ExportToDatabase_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!finished)
            {
                if (mainThread != null)
                {
                    if (mainThread.IsAlive)
                    {
                        ManagedMessageBoxResult result =
                            ManagedMessageBox.ShowQuestionMessage(ls["Message_AreYouSureYouWantRoStopCurrentProgress"],
                            ls["MessageCaption_ExportToDatabaseFile"]);
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

        private void comboBox_format_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_format.SelectedIndex >= 0)
                selectedFormat = (DatabaseFile)comboBox_format.SelectedItem;
        }
    }
}
