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
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;
using MMB;

namespace EmulatorsOrganizer.GUI
{
    public partial class Form_ProfileSettings : Form
    {
        public Form_ProfileSettings()
        {
            InitializeComponent();
            // Load settings
            textBox_name.Text = profileManager.Profile.Name;
            checkBox_rememberSelectedConsole.Checked = profileManager.Profile.RememberLatestSelectedConsoleOnProfileOpen;
            checkBox1.Checked = profileManager.Profile.RememberLatestSelectedRomOnProfileOpen;
        }
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
        // Save
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox_name.Text.Length == 0)
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseEnterTheProfileNameFirst"]);
                return;
            }
            profileManager.Profile.Name = textBox_name.Text;
            profileManager.Profile.RememberLatestSelectedConsoleOnProfileOpen = checkBox_rememberSelectedConsole.Checked;
            profileManager.Profile.RememberLatestSelectedRomOnProfileOpen = checkBox1.Checked;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
    }
}
