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
using System.Windows.Forms;

namespace EmulatorsOrganizer.Core
{
    public partial class DatabaseFileControl_CSVNew : UserControl
    {
        public DatabaseFileControl_CSVNew(DatabaseFile_CSV_NEW db)
        {
            db_file = db;
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            db_file.SPLITTER = comboBox1.SelectedItem.ToString();
            checkBox_useAlternativeNameForCompare.Checked = db.UseAlternativeNameForComparing;
            checkBox_useAlternativeNameForRenaming.Checked = db.UseAlternativeNameForApplyingName;
        }
        DatabaseFile_CSV_NEW db_file;

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            db_file.SPLITTER = comboBox1.SelectedItem.ToString();
        }
        private void checkBox_useAlternativeName_CheckedChanged(object sender, EventArgs e)
        {
            db_file.UseAlternativeNameForComparing = checkBox_useAlternativeNameForCompare.Checked;
        }
        private void checkBox_useAlternativeNameForRenaming_CheckedChanged(object sender, EventArgs e)
        {
            db_file.UseAlternativeNameForApplyingName = checkBox_useAlternativeNameForRenaming.Checked;
        }
    }
}
