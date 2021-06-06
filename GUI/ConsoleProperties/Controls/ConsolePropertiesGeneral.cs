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
    public partial class ConsolePropertiesGeneral : IConsolePropertiesControl
    {
        public ConsolePropertiesGeneral()
        {
            InitializeComponent();

        }
        public override string ToString()
        {
            return ls["Title_General"];
        }
        public override string Description
        {
            get
            {
                return ls["ConsolePropertiesDescription_General"];
            }
        }
        private string ListToText(List<string> list)
        {
            string val = "";
            foreach (string ext in list)
            {
                val += ext + ",";
            }
            if (val.Length > 0)
                val = val.Substring(0, val.Length - 1);
            return val;
        }
        private List<string> StringToList(string extensions)
        {
            List<string> val = new List<string>();
            string[] values = extensions.Split(new char[] { ',' });
            for (int i = 0; i < values.Length; i++)
            {
                if (!values[i].Contains("."))
                    values[i] = "." + values[i];

                val.Add(values[i].ToLower());
            }
            return val;
        }

        public override bool CanSaveSettings
        {
            get
            {
                if (textBox_consoleName.Text.Length == 0)
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseEnterConsoleNameFirst"],
                        ls["MessageCaption_ConsoleProperties"]);
                    return false;
                }
                if (profileManager.Profile.Consoles.Contains(textBox_consoleName.Text, profileManager.Profile.Consoles[consoleID].ParentGroupID, profileManager.Profile.Consoles[consoleID].ID))
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_ThisConsoleNameAlreadyTaken"],
                        ls["MessageCaption_ConsoleProperties"]);
                    return false;
                }
                if (textBox_extensions.Text.Length == 0)
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseEnterExtensionsFirst"],
                        ls["MessageCaption_ConsoleProperties"]);
                    return false;
                }
                return true;
            }
        }
        public override void LoadSettings()
        {
            textBox_consoleName.Text = profileManager.Profile.Consoles[consoleID].Name;
            textBox_extensions.Text = ListToText(profileManager.Profile.Consoles[consoleID].Extensions);
            richTextBox1.Text = profileManager.Profile.Consoles[consoleID].ShortDescription;
        }
        public override void SaveSettings()
        {
            profileManager.Profile.Consoles[consoleID].Name = textBox_consoleName.Text;
            profileManager.Profile.Consoles[consoleID].Extensions = StringToList(textBox_extensions.Text);
            profileManager.Profile.Consoles[consoleID].ShortDescription = richTextBox1.Text;
        }
        public override void DefaultSettings()
        {
            textBox_extensions.Text = "7z,rar,zip";
        }
    }
}
