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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MMB;

namespace EmulatorsOrganizer.GUI
{
    public partial class ConsolePropertiesArchive : IConsolePropertiesControl
    {
        public ConsolePropertiesArchive()
        {
            InitializeComponent();
        }
        public override string ToString()
        {
            return ls["Title_Archive"];
        }
        public override string Description
        {
            get
            {
                return ls["ConsolePropertiesDescription_Archive"];
            }
        }
        public override bool CanSaveSettings
        {
            get
            {
                if (listBox1.Items.Count == 0)
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_YouMustEnterArchiveExtension"],
                       ls["MessageCaption_ConsoleProperties"]);
                    return false;
                }
                return true;
            }
        }
        public override void LoadSettings()
        {
            base.LoadSettings();
            checkBox_extractRomIfArchive.Checked = profileManager.Profile.Consoles[consoleID].ExtractRomIfArchive;
            checkBox_extractTheFirstOne.Checked = profileManager.Profile.Consoles[consoleID].ExtractFirstFileIfArchiveIncludeMoreThanOne;
            radioButton_extractAllArchive.Checked = profileManager.Profile.Consoles[consoleID].ExtractAllFilesOfArchive;
            if (profileManager.Profile.Consoles[consoleID].ArchiveExtensions == null)
                profileManager.Profile.Consoles[consoleID].ArchiveExtensions = new List<string>(new string[] { ".7z", ".rar", ".zip", ".gzip", ".tar", ".bzip2", ".xz" });
            foreach (string ex in profileManager.Profile.Consoles[consoleID].ArchiveExtensions)
                listBox1.Items.Add(ex);
            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = 0;
            if (profileManager.Profile.Consoles[consoleID].ArchiveAllowedExtractionExtensions == null)
                profileManager.Profile.Consoles[consoleID].ArchiveAllowedExtractionExtensions = new List<string>();
            foreach (string ex in profileManager.Profile.Consoles[consoleID].ArchiveAllowedExtractionExtensions)
                listBox2.Items.Add(ex);
            if (listBox2.Items.Count > 0)
                listBox2.SelectedIndex = 0;
        }
        public override void SaveSettings()
        {
            base.SaveSettings();
            profileManager.Profile.Consoles[consoleID].ExtractRomIfArchive = checkBox_extractRomIfArchive.Checked;
            profileManager.Profile.Consoles[consoleID].ExtractFirstFileIfArchiveIncludeMoreThanOne = checkBox_extractTheFirstOne.Checked;
            profileManager.Profile.Consoles[consoleID].ExtractAllFilesOfArchive = radioButton_extractAllArchive.Checked;
            profileManager.Profile.Consoles[consoleID].ArchiveExtensions = new List<string>();
            foreach (string ex in listBox1.Items)
                profileManager.Profile.Consoles[consoleID].ArchiveExtensions.Add(ex.ToLower());

            profileManager.Profile.Consoles[consoleID].ArchiveAllowedExtractionExtensions = new List<string>();
            foreach (string ex in listBox2.Items)
                profileManager.Profile.Consoles[consoleID].ArchiveAllowedExtractionExtensions.Add(ex.ToLower());
        }
        public override void DefaultSettings()
        {
            base.DefaultSettings();
            checkBox_extractRomIfArchive.Checked = true;
            checkBox_extractTheFirstOne.Checked = false;
            radioButton_extractAllArchive.Checked = false;

            List<string> archiveExtensions = new List<string>(new string[] { ".7z", ".rar", ".zip", ".gzip", ".tar", ".bzip2", ".xz" });
            listBox1.Items.Clear();
            foreach (string ex in archiveExtensions)
                listBox1.Items.Add(ex);
            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = 0;

            listBox2.Items.Clear();
        }
        // Add
        private void button1_Click(object sender, EventArgs e)
        {
            Form_EnterName frm = new Form_EnterName(ls["Title_EnterTheNewExtension"], ".zip", true, false);
            if (frm.ShowDialog(this) == DialogResult.OK)
                if (!listBox1.Items.Contains(frm.EnteredName))
                {
                    string ex = frm.EnteredName.Replace(".", "");
                    ex = "." + ex;
                    listBox1.Items.Add(ex);
                }
        }
        // Remove
        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = 0;
        }
        // Reset
        private void button3_Click(object sender, EventArgs e)
        {
            List<string> archiveExtensions = new List<string>(new string[] { ".7z", ".rar", ".zip", ".gzip", ".tar", ".bzip2", ".xz" });
            listBox1.Items.Clear();
            foreach (string ex in archiveExtensions)
                listBox1.Items.Add(ex);
            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = 0;
        }
        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
        private void button6_Click(object sender, EventArgs e)
        {
            Form_EnterName frm = new Form_EnterName(ls["Title_EnterTheNewExtension"], ".zip", true, false);
            if (frm.ShowDialog(this) == DialogResult.OK)
                if (!listBox2.Items.Contains(frm.EnteredName))
                {
                    string ex = frm.EnteredName.Replace(".", "");
                    ex = "." + ex;
                    listBox2.Items.Add(ex);
                }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex >= 0)
                listBox2.Items.RemoveAt(listBox2.SelectedIndex);
            if (listBox2.Items.Count > 0)
                listBox2.SelectedIndex = 0;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
        }
    }
}
