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
using System.Windows.Forms;

namespace EmulatorsOrganizer.Core
{
    public partial class DatabaseFileControl_MameDat : UserControl
    {
        public DatabaseFileControl_MameDat(DatabaseFile_MameDat dbfile)
        {
            this.dbfile = dbfile; isXML = false;
            InitializeComponent();
            this.checkBox_rename_rom.Checked = dbfile._rename_using_description_instead_of_name;
            this.checkBox_ad_desc.Checked = dbfile._add_rom_data_items;
        }
        public DatabaseFileControl_MameDat(DatabaseFile_MameXML dbfile)
        {
            dbfile_xml = dbfile; isXML = true;
            InitializeComponent();
            checkBox_rename_rom.Checked = dbfile_xml._rename_using_description_instead_of_name;
            checkBox_ad_desc.Checked = dbfile_xml._add_rom_data_items;
        }
        private DatabaseFile_MameDat dbfile;
        private DatabaseFile_MameXML dbfile_xml;
        private bool isXML;

        private void checkBox_rename_rom_CheckedChanged(object sender, EventArgs e)
        {
            if (!isXML)
                dbfile._rename_using_description_instead_of_name = this.checkBox_rename_rom.Checked;
            else
                dbfile_xml._rename_using_description_instead_of_name = this.checkBox_rename_rom.Checked;
        }
        private void checkBox_ad_desc_CheckedChanged(object sender, EventArgs e)
        {
            if (!isXML)
                dbfile._add_rom_data_items = this.checkBox_ad_desc.Checked;
            else
                dbfile_xml._add_rom_data_items = this.checkBox_ad_desc.Checked;
        }
    }
}
