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
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;
using EmulatorsOrganizer.Services.DefaultServices.Settings;
using MLV;
using MMB;
using MTC;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace EmulatorsOrganizer.GUI
{
    public partial class StyleEditorControl : UserControl
    {
        public StyleEditorControl()
        {
            InitializeComponent();
            // List view preview
            ManagedListViewColumn column = new ManagedListViewColumn();
            column.HeaderText = "Test 1";
            column.ID = "1";
            managedListView1.Columns.Add(column);
            column = new ManagedListViewColumn();
            column.HeaderText = "Test 2";
            column.ID = "2";
            managedListView1.Columns.Add(column);

            ManagedListViewItem item = new ManagedListViewItem();
            ManagedListViewSubItem subItem = new ManagedListViewSubItem();
            subItem.ColumnID = "1";
            subItem.Text = "Item 1";
            item.SubItems.Add(subItem);
            subItem = new ManagedListViewSubItem();
            subItem.ColumnID = "2";
            subItem.Text = "Item 1";
            item.SubItems.Add(subItem);
            managedListView1.Items.Add(item);
            item = new ManagedListViewItem();
            subItem = new ManagedListViewSubItem();
            subItem.ColumnID = "1";
            subItem.Text = "Item 2";
            item.SubItems.Add(subItem);
            subItem = new ManagedListViewSubItem();
            subItem.ColumnID = "2";
            subItem.Text = "Item 2";
            item.SubItems.Add(subItem);
            managedListView1.Items.Add(item);
            // Tabs
            // add new tabs for test
            MTCTabPage page = new MTCTabPage();
            page.DrawType = MTCTabPageDrawType.Text;
            page.Text = "Test 1";
            managedTabControl1.TabPages.Add(page);

            page = new MTCTabPage();
            page.DrawType = MTCTabPageDrawType.Text;
            page.Text = "Test 2";
            managedTabControl1.TabPages.Add(page);
        }

        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private SettingsService settings = (SettingsService)ServicesManager.GetService("Global Settings");
        private Font font;
        private FontConverter fontConvert = new FontConverter();

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
        public Color txtColor_MainMenu = Color.FromArgb(-16777216);
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

        private Image image_CategoriesBrowser;
        private Image image_ConsolesBrowser;
        private Image image_EmulatorsBrowser;
        private Image image_FiltersBrowser;
        private Image image_InformationContainerBrowser;
        private Image image_InformationContainerTabs;
        private Image image_PlaylistsBrowser;
        private Image image_RomsBrowser;
        private Image image_StartOptions;
        private BackgroundImageMode imageMode_CategoriesBrowser;
        private BackgroundImageMode imageMode_ConsolesBrowser;
        private BackgroundImageMode imageMode_EmulatorsBrowser;
        private BackgroundImageMode imageMode_FiltersBrowser;
        private BackgroundImageMode imageMode_InformationContainerBrowser;
        private BackgroundImageMode imageMode_InformationContainerTabs;
        private BackgroundImageMode imageMode_PlaylistsBrowser;
        private BackgroundImageMode imageMode_RomsBrowser;
        private BackgroundImageMode imageMode_StartOptions;

        public override string ToString()
        {
            return ls["Title_TabDefaultStyle"];
        }

        private void UpdatePreview()
        {
            // List view
            foreach (ManagedListViewColumn column in managedListView1.Columns)
                column.HeaderTextColor = button_columnTextColor.BackColor;
            foreach (ManagedListViewItem item in managedListView1.Items)
                foreach (ManagedListViewSubItem subItem in item.SubItems)
                    subItem.Color = button_ItemtextsColor.BackColor;
            managedListView1.ItemHighlightColor = button_itemHighlightColor.BackColor;
            managedListView1.ItemMouseOverColor = button_itemMouseOverColor.BackColor;
            managedListView1.ItemSpecialColor = button_itemSpecialColor.BackColor;
            managedListView1.ColumnClickColor = button_columnClickColor.BackColor;
            managedListView1.ColumnColor = button_columnColor.BackColor;
            managedListView1.ColumnHighlightColor = button_columnHighlightColor.BackColor;
            managedListView1.DrawHighlight = checkBox_drawMouseOver.Checked;
            // Tabs
            managedTabControl1.TabPageColor = button_tabPageColor.BackColor;
            managedTabControl1.TabPageHighlightedColor = button_TabPageHiglightColor.BackColor;
            managedTabControl1.TabPageSelectedColor = button_tabPageSelectedColor.BackColor;
            managedTabControl1.TabPageSplitColor = button_tapPageSplitColor.BackColor;
            managedTabControl1.ForeColor = button_tabPageTextsColor.BackColor;
            managedTabControl1.BackColor = button_eoBackgroundColor.BackColor;
            managedTabControl1.Invalidate();
        }
        private void UpdateBrowsersPreview()
        {
            switch (listBox_images.SelectedIndex)
            {
                case 0:
                    {
                        button_browserBackgroundColor.BackColor = bkgColor_CategoriesBrowser;
                        button_browserBackgroundColor.Text = bkgColor_CategoriesBrowser.ToArgb().ToString("X6");
                        button_browserTextColor.BackColor = txtColor_CategoriesBrowser;
                        button_browserTextColor.Text = txtColor_CategoriesBrowser.ToArgb().ToString("X6");
                        // Font
                        textBox_browserFont.Text = font_CategoriesBrowser;
                        // Image
                        imagePanel1.ImageToView = (Bitmap)image_CategoriesBrowser;
                        comboBox_imageMode.SelectedIndex = (int)imageMode_CategoriesBrowser;
                        imagePanel1.Invalidate();
                        break;
                    }
                case 1:
                    {
                        button_browserBackgroundColor.BackColor = bkgColor_ConsolesBrowser;
                        button_browserBackgroundColor.Text = bkgColor_ConsolesBrowser.ToArgb().ToString("X6");
                        button_browserTextColor.BackColor = txtColor_ConsolesBrowser;
                        button_browserTextColor.Text = txtColor_ConsolesBrowser.ToArgb().ToString("X6");
                        // Font
                        textBox_browserFont.Text = font_ConsolesBrowser;
                        // Image
                        imagePanel1.ImageToView = (Bitmap)image_ConsolesBrowser;
                        comboBox_imageMode.SelectedIndex = (int)imageMode_ConsolesBrowser;
                        imagePanel1.Invalidate();
                        break;
                    }
                case 2:
                    {
                        button_browserBackgroundColor.BackColor = bkgColor_EmulatorsBrowser;
                        button_browserBackgroundColor.Text = bkgColor_EmulatorsBrowser.ToArgb().ToString("X6");
                        button_browserTextColor.BackColor = txtColor_EmulatorsBrowser;
                        button_browserTextColor.Text = txtColor_EmulatorsBrowser.ToArgb().ToString("X6");
                        // Font
                        textBox_browserFont.Text = font_EmulatorsBrowser;
                        // Image
                        imagePanel1.ImageToView = (Bitmap)image_EmulatorsBrowser;
                        comboBox_imageMode.SelectedIndex = (int)imageMode_EmulatorsBrowser;
                        imagePanel1.Invalidate();
                        break;
                    }
                case 3:
                    {
                        button_browserBackgroundColor.BackColor = bkgColor_FiltersBrowser;
                        button_browserBackgroundColor.Text = bkgColor_FiltersBrowser.ToArgb().ToString("X6");
                        button_browserTextColor.BackColor = txtColor_FiltersBrowser;
                        button_browserTextColor.Text = txtColor_FiltersBrowser.ToArgb().ToString("X6");
                        // Font
                        textBox_browserFont.Text = font_FiltersBrowser;
                        // Image
                        imagePanel1.ImageToView = (Bitmap)image_FiltersBrowser;
                        comboBox_imageMode.SelectedIndex = (int)imageMode_FiltersBrowser;
                        imagePanel1.Invalidate();
                        break;
                    }
                case 4:
                    {
                        button_browserBackgroundColor.BackColor = bkgColor_InformationContainerBrowser;
                        button_browserBackgroundColor.Text = bkgColor_InformationContainerBrowser.ToArgb().ToString("X6");
                        button_browserTextColor.BackColor = txtColor_InformationContainerBrowser;
                        button_browserTextColor.Text = txtColor_InformationContainerBrowser.ToArgb().ToString("X6");
                        // Font
                        textBox_browserFont.Text = font_InformationContainerBrowser;
                        // Image
                        imagePanel1.ImageToView = (Bitmap)image_InformationContainerBrowser;
                        comboBox_imageMode.SelectedIndex = (int)imageMode_InformationContainerBrowser;
                        imagePanel1.Invalidate();
                        break;
                    }
                case 5:
                    {
                        button_browserBackgroundColor.BackColor = bkgColor_InformationContainerTabs;
                        button_browserBackgroundColor.Text = bkgColor_InformationContainerTabs.ToArgb().ToString("X6");
                        button_browserTextColor.BackColor = txtColor_InformationContainerTabs;
                        button_browserTextColor.Text = txtColor_InformationContainerTabs.ToArgb().ToString("X6");
                        // Font
                        textBox_browserFont.Text = font_InformationContainerTabs;
                        // Image
                        imagePanel1.ImageToView = (Bitmap)image_InformationContainerTabs;
                        comboBox_imageMode.SelectedIndex = (int)imageMode_InformationContainerTabs;
                        imagePanel1.Invalidate();
                        break;
                    }
                case 6:
                    {
                        button_browserBackgroundColor.BackColor = bkgColor_PlaylistsBrowser;
                        button_browserBackgroundColor.Text = bkgColor_PlaylistsBrowser.ToArgb().ToString("X6");
                        button_browserTextColor.BackColor = txtColor_PlaylistsBrowser;
                        button_browserTextColor.Text = txtColor_PlaylistsBrowser.ToArgb().ToString("X6");
                        // Font
                        textBox_browserFont.Text = font_PlaylistsBrowser;
                        // Image
                        imagePanel1.ImageToView = (Bitmap)image_PlaylistsBrowser;
                        comboBox_imageMode.SelectedIndex = (int)imageMode_PlaylistsBrowser;
                        imagePanel1.Invalidate();
                        break;
                    }
                case 7:
                    {
                        button_browserBackgroundColor.BackColor = bkgColor_RomsBrowser;
                        button_browserBackgroundColor.Text = bkgColor_RomsBrowser.ToArgb().ToString("X6");
                        button_browserTextColor.BackColor = txtColor_RomsBrowser;
                        button_browserTextColor.Text = txtColor_RomsBrowser.ToArgb().ToString("X6");
                        // Font
                        textBox_browserFont.Text = font_RomsBrowser;
                        // Image
                        imagePanel1.ImageToView = (Bitmap)image_RomsBrowser;
                        comboBox_imageMode.SelectedIndex = (int)imageMode_RomsBrowser;
                        imagePanel1.Invalidate();
                        break;
                    }
                case 8:
                    {
                        button_browserBackgroundColor.BackColor = bkgColor_StartOptions;
                        button_browserBackgroundColor.Text = bkgColor_StartOptions.ToArgb().ToString("X6");
                        button_browserTextColor.BackColor = txtColor_StartOptions;
                        button_browserTextColor.Text = txtColor_StartOptions.ToArgb().ToString("X6");
                        // Font
                        textBox_browserFont.Text = font_StartOptions;
                        // Image
                        imagePanel1.ImageToView = (Bitmap)image_StartOptions;
                        comboBox_imageMode.SelectedIndex = (int)imageMode_StartOptions;
                        imagePanel1.Invalidate();
                        break;
                    }
            }
        }
        public void LoadSettings(EOStyle defaultStyle)
        {
            // List view
            checkBox_drawMouseOver.Checked = defaultStyle.listviewDrawHighlight;
            checkBox_resizeTheMainWindow.Checked = defaultStyle.mainWindowResize;
            button_columnClickColor.BackColor = defaultStyle.listviewColumnClickColor;
            button_columnClickColor.Text = button_columnClickColor.BackColor.ToArgb().ToString("X6");
            button_columnHighlightColor.BackColor = defaultStyle.listviewColumnHighlightColor;
            button_columnHighlightColor.Text = button_columnHighlightColor.BackColor.ToArgb().ToString("X6");
            button_columnTextColor.BackColor = defaultStyle.listviewColumnTextColor;
            button_columnTextColor.Text = button_columnTextColor.BackColor.ToArgb().ToString("X6");
            button_columnColor.BackColor = defaultStyle.listviewColumnColor;
            button_columnColor.Text = button_columnColor.BackColor.ToArgb().ToString("X6");
            button_itemHighlightColor.BackColor = defaultStyle.listviewHighlightColor;
            button_itemHighlightColor.Text = button_itemHighlightColor.BackColor.ToArgb().ToString("X6");
            button_itemMouseOverColor.BackColor = defaultStyle.listviewMouseOverColor;
            button_itemMouseOverColor.Text = button_itemMouseOverColor.BackColor.ToArgb().ToString("X6");
            button_itemSpecialColor.BackColor = defaultStyle.listviewSpecialColor;
            button_itemSpecialColor.Text = button_itemSpecialColor.BackColor.ToArgb().ToString("X6");
            button_ItemtextsColor.BackColor = defaultStyle.listviewTextsColor;
            button_ItemtextsColor.Text = button_ItemtextsColor.BackColor.ToArgb().ToString("X6");
            button_mainMenuTextColor.BackColor = defaultStyle.txtColor_MainWindowMainMenu;
            button_mainMenuTextColor.Text = button_mainMenuTextColor.BackColor.ToArgb().ToString("X6");
            // Tabs

            button_tabPageColor.BackColor = defaultStyle.TabPageColor;
            button_tabPageColor.Text = defaultStyle.TabPageColor.ToArgb().ToString("X6");

            button_TabPageHiglightColor.BackColor = defaultStyle.TabPageHighlightedColor;
            button_TabPageHiglightColor.Text = defaultStyle.TabPageHighlightedColor.ToArgb().ToString("X6");

            button_tabPageSelectedColor.BackColor = defaultStyle.TabPageSelectedColor;
            button_tabPageSelectedColor.Text = defaultStyle.TabPageSelectedColor.ToArgb().ToString("X6");

            button_tapPageSplitColor.BackColor = defaultStyle.TabPageSplitColor;
            button_tapPageSplitColor.Text = defaultStyle.TabPageSplitColor.ToArgb().ToString("X6");

            button_tabPageTextsColor.BackColor = defaultStyle.TabPageTextsColor;
            button_tabPageTextsColor.Text = defaultStyle.TabPageTextsColor.ToArgb().ToString("X6");

            button_eoBackgroundColor.BackColor = defaultStyle.bkgColor_MainWindow;
            button_eoBackgroundColor.Text = defaultStyle.bkgColor_MainWindow.ToArgb().ToString("X6");
            // Images
            image_CategoriesBrowser = defaultStyle.image_CategoriesBrowser;
            image_ConsolesBrowser = defaultStyle.image_ConsolesBrowser;
            image_EmulatorsBrowser = defaultStyle.image_EmulatorsBrowser;
            image_FiltersBrowser = defaultStyle.image_FiltersBrowser;
            image_InformationContainerBrowser = defaultStyle.image_InformationContainerBrowser;
            image_InformationContainerTabs = defaultStyle.image_InformationContainerTabs;
            image_PlaylistsBrowser = defaultStyle.image_PlaylistsBrowser;
            image_RomsBrowser = defaultStyle.image_RomsBrowser;
            image_StartOptions = defaultStyle.image_StartOptions;
            imageMode_CategoriesBrowser = defaultStyle.imageMode_CategoriesBrowser;
            imageMode_ConsolesBrowser = defaultStyle.imageMode_ConsolesBrowser;
            imageMode_EmulatorsBrowser = defaultStyle.imageMode_EmulatorsBrowser;
            imageMode_FiltersBrowser = defaultStyle.imageMode_FiltersBrowser;
            imageMode_InformationContainerBrowser = defaultStyle.imageMode_InformationContainerBrowser;
            imageMode_InformationContainerTabs = defaultStyle.imageMode_InformationContainerTabs;
            imageMode_PlaylistsBrowser = defaultStyle.imageMode_PlaylistsBrowser;
            imageMode_RomsBrowser = defaultStyle.imageMode_RomsBrowser;
            imageMode_StartOptions = defaultStyle.imageMode_StartOptions;
            // Fonts and colors
            bkgColor_MainWindow = defaultStyle.bkgColor_MainWindow;
            bkgColor_CategoriesBrowser = defaultStyle.bkgColor_CategoriesBrowser;
            bkgColor_ConsolesBrowser = defaultStyle.bkgColor_ConsolesBrowser;
            bkgColor_EmulatorsBrowser = defaultStyle.bkgColor_EmulatorsBrowser;
            bkgColor_FiltersBrowser = defaultStyle.bkgColor_FiltersBrowser;
            bkgColor_InformationContainerBrowser = defaultStyle.bkgColor_InformationContainerBrowser;
            bkgColor_InformationContainerTabs = defaultStyle.bkgColor_InformationContainerTabs;
            bkgColor_PlaylistsBrowser = defaultStyle.bkgColor_PlaylistsBrowser;
            bkgColor_RomsBrowser = defaultStyle.bkgColor_RomsBrowser;
            bkgColor_StartOptions = defaultStyle.bkgColor_StartOptions;
            txtColor_MainMenu = defaultStyle.txtColor_MainWindowMainMenu;
            txtColor_CategoriesBrowser = defaultStyle.txtColor_CategoriesBrowser;
            txtColor_ConsolesBrowser = defaultStyle.txtColor_ConsolesBrowser;
            txtColor_EmulatorsBrowser = defaultStyle.txtColor_EmulatorsBrowser;
            txtColor_FiltersBrowser = defaultStyle.txtColor_FiltersBrowser;
            txtColor_InformationContainerBrowser = defaultStyle.txtColor_InformationContainerBrowser;
            txtColor_InformationContainerTabs = defaultStyle.txtColor_InformationContainerTabs;
            txtColor_PlaylistsBrowser = defaultStyle.txtColor_PlaylistsBrowser;
            txtColor_RomsBrowser = defaultStyle.txtColor_RomsBrowser;
            txtColor_StartOptions = defaultStyle.txtColor_StartOptions;
            /*Font*/
            font_CategoriesBrowser = defaultStyle.font_CategoriesBrowser;
            font_ConsolesBrowser = defaultStyle.font_ConsolesBrowser;
            font_EmulatorsBrowser = defaultStyle.font_EmulatorsBrowser;
            font_FiltersBrowser = defaultStyle.font_FiltersBrowser;
            font_InformationContainerBrowser = defaultStyle.font_InformationContainerBrowser;
            font_InformationContainerTabs = defaultStyle.font_InformationContainerTabs;
            font_PlaylistsBrowser = defaultStyle.font_PlaylistsBrowser;
            font_RomsBrowser = defaultStyle.font_RomsBrowser;
            font_StartOptions = defaultStyle.font_StartOptions;

            listBox_images.SelectedIndex = 0;
            UpdateBrowsersPreview();
            UpdatePreview();
        }
        public EOStyle SaveSettings()
        {
            EOStyle defaultStyle = new EOStyle();
            // List view
            defaultStyle.listviewColumnClickColor = button_columnClickColor.BackColor;
            defaultStyle.listviewColumnHighlightColor = button_columnHighlightColor.BackColor;
            defaultStyle.listviewColumnTextColor = button_columnTextColor.BackColor;
            defaultStyle.listviewColumnColor = button_columnColor.BackColor;
            defaultStyle.listviewHighlightColor = button_itemHighlightColor.BackColor;
            defaultStyle.listviewMouseOverColor = button_itemMouseOverColor.BackColor;
            defaultStyle.listviewSpecialColor = button_itemSpecialColor.BackColor;
            defaultStyle.listviewTextsColor = button_ItemtextsColor.BackColor;
            defaultStyle.listviewDrawHighlight = checkBox_drawMouseOver.Checked;
            defaultStyle.mainWindowResize = checkBox_resizeTheMainWindow.Checked;
            // Tabs
            defaultStyle.TabPageColor = button_tabPageColor.BackColor;
            defaultStyle.TabPageHighlightedColor = button_TabPageHiglightColor.BackColor;
            defaultStyle.TabPageSelectedColor = button_tabPageSelectedColor.BackColor;
            defaultStyle.TabPageSplitColor = button_tapPageSplitColor.BackColor;
            defaultStyle.TabPageTextsColor = button_tabPageTextsColor.BackColor;
            defaultStyle.bkgColor_MainWindow = button_eoBackgroundColor.BackColor;
            // Images
            defaultStyle.image_CategoriesBrowser = image_CategoriesBrowser;
            defaultStyle.image_ConsolesBrowser = image_ConsolesBrowser;
            defaultStyle.image_EmulatorsBrowser = image_EmulatorsBrowser;
            defaultStyle.image_FiltersBrowser = image_FiltersBrowser;
            defaultStyle.image_InformationContainerBrowser = image_InformationContainerBrowser;
            defaultStyle.image_InformationContainerTabs = image_InformationContainerTabs;
            defaultStyle.image_PlaylistsBrowser = image_PlaylistsBrowser;
            defaultStyle.image_RomsBrowser = image_RomsBrowser;
            defaultStyle.image_StartOptions = image_StartOptions;

            defaultStyle.imageMode_CategoriesBrowser = imageMode_CategoriesBrowser;
            defaultStyle.imageMode_ConsolesBrowser = imageMode_ConsolesBrowser;
            defaultStyle.imageMode_EmulatorsBrowser = imageMode_EmulatorsBrowser;
            defaultStyle.imageMode_FiltersBrowser = imageMode_FiltersBrowser;
            defaultStyle.imageMode_InformationContainerBrowser = imageMode_InformationContainerBrowser;
            defaultStyle.imageMode_InformationContainerTabs = imageMode_InformationContainerTabs;
            defaultStyle.imageMode_PlaylistsBrowser = imageMode_PlaylistsBrowser;
            defaultStyle.imageMode_RomsBrowser = imageMode_RomsBrowser;
            defaultStyle.imageMode_StartOptions = imageMode_StartOptions;
            // Fonts and colors
            defaultStyle.bkgColor_MainWindow = bkgColor_MainWindow;
            defaultStyle.bkgColor_CategoriesBrowser = bkgColor_CategoriesBrowser;
            defaultStyle.bkgColor_ConsolesBrowser = bkgColor_ConsolesBrowser;
            defaultStyle.bkgColor_EmulatorsBrowser = bkgColor_EmulatorsBrowser;
            defaultStyle.bkgColor_FiltersBrowser = bkgColor_FiltersBrowser;
            defaultStyle.bkgColor_InformationContainerBrowser = bkgColor_InformationContainerBrowser;
            defaultStyle.bkgColor_InformationContainerTabs = bkgColor_InformationContainerTabs;
            defaultStyle.bkgColor_PlaylistsBrowser = bkgColor_PlaylistsBrowser;
            defaultStyle.bkgColor_RomsBrowser = bkgColor_RomsBrowser;
            defaultStyle.bkgColor_StartOptions = bkgColor_StartOptions;
            defaultStyle.txtColor_MainWindowMainMenu = txtColor_MainMenu;
            defaultStyle.txtColor_CategoriesBrowser = txtColor_CategoriesBrowser;
            defaultStyle.txtColor_ConsolesBrowser = txtColor_ConsolesBrowser;
            defaultStyle.txtColor_EmulatorsBrowser = txtColor_EmulatorsBrowser;
            defaultStyle.txtColor_FiltersBrowser = txtColor_FiltersBrowser;
            defaultStyle.txtColor_InformationContainerBrowser = txtColor_InformationContainerBrowser;
            defaultStyle.txtColor_InformationContainerTabs = txtColor_InformationContainerTabs;
            defaultStyle.txtColor_PlaylistsBrowser = txtColor_PlaylistsBrowser;
            defaultStyle.txtColor_RomsBrowser = txtColor_RomsBrowser;
            defaultStyle.txtColor_StartOptions = txtColor_StartOptions;
            /*Font*/
            defaultStyle.font_CategoriesBrowser = font_CategoriesBrowser;
            defaultStyle.font_ConsolesBrowser = font_ConsolesBrowser;
            defaultStyle.font_EmulatorsBrowser = font_EmulatorsBrowser;
            defaultStyle.font_FiltersBrowser = font_FiltersBrowser;
            defaultStyle.font_InformationContainerBrowser = font_InformationContainerBrowser;
            defaultStyle.font_InformationContainerTabs = font_InformationContainerTabs;
            defaultStyle.font_PlaylistsBrowser = font_PlaylistsBrowser;
            defaultStyle.font_RomsBrowser = font_RomsBrowser;
            defaultStyle.font_StartOptions = font_StartOptions;
            return defaultStyle;
        }
        public void DefaultSettings()
        {
            LoadSettings(new EOStyle());
        }

        private void ChangeButtonColor(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.Color = ((Button)sender).BackColor;
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                ((Button)sender).BackColor = dialog.Color;
                ((Button)sender).Text = dialog.Color.ToArgb().ToString("X6");
                UpdatePreview();
            }
        }
        private void checkBox_drawMouseOver_CheckedChanged(object sender, EventArgs e)
        {
            managedListView1.DrawHighlight = checkBox_drawMouseOver.Checked;
        }
        private void listBox_images_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateBrowsersPreview();
        }
        // Change selected image
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog Op = new OpenFileDialog();
            Op.Title = ls["Title_BrowseForImage"];
            Op.Filter = ls["Filter_Image"]; ;
            Op.Multiselect = false;
            if (Op.ShowDialog(this) == DialogResult.OK)
            {
                // Stream str = new FileStream(Op.FileName, FileMode.Open, FileAccess.Read);
                // byte[] buff = new byte[str.Length];
                // str.Read(buff, 0, (int)str.Length);
                // str.Dispose();
                //str.Close();

                // Bitmap loadedImage = (Bitmap)Image.FromStream(new MemoryStream(buff));
                Bitmap loadedImage = null;
                using (var bmpTemp = new Bitmap(Op.FileName))
                {
                    loadedImage = new Bitmap(bmpTemp);
                }
                switch (listBox_images.SelectedIndex)
                {
                    case 0:
                        {
                            image_CategoriesBrowser = loadedImage;
                            imagePanel1.Invalidate();
                            break;
                        }
                    case 1:
                        {
                            image_ConsolesBrowser = loadedImage;
                            imagePanel1.Invalidate();
                            break;
                        }
                    case 2:
                        {
                            image_EmulatorsBrowser = loadedImage;
                            imagePanel1.Invalidate();
                            break;
                        }
                    case 3:
                        {
                            image_FiltersBrowser = loadedImage;
                            imagePanel1.Invalidate();
                            break;
                        }
                    case 4:
                        {
                            image_InformationContainerBrowser = loadedImage;
                            imagePanel1.Invalidate();
                            break;
                        }
                    case 5:
                        {
                            image_InformationContainerTabs = loadedImage;
                            imagePanel1.Invalidate();
                            break;
                        }
                    case 6:
                        {
                            image_PlaylistsBrowser = loadedImage;
                            imagePanel1.Invalidate();
                            break;
                        }
                    case 7:
                        {
                            image_RomsBrowser = loadedImage;
                            imagePanel1.Invalidate();
                            break;
                        }
                    case 8:
                        {
                            image_StartOptions = loadedImage;
                            imagePanel1.Invalidate();
                            break;
                        }
                }
                UpdateBrowsersPreview();
            }
        }
        // Clear selected image
        private void button3_Click(object sender, EventArgs e)
        {
            ManagedMessageBoxResult result =
                ManagedMessageBox.ShowQuestionMessage(ls["Message_AreYouSureYouWantToClearSelectedImage"]);
            if (result.ClickedButtonIndex == 0)
            {
                switch (listBox_images.SelectedIndex)
                {
                    case 0:
                        {
                            image_CategoriesBrowser = null;
                            imagePanel1.Invalidate();
                            break;
                        }
                    case 1:
                        {
                            image_ConsolesBrowser = null;
                            imagePanel1.Invalidate();
                            break;
                        }
                    case 2:
                        {
                            image_EmulatorsBrowser = null;
                            imagePanel1.Invalidate();
                            break;
                        }
                    case 3:
                        {
                            image_FiltersBrowser = null;
                            imagePanel1.Invalidate();
                            break;
                        }
                    case 4:
                        {
                            image_InformationContainerBrowser = null;
                            imagePanel1.Invalidate();
                            break;
                        }
                    case 5:
                        {
                            image_InformationContainerTabs = null;
                            imagePanel1.Invalidate();
                            break;
                        }
                    case 6:
                        {
                            image_PlaylistsBrowser = null;
                            imagePanel1.Invalidate();
                            break;
                        }
                    case 7:
                        {
                            image_RomsBrowser = null;
                            imagePanel1.Invalidate();
                            break;
                        }
                    case 8:
                        {
                            image_StartOptions = null;
                            imagePanel1.Invalidate();
                            break;
                        }
                }
                UpdateBrowsersPreview();
            }
        }
        // Change for all images
        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog Op = new OpenFileDialog();
            Op.Title = ls["Title_BrowseForImage"];
            Op.Filter = ls["Filter_Image"]; ;
            Op.Multiselect = false;
            if (Op.ShowDialog(this) == DialogResult.OK)
            {
                ManagedMessageBoxResult result =
                    ManagedMessageBox.ShowQuestionMessage(ls["Message_ThisWillChangeAllBrowserImagesToLoadedOneContinue"]);
                if (result.ClickedButtonIndex == 0)
                {
                    //Stream str = new FileStream(Op.FileName, FileMode.Open, FileAccess.Read);
                    // byte[] buff = new byte[str.Length];
                    // str.Read(buff, 0, (int)str.Length);
                    // str.Dispose();
                    //  str.Close();

                    //Bitmap loadedImage = (Bitmap)Image.FromStream(new MemoryStream(buff));
                    Bitmap loadedImage = null;

                    using (var bmpTemp = new Bitmap(Op.FileName))
                    {
                        loadedImage = new Bitmap(bmpTemp);
                    }

                    image_CategoriesBrowser = loadedImage;
                    image_ConsolesBrowser = loadedImage;
                    image_EmulatorsBrowser = loadedImage;
                    image_InformationContainerBrowser = loadedImage;
                    image_FiltersBrowser = loadedImage;
                    image_InformationContainerTabs = loadedImage;
                    image_PlaylistsBrowser = loadedImage;
                    image_RomsBrowser = loadedImage;
                    image_StartOptions = loadedImage;
                    UpdateBrowsersPreview();
                }
            }
        }
        // Clear all images
        private void button5_Click(object sender, EventArgs e)
        {
            ManagedMessageBoxResult result =
                    ManagedMessageBox.ShowQuestionMessage(ls["Message_ThisWillClearAllBrowserImagesContinue"]);
            if (result.ClickedButtonIndex == 0)
            {
                image_CategoriesBrowser = null;
                image_ConsolesBrowser = null;
                image_EmulatorsBrowser = null;
                image_InformationContainerBrowser = null;
                image_FiltersBrowser = null;
                image_InformationContainerTabs = null;
                image_PlaylistsBrowser = null;
                image_RomsBrowser = null;
                image_StartOptions = null;
                UpdateBrowsersPreview();
            }
        }
        // Change font
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            switch (listBox_images.SelectedIndex)
            {
                case 0:
                    {
                        FontDialog fontForm = new FontDialog();
                        fontForm.Font = (Font)fontConvert.ConvertFromString(font_CategoriesBrowser);
                        if (fontForm.ShowDialog(this) == DialogResult.OK)
                        {
                            font_CategoriesBrowser = fontConvert.ConvertToString(fontForm.Font);
                        }
                        break;
                    }
                case 1:
                    {
                        FontDialog fontForm = new FontDialog();
                        fontForm.Font = (Font)fontConvert.ConvertFromString(font_ConsolesBrowser);
                        if (fontForm.ShowDialog(this) == DialogResult.OK)
                        {
                            font_ConsolesBrowser = fontConvert.ConvertToString(fontForm.Font);
                        }
                        break;
                    }
                case 2:
                    {
                        FontDialog fontForm = new FontDialog();
                        fontForm.Font = (Font)fontConvert.ConvertFromString(font_EmulatorsBrowser);
                        if (fontForm.ShowDialog(this) == DialogResult.OK)
                        {
                            font_EmulatorsBrowser = fontConvert.ConvertToString(fontForm.Font);
                        }
                        break;
                    }
                case 3:
                    {
                        FontDialog fontForm = new FontDialog();
                        fontForm.Font = (Font)fontConvert.ConvertFromString(font_FiltersBrowser);
                        if (fontForm.ShowDialog(this) == DialogResult.OK)
                        {
                            font_FiltersBrowser = fontConvert.ConvertToString(fontForm.Font);
                        }
                        break;
                    }
                case 4:
                    {
                        FontDialog fontForm = new FontDialog();
                        fontForm.Font = (Font)fontConvert.ConvertFromString(font_InformationContainerBrowser);
                        if (fontForm.ShowDialog(this) == DialogResult.OK)
                        {
                            font_InformationContainerBrowser = fontConvert.ConvertToString(fontForm.Font);
                        }
                        break;
                    }
                case 5:
                    {
                        FontDialog fontForm = new FontDialog();
                        fontForm.Font = (Font)fontConvert.ConvertFromString(font_InformationContainerTabs);
                        if (fontForm.ShowDialog(this) == DialogResult.OK)
                        {
                            font_InformationContainerTabs = fontConvert.ConvertToString(fontForm.Font);
                        }
                        break;
                    }
                case 6:
                    {
                        FontDialog fontForm = new FontDialog();
                        fontForm.Font = (Font)fontConvert.ConvertFromString(font_PlaylistsBrowser);
                        if (fontForm.ShowDialog(this) == DialogResult.OK)
                        {
                            font_PlaylistsBrowser = fontConvert.ConvertToString(fontForm.Font);
                        }
                        break;
                    }
                case 7:
                    {
                        FontDialog fontForm = new FontDialog();
                        fontForm.Font = (Font)fontConvert.ConvertFromString(font_RomsBrowser);
                        if (fontForm.ShowDialog(this) == DialogResult.OK)
                        {
                            font_RomsBrowser = fontConvert.ConvertToString(fontForm.Font);
                        }
                        break;
                    }
                case 8:
                    {
                        FontDialog fontForm = new FontDialog();
                        fontForm.Font = (Font)fontConvert.ConvertFromString(font_StartOptions);
                        if (fontForm.ShowDialog(this) == DialogResult.OK)
                        {
                            font_StartOptions = fontConvert.ConvertToString(fontForm.Font);
                        }
                        break;
                    }
            }
            UpdateBrowsersPreview();
        }
        // Change browser text color
        private void button_browserTextColor_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.Color = ((Button)sender).BackColor;
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                ((Button)sender).BackColor = dialog.Color;
                ((Button)sender).Text = dialog.Color.ToArgb().ToString("X6");
                // Apply
                switch (listBox_images.SelectedIndex)
                {
                    case 0:
                        {
                            txtColor_CategoriesBrowser = dialog.Color;
                            break;
                        }
                    case 1:
                        {
                            txtColor_ConsolesBrowser = dialog.Color;
                            break;
                        }
                    case 2:
                        {
                            txtColor_EmulatorsBrowser = dialog.Color;
                            break;
                        }
                    case 3:
                        {
                            txtColor_FiltersBrowser = dialog.Color;
                            break;
                        }
                    case 4:
                        {
                            txtColor_InformationContainerBrowser = dialog.Color;
                            break;
                        }
                    case 5:
                        {
                            txtColor_InformationContainerTabs = dialog.Color;
                            break;
                        }
                    case 6:
                        {
                            txtColor_PlaylistsBrowser = dialog.Color;
                            break;
                        }
                    case 7:
                        {
                            txtColor_RomsBrowser = dialog.Color;
                            break;
                        }
                    case 8:
                        {
                            txtColor_StartOptions = dialog.Color;
                            break;
                        }
                }
                UpdateBrowsersPreview();
            }
        }
        // Change browser background color
        private void button_browserBackgroundColor_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.Color = ((Button)sender).BackColor;
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                ((Button)sender).BackColor = dialog.Color;
                ((Button)sender).Text = dialog.Color.ToArgb().ToString("X6");
                // Apply
                switch (listBox_images.SelectedIndex)
                {
                    case 0:
                        {
                            bkgColor_CategoriesBrowser = dialog.Color;
                            break;
                        }
                    case 1:
                        {
                            bkgColor_ConsolesBrowser = dialog.Color;
                            break;
                        }
                    case 2:
                        {
                            bkgColor_EmulatorsBrowser = dialog.Color;
                            break;
                        }
                    case 3:
                        {
                            bkgColor_FiltersBrowser = dialog.Color;
                            break;
                        }
                    case 4:
                        {
                            bkgColor_InformationContainerBrowser = dialog.Color;
                            break;
                        }
                    case 5:
                        {
                            bkgColor_InformationContainerTabs = dialog.Color;
                            break;
                        }
                    case 6:
                        {
                            bkgColor_PlaylistsBrowser = dialog.Color;
                            break;
                        }
                    case 7:
                        {
                            bkgColor_RomsBrowser = dialog.Color;
                            break;
                        }
                    case 8:
                        {
                            bkgColor_StartOptions = dialog.Color;
                            break;
                        }
                }
                UpdateBrowsersPreview();
            }
        }
        // Apply font and color to all browsers
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ManagedMessageBoxResult result =
                    ManagedMessageBox.ShowQuestionMessage(ls["Message_ThisWillApplyTheseFontAndColorsToAllBrowsersContinue"]);
            if (result.ClickedButtonIndex == 0)
            {
                font_CategoriesBrowser = textBox_browserFont.Text;
                font_ConsolesBrowser = textBox_browserFont.Text;
                font_EmulatorsBrowser = textBox_browserFont.Text;
                font_FiltersBrowser = textBox_browserFont.Text;
                font_InformationContainerBrowser = textBox_browserFont.Text;
                font_InformationContainerTabs = textBox_browserFont.Text;
                font_PlaylistsBrowser = textBox_browserFont.Text;
                font_RomsBrowser = textBox_browserFont.Text;
                font_StartOptions = textBox_browserFont.Text;
                bkgColor_CategoriesBrowser = button_browserBackgroundColor.BackColor;
                bkgColor_ConsolesBrowser = button_browserBackgroundColor.BackColor;
                bkgColor_EmulatorsBrowser = button_browserBackgroundColor.BackColor;
                bkgColor_FiltersBrowser = button_browserBackgroundColor.BackColor;
                bkgColor_InformationContainerBrowser = button_browserBackgroundColor.BackColor;
                bkgColor_InformationContainerTabs = button_browserBackgroundColor.BackColor;
                bkgColor_PlaylistsBrowser = button_browserBackgroundColor.BackColor;
                bkgColor_RomsBrowser = button_browserBackgroundColor.BackColor;
                bkgColor_StartOptions = button_browserBackgroundColor.BackColor;

                txtColor_CategoriesBrowser = button_browserTextColor.BackColor;
                txtColor_ConsolesBrowser = button_browserTextColor.BackColor;
                txtColor_EmulatorsBrowser = button_browserTextColor.BackColor;
                txtColor_FiltersBrowser = button_browserTextColor.BackColor;
                txtColor_InformationContainerBrowser = button_browserTextColor.BackColor;
                txtColor_InformationContainerTabs = button_browserTextColor.BackColor;
                txtColor_PlaylistsBrowser = button_browserTextColor.BackColor;
                txtColor_RomsBrowser = button_browserTextColor.BackColor;
                txtColor_StartOptions = button_browserTextColor.BackColor;
            }
        }
        // Change main window background color
        private void button_eoBackgroundColor_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.Color = ((Button)sender).BackColor;
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                ((Button)sender).BackColor = dialog.Color;
                ((Button)sender).Text = dialog.Color.ToArgb().ToString("X6");
                // Apply
                bkgColor_MainWindow = dialog.Color;
            }
        }
        // Change image mode
        private void comboBox_imageMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (listBox_images.SelectedIndex)
            {
                case 0:
                    {
                        imageMode_CategoriesBrowser = (BackgroundImageMode)comboBox_imageMode.SelectedIndex;
                        break;
                    }
                case 1:
                    {
                        imageMode_ConsolesBrowser = (BackgroundImageMode)comboBox_imageMode.SelectedIndex;
                        break;
                    }
                case 2:
                    {
                        imageMode_EmulatorsBrowser = (BackgroundImageMode)comboBox_imageMode.SelectedIndex;
                        break;
                    }
                case 3:
                    {
                        imageMode_FiltersBrowser = (BackgroundImageMode)comboBox_imageMode.SelectedIndex;
                        break;
                    }
                case 4:
                    {
                        imageMode_InformationContainerBrowser = (BackgroundImageMode)comboBox_imageMode.SelectedIndex;
                        break;
                    }
                case 5:
                    {
                        imageMode_InformationContainerTabs = (BackgroundImageMode)comboBox_imageMode.SelectedIndex;
                        break;
                    }
                case 6:
                    {
                        imageMode_PlaylistsBrowser = (BackgroundImageMode)comboBox_imageMode.SelectedIndex;
                        break;
                    }
                case 7:
                    {
                        imageMode_RomsBrowser = (BackgroundImageMode)comboBox_imageMode.SelectedIndex;
                        break;
                    }
                case 8:
                    {
                        imageMode_StartOptions = (BackgroundImageMode)comboBox_imageMode.SelectedIndex;
                        break;
                    }
            }
        }
        // Change main menu text color
        private void button_mainMenuTextColor_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.Color = ((Button)sender).BackColor;
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                ((Button)sender).BackColor = dialog.Color;
                ((Button)sender).Text = dialog.Color.ToArgb().ToString("X6");
                // Apply
                txtColor_MainMenu = dialog.Color;
            }
        }
    }
}
