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
using System.Diagnostics;
using System.Windows.Forms;
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;
namespace EmulatorsOrganizer.GUI
{
    public partial class StartOptionsPanelBrowser : IBrowserControl
    {
        public StartOptionsPanelBrowser()
        {
            InitializeComponent();
            InitializeControls();
            base.InitializeEvents();
            // InitializeEvents();
        }
        private CommandlinesEditor commandlinesEditor1;
        private ToolStripButton toolStripButton_useRomWorking;
        private ToolStrip toolstrip2;
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = this.toolStrip1.BackColor = this.toolstrip2.BackColor = commandlinesEditor1.BackColor = value;
            }
        }
        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                commandlinesEditor1.BackgroundImage = base.BackgroundImage = value;
            }
        }
        public override void InitializeEvents()
        {
            profileManager.Profile.RomSelectionChanged += Profile_RomSelectionChanged;
            profileManager.Profile.EmulatorSelectionChanged += Profile_RomSelectionChanged;
            profileManager.Profile.BeforeRomLaunch += Profile_BeforeRomLaunch;
            profileManager.Profile.EmulatorPropertiesChanged += Profile_EmulatorPropertiesChanged;
            profileManager.Profile.RomPropertiesChanged += Profile_RomPropertiesChanged;
        }
        private void InitializeControls()
        {
            // Add tool strip for use rom directory
            toolstrip2 = new ToolStrip();
            toolstrip2.Dock = DockStyle.Top;
            toolstrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;

            toolStripButton_useRomWorking = new ToolStripButton();
            this.toolStripButton_useRomWorking.Checked = false;
            this.toolStripButton_useRomWorking.CheckState = System.Windows.Forms.CheckState.Unchecked;
            this.toolStripButton_useRomWorking.CheckOnClick = true;
            this.toolStripButton_useRomWorking.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_useRomWorking.Name = "toolStripButton_useRomWorkingDirectory";
            this.toolStripButton_useRomWorking.Size = new System.Drawing.Size(59, 22);
            this.toolStripButton_useRomWorking.Text = "Use rom working directory";
            this.toolStripButton_useRomWorking.ToolTipText = "Use the rom folder as working directory with the emulator\notherwise the emulator's folder will be used as working directory.\n\nEnable this only when you face problems with command-lines.\nAlso enabling this will make some old emulator save files in rom folder.";
            this.toolStripButton_useRomWorking.Click += toolStripButton_useRomWorking_Click;

            toolstrip2.Items.Add(toolStripButton_useRomWorking);

            this.Controls.Add(toolstrip2);

            // Command lines editor
            this.commandlinesEditor1 = new EmulatorsOrganizer.GUI.CommandlinesEditor();
            this.commandlinesEditor1.AllowEdit = false;
            this.commandlinesEditor1.CommandlineGroups = new List<CommandlinesGroup>();
            this.commandlinesEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commandlinesEditor1.Location = new System.Drawing.Point(0, 25);
            this.commandlinesEditor1.Name = "commandlinesEditor1";
            this.commandlinesEditor1.Size = new System.Drawing.Size(315, 303);
            this.commandlinesEditor1.TabIndex = 3;
            this.commandlinesEditor1.CommandlinesEnableChange += commandlinesEditor1_CommandlinesEnableChange;
            this.Controls.Add(this.commandlinesEditor1);
            this.toolstrip2.BringToFront();
            this.commandlinesEditor1.BringToFront();
        }
        public override void DisposeEvents()
        {
            profileManager.Profile.RomSelectionChanged -= Profile_RomSelectionChanged;
            profileManager.Profile.EmulatorSelectionChanged -= Profile_RomSelectionChanged;
            profileManager.Profile.BeforeRomLaunch -= Profile_BeforeRomLaunch;
            profileManager.Profile.EmulatorPropertiesChanged -= Profile_EmulatorPropertiesChanged;
            profileManager.Profile.RomPropertiesChanged -= Profile_RomPropertiesChanged;
        }
        private string currentConsoleID;
        private string currentRomID;
        private string currentEmuID;
        private bool COMMANDLINESENABLED;

        public override void ApplyStyle(EOStyle style)
        {
            base.ApplyStyle(style);
            this.BackColor = style.bkgColor_StartOptions;
            this.BackgroundImage = style.image_StartOptions;
            switch (style.imageMode_StartOptions)
            {
                case BackgroundImageMode.NormalStretchNoAspectRatio: this.commandlinesEditor1.BackgroundImageMode = ImageViewMode.Normal; break;
                case BackgroundImageMode.StretchIfLarger: this.commandlinesEditor1.BackgroundImageMode = ImageViewMode.StretchIfLarger; break;
                case BackgroundImageMode.StretchToFit: this.commandlinesEditor1.BackgroundImageMode = ImageViewMode.StretchToFit; break;
            }
            this.commandlinesEditor1.ForeColor = style.txtColor_StartOptions;
            // Font
            try
            {
                FontConverter conv = new FontConverter();
                commandlinesEditor1.Font = (Font)conv.ConvertFromString(style.font_StartOptions);
            }
            catch { }
        }
        public void RefreshSelection(bool Clear)
        {
            // Clear everything
            if (Clear)
            {
                this.Enabled = false;
                commandlinesEditor1.CommandlineGroups = new List<CommandlinesGroup>();

                toolStripButton_emulator.Checked = true;
                toolStripButton_rom.Checked = false;
                toolStripButton_useRomWorking.Checked = false;

                COMMANDLINESENABLED = false;
                currentConsoleID = "";
                currentEmuID = "";
                currentRomID = "";

                ServicesManager.OnDisableWindowListner();
                // Determine the ids
                switch (profileManager.Profile.RecentSelectedType)
                {
                    case SelectionType.Console:
                        {
                            COMMANDLINESENABLED = profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID].EnableCommandlines;
                            currentConsoleID = profileManager.Profile.SelectedConsoleID;
                            break;
                        }
                    case SelectionType.ConsolesGroup:
                        {
                            COMMANDLINESENABLED = profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID].EnableCommandlines;
                            currentConsoleID = "";
                            break;
                        }
                    case SelectionType.Playlist:
                        {
                            COMMANDLINESENABLED = profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID].EnableCommandlines;
                            currentConsoleID = "";
                            break;
                        }
                    case SelectionType.PlaylistsGroup:
                        {
                            COMMANDLINESENABLED = profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID].EnableCommandlines;
                            currentConsoleID = "";
                            break;
                        }
                    case SelectionType.None:
                        {
                            this.Enabled = false;
                            return;
                        }
                }
                // The emulator id
                currentEmuID = profileManager.Profile.SelectedEmulatorID;
                if (currentEmuID == "")
                {
                    Trace.WriteLine("No emulator selected to display command-lines.", "Start Options");
                    return;
                }
                // The rom id
                if (profileManager.Profile.SelectedRomIDS.Count == 1)
                {
                    currentRomID = profileManager.Profile.SelectedRomIDS[0];
                    Rom rom = profileManager.Profile.Roms[currentRomID];
                    if (rom != null)
                    {
                        switch (rom.CommandlinesUsageMode)
                        {
                            case CommandlinesUsageMode.Emulator:
                                {
                                    toolStripButton_emulator.Checked = true;
                                    toolStripButton_rom.Checked = false;
                                    break;
                                }
                            case CommandlinesUsageMode.Rom:
                                {
                                    toolStripButton_emulator.Checked = false;
                                    toolStripButton_rom.Checked = true;
                                    break;
                                }
                        }
                    }
                }
                if (COMMANDLINESENABLED)
                {
                    if (toolStripButton_emulator.Checked)
                    {
                        // Show emulator commandlines
                        Emulator emu = profileManager.Profile.Emulators[currentEmuID];
                        if (emu != null && currentConsoleID != "")
                        {
                            this.Enabled = true;
                            commandlinesEditor1.CommandlineGroups =
                                emu.GetCommandlinesGroupsForConsole(currentConsoleID);
                        }
                        else
                        {
                            Trace.WriteLine("Unable to show command-lines, the console id is invalid.", "Start Options");
                        }
                    }
                    else
                    {
                        // Show rom special command-lines
                        if (currentRomID != "")
                        {
                            this.Enabled = true;
                            commandlinesEditor1.CommandlineGroups =
                                profileManager.Profile.Roms[currentRomID].GetCommandlinesGroupsForEmulator(currentEmuID);
                        }
                    }
                }
                ServicesManager.OnEnableWindowListner();
            }
            else
            {
                // Just show what we have
                if (COMMANDLINESENABLED)
                {
                    if (toolStripButton_emulator.Checked)
                    {
                        // Show emulator commandlines
                        Emulator emu = profileManager.Profile.Emulators[currentEmuID];
                        if (emu != null && currentConsoleID != "")
                        {
                            this.Enabled = true;
                            commandlinesEditor1.CommandlineGroups =
                                emu.GetCommandlinesGroupsForConsole(currentConsoleID);
                        }
                        else
                        {
                            Trace.WriteLine("Unable to show command-lines, the console id is invalid.", "Start Options");
                        }
                    }
                    else
                    {
                        // Show rom special command-lines
                        if (currentRomID != "")
                        {
                            this.Enabled = true;
                            commandlinesEditor1.CommandlineGroups =
                                profileManager.Profile.Roms[currentRomID].GetCommandlinesGroupsForEmulator(currentEmuID);
                        }
                    }
                }
            }
            EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[currentConsoleID];
            if (console != null)
                toolStripButton_useRomWorking.Checked = console.UseRomWorkingDirectory;
            else
                toolStripButton_useRomWorking.Checked = false;
        }
        protected override void OnCreatingNewProfile()
        {
            base.OnCreatingNewProfile();
            this.Enabled = false;
            commandlinesEditor1.CommandlineGroups.Clear();
            toolStripButton_emulator.Checked = true;
            toolStripButton_rom.Checked = false;
            COMMANDLINESENABLED = false;
            currentConsoleID = "";
            currentEmuID = "";
            currentRomID = "";
        }
        protected override void OnOpeningProfile()
        {
            base.OnOpeningProfile();
            this.Enabled = false;
            commandlinesEditor1.CommandlineGroups.Clear();
            toolStripButton_emulator.Checked = true;
            toolStripButton_rom.Checked = false;
            COMMANDLINESENABLED = false;
            currentConsoleID = "";
            currentEmuID = "";
            currentRomID = "";
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (!toolStripButton_rom.Checked)
            {
                toolStripButton_emulator.Checked = false;
                toolStripButton_rom.Checked = true;
                RefreshSelection(false);
                if (profileManager.IsSaving)
                    return;
                Rom rom = profileManager.Profile.Roms[currentRomID];
                if (rom != null)
                {
                    rom.CommandlinesUsageMode = CommandlinesUsageMode.Rom;
                }
            }
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!toolStripButton_emulator.Checked)
            {
                toolStripButton_emulator.Checked = true;
                toolStripButton_rom.Checked = false;
                RefreshSelection(false);
                if (profileManager.IsSaving)
                    return;
                Rom rom = profileManager.Profile.Roms[currentRomID];
                if (rom != null)
                {
                    rom.CommandlinesUsageMode = CommandlinesUsageMode.Emulator;
                }
            }
        }
        private void Profile_RomSelectionChanged(object sender, EventArgs e)
        {
            RefreshSelection(true);
        }
        private void Profile_BeforeRomLaunch(object sender, EventArgs e)
        {
            // Save options
            profileManager.Profile.CommandlinesUsage = toolStripButton_rom.Checked ?
                CommandlinesUsageMode.Rom : CommandlinesUsageMode.Emulator;
            // Update commandlines
            if (profileManager.IsSaving)
                return;
            if (COMMANDLINESENABLED)
            {
                if (toolStripButton_emulator.Checked)
                {
                    // Show emulator commandlines
                    Emulator emu = profileManager.Profile.Emulators[currentEmuID];
                    if (emu != null && currentConsoleID != "")
                    {
                        this.Enabled = true;
                        if (profileManager.Profile.Emulators[currentEmuID].IsConsoleSupported(currentConsoleID)) // make sure
                        {
                            int index = profileManager.Profile.Emulators[currentEmuID].GetParentIndex(currentConsoleID);
                            profileManager.Profile.Emulators[currentEmuID].ParentConsoles[index].CommandlineGroups =
                                new List<Core.CommandlinesGroup>();

                            foreach (CommandlinesGroup gr in commandlinesEditor1.CommandlineGroups)
                                profileManager.Profile.Emulators[currentEmuID].ParentConsoles[index].CommandlineGroups.Add(gr.Clone());
                        }
                    }
                    else
                    {
                        Trace.WriteLine("Unable to save command-lines, the console id is invalid.", "Start Options");
                    }
                }
                else
                {
                    // Show rom special command-lines
                    if (currentRomID != "" && currentEmuID != "")
                    {
                        try
                        {
                            this.Enabled = true;
                            profileManager.Profile.Roms[currentRomID].UpdateEmulatorCommandlines(currentEmuID, commandlinesEditor1.CommandlineGroups);
                        }
                        catch { }
                    }
                }
            }
        }
        private void Profile_EmulatorPropertiesChanged(object sender, EventArgs e)
        {
            RefreshSelection(true);
        }
        private void Profile_RomPropertiesChanged(object sender, RomPropertiesChangedArgs e)
        {
            RefreshSelection(true);
        }
        private void commandlinesEditor1_CommandlinesEnableChange(object sender, System.EventArgs e)
        {
            if (profileManager.IsSaving)
                return;
            // Update commandlines
            if (COMMANDLINESENABLED)
            {
                if (toolStripButton_emulator.Checked)
                {
                    // Show emulator commandlines
                    Emulator emu = profileManager.Profile.Emulators[currentEmuID];
                    if (emu != null && currentConsoleID != "")
                    {
                        this.Enabled = true;
                        if (profileManager.Profile.Emulators[currentEmuID].IsConsoleSupported(currentConsoleID)) // make sure
                        {
                            int index = profileManager.Profile.Emulators[currentEmuID].GetParentIndex(currentConsoleID);
                            profileManager.Profile.Emulators[currentEmuID].ParentConsoles[index].CommandlineGroups =
                                new List<Core.CommandlinesGroup>();

                            foreach (CommandlinesGroup gr in commandlinesEditor1.CommandlineGroups)
                                profileManager.Profile.Emulators[currentEmuID].ParentConsoles[index].CommandlineGroups.Add(gr.Clone());
                        }
                    }
                    else
                    {
                        Trace.WriteLine("Unable to save command-lines, the console id is invalid.", "Start Options");
                    }
                }
                else
                {
                    // Show rom special command-lines
                    if (currentRomID != "" && currentEmuID != "")
                    {
                        try
                        {
                            this.Enabled = true;
                            profileManager.Profile.Roms[currentRomID].UpdateEmulatorCommandlines(currentEmuID, commandlinesEditor1.CommandlineGroups);
                        }
                        catch { }
                    }
                }
            }
        }
        private void toolStripButton_useRomWorking_Click(object sender, EventArgs e)
        {
            if (profileManager.IsSaving)
                return;
            EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[currentConsoleID];
            if (console != null)
                console.UseRomWorkingDirectory = toolStripButton_useRomWorking.Checked;
        }
    }
}
