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
using EmulatorsOrganizer.GUI;
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services.DefaultServices.Settings;
using MMB;
namespace EmulatorsOrganizer
{
    public partial class SettingsControlTempFolder : ISettingsControl
    {
        public SettingsControlTempFolder()
        {
            InitializeComponent();
        }
        public override string ToString()
        {
            return ls["Title_TempFolder"];
        }
        public override void LoadSettings()
        {
            base.LoadSettings();

            textBox1.Text = (string)settings.GetValue(DefaultProfileSettings.TempFolder_Key,
                true, DefaultProfileSettings.TempFolder);
            checkBox_clearTempFolder.Checked = (bool)settings.GetValue(DefaultProfileSettings.ClearTempFolder_Key,
                true, DefaultProfileSettings.ClearTempFolder);
            string[] exs = (string[])settings.GetValue(DefaultProfileSettings.TempFolderExclude_Key,
                true, DefaultProfileSettings.TempFolderExclude);

            foreach (string ex in exs)
                listBox1.Items.Add(ex);
        }
        public override void SaveSettings()
        {
            base.SaveSettings();
            settings.AddValue(new SettingsValue(DefaultProfileSettings.TempFolder_Key, textBox1.Text));
            settings.AddValue(new SettingsValue(DefaultProfileSettings.ClearTempFolder_Key, checkBox_clearTempFolder.Checked));
            List<string> exs = new List<string>();
            foreach (string ex in listBox1.Items)
                exs.Add(ex);
            settings.AddValue(new SettingsValue(DefaultProfileSettings.TempFolderExclude_Key, exs.ToArray()));
        }
        public override void DefaultSettings()
        {
            base.DefaultSettings();
            checkBox_clearTempFolder.Checked = true;
            textBox1.Text = HelperTools.GetFullPath(DefaultProfileSettings.TempFolder);
            listBox1.Items.Clear();
        }
        public override bool CanSaveSettings
        {
            get
            {
                if (!System.IO.Directory.Exists(textBox1.Text))
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_TheTempFolderIsNotExist"]);
                    return false;
                }
                return true;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.SelectedPath = HelperTools.GetFullPath(textBox1.Text);
            if (folder.ShowDialog(this) == DialogResult.OK)
                textBox1.Text = HelperTools.GetDotPath(folder.SelectedPath);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = 0;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Form_EnterName frm = new Form_EnterName(ls["Title_EnterNewExtensionWithDot"], "", true, false);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                string ex = frm.EnteredName;
                if (!ex.Contains("."))
                    ex = "." + ex;
                ex = ex.ToLower();
                if (!listBox1.Items.Contains(ex))
                    listBox1.Items.Add(ex);
            }
        }
    }
}
