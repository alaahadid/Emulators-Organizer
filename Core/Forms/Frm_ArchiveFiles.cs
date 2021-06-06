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
using System.IO;

namespace EmulatorsOrganizer.Core
{
    public partial class Frm_ArchiveFiles : Form
    {
        public string SelectedRom
        { get { return listBox1.SelectedItem.ToString(); } }
        public int SelectedRomIndex
        { get { return listBox1.SelectedIndex; } }

        public Frm_ArchiveFiles(string[] Files,bool selectFirstFile)
        {
            InitializeComponent();
            for (int i = 0; i < Files.Length; i++)
            {
                listBox1.Items.Add(Files[i]);
            }
            listBox1.SelectedIndex = (listBox1.Items.Count > 0) ? 0 : -1;
            if (selectFirstFile)
            {
                if (listBox1.SelectedIndex < 0)
                    return;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = listBox1.SelectedIndex >= 0;
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
                return;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
    }
}