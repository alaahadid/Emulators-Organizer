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
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;
using MMB;
namespace EmulatorsOrganizer.GUI
{
    public partial class Form_AddEmulator : Form
    {
        public Form_AddEmulator(bool browseForNewEmulator, string recommendedConsoleID)
        {
            InitializeComponent();
            if (browseForNewEmulator)
                button1_Click(this, null);// browse
            // Refresh consoles ...
            foreach (Core.Console console in profileManager.Profile.Consoles)
            {
                checkedListBox1.Items.Add(console);
                if (recommendedConsoleID == console.ID)
                {
                    checkedListBox1.SetItemChecked(checkedListBox1.Items.Count - 1, true);
                }
                else if (profileManager.Profile.RecentSelectedType == SelectionType.Console)
                {
                    if (console.ID == profileManager.Profile.SelectedConsoleID)
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.Count - 1, true);
                }
                else if (profileManager.Profile.RecentSelectedType == SelectionType.ConsolesGroup)
                {
                    if (console.ParentGroupID == profileManager.Profile.SelectedConsolesGroupID)
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.Count - 1, true);
                }
            }
        }
        public Form_AddEmulator(bool browseForNewEmulator)
        {
            InitializeComponent();
            if (browseForNewEmulator)
                button1_Click(this, null);// browse
            // Refresh consoles ...
            foreach (Core.Console console in profileManager.Profile.Consoles)
            {
                checkedListBox1.Items.Add(console);
                if (profileManager.Profile.RecentSelectedType == SelectionType.Console)
                {
                    if (console.ID == profileManager.Profile.SelectedConsoleID)
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.Count - 1, true);
                }
                else if (profileManager.Profile.RecentSelectedType == SelectionType.ConsolesGroup)
                {
                    if (console.ParentGroupID == profileManager.Profile.SelectedConsolesGroupID)
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.Count - 1, true);
                }
            }
        }
        public Form_AddEmulator(string fileName)
        {
            InitializeComponent();
            textBox1.Text = fileName;
            if (textBox_emuName.Text.Length == 0)
                textBox_emuName.Text = Path.GetFileNameWithoutExtension(fileName);
            // Refresh consoles ...
            foreach (Core.Console console in profileManager.Profile.Consoles)
            {
                checkedListBox1.Items.Add(console);
                if (profileManager.Profile.RecentSelectedType == SelectionType.Console)
                {
                    if (console.ID == profileManager.Profile.SelectedConsoleID)
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.Count - 1, true);
                }
                else if (profileManager.Profile.RecentSelectedType == SelectionType.ConsolesGroup)
                {
                    if (console.ParentGroupID == profileManager.Profile.SelectedConsolesGroupID)
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.Count - 1, true);
                }
            }
        }

        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");

        // Cancel
        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
        // Ok
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox_emuName.Text.Length == 0)
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseEnterEmulatorNameFirst"], ls["MessageCaption_AddEmulator"]);
                return;
            }
            if (profileManager.Profile.Emulators.Contains(textBox_emuName.Text, ""))
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_ThisNameAlreadyTakenByAnotherEmulator"], ls["MessageCaption_AddEmulator"]);
                return;
            }
            if (radioButton_useFile.Checked)
            {
                if (!File.Exists(HelperTools.GetFullPath(textBox1.Text)))
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_ThisEmulatorExcutableFileNotExists"], ls["MessageCaption_AddEmulator"]);
                    return;
                }
            }
            else
            {
                if (richTextBox1.Text.Length == 0)
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_YouMustEnterScript"], ls["MessageCaption_EmulatorProperties"]);
                    return;
                }
            }
            if (checkedListBox1.CheckedItems.Count == 0)
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseCheckOneConsole"], ls["MessageCaption_AddEmulator"]);
                return;
            }
            Emulator emu = new Emulator(textBox_emuName.Text, profileManager.Profile.GenerateID());
            foreach (Core.Console console in checkedListBox1.CheckedItems)
            {
                EmulatorParentConsole p = new EmulatorParentConsole(console.ID);
                emu.ParentConsoles.Add(p);
            }
            emu.ExcutablePath = textBox1.Text;
            emu.BatMode = radioButton_useBat.Checked;
            emu.BatScript = richTextBox1.Text;
            try
            {
                emu.Icon = Icon.ExtractAssociatedIcon(textBox1.Text).ToBitmap();
            }
            catch (Exception ex)
            {
                emu.Icon = null;
                // ManagedMessageBox.ShowErrorMessage(ex.Message);
                System.Diagnostics.Trace.TraceError("Unable to get emulator icon: " + ex.Message);
            }
            profileManager.Profile.Emulators.Add(emu);// this will raise the event!
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
        // Browse
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = ls["Title_OpenEmulatorExcutableFile"];
            op.Filter = ls["Filter_AllFiles"];
            op.FileName = HelperTools.GetFullPath(textBox1.Text);
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = HelperTools.GetDotPath(op.FileName);
                if (textBox_emuName.Text.Length == 0)
                    textBox_emuName.Text = Path.GetFileNameWithoutExtension(op.FileName);
            }
        }
    }
}
