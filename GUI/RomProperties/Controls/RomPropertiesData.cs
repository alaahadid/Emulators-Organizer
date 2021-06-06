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

namespace EmulatorsOrganizer.GUI
{
    public partial class RomPropertiesData : IRomPropertiesControl
    {
        public RomPropertiesData()
        {
            InitializeComponent();
        }
        private object temp;
        public override string ToString()
        {
            return ls["Title_Data"];
        }
        public override string Description
        {
            get
            {
                return ls["RomPropertiesDescription_Data"];
            }
        }
        public override void LoadSettings()
        {
            dataGridView1.Rows.Clear();
            foreach (RomData d in profileManager.Profile.Consoles[profileManager.Profile.Roms[romID].ParentConsoleID].RomDataInfoElements)
            {
                dataGridView1.Rows.Add(d, profileManager.Profile.Roms[romID].GetDataItemValue(d.ID), d.Type.ToString());
            }
        }
        public override bool CanSaveSettings
        {
            get
            {
                return true;
            }
        }
        public override void DefaultSettings()
        {
            LoadSettings();// Reset to old values
        }
        public override void SaveSettings()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                string id = ((RomData)dataGridView1.Rows[i].Cells[0].Value).ID;
                object val = dataGridView1.Rows[i].Cells[1].Value;
                if (val == null)
                {
                    switch (((RomData)dataGridView1.Rows[i].Cells[0].Value).Type) 
                    {
                        case RomDataType.Text: val = ""; break;
                        case RomDataType.Number: val = 0; break;
                    }
                }
                profileManager.Profile.Roms[romID].UpdateDataInfoItemValue(id, val);
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Value changing
             if (e.ColumnIndex == 1)
            {
                RomData d = (RomData)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
                switch (d.Type)
                {
                    case RomDataType.Text: break;// it's ok
                    
                    case RomDataType.Number:// must be number
                        {
                            int val = 0;
                            if (!int.TryParse((string)dataGridView1.Rows[e.RowIndex].Cells[1].Value, out val))
                            {
                                // Reset
                                dataGridView1.Rows[e.RowIndex].Cells[1].Value = temp;
                            }
                            else
                            {
                                // Confirm
                                dataGridView1.Rows[e.RowIndex].Cells[1].Value = val;
                            }
                            break;
                        }
                }
            }
        }
        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                // Save object
                temp = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            }
        }
    }
}
