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
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;
using EmulatorsOrganizer.Services.DefaultServices.Settings;
using MMB;

namespace EmulatorsOrganizer.GUI
{
    public partial class FormFieldsEdit : Form
    {
        public FormFieldsEdit(string[] fields)
        {
            InitializeComponent();
            foreach (string f in fields)
                listBox1.Items.Add(f);
            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = 0;
        }
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private bool sortAtoZ;

        public string[] FieldsAfterEdit
        {
            get
            {
                List<string> files = new List<string>();
                foreach (string f in listBox1.Items)
                    files.Add(f);
                return files.ToArray();
            }
        }
        // Close
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
        // OK
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
        // Remove
        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                ManagedMessageBoxResult res = ManagedMessageBox.ShowMessage(
                    ls["Message_AreYouSureYouWantToDeleteThisFieldFormTheList"],
                    ls["MessageCaption_FieldsEdit"],
                    new string[] { ls["Button_Yes"], ls["Button_No"] }, 1, ManagedMessageBoxIcon.Question);
                if (res.ClickedButtonIndex == 0)
                {
                    listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                    if (listBox1.Items.Count > 0)
                        listBox1.SelectedIndex = 0;
                    button8.Image = null;
                }
            }
        }
        // Move up
        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0) { return; }

            object selected = listBox1.SelectedItem;
            int index = listBox1.SelectedIndex;

            listBox1.Items.RemoveAt(index);

            if (index > 0)
                index--;

            listBox1.Items.Insert(index, selected);

            listBox1.SelectedIndex = index;

            button8.Image = null;
        }
        // Move down
        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0) { return; }

            object selected = listBox1.SelectedItem;
            int index = listBox1.SelectedIndex;

            listBox1.Items.RemoveAt(index);

            if (index < listBox1.Items.Count - 1)
                index++;

            listBox1.Items.Insert(index, selected);

            listBox1.SelectedIndex = index;

            button8.Image = null;
        }
        // Clear
        private void button6_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            button8.Image = null;
        }
        // Sort by name
        private void button8_Click(object sender, EventArgs e)
        {
            // Get the list
            List<string> files = new List<string>();
            foreach (string f in listBox1.Items)
                files.Add(f);
            sortAtoZ = !sortAtoZ;
            files.Sort(new TextComparer(sortAtoZ));
            // Give it back to list !!
            listBox1.Items.Clear();
            foreach (string f in files)
                listBox1.Items.Add(f);
            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = 0;

            button8.Image = sortAtoZ ? Properties.Resources.arrow_up : Properties.Resources.arrow_down;
        }
        // Add
        private void button7_Click(object sender, EventArgs e)
        {
            Form_EnterName frm = new Form_EnterName(ls["Title_EnterFieldName"], "", true, false);
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string item in listBox1.Items)
                {
                    if (item.ToLower() == frm.EnteredName.ToLower())
                    {
                        ManagedMessageBox.ShowErrorMessage(ls["Message_ThisNameAlreadyTaken"],
                         ls["MessageCaption_Rename"]);
                        return;
                    }
                }
                listBox1.Items.Add(frm.EnteredName);
            }
        }
        // Edit
        private void button9_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0) return;
            string oldName = listBox1.SelectedItem.ToString();
            int index = listBox1.SelectedIndex;
            Form_EnterName frm = new Form_EnterName(ls["Title_EnterFieldName"], listBox1.SelectedItem.ToString(), true, false);
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                if (frm.EnteredName.ToLower() == oldName.ToLower()) return;
                foreach (string item in listBox1.Items)
                {
                    if (item.ToLower() == frm.EnteredName.ToLower() && item.ToLower() != oldName.ToLower())
                    {
                        ManagedMessageBox.ShowErrorMessage(ls["Message_ThisNameAlreadyTaken"],
                         ls["MessageCaption_Rename"]);
                        return;
                    }
                }
                listBox1.Items.RemoveAt(index);
                listBox1.Items.Insert(index, frm.EnteredName);
            }
        }
    }
}
