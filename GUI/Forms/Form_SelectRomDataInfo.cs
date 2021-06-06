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
using MMB;

namespace EmulatorsOrganizer.GUI
{
    public partial class Form_SelectRomDataInfo : Form
    {
        public Form_SelectRomDataInfo(RomData[] elements)
        {
            InitializeComponent();
            foreach (RomData dat in elements)
            {
                checkedListBox1.Items.Add(dat);
                checkedListBox1.SetItemChecked(checkedListBox1.Items.Count - 1, true);
            }
        }
        public RomData[] SelectedElements
        {
            get
            {
                List<RomData> selected = new List<RomData>();
                for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
                {
                    selected.Add((RomData)checkedListBox1.CheckedItems[i]);
                }
                return selected.ToArray();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.CheckedItems.Count == 0)
            {
                ManagedMessageBox.ShowErrorMessage("Please select at least one element !!");
                return;
            }
            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}
