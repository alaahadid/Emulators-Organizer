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
using System.Windows.Forms;
using EmulatorsOrganizer.Core;
using MMB;
namespace EmulatorsOrganizer.GUI
{
    public partial class EmulatorPropertiesParentConsoles : IEmulatorPropertiesControl
    {
        public EmulatorPropertiesParentConsoles()
        {
            InitializeComponent();
        }
        private List<string> temp = new List<string>();
        public override string ToString()
        {
            return ls["Title_ParentConsoles"];
        }
        public override string Description
        {
            get
            {
                return ls["EmulatorPropertiesDescription_ParentConsoles"];
            }
        }
        public override void LoadSettings()
        {
            // Save temp
            temp = new List<string>();
            foreach (EmulatorParentConsole p in profileManager.Profile.Emulators[emulatorID].ParentConsoles)
                temp.Add(p.ConsoleID);
            // Load consoles
            checkedListBox1.Items.Clear();
            foreach (Core.Console con in profileManager.Profile.Consoles)
            {
                if (profileManager.Profile.Emulators[emulatorID].IsConsoleSupported(con.ID))
                    checkedListBox1.Items.Add(con, true);
                else
                    checkedListBox1.Items.Add(con, false);
            }
        }
        public override void DefaultSettings()
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                Core.Console con = (Core.Console)checkedListBox1.Items[i];
                checkedListBox1.SetItemChecked(i, temp.Contains(con.ID));
            }
        }
        public override bool CanSaveSettings
        {
            get
            {
                if (checkedListBox1.CheckedItems.Count == 0)
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_YouMustEnableAtLeastOneParentConsole"],
                        ls["MessageCaption_EmulatorProperties"]);
                    return false;
                }
                return true;
            }
        }
        public override void SaveSettings()
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                Core.Console con = (Core.Console)checkedListBox1.Items[i];
                if (checkedListBox1.GetItemChecked(i))
                {
                    // The console is enabled, see if should add it
                    if (!profileManager.Profile.Emulators[emulatorID].IsConsoleSupported(con.ID))
                    {
                        // Add it !
                        profileManager.Profile.Emulators[emulatorID].ParentConsoles.Add(new EmulatorParentConsole(con.ID));
                    }
                    else
                    {
                        // The console is already enabled ...
                    }
                }
                else
                {
                    // The console is disabled, see if we need to delete it !
                    if (profileManager.Profile.Emulators[emulatorID].IsConsoleSupported(con.ID))
                    {
                        // Delete it !
                        profileManager.Profile.Emulators[emulatorID].RemoveParent(con.ID);
                    }
                    else
                    {
                        // The console is already disabled ...
                    }
                }
            }
        }
    }
}
