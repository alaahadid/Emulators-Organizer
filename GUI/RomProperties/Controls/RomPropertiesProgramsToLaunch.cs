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
using MMB;
namespace EmulatorsOrganizer.GUI
{
    public partial class RomPropertiesProgramsToLaunch : IRomPropertiesControl
    {
        public RomPropertiesProgramsToLaunch()
        {
            InitializeComponent();
        }

        public override string ToString()
        {
            return ls["Title_ProgramsToLaunch"];
        }
        public override string Description
        {
            get
            {
                return ls["RomPropertiesDescription_ProgramsToLaunch"];
            }
        }
        public override void LoadSettings()
        {
            Rom rom = profileManager.Profile.Roms[romID];
            checkBox_forcePrograms.Checked = rom.ProgramsUsageMode == ProgramUsageMode.Rom;
            // programs before
            if (rom.ProgramsToLaucnhBefore != null)
            {
                foreach (ProgramProperties prog in rom.ProgramsToLaucnhBefore)
                {
                    listView_programsBefore.Items.Add(prog.ProgramName);
                    listView_programsBefore.Items[listView_programsBefore.Items.Count - 1].SubItems.Add(prog.ProgramPath);
                    listView_programsBefore.Items[listView_programsBefore.Items.Count - 1].SubItems.Add(prog.Arguments);
                    switch (prog.StartMode)
                    {
                        case ProgramStartMode.INSTANT:
                            {
                                listView_programsBefore.Items[listView_programsBefore.Items.Count - 1].
                                    SubItems.Add(ls["Word_Instant"]);
                                break;
                            }
                        case ProgramStartMode.WAIT_TO_FINISH:
                            {
                                listView_programsBefore.Items[listView_programsBefore.Items.Count - 1].
                                    SubItems.Add(ls["Word_WaitToFinish"]);
                                break;
                            }
                        case ProgramStartMode.WAIT_SECONDS:
                            {
                                listView_programsBefore.Items[listView_programsBefore.Items.Count - 1].
                                    SubItems.Add(ls["Word_Wait"] + " " + prog.WaitSeconds.ToString() + " " + ls["Word_Seconds"]);
                                break;
                            }
                    }
                    listView_programsBefore.Items[listView_programsBefore.Items.Count - 1].SubItems.Add(prog.BatMode ? "SCRIPT" : "EXE");
                    listView_programsBefore.Items[listView_programsBefore.Items.Count - 1].SubItems.Add(prog.BatScript);

                    listView_programsBefore.Items[listView_programsBefore.Items.Count - 1].Tag = prog;
                }
            }
            // programs after
            if (rom.ProgramsToLaucnhAfter != null)
            {
                foreach (ProgramProperties prog in rom.ProgramsToLaucnhAfter)
                {
                    listView_lauchAfter.Items.Add(prog.ProgramName);
                    listView_lauchAfter.Items[listView_lauchAfter.Items.Count - 1].SubItems.Add(prog.ProgramPath);
                    listView_lauchAfter.Items[listView_lauchAfter.Items.Count - 1].SubItems.Add(prog.Arguments);
                    switch (prog.StartMode)
                    {
                        case ProgramStartMode.INSTANT:
                            {
                                listView_lauchAfter.Items[listView_lauchAfter.Items.Count - 1].
                                    SubItems.Add(ls["Word_Instant"]);
                                break;
                            }
                        case ProgramStartMode.WAIT_TO_FINISH:
                            {
                                listView_lauchAfter.Items[listView_lauchAfter.Items.Count - 1].
                                    SubItems.Add(ls["Word_WaitToFinish"]);
                                break;
                            }
                        case ProgramStartMode.WAIT_SECONDS:
                            {
                                listView_lauchAfter.Items[listView_lauchAfter.Items.Count - 1].
                                    SubItems.Add(ls["Word_Wait"] + " " + prog.WaitSeconds.ToString() + " " + ls["Word_Seconds"]);
                                break;
                            }
                    }
                    listView_lauchAfter.Items[listView_lauchAfter.Items.Count - 1].SubItems.Add(prog.BatMode ? "SCRIPT" : "EXE");
                    listView_lauchAfter.Items[listView_lauchAfter.Items.Count - 1].SubItems.Add(prog.BatScript);
                    listView_lauchAfter.Items[listView_lauchAfter.Items.Count - 1].Tag = prog;
                }
            }
        }
        public override void DefaultSettings()
        {

        }
        public override bool CanSaveSettings
        {
            get
            {
                return true;
            }
        }
        public override void SaveSettings()
        {
            profileManager.Profile.Roms[romID].ProgramsUsageMode = checkBox_forcePrograms.Checked ?
                ProgramUsageMode.Rom : ProgramUsageMode.Emulator;
            //programs before
            profileManager.Profile.Roms[romID].ProgramsToLaucnhBefore = new List<ProgramProperties>();
            foreach (ListViewItem item in listView_programsBefore.Items)
            {
                ProgramProperties prog = new ProgramProperties();
                ProgramProperties tagged = (ProgramProperties)item.Tag;
                prog.ProgramName = tagged.ProgramName;
                prog.ProgramPath = tagged.ProgramPath;
                prog.Arguments = tagged.Arguments;
                prog.StartMode = tagged.StartMode;
                prog.WaitSeconds = tagged.WaitSeconds;
                prog.BatMode = tagged.BatMode;
                prog.BatScript = tagged.BatScript;
                profileManager.Profile.Roms[romID].ProgramsToLaucnhBefore.Add(prog);
            }
            //programs after
            profileManager.Profile.Roms[romID].ProgramsToLaucnhAfter = new List<ProgramProperties>();
            foreach (ListViewItem item in listView_lauchAfter.Items)
            {
                ProgramProperties prog = new ProgramProperties();
                ProgramProperties tagged = (ProgramProperties)item.Tag;
                prog.ProgramName = tagged.ProgramName;
                prog.ProgramPath = tagged.ProgramPath;
                prog.Arguments = tagged.Arguments;
                prog.StartMode = tagged.StartMode;
                prog.WaitSeconds = tagged.WaitSeconds;
                prog.BatMode = tagged.BatMode;
                prog.BatScript = tagged.BatScript;
                profileManager.Profile.Roms[romID].ProgramsToLaucnhAfter.Add(prog);
            }
            profileManager.Profile.Roms[romID].Modified = true;
        }
        // Add program before
        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form_ProgamAdd frm = new Form_ProgamAdd(new ProgramProperties());
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                listView_programsBefore.Items.Add(frm.Program.ProgramName);
                listView_programsBefore.Items[listView_programsBefore.Items.Count - 1].SubItems.Add(frm.Program.ProgramPath);
                listView_programsBefore.Items[listView_programsBefore.Items.Count - 1].SubItems.Add(frm.Program.Arguments);
                switch (frm.Program.StartMode)
                {
                    case ProgramStartMode.INSTANT:
                        {
                            listView_programsBefore.Items[listView_programsBefore.Items.Count - 1].
                                SubItems.Add(ls["Word_Instant"]);
                            break;
                        }
                    case ProgramStartMode.WAIT_TO_FINISH:
                        {
                            listView_programsBefore.Items[listView_programsBefore.Items.Count - 1].
                                SubItems.Add(ls["Word_WaitToFinish"]);
                            break;
                        }
                    case ProgramStartMode.WAIT_SECONDS:
                        {
                            listView_programsBefore.Items[listView_programsBefore.Items.Count - 1].
                                SubItems.Add(ls["Word_Wait"] + " " + frm.Program.WaitSeconds.ToString() + " " + ls["Word_Seconds"]);
                            break;
                        }
                }
                listView_programsBefore.Items[listView_programsBefore.Items.Count - 1].SubItems.Add(frm.Program.BatMode ? "SCRIPT" : "EXE");
                listView_programsBefore.Items[listView_programsBefore.Items.Count - 1].SubItems.Add(frm.Program.BatScript);
                listView_programsBefore.Items[listView_programsBefore.Items.Count - 1].Tag = frm.Program;
            }
        }
        // Remove program before
        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (listView_programsBefore.SelectedItems.Count == 1)
            {
                listView_programsBefore.Items.Remove(listView_programsBefore.SelectedItems[0]);
            }
        }
        // Edit program before
        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (listView_programsBefore.SelectedItems.Count == 1)
            {
                Form_ProgamAdd frm = new Form_ProgamAdd((ProgramProperties)listView_programsBefore.SelectedItems[0].Tag);
                if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    listView_programsBefore.SelectedItems[0].Text = frm.Program.ProgramName;
                    listView_programsBefore.SelectedItems[0].SubItems[1].Text = (frm.Program.ProgramPath);
                    listView_programsBefore.SelectedItems[0].SubItems[2].Text = (frm.Program.Arguments);
                    switch (frm.Program.StartMode)
                    {
                        case ProgramStartMode.INSTANT:
                            {
                                listView_programsBefore.SelectedItems[0].
                                    SubItems[3].Text = ls["Word_Instant"];
                                break;
                            }
                        case ProgramStartMode.WAIT_TO_FINISH:
                            {
                                listView_programsBefore.SelectedItems[0].
                        SubItems[3].Text = ls["Word_WaitToFinish"];
                                break;
                            }
                        case ProgramStartMode.WAIT_SECONDS:
                            {
                                listView_programsBefore.SelectedItems[0].
                                 SubItems[3].Text = ls["Word_Wait"] + " " + frm.Program.WaitSeconds.ToString() + " " + ls["Word_Seconds"];
                                break;
                            }
                    }
                    listView_programsBefore.Items[listView_programsBefore.Items.Count - 1].SubItems.Add(frm.Program.BatMode ? "SCRIPT" : "EXE");
                    listView_programsBefore.Items[listView_programsBefore.Items.Count - 1].SubItems.Add(frm.Program.BatScript);
                    listView_programsBefore.SelectedItems[0].Tag = frm.Program;
                }
            }
        }
        // Add program after
        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form_ProgamAdd frm = new Form_ProgamAdd(new ProgramProperties());
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                listView_lauchAfter.Items.Add(frm.Program.ProgramName);
                listView_lauchAfter.Items[listView_lauchAfter.Items.Count - 1].SubItems.Add(frm.Program.ProgramPath);
                listView_lauchAfter.Items[listView_lauchAfter.Items.Count - 1].SubItems.Add(frm.Program.Arguments);
                switch (frm.Program.StartMode)
                {
                    case ProgramStartMode.INSTANT:
                        {
                            listView_lauchAfter.Items[listView_lauchAfter.Items.Count - 1].
                                SubItems.Add(ls["Word_Instant"]);
                            break;
                        }
                    case ProgramStartMode.WAIT_TO_FINISH:
                        {
                            listView_lauchAfter.Items[listView_lauchAfter.Items.Count - 1].
                                SubItems.Add(ls["Word_WaitToFinish"]);
                            break;
                        }
                    case ProgramStartMode.WAIT_SECONDS:
                        {
                            listView_lauchAfter.Items[listView_lauchAfter.Items.Count - 1].
                                SubItems.Add(ls["Word_Wait"] + " " + frm.Program.WaitSeconds.ToString() + " " + ls["Word_Seconds"]);
                            break;
                        }
                }
                listView_lauchAfter.Items[listView_lauchAfter.Items.Count - 1].SubItems.Add(frm.Program.BatMode ? "SCRIPT" : "EXE");
                listView_lauchAfter.Items[listView_lauchAfter.Items.Count - 1].SubItems.Add(frm.Program.BatScript);
                listView_lauchAfter.Items[listView_lauchAfter.Items.Count - 1].Tag = frm.Program;
            }
        }
        // Remove program after
        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (listView_lauchAfter.SelectedItems.Count == 1)
            {
                listView_lauchAfter.Items.Remove(listView_lauchAfter.SelectedItems[0]);
            }
        }
        // Edit program after
        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (listView_lauchAfter.SelectedItems.Count == 1)
            {
                Form_ProgamAdd frm = new Form_ProgamAdd((ProgramProperties)listView_lauchAfter.SelectedItems[0].Tag);
                if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    listView_lauchAfter.SelectedItems[0].Text = frm.Program.ProgramName;
                    listView_lauchAfter.SelectedItems[0].SubItems[1].Text = (frm.Program.ProgramPath);
                    listView_lauchAfter.SelectedItems[0].SubItems[2].Text = (frm.Program.Arguments);
                    switch (frm.Program.StartMode)
                    {
                        case ProgramStartMode.INSTANT:
                            {
                                listView_lauchAfter.Items[listView_lauchAfter.Items.Count - 1].
                                    SubItems.Add(ls["Word_Instant"]);
                                break;
                            }
                        case ProgramStartMode.WAIT_TO_FINISH:
                            {
                                listView_lauchAfter.Items[listView_lauchAfter.Items.Count - 1].
                                    SubItems.Add(ls["Word_WaitToFinish"]);
                                break;
                            }
                        case ProgramStartMode.WAIT_SECONDS:
                            {
                                listView_lauchAfter.Items[listView_lauchAfter.Items.Count - 1].
                                    SubItems.Add(ls["Word_Wait"] + " " + frm.Program.WaitSeconds.ToString() + " " + ls["Word_Seconds"]);
                                break;
                            }
                    }
                    listView_lauchAfter.SelectedItems[0].SubItems[3].Text = frm.Program.BatMode ? "SCRIPT" : "EXE";
                    listView_lauchAfter.SelectedItems[0].SubItems[4].Text = frm.Program.BatScript;
                    listView_lauchAfter.SelectedItems[0].Tag = frm.Program;
                }
            }
        }
    }
}
