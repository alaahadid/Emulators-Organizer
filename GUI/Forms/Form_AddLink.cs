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
using EmulatorsOrganizer.Services;
using MMB;
namespace EmulatorsOrganizer.GUI
{
    public partial class Form_AddLink : Form
    {
        public Form_AddLink(Dictionary<string,string> links)
        {
            this.links = links;
            InitializeComponent();
        }
        private Dictionary<string, string> links;
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        public string LinkName { get { return textBox_linkName.Text; } }
        public string LinkURL { get { return textBox_url.Text; } }
        // OK
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox_linkName.Text.Length == 0)
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseEnterTheLinkNameFirst"], ls["MessageCaption_AddLink"]);
                return;
            }
            if (textBox_url.Text.Length == 0)
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseEnterTheLinkFirst"], ls["MessageCaption_AddLink"]);
                return;
            }
            if (links.ContainsKey(textBox_linkName.Text))
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_EnteredNameAlreadyTakenForrom"], ls["MessageCaption_AddLink"]);
                return;
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
        // Cancel
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
