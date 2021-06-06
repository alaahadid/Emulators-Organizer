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

namespace EmulatorsOrganizer.Core
{
    public partial class FormChildPick : Form
    {
        public FormChildPick(Rom[] children, bool multiSelect)
        {
            InitializeComponent();

            foreach (Rom child in children)
            {
                ListViewItem item = new ListViewItem();
                item.Text = child.Name;
                item.SubItems.Add(child.Path);
                item.Tag = child.ID;

                listView1.Items.Add(item);
            }
            if (listView1.Items.Count > 0)
                listView1.Items[0].Selected = true;
            this.listView1.MultiSelect = multiSelect;
            this.button3.Visible  = !multiSelect;
        }

        public string SelectedRomID
        {
            get
            {
                return listView1.SelectedItems[0].Tag.ToString();
            }
        }
        public string[] SelectedRomIDS
        {
            get
            {
                List<string> ids = new List<string>();
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    ids.Add(item.Tag.ToString());
                }
                return ids.ToArray();
            }
        }
        public bool PlayParentInstead
        {
            get;
            private set;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = listView1.SelectedItems.Count > 0;
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            button1_Click(this, null);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.PlayParentInstead = true;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
