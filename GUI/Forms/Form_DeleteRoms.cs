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
using System.Threading.Tasks;
using System.Windows.Forms;
using EmulatorsOrganizer.Services;
using EmulatorsOrganizer.Core;
using MMB;
namespace EmulatorsOrganizer.GUI
{
    public partial class Form_DeleteRoms : Form
    {
        public Form_DeleteRoms()
        {
            InitializeComponent();
            UpdateInfo();
        }
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        public bool DeleteRomFiles
        { get { return checkBox_deleteRomFiles.Checked; } }
        public bool DeleteRomRelatedFiles
        { get { return checkBox_deleteRelatedFiles.Checked; } }

        public bool DeleteParentChildren
        {
            get { return checkBox_delete_children_when_deleting_parent.Checked; }
        }
        private void UpdateInfo()
        {
            richTextBox1.Text = "*" + ls["Message_SelectedRomFilesWillBeDeletedFromDatabase"] + "\n*" +
                ls["Message_DeletingRomWillDeleteAllRelatedFilesFromDatabase"] + "\n";
            if (checkBox_deleteRomFiles.Checked)
                richTextBox1.Text += "*" + ls["Message_RomFilesWillBePermanently"] + "\n";
            if (checkBox_deleteRelatedFiles.Checked)
                richTextBox1.Text += "*" + ls["Message_RomRelatedFilesWillBeDeletedPermanently"] + "\n";
            if (checkBox_delete_children_when_deleting_parent.Checked)
                richTextBox1.Text += "*" + ls["Message_IfRomIsParentRomAllChildrenWillBeRemoved"] + "\n";
        }
        // Delete
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkBox_deleteRomFiles_CheckedChanged(object sender, EventArgs e)
        {
            UpdateInfo();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            UpdateInfo();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox_delete_children_when_deleting_parent_CheckedChanged(object sender, EventArgs e)
        {
            UpdateInfo();
        }
    }
}
