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
using System.Diagnostics;
using EmulatorsOrganizer.Services;
using EmulatorsOrganizer.Core;

namespace EmulatorsOrganizer.GUI
{
    public partial class Form_DetectICSWizard : Form
    {
        public Form_DetectICSWizard(string consoleID)
        {
            this.consoleID = consoleID;
            InitializeComponent();
            // Fill ics
            EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[consoleID];
            if (console == null) return;

            foreach (InformationContainer ic in console.InformationContainers)
            {
                if (ic.Dectable)
                {
                    TreeNode node = new TreeNode();
                    node.Text = ic.DisplayName;
                    node.Tag = ic.ID;
                    treeView1.Nodes.Add(node);
                }
            }
        }
        private string consoleID;
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");

        // Detect
        private void button1_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;

            // Get ic id
            string icid = treeView1.SelectedNode.Tag.ToString();

            // Detect !
            Form_Detect frm = new Form_Detect(consoleID, icid);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                treeView1.SelectedNode.ImageIndex = treeView1.SelectedNode.SelectedImageIndex = 1;
                treeView1.Invalidate();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
        // Restore
        private void button3_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;

            // Get ic id
            string icid = treeView1.SelectedNode.Tag.ToString();

            // Detect !
            Form_RestoreBackupFile frm = new Form_RestoreBackupFile(consoleID, icid);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                treeView1.SelectedNode.ImageIndex = treeView1.SelectedNode.SelectedImageIndex = 1;
                treeView1.Invalidate();
            }
        }
    }
}
