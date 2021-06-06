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
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;
using MMB;
namespace EmulatorsOrganizer.GUI
{
    public partial class Form_RenameRoms : Form
    {
        public Form_RenameRoms(string romName, string parentConsoleID, bool allowApplyToFile)
        {
            InitializeComponent();
            textBox_currentName.Text = romName;
            textBox_name.Text = romName;
            this.parentConsoleID = parentConsoleID;
            comboBox1.SelectedIndex = 0;
            checkBox_applyNameOnFile.Enabled = allowApplyToFile;
        }
        private string parentConsoleID;
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");

        public string RomName
        { get { return textBox_name.Text; } }
        public bool ApplyNameOnFile
        { get { return checkBox_applyNameOnFile.Checked; } }
        public bool RenameRelatedFiles
        { get { return checkBox_renameRelatedFiles.Checked; } }
        public RenameingMethod RenameingMethodChosen
        {
            get { return (RenameingMethod)comboBox1.SelectedIndex; }
        }
        public enum RenameingMethod : int
        {
            NewRomName_InfoFileIndex = 0,
            NewRomName_InfoName_InfoFileIndex = 1,
            InfoName_NewRomName_InfoFileIndex = 2
        }
        //ok
        private void button1_Click(object sender, EventArgs e)
        {

            string id = "";
            if (textBox_name.Text.Length == 0)
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseEnterNameFirst"], ls["MessageCaption_RenameRom"]);
                return;
            }
            if (profileManager.Profile.Roms.Contains(textBox_name.Text, out id, parentConsoleID))
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_ThisRomNameIsAlreadyTaken"], ls["MessageCaption_RenameRom"]);
                return;
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkBox_renameRelatedFiles_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1.Enabled = checkBox_renameRelatedFiles.Checked;
        }
    }
}
