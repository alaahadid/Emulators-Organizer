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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using MMB;
namespace EmulatorsOrganizer.Core
{
    [Serializable()]
    public class EOStyle
    {
        public EOStyle()
        {
            fontConvertor = new FontConverter();
            font_CategoriesBrowser = font_ConsolesBrowser = font_EmulatorsBrowser = font_FiltersBrowser =
            font_InformationContainerBrowser = font_InformationContainerTabs = font_PlaylistsBrowser =
            font_RomsBrowser = font_StartOptions = fontConvertor.ConvertToString(new Font("Tahoma", 8, FontStyle.Regular));
        }
        [NonSerialized]
        private FontConverter fontConvertor = new FontConverter();

        /*Background colors*/
        public Color bkgColor_MainWindow = Color.FromArgb(-1);
        public Color bkgColor_CategoriesBrowser = Color.FromArgb(-1);
        public Color bkgColor_ConsolesBrowser = Color.FromArgb(-1);
        public Color bkgColor_EmulatorsBrowser = Color.FromArgb(-1);
        public Color bkgColor_FiltersBrowser = Color.FromArgb(-1);
        public Color bkgColor_InformationContainerBrowser = Color.FromArgb(-1);
        public Color bkgColor_InformationContainerTabs = Color.FromArgb(-1);
        public Color bkgColor_PlaylistsBrowser = Color.FromArgb(-1);
        public Color bkgColor_RomsBrowser = Color.FromArgb(-1);
        public Color bkgColor_StartOptions = Color.FromArgb(-1);
        /*Browsers text colors*/
        public Color txtColor_MainWindowMainMenu = Color.FromArgb(-16777216);
        public Color txtColor_CategoriesBrowser = Color.FromArgb(-16777216);
        public Color txtColor_ConsolesBrowser = Color.FromArgb(-16777216);
        public Color txtColor_EmulatorsBrowser = Color.FromArgb(-16777216);
        public Color txtColor_FiltersBrowser = Color.FromArgb(-16777216);
        public Color txtColor_InformationContainerBrowser = Color.FromArgb(-16777216);
        public Color txtColor_InformationContainerTabs = Color.FromArgb(-16777216);
        public Color txtColor_PlaylistsBrowser = Color.FromArgb(-16777216);
        public Color txtColor_RomsBrowser = Color.FromArgb(-16777216);
        public Color txtColor_StartOptions = Color.FromArgb(-16777216);
        /*Font*/
        public string font_CategoriesBrowser;
        public string font_ConsolesBrowser;
        public string font_EmulatorsBrowser;
        public string font_FiltersBrowser;
        public string font_InformationContainerBrowser;
        public string font_InformationContainerTabs;
        public string font_PlaylistsBrowser;
        public string font_RomsBrowser;
        public string font_StartOptions;
        /*Images*/
        public Image image_CategoriesBrowser;
        public Image image_ConsolesBrowser;
        public Image image_EmulatorsBrowser;
        public Image image_FiltersBrowser;
        public Image image_InformationContainerBrowser;
        public Image image_InformationContainerTabs;
        public Image image_PlaylistsBrowser;
        public Image image_RomsBrowser;
        public Image image_StartOptions;

        public BackgroundImageMode imageMode_CategoriesBrowser = BackgroundImageMode.StretchIfLarger;
        public BackgroundImageMode imageMode_ConsolesBrowser = BackgroundImageMode.StretchIfLarger;
        public BackgroundImageMode imageMode_EmulatorsBrowser = BackgroundImageMode.StretchIfLarger;
        public BackgroundImageMode imageMode_FiltersBrowser = BackgroundImageMode.StretchIfLarger;
        public BackgroundImageMode imageMode_InformationContainerBrowser = BackgroundImageMode.StretchIfLarger;
        public BackgroundImageMode imageMode_InformationContainerTabs = BackgroundImageMode.StretchIfLarger;
        public BackgroundImageMode imageMode_PlaylistsBrowser = BackgroundImageMode.StretchIfLarger;
        public BackgroundImageMode imageMode_RomsBrowser = BackgroundImageMode.StretchIfLarger;
        public BackgroundImageMode imageMode_StartOptions = BackgroundImageMode.StretchIfLarger;
        /*Window Sizes*/
        public Size mainWindowSize = new Size(1024, 720);
        public bool mainWindowResize = true;
        public bool mainWindowHideLeftPanel = false;
        public int mainWindowSplitContainer_left = 200;
        public int mainWindowSplitContainer_left_down = 200;
        public int mainWindowSplitContainer_main = 200;
        public int mainWindowSplitContainer_right = 500;
        /*Listview style*/
        public Color listviewTextsColor = Color.FromArgb(-16777216);
        public Color listviewHighlightColor = Color.FromArgb(-16744193);
        public Color listviewSpecialColor = Color.FromArgb(-6632142);
        public Color listviewMouseOverColor = Color.FromArgb(-657931);
        public Color listviewColumnTextColor = Color.FromArgb(-16777216);
        public Color listviewColumnColor = Color.FromArgb(-1);
        public Color listviewColumnHighlightColor = Color.FromArgb(-16744193);
        public Color listviewColumnClickColor = Color.FromArgb(-7667712);
        public bool listviewDrawHighlight = true;
        /*MTC style*/
        public Color TabPageColor = Color.FromArgb(-1);
        public Color TabPageSelectedColor = Color.FromArgb(-8798977);
        public Color TabPageHighlightedColor = Color.FromArgb(-6566401);
        public Color TabPageSplitColor = Color.FromArgb(-16777216);
        public Color TabPageTextsColor = Color.FromArgb(-16777216);

        /// <summary>
        /// Create an exact clone of this style object
        /// </summary>
        /// <returns>An exact clone of this style object</returns>
        public EOStyle Clone()
        {
            EOStyle newStyle = new EOStyle();
            newStyle.bkgColor_MainWindow = bkgColor_MainWindow;
            newStyle.bkgColor_CategoriesBrowser = bkgColor_CategoriesBrowser;
            newStyle.bkgColor_ConsolesBrowser = bkgColor_ConsolesBrowser;
            newStyle.bkgColor_EmulatorsBrowser = bkgColor_EmulatorsBrowser;
            newStyle.bkgColor_FiltersBrowser = bkgColor_FiltersBrowser;
            newStyle.bkgColor_InformationContainerBrowser = bkgColor_InformationContainerBrowser;
            newStyle.bkgColor_InformationContainerTabs = bkgColor_InformationContainerTabs;
            newStyle.bkgColor_PlaylistsBrowser = bkgColor_PlaylistsBrowser;
            newStyle.bkgColor_RomsBrowser = bkgColor_RomsBrowser;
            newStyle.bkgColor_StartOptions = bkgColor_StartOptions;
            newStyle.txtColor_MainWindowMainMenu = txtColor_MainWindowMainMenu;
            newStyle.txtColor_CategoriesBrowser = txtColor_CategoriesBrowser;
            newStyle.txtColor_ConsolesBrowser = txtColor_ConsolesBrowser;
            newStyle.txtColor_EmulatorsBrowser = txtColor_EmulatorsBrowser;
            newStyle.txtColor_FiltersBrowser = txtColor_FiltersBrowser;
            newStyle.txtColor_InformationContainerBrowser = txtColor_InformationContainerBrowser;
            newStyle.txtColor_InformationContainerTabs = txtColor_InformationContainerTabs;
            newStyle.txtColor_PlaylistsBrowser = txtColor_PlaylistsBrowser;
            newStyle.txtColor_RomsBrowser = txtColor_RomsBrowser;
            newStyle.txtColor_StartOptions = txtColor_StartOptions;
            newStyle.image_CategoriesBrowser = image_CategoriesBrowser;
            newStyle.image_ConsolesBrowser = image_ConsolesBrowser;
            newStyle.image_EmulatorsBrowser = image_EmulatorsBrowser;
            newStyle.image_FiltersBrowser = image_FiltersBrowser;
            newStyle.image_InformationContainerBrowser = image_InformationContainerBrowser;
            newStyle.image_InformationContainerTabs = image_InformationContainerTabs;
            newStyle.image_PlaylistsBrowser = image_PlaylistsBrowser;
            newStyle.image_RomsBrowser = image_RomsBrowser;
            newStyle.image_StartOptions = image_StartOptions;

            newStyle.imageMode_CategoriesBrowser = imageMode_CategoriesBrowser;
            newStyle.imageMode_ConsolesBrowser = imageMode_ConsolesBrowser;
            newStyle.imageMode_EmulatorsBrowser = imageMode_EmulatorsBrowser;
            newStyle.imageMode_FiltersBrowser = imageMode_FiltersBrowser;
            newStyle.imageMode_InformationContainerBrowser = imageMode_InformationContainerBrowser;
            newStyle.imageMode_InformationContainerTabs = imageMode_InformationContainerTabs;
            newStyle.imageMode_PlaylistsBrowser = imageMode_PlaylistsBrowser;
            newStyle.imageMode_RomsBrowser = imageMode_RomsBrowser;
            newStyle.imageMode_StartOptions = imageMode_StartOptions;

            newStyle.font_CategoriesBrowser = font_CategoriesBrowser;
            newStyle.font_ConsolesBrowser = font_ConsolesBrowser;
            newStyle.font_EmulatorsBrowser = font_EmulatorsBrowser;
            newStyle.font_FiltersBrowser = font_FiltersBrowser;
            newStyle.font_InformationContainerBrowser = font_InformationContainerBrowser;
            newStyle.font_InformationContainerTabs = font_InformationContainerTabs;
            newStyle.font_PlaylistsBrowser = font_PlaylistsBrowser;
            newStyle.font_RomsBrowser = font_RomsBrowser;
            newStyle.font_StartOptions = font_StartOptions;
            /*Window Sizes*/
            newStyle.mainWindowSize = mainWindowSize;
            newStyle.mainWindowHideLeftPanel = mainWindowHideLeftPanel;
            newStyle.mainWindowResize = mainWindowResize;
            newStyle.mainWindowSplitContainer_left = mainWindowSplitContainer_left;
            newStyle.mainWindowSplitContainer_left_down = mainWindowSplitContainer_left_down;
            newStyle.mainWindowSplitContainer_main = mainWindowSplitContainer_main;
            newStyle.mainWindowSplitContainer_right = mainWindowSplitContainer_right;

            newStyle.TabPageColor = this.TabPageColor;
            newStyle.TabPageHighlightedColor = this.TabPageHighlightedColor;
            newStyle.TabPageSelectedColor = this.TabPageSelectedColor;
            newStyle.TabPageSplitColor = this.TabPageSplitColor;
            newStyle.TabPageTextsColor = this.TabPageTextsColor;
            newStyle.listviewColumnClickColor = this.listviewColumnClickColor;
            newStyle.listviewColumnColor = this.listviewColumnColor;
            newStyle.listviewColumnHighlightColor = this.listviewColumnHighlightColor;
            newStyle.listviewColumnTextColor = this.listviewColumnTextColor;
            newStyle.listviewDrawHighlight = this.listviewDrawHighlight;
            newStyle.listviewHighlightColor = this.listviewHighlightColor;
            newStyle.listviewMouseOverColor = this.listviewMouseOverColor;
            newStyle.listviewSpecialColor = this.listviewSpecialColor;
            newStyle.listviewTextsColor = this.listviewTextsColor;
            return newStyle;
        }

        #region Static member, for save and load
        public static void SaveStyle(string filePath, EOStyle style)
        {
            FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            try
            {
                Trace.WriteLine("Saving style as ...", "Style");
                // Make a memory stream
                MemoryStream memStream = new MemoryStream();
                // Serialize inside of it
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memStream, style);
                // Compress
                byte[] data = new byte[0];
                HelperTools.CompressData(memStream.GetBuffer(), out data);
                // Write the compressed data into file
                fs.Write(data, 0, data.Length);
                fs.Close();
                Trace.WriteLine("Style saved successfully.", "Style");
                ManagedMessageBox.ShowMessage("Style saved successfully.");
            }
            catch (Exception ex)
            {
                fs.Close();
                Trace.TraceError("Unable to save style: " + ex.Message, "Style");
                ManagedMessageBox.ShowErrorMessage(ex.Message + "\n\n" + ex.ToString());
            }
        }
        public static EOStyle LoadStyle(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                Trace.WriteLine("Loading style ...", "Style");
                // Read file data
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                fs.Close();
                // Decompress
                byte[] decompressed = new byte[0];
                HelperTools.DecompressData(data, out decompressed);
                // Deserialize
                MemoryStream st = new MemoryStream(decompressed);
                BinaryFormatter formatter = new BinaryFormatter();
                EOStyle style = (EOStyle)formatter.Deserialize(st);

                Trace.WriteLine("Style loaded successfully.", "Style");
                ManagedMessageBox.ShowMessage("Style loaded successfully.");
                return style;
            }
            catch (Exception ex)
            {
                fs.Close();
                Trace.TraceError("Unable to load style: " + ex.Message, "Style");
                ManagedMessageBox.ShowErrorMessage(ex.Message + "\n\n" + ex.ToString());
            }
            return null;
        }
        #endregion
    }
    public enum BackgroundImageMode : int
    {
        NormalStretchNoAspectRatio = 0,
        StretchIfLarger = 1,
        StretchToFit = 2
    }
}
