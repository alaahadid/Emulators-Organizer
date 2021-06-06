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
    public partial class ConsolesGroupPropertiesGeneral : IConsolesGroupPropertiesControl
    {
        public ConsolesGroupPropertiesGeneral()
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
                return ls["ConsolesGroupPropertiesDescription_General"];
            }
        }

        public override bool CanSaveSettings
        {
            get
            {
                if (textBox_consoleName.Text.Length == 0)
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseEnterConsolesGroupNameFirst"],
                        ls["MessageCaption_ConsolesGroupProperties"]);
                    return false;
                }
                if (profileManager.Profile.ConsoleGroups.Contains(textBox_consoleName.Text, consolesGroupID))
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_ThisConsolesGroupNameAlreadyTaken"],
                        ls["MessageCaption_ConsolesGroupProperties"]);
                    return false;
                }
                return true;
            }
        }
        public override void LoadSettings()
        {
            textBox_consoleName.Text = profileManager.Profile.ConsoleGroups[consolesGroupID].Name;
        }
        public override void SaveSettings()
        {
            profileManager.Profile.ConsoleGroups[consolesGroupID].Name = textBox_consoleName.Text;
        }
        public override void DefaultSettings()
        {
        }
    }
}
