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
    public partial class Frm_CsvExportOptions : Form
    {
        public Frm_CsvExportOptions(DatabaseFile_CSV csv)
        {
            InitializeComponent();

            checkBox_ignoreEmptyFields.Checked = csv._IgnoreEmptyFields;
            checkBox_addAllDataItems.Checked = csv._AddAllDataItems;
        }
        public bool _IgnoreEmptyFields { get { return checkBox_ignoreEmptyFields.Checked; } }
        public bool _AddAllDataItems { get { return checkBox_addAllDataItems.Checked; } }
        public bool IncludeCategoriesOnExport { get { return checkBox2.Checked; } }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            checkBox_ignoreEmptyFields.Checked = checkBox_addAllDataItems.Checked = checkBox2.Checked = false;
        }
    }
}
