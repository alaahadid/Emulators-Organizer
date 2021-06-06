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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services;
using MMB;
namespace EmulatorsOrganizer.GUI
{
    public partial class Form_AddFilter : Form
    {
        public Form_AddFilter()
        {
            InitializeComponent();
            RefreshModeCombobox();
            comboBox_condition.SelectedIndex = 0;
        }
        public Form_AddFilter(Filter filter)
        {
            InitializeComponent();
            this.Text = ls["Title_EditFilter"] + ": " + filter.Name;
            RefreshModeCombobox();
            if (filter.Parameters.IsDataItem)
                comboBox_searchBy.SelectedItem = filter.Parameters.DataItemName;
            else
                comboBox_searchBy.SelectedIndex = (int)filter.Parameters.SearchMode;
            if (comboBox_searchBy.SelectedIndex < 0)
                comboBox_searchBy.SelectedIndex = 0;

            textBox_filterName.Text = filter.Name;
            textBox1.Text = filter.Parameters.SearchWhat;
            checkBox_caseSensitive.Checked = filter.Parameters.CaseSensitive;
            if (IsNumberSelection)
                comboBox_condition.SelectedIndex = (int)filter.Parameters.ConditionForNumber;
            else
                comboBox_condition.SelectedIndex = (int)filter.Parameters.ConditionForText;
        }

        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private ProfileManager profileManager = (ProfileManager)ServicesManager.GetService("Profile Manager");
        private Dictionary<string, RomDataType> dataItems = new Dictionary<string, RomDataType>();
        private bool IsNumberSelection;
        public event EventHandler<AddFilterArgs> OkPressed;

        public string FilterName
        { get { return textBox_filterName.Text; } }
        public SearchRequestArgs FilterParameters
        {
            get
            {
                SearchMode mode = SearchMode.Name;
                TextSearchCondition textCondition = TextSearchCondition.None;
                NumberSearchCondition numberCondition = NumberSearchCondition.None;
                bool isNumber = false;
                bool isDataItem = false;
                string dataItemName = "";
                switch (comboBox_searchBy.SelectedIndex)
                {
                    case 0: mode = SearchMode.Name; isNumber = false; break;
                    case 1: mode = SearchMode.FileType; isNumber = false; break;
                    case 2: mode = SearchMode.FilePath; isNumber = false; break;
                    case 3: mode = SearchMode.Size; isNumber = true; break;
                    case 4: mode = SearchMode.PlayedTimes; isNumber = true; break;
                    case 5: mode = SearchMode.LastPlayed; isNumber = true; break;
                    case 6: mode = SearchMode.PlayTime; isNumber = true; break;
                    case 7: mode = SearchMode.Rating; isNumber = true; break;
                    default:
                        {
                            isDataItem = true;
                            // Get data item name
                            dataItemName = comboBox_searchBy.Items[comboBox_searchBy.SelectedIndex].ToString();
                            // Determine the type
                            isNumber = dataItems[dataItemName] == RomDataType.Number;
                            break;
                        }
                }
                if (!isNumber)
                {
                    switch (comboBox_condition.SelectedIndex)
                    {
                        case 0: textCondition = TextSearchCondition.Contains; break;
                        case 1: textCondition = TextSearchCondition.DoesNotContain; break;
                        case 2: textCondition = TextSearchCondition.Is; break;
                        case 3: textCondition = TextSearchCondition.IsNot; break;
                        case 4: textCondition = TextSearchCondition.StartWith; break;
                        case 5: textCondition = TextSearchCondition.DoesNotStartWith; break;
                        case 6: textCondition = TextSearchCondition.EndWith; break;
                        case 7: textCondition = TextSearchCondition.DoesNotEndWith; break;
                    }
                }
                else
                {
                    switch (comboBox_condition.SelectedIndex)
                    {
                        case 0: numberCondition = NumberSearchCondition.Equal; break;
                        case 1: numberCondition = NumberSearchCondition.DoesNotEqual; break;
                        case 2: numberCondition = NumberSearchCondition.Larger; break;
                        case 3: numberCondition = NumberSearchCondition.EuqalLarger; break;
                        case 4: numberCondition = NumberSearchCondition.Smaller; break;
                        case 5: numberCondition = NumberSearchCondition.EqualSmaller; break;
                    }
                }
                if (!isDataItem)
                {
                    return new SearchRequestArgs(textBox1.Text, mode,
                       textCondition, numberCondition, checkBox_caseSensitive.Checked);
                }
                else
                {
                    return new SearchRequestArgs(textBox1.Text, dataItemName,
                       textCondition, numberCondition, checkBox_caseSensitive.Checked);
                }
            }
        }

        private void RefreshModeCombobox()
        {
            #region Get Data Items
            // Let's see what is the selection
            switch (profileManager.Profile.RecentSelectedType)
            {
                case SelectionType.Console:
                    {
                        Core.Console console = profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID];
                        if (console.Filters == null)
                            console.Filters = new List<Filter>();
                        dataItems = new Dictionary<string, RomDataType>();
                        if (console.RomDataInfoElements != null)
                        {
                            foreach (RomData d in console.RomDataInfoElements)
                            {
                                dataItems.Add(d.Name, d.Type);
                            }
                        }
                        break;
                    }
                case SelectionType.ConsolesGroup:
                    {
                        ConsolesGroup gr = profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID];
                        if (gr.Filters == null)
                            gr.Filters = new List<Filter>();
                        // Add data infos ...
                        dataItems = new Dictionary<string, RomDataType>();
                        Core.Console[] consoles = profileManager.Profile.Consoles[gr.ID, false];
                        foreach (Core.Console c in consoles)
                        {
                            if (c.RomDataInfoElements != null)
                            {
                                foreach (RomData d in c.RomDataInfoElements)
                                {
                                    if (!dataItems.Keys.Contains(d.Name))
                                    { dataItems.Add(d.Name, d.Type); }
                                }
                            }
                        }
                        break;
                    }
                case SelectionType.PlaylistsGroup:
                    {
                        // Get console
                        PlaylistsGroup plg = profileManager.Profile.PlaylistGroups[profileManager.Profile.SelectedPlaylistsGroupID];
                        Playlist[] pls = profileManager.Profile.Playlists[plg.ID, false];
                        dataItems = new Dictionary<string, RomDataType>();
                        foreach (Playlist pl in pls)
                        {
                            if (pl.Filters == null)
                                pl.Filters = new List<Filter>();

                            Rom[] roms = profileManager.Profile.Roms[pl.RomIDS.ToArray()];
                            string preciousParent = "";
                            foreach (Rom r in roms)
                            {
                                // get parent console
                                if (r.ParentConsoleID != preciousParent)
                                {
                                    Core.Console pcon = profileManager.Profile.Consoles[r.ParentConsoleID];
                                    foreach (RomData d in pcon.RomDataInfoElements)
                                    {
                                        if (!dataItems.Keys.Contains(d.Name))
                                        { dataItems.Add(d.Name, d.Type); }
                                    }
                                    preciousParent = r.ParentConsoleID;
                                }
                            }
                        }
                        break;
                    }
                case SelectionType.Playlist:
                    {
                        // Get console
                        Playlist pl = profileManager.Profile.Playlists[profileManager.Profile.SelectedPlaylistID];
                        if (pl.Filters == null)
                            pl.Filters = new List<Filter>();
                        dataItems = new Dictionary<string, RomDataType>();
                        Rom[] roms = profileManager.Profile.Roms[pl.RomIDS.ToArray()];
                        string preciousParent = "";
                        foreach (Rom r in roms)
                        {
                            // get parent console
                            if (r.ParentConsoleID != preciousParent)
                            {
                                Core.Console pcon = profileManager.Profile.Consoles[r.ParentConsoleID];
                                foreach (RomData d in pcon.RomDataInfoElements)
                                {
                                    if (!dataItems.Keys.Contains(d.Name))
                                    { dataItems.Add(d.Name, d.Type); }
                                }
                                preciousParent = r.ParentConsoleID;
                            }
                        }
                        break;
                    }
            }
            #endregion
            comboBox_searchBy.Items.Clear();
            comboBox_searchBy.Items.Add(ls["SearchMode_Name"]);
            comboBox_searchBy.Items.Add(ls["SearchMode_FileType"]);
            comboBox_searchBy.Items.Add(ls["SearchMode_FilePath"]);
            comboBox_searchBy.Items.Add(ls["SearchMode_Size"]);
            comboBox_searchBy.Items.Add(ls["SearchMode_PlayedTimes"]);
            comboBox_searchBy.Items.Add(ls["SearchMode_LastPlayed"]);
            comboBox_searchBy.Items.Add(ls["SearchMode_PlayTime"]);
            comboBox_searchBy.Items.Add(ls["SearchMode_Rating"]);
            // Add the data items ...
            foreach (string key in dataItems.Keys)
            {
                comboBox_searchBy.Items.Add(key);
            }
            comboBox_searchBy.SelectedIndex = 0;
        }
        // Cancel
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
        // Ok
        private void button1_Click(object sender, EventArgs e)
        {
            // Check ...
            if (textBox_filterName.Text.Length == 0)
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseEnterTheFilterName"], ls["MessageCaption_AddFilter"]);
                return;
            }
            if (textBox1.Text.Length == 0)
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseEnterTheFilterWord"], ls["MessageCaption_AddFilter"]);
                return;
            }
            AddFilterArgs args = new AddFilterArgs();
            if (OkPressed != null)
                OkPressed(this, args);
            if (args.Cancel)
                return;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void comboBox_searchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool _temp = IsNumberSelection;
            switch (comboBox_searchBy.SelectedIndex)
            {
                case 0: IsNumberSelection = false; break;
                case 1: IsNumberSelection = false; break;
                case 2: IsNumberSelection = false; break;
                case 3: IsNumberSelection = true; break;
                case 4: IsNumberSelection = true; break;
                case 5: IsNumberSelection = true; break;
                case 6: IsNumberSelection = true; break;
                case 7: IsNumberSelection = true; break;
                default:
                    {
                        // Get data item name
                        string dataItemName = comboBox_searchBy.Items[comboBox_searchBy.SelectedIndex].ToString();
                        // Determine the type
                        IsNumberSelection = dataItems[dataItemName] == RomDataType.Number;
                        break;
                    }
            }
            if (_temp != IsNumberSelection)
            {
                // Refresh condition combobox
                comboBox_condition.Items.Clear();
                if (!IsNumberSelection)
                {
                    comboBox_condition.Items.Add(ls["SearchCondition_Contains"]);
                    comboBox_condition.Items.Add(ls["SearchCondition_DoesNotContain"]);
                    comboBox_condition.Items.Add(ls["SearchCondition_Is"]);
                    comboBox_condition.Items.Add(ls["SearchCondition_IsNot"]);
                    comboBox_condition.Items.Add(ls["SearchCondition_StartWith"]);
                    comboBox_condition.Items.Add(ls["SearchCondition_DoesNotStartWith"]);
                    comboBox_condition.Items.Add(ls["SearchCondition_EndWith"]);
                    comboBox_condition.Items.Add(ls["SearchCondition_DoesNotEndWith"]);
                }
                else
                {
                    comboBox_condition.Items.Add(ls["SearchCondition_Equal"]);
                    comboBox_condition.Items.Add(ls["Searchcondition_DoesNotEqual"]);
                    comboBox_condition.Items.Add(ls["SearchCondition_Larger"]);
                    comboBox_condition.Items.Add(ls["SearchCondition_EuqalLarger"]);
                    comboBox_condition.Items.Add(ls["SearchCondition_Smaller"]);
                    comboBox_condition.Items.Add(ls["Searchcondition_EqualSmaller"]);
                }
                comboBox_condition.SelectedIndex = 0;
            }
        }
    }
    public class AddFilterArgs : EventArgs
    {
        public AddFilterArgs()
        {
        }
        private bool cancel;
        /// <summary>
        /// Get or set if should cancel
        /// </summary>
        public bool Cancel { get { return cancel; } set { cancel = value; } }
    }
}
