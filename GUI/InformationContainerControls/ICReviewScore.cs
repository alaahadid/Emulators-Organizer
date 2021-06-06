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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EmulatorsOrganizer.Core;

namespace EmulatorsOrganizer.GUI
{
    public partial class ICReviewScore : ICControl
    {
        public ICReviewScore(string icid, string parentConsoleID)
        {
            base.ICID = icid;
            base.parentConsoleID = parentConsoleID;
            base.canAcceptDraggedFiles = false;
            base.canSelectionReverse = false;
            console = profileManager.Profile.Consoles[base.parentConsoleID];
            InitializeComponent();
            RefreshFields();
            base.ApplyStyleOnRomSelection();


            // Load settings
            if (parentConsoleID != "" && parentConsoleID != null)
            {
                int index = profileManager.Profile.Consoles[parentConsoleID].GetInformaitonContainerIndex(ICID);
                if (index >= 0)
                    toolStrip1.Visible = ((InformationContainerReviewScore)profileManager.Profile.Consoles[parentConsoleID].
                        InformationContainers[index]).ShowToolstrip;
            }
        }
        private Core.Console console;
        private void RefreshFields()
        {
            scoreField1.Enabled = panel_fields.Enabled = false;
            panel_fields.Controls.Clear();
            if (console == null) return;
            InformationContainerReviewScore inf = (InformationContainerReviewScore)console.GetInformationContainer(ICID);
            if (inf == null) return;
            foreach (string field in inf.Fields)
            {
                ScoreField fi = new ScoreField();
                fi.IsTitleScore = false;
                fi.FieldName = field;
                fi.Score = 0;
                fi.Dock = DockStyle.Top;
                fi.ScoreChanged += fi_ScoreChanged;
                panel_fields.Controls.Add(fi);
                fi.BringToFront();
            }

        }
        protected override void OnProfileSavingStarted()
        {
            base.OnProfileSavingStarted();
            toolStripButton1.Enabled = false;
            editFieldsToolStripMenuItem.Enabled = false;
        }
        protected override void OnProfileSavingFinished()
        {
            base.OnProfileSavingFinished();
            toolStripButton1.Enabled = true;
            editFieldsToolStripMenuItem.Enabled = true;
        }

        protected override void RefreshFiles()
        {
            if (profileManager.Profile.SelectedRomIDS.Count == 1)
            {
                scoreField1.Enabled = panel_fields.Enabled = true;
                // Load fields
                Rom rom = profileManager.Profile.Roms[profileManager.Profile.SelectedRomIDS[0]];
                InformationContainerItem item = rom.GetInformationContainerItem(ICID);
                if (item != null && item is InformationContainerItemReviewScore)
                {
                    InformationContainerItemReviewScore itemIC = (InformationContainerItemReviewScore)item;
                    scoreField1.Score = itemIC.TotalScore;
                    foreach (ScoreField con in panel_fields.Controls)
                    {
                        if (itemIC.Scores.ContainsKey(con.FieldName))
                        {
                            con.Score = itemIC.Scores[con.FieldName];
                        }
                        else
                        {
                            con.Score = 0;// Clear it !
                        }
                    }
                }
                else
                {
                    scoreField1.Score = 0;
                    foreach (ScoreField con in panel_fields.Controls)
                    {
                        con.Score = 0;// Clear it !   
                    }
                }
            }
            else
            {
                scoreField1.Score = 0;
                foreach (ScoreField con in panel_fields.Controls)
                {
                    con.Score = 0;// Clear it !   
                }
                scoreField1.Enabled = panel_fields.Enabled = false;
            }
        }
        private void fi_ScoreChanged(object sender, EventArgs e)
        {
            if (profileManager.Profile.SelectedRomIDS.Count == 1)
            {
                // Load fields
                Rom rom = profileManager.Profile.Roms[profileManager.Profile.SelectedRomIDS[0]];
                InformationContainerItem item = rom.GetInformationContainerItem(ICID);
                if (item != null)
                {
                    InformationContainerItemReviewScore itemIC = (InformationContainerItemReviewScore)item;
                    if (itemIC.Scores.ContainsKey(((ScoreField)sender).FieldName))
                    {
                        itemIC.Scores[((ScoreField)sender).FieldName] = ((ScoreField)sender).Score;
                    }
                    else
                    {
                        // Add the field score 
                        itemIC.Scores.Add(((ScoreField)sender).FieldName, ((ScoreField)sender).Score);
                    }
                    scoreField1.Score = itemIC.TotalScore;
                }
                else
                {
                    // Add new item
                    InformationContainerItemReviewScore itemIC = new InformationContainerItemReviewScore(profileManager.Profile.GenerateID(), this.ICID);
                    if (itemIC.Scores.ContainsKey(((ScoreField)sender).FieldName))
                    {
                        itemIC.Scores[((ScoreField)sender).FieldName] = ((ScoreField)sender).Score;
                    }
                    else
                    {
                        // Add the field score 
                        itemIC.Scores.Add(((ScoreField)sender).FieldName, ((ScoreField)sender).Score);
                    }
                    rom.RomInfoItems.Add(itemIC);
                    rom.Modified = true;
                    scoreField1.Score = itemIC.TotalScore;
                }
                InformationContainerReviewScore inf = (InformationContainerReviewScore)console.GetInformationContainer(ICID);
                if (inf != null)
                    profileManager.Profile.OnInformationContainerItemsModified(inf.DisplayName);
            }
            else
            {
            }
        }
        // Edit fields
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (console == null) return;
            InformationContainerReviewScore inf = (InformationContainerReviewScore)console.GetInformationContainer(ICID);
            if (inf == null) return;
            FormFieldsEdit frm = new FormFieldsEdit(inf.Fields.ToArray());
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                inf.Fields = new List<string>(frm.FieldsAfterEdit);
                RefreshFields();
                RefreshFiles();
                profileManager.Profile.OnInformationContainerItemsModified(inf.DisplayName);
            }
        }
        private void scoreField1_ScoreChanged(object sender, EventArgs e)
        {
            if (profileManager.Profile.SelectedRomIDS.Count == 1)
            {
                // Load fields
                Rom rom = profileManager.Profile.Roms[profileManager.Profile.SelectedRomIDS[0]];
                InformationContainerItem item = rom.GetInformationContainerItem(ICID);
                InformationContainerReviewScore inf = (InformationContainerReviewScore)console.GetInformationContainer(ICID);
                if (item != null)
                {
                    InformationContainerItemReviewScore itemIC = (InformationContainerItemReviewScore)item;
                    foreach (string k in inf.Fields)
                    {
                        if (itemIC.Scores.ContainsKey(k))
                            itemIC.Scores[k] = scoreField1.Score;
                        else
                            itemIC.Scores.Add(k, scoreField1.Score);
                    }
                }
                else
                {
                    // Add new item
                    InformationContainerItemReviewScore itemIC = new InformationContainerItemReviewScore(profileManager.Profile.GenerateID(), this.ICID);
                    foreach (string k in inf.Fields)
                    {
                        if (itemIC.Scores.ContainsKey(k))
                            itemIC.Scores[k] = scoreField1.Score;
                        else
                            itemIC.Scores.Add(k, scoreField1.Score);
                    }
                    rom.RomInfoItems.Add(itemIC);
                    rom.Modified = true;
                    scoreField1.Score = itemIC.TotalScore;
                }

                profileManager.Profile.OnInformationContainerItemsModified(inf.DisplayName);
                RefreshFiles();
            }
            else
            {
            }
        }
        private void showToolstripToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip1.Visible = !toolStrip1.Visible;
            // Save settings
            if (parentConsoleID != "" && parentConsoleID != null)
            {
                int index = profileManager.Profile.Consoles[parentConsoleID].GetInformaitonContainerIndex(ICID);
                if (index >= 0)
                    ((InformationContainerReviewScore)profileManager.Profile.Consoles[parentConsoleID].
                        InformationContainers[index]).ShowToolstrip = toolStrip1.Visible;
            }
        }
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            showToolstripToolStripMenuItem.Checked = toolStrip1.Visible;
        }
        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (profileManager.Profile.SelectedRomIDS.Count == 1)
            {
                // Load fields
                Rom rom = profileManager.Profile.Roms[profileManager.Profile.SelectedRomIDS[0]];
                InformationContainerItem item = rom.GetInformationContainerItem(ICID);
                InformationContainerReviewScore inf = (InformationContainerReviewScore)console.GetInformationContainer(ICID);
                if (item != null)
                {
                    InformationContainerItemReviewScore itemIC = (InformationContainerItemReviewScore)item;
                    foreach (string k in inf.Fields)
                    {
                        if (itemIC.Scores.ContainsKey(k))
                            itemIC.Scores[k] = 0;
                        else
                            itemIC.Scores.Add(k, 0);
                    }
                }
                else
                {
                    // Add new item
                    InformationContainerItemReviewScore itemIC = new InformationContainerItemReviewScore(profileManager.Profile.GenerateID(), this.ICID);
                    foreach (string k in inf.Fields)
                    {
                        if (itemIC.Scores.ContainsKey(k))
                            itemIC.Scores[k] = 0;
                        else
                            itemIC.Scores.Add(k, 0);
                    }
                    rom.RomInfoItems.Add(itemIC);
                    rom.Modified = true;
                    scoreField1.Score = itemIC.TotalScore;
                }

                profileManager.Profile.OnInformationContainerItemsModified(inf.DisplayName);
                RefreshFiles();
            }
            else
            {
            }
        }

        private void likeItToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (profileManager.Profile.SelectedRomIDS.Count == 1)
            {
                // Load fields
                Rom rom = profileManager.Profile.Roms[profileManager.Profile.SelectedRomIDS[0]];
                InformationContainerItem item = rom.GetInformationContainerItem(ICID);
                InformationContainerReviewScore inf = (InformationContainerReviewScore)console.GetInformationContainer(ICID);
                if (item != null)
                {
                    InformationContainerItemReviewScore itemIC = (InformationContainerItemReviewScore)item;
                    foreach (string k in inf.Fields)
                    {
                        if (itemIC.Scores.ContainsKey(k))
                            itemIC.Scores[k] = 100;
                        else
                            itemIC.Scores.Add(k, 100);
                    }
                }
                else
                {
                    // Add new item
                    InformationContainerItemReviewScore itemIC = new InformationContainerItemReviewScore(profileManager.Profile.GenerateID(), this.ICID);
                    foreach (string k in inf.Fields)
                    {
                        if (itemIC.Scores.ContainsKey(k))
                            itemIC.Scores[k] = 100;
                        else
                            itemIC.Scores.Add(k, 100);
                    }
                    rom.RomInfoItems.Add(itemIC);
                    rom.Modified = true;
                    scoreField1.Score = itemIC.TotalScore;
                }

                profileManager.Profile.OnInformationContainerItemsModified(inf.DisplayName);
                RefreshFiles();
            }
            else
            {
            }
        }
    }
}
