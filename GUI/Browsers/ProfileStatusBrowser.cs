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
using MTC;

namespace EmulatorsOrganizer.GUI
{
    public partial class ProfileStatusBrowser : IBrowserControl
    {
        public ProfileStatusBrowser(ManagedTabControl parentMTC)
        {
            InitializeComponent();
            base.InitializeEvents();
            this.parentMTC = parentMTC;
        }
        private ManagedTabControl parentMTC;

        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }
        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                base.BackgroundImage = value;
            }
        }

        public override void InitializeEvents()
        {
            profileManager.Profile.ConsolePropertiesChanged += Profile_ConsolePropertiesChanged;
        }
        public override void DisposeEvents()
        {
            profileManager.Profile.ConsolePropertiesChanged -= Profile_ConsolePropertiesChanged;

        }
        public override void ApplyStyle(EOStyle style)
        {
            base.ApplyStyle(style);
            this.BackColor = style.bkgColor_CategoriesBrowser;
            this.BackgroundImage = style.image_CategoriesBrowser;
            // switch (style.imageMode_CategoriesBrowser)
            // {
            //     case BackgroundImageMode.NormalStretchNoAspectRatio: this.optimizedTreeview1.BackgroundImageMode = ImageViewMode.Normal; break;
            //     case BackgroundImageMode.StretchIfLarger: this.optimizedTreeview1.BackgroundImageMode = ImageViewMode.StretchIfLarger; break;
            //     case BackgroundImageMode.StretchToFit: this.optimizedTreeview1.BackgroundImageMode = ImageViewMode.StretchToFit; break;
            //}
            //  optimizedTreeview1.ForeColor = style.txtColor_CategoriesBrowser;
            // Font
            //  try
            //  {
            //   FontConverter conv = new FontConverter();
            //      optimizedTreeview1.Font = (Font)conv.ConvertFromString(style.font_CategoriesBrowser);
            //  }
            //  catch { }
        }
        public void RefreshStatus()
        {
            panel1.Controls.Clear();
            // Add general info
            ProfileGeneralInfo inf = new ProfileGeneralInfo();
            panel1.Controls.Add(inf);
            inf.Dock = DockStyle.Top;
            // Add consoles ...
            foreach (EmulatorsOrganizer.Core.Console console in profileManager.Profile.Consoles)
            {
                ConsoleInfoControl conInf = new ConsoleInfoControl(console, parentMTC);
                panel1.Controls.Add(conInf);
                conInf.Dock = DockStyle.Top;
                conInf.BringToFront();
            }
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            RefreshStatus();
        }
        // Rename the profile
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Form_EnterName frm = new Form_EnterName(ls["Caption_EnterProfileName"], profileManager.Profile.Name, true, false);
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                profileManager.Profile.Name = frm.EnteredName;
                RefreshStatus();
            }
        }
        private void Profile_ConsolePropertiesChanged(object sender, EventArgs e)
        {
            switch (profileManager.Profile.RecentSelectedType)
            {
                case SelectionType.Console:
                    {
                        foreach (Control con in panel1.Controls)
                        {
                            if (con is ConsoleInfoControl)
                            {
                                if (((ConsoleInfoControl)con).CurrentConsoleID == profileManager.Profile.SelectedConsoleID)
                                    ((ConsoleInfoControl)con).RefreshInformation(profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID]);
                            }
                        }
                        break;
                    }
            }
        }
    }
}
