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

namespace EmulatorsOrganizer
{
    public partial class SettingsControlVisual : ISettingsControl
    {
        public SettingsControlVisual()
        {
            InitializeComponent();
        }
        public override string ToString()
        {
            return ls["Title_VisualAndBehavior"];
        }
        public override void LoadSettings()
        {
            base.LoadSettings();
            checkBox_consolesBrowser_doubleClick.Checked = (bool)settings.GetValue("ConsolesBrowser:DoubleClick",
                true, true);
            checkBox_Emulators_StretchToFit.Checked = (bool)settings.GetValue("EmulatorsBrowser:StretchImagesToFitThumbnail",
                true, false);
            checkBox_roms_ClearFilesAfterDrag.Checked = (bool)settings.GetValue("RomsBrowser:ClearRemovedRomsAfterDragAndDrop",
                true, true);
            checkBox_showSubItmesToolTip.Checked = (bool)settings.GetValue("RomsBrowser:ShowSubItemToolTip", true, true);
            checkBox_thumbsAutoCycle.Checked = (bool)settings.GetValue("RomsBrowser:AutoCycleThumbs", true, true);
            int interval = (int)settings.GetValue("RomsBrowser:AutoCycleThumbsInterval", true, 2000);
            interval /= 1000;
            if (interval < 1)
                interval = 1;
            numericUpDown1.Value = interval;

            checkBox_playerStopOnTabChange.Checked = (bool)settings.GetValue("MediaTab:StopPlayerOnTabChange", true, true);
            checkBox_ShowListOnMouseClick.Checked = (bool)settings.GetValue("MediaTab:ShowListWhenClick", true, false);
            checkBox_showCloseButtonsOnTabs.Checked = (bool)settings.GetValue("Show close buttons on tabs", true, true);
            checkBox_closeButtonsAlwaysVisibleOnTabs.Checked = (bool)settings.GetValue("Close buttons always visible on tabs", true, false);
            checkBox_showIconsOnTabs.Checked = (bool)settings.GetValue("Show icons on tabs", true, true);
            checkBox_show_tooptip_in_thumb_mode.Checked = (bool)settings.GetValue("RomsBrowser:ShowTooltipThumbMode", true, true);
            checkBox_tootlip_thumbmode_show_all.Checked = (bool)settings.GetValue("RomsBrowser:ShowTooltipThumbModeAllInfo", true, false);
            checkBox_tooltip_thumb_show_rating.Checked = (bool)settings.GetValue("RomsBrowser:ShowTooltipThumbModeRating", true, false);
        }
        public override void SaveSettings()
        {
            settings.AddValue(new Services.DefaultServices.Settings.SettingsValue("ConsolesBrowser:DoubleClick",
                 checkBox_consolesBrowser_doubleClick.Checked));
            settings.AddValue(new Services.DefaultServices.Settings.SettingsValue("EmulatorsBrowser:StretchImagesToFitThumbnail",
                 checkBox_Emulators_StretchToFit.Checked));
            settings.AddValue(new Services.DefaultServices.Settings.SettingsValue("RomsBrowser:ClearRemovedRomsAfterDragAndDrop",
                 checkBox_roms_ClearFilesAfterDrag.Checked));
            settings.AddValue(new Services.DefaultServices.Settings.SettingsValue("RomsBrowser:ShowSubItemToolTip",
                 checkBox_showSubItmesToolTip.Checked));
            settings.AddValue("RomsBrowser:AutoCycleThumbs", checkBox_thumbsAutoCycle.Checked);
            settings.AddValue("RomsBrowser:AutoCycleThumbsInterval", (int)(numericUpDown1.Value * 1000));
            settings.AddValue("MediaTab:StopPlayerOnTabChange", checkBox_playerStopOnTabChange.Checked);
            settings.AddValue("MediaTab:ShowListWhenClick", checkBox_ShowListOnMouseClick.Checked);
            settings.AddValue("Show close buttons on tabs", checkBox_showCloseButtonsOnTabs.Checked);
            settings.AddValue("Close buttons always visible on tabs", checkBox_closeButtonsAlwaysVisibleOnTabs.Checked);
            settings.AddValue("Show icons on tabs", checkBox_showIconsOnTabs.Checked);
            settings.AddValue("RomsBrowser:ShowTooltipThumbMode", checkBox_show_tooptip_in_thumb_mode.Checked);
            settings.AddValue("RomsBrowser:ShowTooltipThumbModeAllInfo", checkBox_tootlip_thumbmode_show_all.Checked);
            settings.AddValue("RomsBrowser:ShowTooltipThumbModeRating", checkBox_tooltip_thumb_show_rating.Checked);
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
            checkBox_consolesBrowser_doubleClick.Checked = true;
            checkBox_Emulators_StretchToFit.Checked = false;
            checkBox_roms_ClearFilesAfterDrag.Checked = true;
            checkBox_showSubItmesToolTip.Checked = true;
            checkBox_thumbsAutoCycle.Checked = true;
            checkBox_playerStopOnTabChange.Checked = true;
            checkBox_ShowListOnMouseClick.Checked = false;
            checkBox_showCloseButtonsOnTabs.Checked = true;
            checkBox_closeButtonsAlwaysVisibleOnTabs.Checked = false;
            checkBox_showIconsOnTabs.Checked = true;
            checkBox_show_tooptip_in_thumb_mode.Checked = true;
            checkBox_tootlip_thumbmode_show_all.Checked = false;
            checkBox_tooltip_thumb_show_rating.Checked = false;
            numericUpDown1.Value = 2;
        }
        private void checkBox_show_tooptip_in_thumb_mode_CheckedChanged(object sender, EventArgs e)
        {
            checkBox_tootlip_thumbmode_show_all.Enabled =
            checkBox_tooltip_thumb_show_rating.Enabled = checkBox_show_tooptip_in_thumb_mode.Checked;
        }
    }
}
