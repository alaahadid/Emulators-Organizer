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
using EmulatorsOrganizer.Core;
using MMB;
namespace EmulatorsOrganizer.GUI
{
    public partial class EmulatorPropertiesGeneral : IEmulatorPropertiesControl
    {
        public EmulatorPropertiesGeneral()
        {
            InitializeComponent();

        }
        private string oldName;
        private string oldPath;
        private string oldScript;
        private bool oldBatMode;
        public override string ToString()
        {
            return ls["Title_General"];
        }
        public override string Description
        {
            get
            {
                return ls["EmulatorPropertiesDescription_General"];
            }
        }

        public override bool CanSaveSettings
        {
            get
            {
                if (textBox_emulatorName.Text.Length == 0)
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseEnterEmulatorNameFirst"],
                        ls["MessageCaption_EmulatorProperties"]);
                    return false;
                }
                if (profileManager.Profile.Emulators.Contains(textBox_emulatorName.Text,
                    profileManager.Profile.Emulators[emulatorID].ID))
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_ThisNameAlreadyTakenByAnotherEmulator"],
                        ls["MessageCaption_EmulatorProperties"]);
                    return false;
                }
                if (!radioButton_useBat.Checked)
                {
                    if (!System.IO.File.Exists(HelperTools.GetFullPath(textBox_path.Text)))
                    {
                        ManagedMessageBox.ShowErrorMessage(ls["Message_ThisEmulatorExcutableFileNotExists"],
                            ls["MessageCaption_EmulatorProperties"]);
                        return false;
                    }
                }
                else
                {
                    if (richTextBox1.Text.Length == 0)
                    {
                        ManagedMessageBox.ShowErrorMessage(ls["Message_YouMustEnterScript"], ls["MessageCaption_EmulatorProperties"]);
                        return false;
                    }
                }
                return true;
            }
        }
        public override void LoadSettings()
        {
            oldName = textBox_emulatorName.Text = profileManager.Profile.Emulators[emulatorID].Name;
            oldPath = textBox_path.Text = profileManager.Profile.Emulators[emulatorID].ExcutablePath;
            oldScript = richTextBox1.Text = profileManager.Profile.Emulators[emulatorID].BatScript;
            oldBatMode = radioButton_useBat.Checked = profileManager.Profile.Emulators[emulatorID].BatMode;
        }
        public override void SaveSettings()
        {
            profileManager.Profile.Emulators[emulatorID].Name = textBox_emulatorName.Text;
            profileManager.Profile.Emulators[emulatorID].ExcutablePath = textBox_path.Text;
            profileManager.Profile.Emulators[emulatorID].BatScript = richTextBox1.Text;
            profileManager.Profile.Emulators[emulatorID].BatMode = radioButton_useBat.Checked;
        }
        public override void DefaultSettings()
        {
            textBox_emulatorName.Text = oldName;
            textBox_path.Text = oldPath;
            richTextBox1.Text = oldScript;
            radioButton_useBat.Checked = oldBatMode;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = ls["Title_OpenEmulatorExcutableFile"];
            op.Filter = ls["Filter_AllFiles"];
            op.FileName = HelperTools.GetFullPath(textBox_path.Text);
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_path.Text = HelperTools.GetDotPath(op.FileName);
                if (textBox_emulatorName.Text.Length == 0)
                    textBox_emulatorName.Text = System.IO.Path.GetFileNameWithoutExtension(op.FileName);
            }
        }
    }
}
