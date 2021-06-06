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
    public partial class ConsolePropertiesDataRomInfo : IConsolePropertiesControl
    {
        public ConsolePropertiesDataRomInfo()
        {
            InitializeComponent();
        }

        private bool needToSave = false;
        private string tname = "";

        public override string ToString()
        {
            return ls["Title_RomDataInfo"];
        }
        public override string Description
        {
            get
            {
                return ls["ConsolePropertiesDescription_RomDataInfo"];
            }
        }
        public override void LoadSettings()
        {
            base.LoadSettings();
            dataGridView1.Rows.Clear();
            foreach (RomData data in profileManager.Profile.Consoles[consoleID].RomDataInfoElements)
            {
                dataGridView1.Rows.Add(data, data.Type.ToString());
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
            base.DefaultSettings();
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add(new RomData(profileManager.Profile.GenerateID(), ls["DefaultDataInfo_ReleaseDate"], RomDataType.Number), "Number");
            dataGridView1.Rows.Add(new RomData(profileManager.Profile.GenerateID(), ls["DefaultDataInfo_Publisher"], RomDataType.Text), "Text");
            dataGridView1.Rows.Add(new RomData(profileManager.Profile.GenerateID(), ls["DefaultDataInfo_DevelopedBy"], RomDataType.Text), "Text");
            dataGridView1.Rows.Add(new RomData(profileManager.Profile.GenerateID(), ls["DefaultDataInfo_Genre"], RomDataType.Text), "Text");
        }
        public override void SaveSettings()
        {
            base.SaveSettings();
            if (needToSave)
            {
                // Update rom data base
                List<RomData> dataList = new List<RomData>();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    RomData d = (RomData)dataGridView1.Rows[i].Cells[0].Value;
                    string t = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    switch (t)
                    {
                        case "Text": d.Type = RomDataType.Text; break;
                        case "Number": d.Type = RomDataType.Number; break;
                    }
                    dataList.Add(d);
                }
                profileManager.Profile.Consoles[consoleID].UpdateRomsWithNewDataInfoList(dataList);
                // Apply list to console
                profileManager.Profile.Consoles[consoleID].RomDataInfoElements.Clear();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    RomData d = (RomData)dataGridView1.Rows[i].Cells[0].Value;
                    string t = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    switch (t)
                    {
                        case "Text": d.Type = RomDataType.Text; break;
                        case "Number": d.Type = RomDataType.Number; break;
                    }
                    profileManager.Profile.Consoles[consoleID].RomDataInfoElements.Add(d);
                }
                profileManager.Profile.Consoles[consoleID].FixColumnsForRomDataInfo();
            }
        }
        // Add new
        private void button1_Click(object sender, EventArgs e)
        {
            Form_EnterName frm = new Form_EnterName(ls["Message_EnterDataInfoName"], "", true, false);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    RomData d = (RomData)dataGridView1.Rows[i].Cells[0].Value;
                    if (d.Name.ToLower() == frm.EnteredName.ToLower())
                        return;
                }
                dataGridView1.Rows.Add(new RomData(profileManager.Profile.GenerateID(), frm.EnteredName, RomDataType.Text), "Text");
                needToSave = true;
            }
        }
        // Remove
        private void button2_Click(object sender, EventArgs e)
        {
            // Confirm row selection
            if (dataGridView1.SelectedCells.Count == 1)
                dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Selected = true;

            if (dataGridView1.SelectedRows.Count == 1)
            {
                dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
                needToSave = true;
            }
        }
        // Edit
        private void button3_Click(object sender, EventArgs e)
        {    
            // Confirm row selection
            if (dataGridView1.SelectedCells.Count == 1)
                dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Selected = true;
            if (dataGridView1.SelectedRows.Count == 1)
            {
                RomData d = (RomData)dataGridView1.SelectedRows[0].Cells[0].Value;
                tname = d.Name;
                Form_EnterName frm = new Form_EnterName(ls["Message_EnterDataInfoName"], d.Name, true, false);
                frm.OkPressed += frm_OkPressed;
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    ((RomData)dataGridView1.SelectedRows[0].Cells[0].Value).Name = frm.EnteredName;
                }
            }
        }
        private void frm_OkPressed(object sender, EnterNameFormOkPressedArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                RomData d = (RomData)dataGridView1.Rows[i].Cells[0].Value;
                if (d.Name.ToLower() == e.NameEntered.ToLower() && d.Name != tname)
                {
                    e.Cancel = true;
                    ManagedMessageBox.ShowErrorMessage(ls["Message_ThisNameAlreadyTaken"], ls["MessageCaption_ConsoleProperties"]);
                    break;
                }
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            needToSave = true;
        }
    }
}
