/* This file is part of Emulators Organizer
   A program that can organize roms and emulators

   Copyright © Ali Hadid and Ala Hadid 2009 - 2013

   This program is free software: you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation, either version 3 of the License, or
   (at your option) any later version.

   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AHD.EO.Base
{
    public partial class Frm_CsvExportOptions : Form
    {
        public Frm_CsvExportOptions(DB_CSV csv)
        {
            this.csv = csv;
            InitializeComponent();

            checkBox_ignoreEmptyFields.Checked = csv._IgnoreEmptyFields;
            checkBox_addAllDataItems.Checked = csv._AddAllDataItems;
        }
        private DB_CSV csv;
        public bool _IgnoreEmptyFields { get { return checkBox_ignoreEmptyFields.Checked; } }
        public bool _AddAllDataItems { get { return checkBox_addAllDataItems.Checked; } }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            checkBox_ignoreEmptyFields.Checked = checkBox_addAllDataItems.Checked = false;
        }
    }
}
