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
using EmulatorsOrganizer.Core;
namespace EmulatorsOrganizer.GUI
{
    public partial class ConsolePropertiesRomRenaming : IConsolePropertiesControl
    {
        public ConsolePropertiesRomRenaming()
        {
            InitializeComponent();

        }
        public override string ToString()
        {
            return ls["Title_RomRenaming"];
        }
        public override string Description
        {
            get
            {
                return ls["ConsolePropertiesDescription_RomRenaming"];
            }
        }

        public override bool CanSaveSettings
        {
            get
            {
                if (checkBox_copyRom.Checked)
                {
                    if (textBox_copy_folder.Text.Length == 0)
                    {
                        ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseEnterFolderName"],
                            ls["MessageCaption_ConsoleProperties"]);
                        return false;
                    }
                    if (!System.IO.Directory.Exists(HelperTools.GetFullPath(textBox_copy_folder.Text)))
                    {
                        ManagedMessageBox.ShowErrorMessage(ls["Message_FoldersNotExist"],
                            ls["MessageCaption_ConsoleProperties"]);
                        return false;
                    }
                    if (checkBox_renameTheRom.Checked)
                    {
                        if (textBox_renaimgName.Text.Length == 0)
                        {
                            ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseEnterRenaming"],
                                ls["MessageCaption_ConsoleProperties"]);
                            return false;
                        }
                    }
                }
                return true;
            }
        }
        public override void LoadSettings()
        {
            checkBox_copyRom.Checked = profileManager.Profile.Consoles[consoleID].CopyRomBeforeLaunch;
            textBox_copy_folder.Text = profileManager.Profile.Consoles[consoleID].FolderWhereToCopyRomWhenLaunch;
            checkBox_renameTheRom.Checked = profileManager.Profile.Consoles[consoleID].RenameRomBeforeLaunch;
            textBox_renaimgName.Text = profileManager.Profile.Consoles[consoleID].RomNameBeforeLaunch;
            checkBox_include_extension.Checked = profileManager.Profile.Consoles[consoleID].IncludeExtensionWhenRenaming;
            checkBox_useEmuFolderIfAvailable.Checked = profileManager.Profile.Consoles[consoleID].UseSelectedEmulatorFolderWhenPossibleInsteadOfFolderWhenLaunch;
        }
        public override void SaveSettings()
        {
            profileManager.Profile.Consoles[consoleID].CopyRomBeforeLaunch = checkBox_copyRom.Checked;
            profileManager.Profile.Consoles[consoleID].FolderWhereToCopyRomWhenLaunch = textBox_copy_folder.Text;
            profileManager.Profile.Consoles[consoleID].RenameRomBeforeLaunch = checkBox_renameTheRom.Checked;
            profileManager.Profile.Consoles[consoleID].RomNameBeforeLaunch = textBox_renaimgName.Text;
            profileManager.Profile.Consoles[consoleID].IncludeExtensionWhenRenaming = checkBox_include_extension.Checked;
            profileManager.Profile.Consoles[consoleID].UseSelectedEmulatorFolderWhenPossibleInsteadOfFolderWhenLaunch =
                checkBox_useEmuFolderIfAvailable.Checked;

        }
        public override void DefaultSettings()
        {
            checkBox_copyRom.Checked = false;
            checkBox_renameTheRom.Checked = false;
            checkBox_include_extension.Checked = true;
            checkBox_useEmuFolderIfAvailable.Checked = false;
        }
        private void checkBox_copyRom_CheckedChanged(object sender, EventArgs e)
        {
            textBox_copy_folder.Enabled = button1.Enabled = checkBox_renameTheRom.Enabled = textBox_renaimgName.Enabled =
        checkBox_include_extension.Enabled = checkBox_useEmuFolderIfAvailable.Enabled = checkBox_copyRom.Checked;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.SelectedPath = textBox_copy_folder.Text;
            fol.ShowNewFolderButton = true;
            if (fol.ShowDialog(this) == DialogResult.OK)
                textBox_copy_folder.Text = fol.SelectedPath;
        }
    }
}
