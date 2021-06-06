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
    public partial class Frm_CSVNewExport : Form
    {
        public Frm_CSVNewExport(DatabaseFile_CSV_NEW db)
        {
            db_file = db;
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }
        DatabaseFile_CSV_NEW db_file;
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            db_file.SPLITTER = comboBox1.SelectedItem.ToString();
            db_file.IncludeCategoriesOnExport = checkBox2.Checked;
            DialogResult = DialogResult.OK;
            Close();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            db_file.SPLITTER = comboBox1.SelectedItem.ToString();
        }
    }
}
