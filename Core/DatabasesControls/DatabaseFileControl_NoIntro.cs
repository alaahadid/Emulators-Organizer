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

namespace EmulatorsOrganizer.Core
{
    public partial class DatabaseFileControl_NoIntro : UserControl
    {
        public DatabaseFileControl_NoIntro(DatabaseFile_NoIntro dbfile)
        {
            InitializeComponent();
            this.dbfile = dbfile;
            this.checkBox_rename_rom.Checked = dbfile.opRenameRomUsingName;
            this.checkBox_ad_desc.Checked = dbfile.opAddDescriptionAsDataItemToRom;
        }
        private DatabaseFile_NoIntro dbfile;

        private void checkBox_rename_rom_CheckedChanged(object sender, EventArgs e)
        {
            dbfile.opRenameRomUsingName = this.checkBox_rename_rom.Checked;
        }
        private void checkBox_ad_desc_CheckedChanged(object sender, EventArgs e)
        {
            dbfile.opAddDescriptionAsDataItemToRom = this.checkBox_ad_desc.Checked;
        }
    }
}
