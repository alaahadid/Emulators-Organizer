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
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using EmulatorsOrganizer.Core;
using MMB;
namespace EmulatorsOrganizer.GUI
{
    public partial class RomPropertiesGeneral : IRomPropertiesControl
    {
        public RomPropertiesGeneral()
        {
            InitializeComponent();
        }
        private string oldName;
        private string oldPath;
        private bool oldIgnore;
        public override string ToString()
        {
            return ls["Title_General"];
        }
        public override string Description
        {
            get
            {
                return ls["RomPropertiesDescription_General"];
            }
        }

        public override bool CanSaveSettings
        {
            get
            {
                // NAME
                if (textBox_romName.Text.Length == 0)
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseEnterRomNameFirst"],
                        ls["MessageCaption_RomProperties"]);
                    return false;
                }
                string parentID = profileManager.Profile.Roms[romID].ParentConsoleID;
                string id = romID;
                if (profileManager.Profile.Roms.Contains(textBox_romName.Text, out id, romID, parentID))
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_ThisNameAlreadyTakenByAnotherRom"],
                        ls["MessageCaption_RomProperties"]);
                    return false;
                }
                // FILE PATH
                if (!checkBox_ignorePathNotExist.Checked)
                {
                    if (textBox_filepath.Text.Length == 0)
                    {
                        ManagedMessageBox.ShowErrorMessage(ls["Message_RomFileNotExist"],
                        ls["MessageCaption_RomProperties"]);
                        return false;
                    }
                    if (!File.Exists(HelperTools.GetFullPath(textBox_filepath.Text)))
                    {
                        ManagedMessageBox.ShowErrorMessage(ls["Message_RomFileNotExist"],
                         ls["MessageCaption_RomProperties"]);
                        return false;
                    }
                }
                if (profileManager.Profile.Roms.ContainsByPath(textBox_filepath.Text, out id, romID, consoleID))
                {
                    ManagedMessageBoxResult result = ManagedMessageBox.ShowQuestionMessage(ls["Message_RomFileAlreadyExistUnderAnotherName"],
                     ls["MessageCaption_RomProperties"]);
                    if (result.ClickedButtonIndex == 1)// no
                        return false;
                }
                return true;
            }
        }
        public override void LoadSettings()
        {
            textBox_romName.Text = oldName = profileManager.Profile.Roms[romID].Name;
            textBox_filepath.Text = oldPath = profileManager.Profile.Roms[romID].Path;
            checkBox_ignorePathNotExist.Checked = oldIgnore = profileManager.Profile.Roms[romID].IgnorePathNotExist;
        }
        public override void SaveSettings()
        {
            profileManager.Profile.Roms[romID].Name = textBox_romName.Text;
            profileManager.Profile.Roms[romID].Path = textBox_filepath.Text;
            profileManager.Profile.Roms[romID].IgnorePathNotExist = checkBox_ignorePathNotExist.Checked;
        }
        public override void DefaultSettings()
        {
            textBox_romName.Text = oldName;
            textBox_filepath.Text = oldPath;
            checkBox_ignorePathNotExist.Checked = oldIgnore;
        }
        // Change location
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fil = new OpenFileDialog();
            fil.Title = ls["Title_BrowseForRomFile"];
            Core.Console console = profileManager.Profile.Consoles[consoleID];
            string filter = console.Name + " roms (";
            if (console.Extensions.Count == 1)
            { filter += "*" + console.Extensions[0]; }
            else
            {
                foreach (string ext in console.Extensions)
                {
                    filter += "*" + ext;
                    if (console.Extensions[console.Extensions.Count - 1] != ext)
                    { filter += ","; }
                }
            }
            filter += ")|";
            foreach (string ext in console.Extensions)
            {
                filter += "*" + ext + ";";
            }
            fil.Filter = filter;
            fil.FileName = HelperTools.GetFullPath(textBox_filepath.Text);
            if (fil.ShowDialog(this) == DialogResult.OK)
            {
                textBox_filepath.Text = HelperTools.GetDotPath(fil.FileName);
                if (textBox_romName.Text.Length == 0)
                    textBox_romName.Text = Path.GetFileNameWithoutExtension(fil.FileName);
            }
        }
    }
}
