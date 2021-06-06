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
using EmulatorsOrganizer.Services;
using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace EmulatorsOrganizer.Core
{
    public partial class Frm_EmulationStationExportOptions : Form
    {
        public Frm_EmulationStationExportOptions(DatabaseFile_EmulationStationXML db)
        {
            db_file = db;
            InitializeComponent();
            // The name should be the console name
            textBox_system.Text = profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID].Name;
            textBox_software.Text = db_file._db_header_software;
            textBox_database.Text = db_file._db_header_database;
            textBox_web.Text = db_file._db_header_web;
            checkBox_use_year.Checked = db_file._db_export_use_year_as_release_date;
            checkBox2.Checked = db_file.IncludeCategoriesOnExport;
            checkBox_use_release_date_format.Checked = db_file._db_export_release_date_format;
        }
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private DatabaseFile_EmulationStationXML db_file;

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
        // OK
        private void button1_Click(object sender, EventArgs e)
        {
            db_file._db_header_system = textBox_system.Text;
            db_file._db_header_software = textBox_software.Text;
            db_file._db_header_database = textBox_database.Text;
            db_file._db_header_web = textBox_web.Text;
            db_file._db_export_release_date_format= checkBox_use_release_date_format.Checked ;
            db_file._db_export_use_year_as_release_date = checkBox_use_year.Checked;
            db_file.IncludeCategoriesOnExport = checkBox2.Checked;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
