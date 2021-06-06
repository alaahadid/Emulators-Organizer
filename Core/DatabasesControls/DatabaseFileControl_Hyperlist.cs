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
    public partial class DatabaseFileControl_Hyperlist : UserControl
    {
        public DatabaseFileControl_Hyperlist(DatabaseFile_HyperListXML db)
        {
            db_file = db;
            InitializeComponent();
        }
        private DatabaseFile_HyperListXML db_file;

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            db_file.SkipBytesInImport = checkBox1.Checked;
            db_file.BytesToSkipInImport = (int)numericUpDown1.Value;
            db_file._rename_using_description_instead_of_name = checkBox_rename_using_desc.Checked;
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            db_file.SkipBytesInImport = checkBox1.Checked;
            db_file.BytesToSkipInImport = (int)numericUpDown1.Value;
            db_file._rename_using_description_instead_of_name = checkBox_rename_using_desc.Checked;
        }
        private void checkBox_rename_using_desc_CheckedChanged(object sender, EventArgs e)
        {
            db_file.SkipBytesInImport = checkBox1.Checked;
            db_file.BytesToSkipInImport = (int)numericUpDown1.Value;
            db_file._rename_using_description_instead_of_name = checkBox_rename_using_desc.Checked;
        }
    }
}
