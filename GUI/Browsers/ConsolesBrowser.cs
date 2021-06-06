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
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Data;
using ClosedXML.Excel;
using System.Data.OleDb;
using System.Runtime.Serialization.Formatters.Binary;
using EmulatorsOrganizer.Core;
using EmulatorsOrganizer.Services.DefaultServices.Settings;
using MMB;

namespace EmulatorsOrganizer.GUI
{
    public partial class ConsolesBrowser : IBrowserControl
    {
        public ConsolesBrowser()
        {
            InitializeComponent();
            base.InitializeEvents();
            //    InitializeEvents();
            // TODO: don't forget the download games from db
            detectAndDownloadFromTheGamesDBnetToolStripMenuItem.Visible = false;
        }

        private bool isRefreshing = false;
        private int currentlySelectedNodeIndex;
        private int currentlySelectedNodeParentIndex;
        private bool consoleGroupsAtoZ = false;
        private bool consolesAtoZ = false;
        private bool canRename = false;
        private bool canDoDragDrop = false;
        private bool doubleClickActiveSelection;

        public override void InitializeEvents()
        {
            profileManager.Profile.ConoslesGroupAdded += Profile_ConoslesGroupAdded;
            profileManager.Profile.ConoslesGroupRemoved += Profile_ConoslesGroupRemoved;
            profileManager.Profile.ConoslesGroupsCleared += Profile_ConoslesGroupsCleared;
            profileManager.Profile.ConoslesGroupsMoved += Profile_ConoslesGroupsMoved;
            profileManager.Profile.ConsoleAdded += Profile_ConosleAdded;
            profileManager.Profile.ConsoleRemoved += Profile_ConosleRemoved;
            profileManager.Profile.ConoslesCleared += Profile_ConoslesCleared;
            profileManager.Profile.ConsoleMoved += Profile_ConsoleMoved;
            profileManager.Profile.ConoslesGroupsSorted += Profile_ConoslesGroupsSorted;
            profileManager.Profile.ConsolesSorted += Profile_ConsolesSorted;
            profileManager.Profile.ConsolePropertiesChanged += Profile_ConsolePropertiesChanged;
            profileManager.Profile.ConsolesGroupPropertiesChanged += Profile_ConsolePropertiesChanged;
            profileManager.Profile.ProfileCleanUpFinished += Profile_ProfileCleanUpFinished;
        }
        public override void DisposeEvents()
        {
            profileManager.Profile.ConoslesGroupAdded -= Profile_ConoslesGroupAdded;
            profileManager.Profile.ConoslesGroupRemoved -= Profile_ConoslesGroupRemoved;
            profileManager.Profile.ConoslesGroupsCleared -= Profile_ConoslesGroupsCleared;
            profileManager.Profile.ConoslesGroupsMoved -= Profile_ConoslesGroupsMoved;
            profileManager.Profile.ConsoleAdded -= Profile_ConosleAdded;
            profileManager.Profile.ConsoleRemoved -= Profile_ConosleRemoved;
            profileManager.Profile.ConoslesCleared -= Profile_ConoslesCleared;
            profileManager.Profile.ConsoleMoved -= Profile_ConsoleMoved;
            profileManager.Profile.ConoslesGroupsSorted -= Profile_ConoslesGroupsSorted;
            profileManager.Profile.ConsolesSorted -= Profile_ConsolesSorted;
            profileManager.Profile.ConsolePropertiesChanged -= Profile_ConsolePropertiesChanged;
            profileManager.Profile.ConsolesGroupPropertiesChanged -= Profile_ConsolePropertiesChanged;
            profileManager.Profile.ProfileCleanUpFinished -= Profile_ProfileCleanUpFinished;
        }
        public override void ApplyStyle(EOStyle style)
        {
            base.ApplyStyle(style);
            this.BackColor = style.bkgColor_ConsolesBrowser;
            this.BackgroundImage = style.image_ConsolesBrowser;
            switch (style.imageMode_ConsolesBrowser)
            {
                case BackgroundImageMode.NormalStretchNoAspectRatio: this.treeView1.BackgroundImageMode = ImageViewMode.Normal; break;
                case BackgroundImageMode.StretchIfLarger: this.treeView1.BackgroundImageMode = ImageViewMode.StretchIfLarger; break;
                case BackgroundImageMode.StretchToFit: this.treeView1.BackgroundImageMode = ImageViewMode.StretchToFit; break;
            }
            treeView1.ForeColor = style.txtColor_ConsolesBrowser;
            // Font
            try
            {
                FontConverter conv = new FontConverter();
                treeView1.Font = (Font)conv.ConvertFromString(style.font_ConsolesBrowser);
            }
            catch { }
            treeView1.CalculateBackgroundBounds();
        }
        public void AddConsole()
        {
            string parentID = "";
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode is TreeNodeConsolesGroup)
                {
                    parentID = ((TreeNodeConsolesGroup)treeView1.SelectedNode).ID;
                }
                else if (treeView1.SelectedNode is TreeNodeConsole)
                {
                    if (treeView1.SelectedNode.Parent != null)
                        parentID = ((TreeNodeConsolesGroup)treeView1.SelectedNode.Parent).ID;
                }
            }
            profileManager.Profile.AddConsole(parentID);
        }
        public void AddRootConsole()
        {
            treeView1.SelectedNode = null;
            AddConsole();
        }
        public void AddConsoleStepStep()
        {
            // 1 Add the console, save the new id.
            string newConsoleID = profileManager.Profile.AddConsole("");
            // 2 Show console properties !
            ConsoleProperties conProperties = new ConsoleProperties(newConsoleID, ls["Title_General"]);
            if (conProperties.ShowDialog(this) == DialogResult.OK)
            {
                // 3 Add roms
                Form_AddRomsFolderScan frmAddRoms = new Form_AddRomsFolderScan(newConsoleID);
                if (frmAddRoms.ShowDialog(this) == DialogResult.OK)
                {
                    // 4 Add emulator
                    Form_AddEmulator frmAddEmulator = new Form_AddEmulator(false, newConsoleID);
                    frmAddEmulator.ShowDialog(this);

                    // 5 Detect tabs !
                    Form_DetectICSWizard frmDetect = new Form_DetectICSWizard(newConsoleID);
                    frmDetect.ShowDialog(this);

                    // 6 Show done message
                    ManagedMessageBox.ShowMessage(ls["Status_Done"]);
                }
            }
            else
            {
                // Remove the added console :(
                profileManager.Profile.Consoles.Remove(newConsoleID);
            }
        }
        public void AddMameConsole()
        {
            Form_AddMameConsole frm = new Form_AddMameConsole();
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                // 2 Show console properties !
                ConsoleProperties conProperties = new ConsoleProperties(frm.NewConsoleID, ls["Title_General"]);
                if (conProperties.ShowDialog(this) == DialogResult.OK)
                {
                    // 3 Add emulator
                    Form_AddEmulator frmAddEmulator = new Form_AddEmulator(false, frm.NewConsoleID);
                    frmAddEmulator.ShowDialog(this);

                    // 4 Detect tabs !
                    Form_DetectICSWizard frmDetect = new Form_DetectICSWizard(frm.NewConsoleID);
                    frmDetect.ShowDialog(this);

                    // 5 Show done message
                    ManagedMessageBox.ShowMessage(ls["Status_Done"]);
                }
            }
        }
        public void AddRomsFolderScan()
        {
            if (HighlightedConsoleID == "")
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseSelectConsoleToAddRoms"], ls["MessageCaption_AddRoms"]);
                return;
            }
            Form_AddRomsFolderScan frm = new Form_AddRomsFolderScan(HighlightedConsoleID);
            frm.ShowDialog(this);
        }
        public void AddRoms()
        {
            if (HighlightedConsoleID == "")
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseSelectConsoleToAddRoms"], ls["MessageCaption_AddRoms"]);
                return;
            }
            Form_AddRoms frm = new Form_AddRoms(HighlightedConsoleID);
            frm.ShowDialog(this);
        }
        public override void LoadControlSettings()
        {
            // Load basic settings
            trackBar_zoom.Value = (int)settings.GetValue("ConsolesBrowser:ZoomValue", true, 16);
            doubleClickActiveSelection = (bool)settings.GetValue("ConsolesBrowser:DoubleClick", true, true);
            trackBar_zoom_Scroll(this, null);
        }
        public override void DeleteSelected()
        {
            if (treeView1.SelectedNode == null)
            { OnEnableDisableButtons(); return; }
            if (treeView1.SelectedNode is TreeNodeConsolesGroup)
            {
                TreeNodeConsolesGroup node = (TreeNodeConsolesGroup)treeView1.SelectedNode;
                ManagedMessageBoxResult result = ManagedMessageBox.ShowQuestionMessage(ls["Message_AreYouSureYouWantToDeleteConsolesGroup"],
                    ls["MessageCaption_DeleteConsolesGroup"]);
                if (result.ClickedButtonIndex == 0)// yes
                {
                    if (profileManager.Profile.RecentSelectedType == SelectionType.ConsolesGroup)
                    {
                        if (node.ID == profileManager.Profile.SelectedConsolesGroupID)
                        {
                            profileManager.Profile.RecentSelectedType = SelectionType.None;
                        }
                    }
                    profileManager.Profile.ConsoleGroups.Remove(node.ID);
                }
            }
            else if (treeView1.SelectedNode is TreeNodeConsole)
            {
                TreeNodeConsole node = (TreeNodeConsole)treeView1.SelectedNode;
                ManagedMessageBoxResult result = ManagedMessageBox.ShowQuestionMessage(ls["Message_AreYouSureYouWantToDeleteConsole"],
                    ls["MessageCaption_DeleteConsole"]);
                if (result.ClickedButtonIndex == 0)// yes
                {
                    if (profileManager.Profile.RecentSelectedType == SelectionType.Console)
                    {
                        if (node.ID == profileManager.Profile.SelectedConsoleID)
                        {
                            profileManager.Profile.RecentSelectedType = SelectionType.None;
                        }
                    }
                    profileManager.Profile.Consoles.Remove(node.ID);
                }
            }
            OnEnableDisableButtons();
        }
        public override void RenameSelected()
        {
            if (treeView1.SelectedNode == null)
                return;
            treeView1.SelectedNode.BeginEdit();
        }
        public void RefreshConsoleGroups()
        {
            if (isRefreshing) return;
            isRefreshing = true;
            treeView1.Nodes.Clear();
            if (profileManager.Profile != null)
            {
                foreach (ConsolesGroup gr in profileManager.Profile.ConsoleGroups)
                {
                    TreeNodeConsolesGroup node = new TreeNodeConsolesGroup(gr.ID);
                    node.RefreshNodes();
                    treeView1.Nodes.Add(node);
                }
            }
            isRefreshing = false;
            RefreshConsoles();
        }
        public void RefreshConsoles()
        {
            if (isRefreshing) return;
            isRefreshing = true;
            // Delete consoles
            for (int i = 0; i < treeView1.Nodes.Count; i++)
            {
                if (treeView1.Nodes[i] is TreeNodeConsole)
                {
                    treeView1.Nodes.RemoveAt(i);
                    i--;
                }
                else if (treeView1.Nodes[i] is TreeNodeConsolesGroup)
                {
                    ((TreeNodeConsolesGroup)treeView1.Nodes[i]).RefreshNodes();
                }
            }
            if (profileManager.Profile != null)
            {
                // Add consoles that have no parent id
                foreach (Core.Console con in profileManager.Profile.Consoles)
                {
                    if (con.ParentGroupID == "")
                    {
                        TreeNodeConsole node = new TreeNodeConsole(con.ID);
                        node.RefreshNodes();
                        treeView1.Nodes.Add(node);
                    }
                }
            }
            isRefreshing = false;
        }
        public void MoveSelectedItemUp()
        {
            if (treeView1.SelectedNode == null)
            { OnEnableDisableButtons(); return; }
            if (treeView1.SelectedNode is TreeNodeConsolesGroup)
            {
                // Get current consoles group index
                int index = profileManager.Profile.ConsoleGroups.IndexOf(((TreeNodeConsolesGroup)treeView1.SelectedNode).ID);
                int newIndex = index - 1;
                if (newIndex < 0)
                    return;
                currentlySelectedNodeIndex = newIndex;
                profileManager.Profile.ConsoleGroups.Move(index, newIndex);
            }
            else if (treeView1.SelectedNode is TreeNodeConsole)
            {
                // Get parent consoles group id
                string parentID = "";
                if (treeView1.SelectedNode.Parent != null)
                    parentID = ((TreeNodeConsolesGroup)treeView1.SelectedNode.Parent).ID;
                // Get all consoles that belong to this parent
                ConsolesCollection collection = new ConsolesCollection(null, profileManager.Profile.Consoles[parentID, false]);
                // Get index of current item
                int tempIndex = collection.IndexOf(((TreeNodeConsole)treeView1.SelectedNode).ID);
                int index = profileManager.Profile.Consoles.IndexOf(collection[((TreeNodeConsole)treeView1.SelectedNode).ID]);
                tempIndex = tempIndex - 1;
                if (tempIndex < 0) return;
                int newIndex = profileManager.Profile.Consoles.IndexOf(collection[tempIndex].ID);// index of previous item
                if (newIndex < 0) return;
                if (newIndex >= profileManager.Profile.Consoles.Count) return;
                if (treeView1.SelectedNode.Parent != null)
                {
                    currentlySelectedNodeParentIndex = treeView1.Nodes.IndexOf(treeView1.SelectedNode.Parent);
                    currentlySelectedNodeIndex = treeView1.SelectedNode.Parent.Nodes.IndexOf(treeView1.SelectedNode) - 1;
                }
                else
                {
                    currentlySelectedNodeParentIndex = -1;
                    currentlySelectedNodeIndex = treeView1.Nodes.IndexOf(treeView1.SelectedNode) - 1;
                }
                // Move the console within collection
                profileManager.Profile.Consoles.Move(index, newIndex);
            }
            OnEnableDisableButtons();
        }
        public void MoveSelectedItemDown()
        {
            if (treeView1.SelectedNode == null)
            { OnEnableDisableButtons(); return; }
            if (treeView1.SelectedNode is TreeNodeConsolesGroup)
            {
                // Get current consoles group index
                int index = profileManager.Profile.ConsoleGroups.IndexOf(((TreeNodeConsolesGroup)treeView1.SelectedNode).ID);
                int newIndex = index + 1;
                if (newIndex > profileManager.Profile.ConsoleGroups.Count - 1)
                    return;
                currentlySelectedNodeIndex = newIndex;
                profileManager.Profile.ConsoleGroups.Move(index, newIndex);
            }
            else if (treeView1.SelectedNode is TreeNodeConsole)
            {
                // Get parent consoles group id
                string parentID = "";
                if (treeView1.SelectedNode.Parent != null)
                    parentID = ((TreeNodeConsolesGroup)treeView1.SelectedNode.Parent).ID;
                // Get all consoles that belong to this parent
                ConsolesCollection collection = new ConsolesCollection(null, profileManager.Profile.Consoles[parentID, false]);
                // Get index of current item
                int tempIndex = collection.IndexOf(((TreeNodeConsole)treeView1.SelectedNode).ID);
                int index = profileManager.Profile.Consoles.IndexOf(collection[((TreeNodeConsole)treeView1.SelectedNode).ID]);
                tempIndex = tempIndex + 1;
                if (tempIndex > collection.Count - 1) return;
                int newIndex = profileManager.Profile.Consoles.IndexOf(collection[tempIndex].ID);// index of next item
                if (newIndex > profileManager.Profile.Consoles.Count - 1) return;
                if (treeView1.SelectedNode.Parent != null)
                {
                    currentlySelectedNodeParentIndex = treeView1.Nodes.IndexOf(treeView1.SelectedNode.Parent);
                    currentlySelectedNodeIndex = treeView1.SelectedNode.Parent.Nodes.IndexOf(treeView1.SelectedNode) + 1;
                }
                else
                {
                    currentlySelectedNodeParentIndex = -1;
                    currentlySelectedNodeIndex = treeView1.Nodes.IndexOf(treeView1.SelectedNode) + 1;
                }
                // Move the console within collection
                profileManager.Profile.Consoles.Move(index, newIndex);
            }
            OnEnableDisableButtons();
        }
        public void SortConsolesGroupByName()
        {
            profileManager.Profile.ConsoleGroups.Sort(new ConsoleGroupsComparer(consoleGroupsAtoZ, ConsoleGroupsCompareType.Name));
            consoleGroupsAtoZ = !consoleGroupsAtoZ;
            OnEnableDisableButtons();
        }
        public void SortRootConsolesByName()
        {
            currentlySelectedNodeParentIndex = -1;
            // Get all consoles of no-parent id
            ConsolesCollection collection = new ConsolesCollection(null, profileManager.Profile.Consoles["", false]);
            // Remove them from original collection
            profileManager.Profile.Consoles.RemoveItems(collection);
            // Sort them
            collection.Sort(new ConsolesComparer(consolesAtoZ, ConsoleCompareType.Name));
            consolesAtoZ = !consolesAtoZ;
            // Re-add add them
            profileManager.Profile.Consoles.AddRange(collection);
            // Raise event
            profileManager.Profile.OnConsolesSort();
            OnEnableDisableButtons();
        }
        public void SortConsolesByName()
        {
            if (treeView1.SelectedNode == null)
            {
                SortRootConsolesByName();
            }
            else
            {
                string parentid = "";
                currentlySelectedNodeParentIndex = -1;
                if (treeView1.SelectedNode is TreeNodeConsolesGroup)
                {
                    currentlySelectedNodeParentIndex = treeView1.Nodes.IndexOf(treeView1.SelectedNode);
                    parentid = ((TreeNodeConsolesGroup)treeView1.SelectedNode).ID;
                }
                else
                {
                    if (treeView1.SelectedNode.Parent != null)
                    {
                        currentlySelectedNodeParentIndex = treeView1.Nodes.IndexOf(treeView1.SelectedNode.Parent);
                        parentid = ((TreeNodeConsolesGroup)treeView1.SelectedNode.Parent).ID;
                    }
                }
                // Get all consoles of no-parent id
                ConsolesCollection collection = new ConsolesCollection(null, profileManager.Profile.Consoles[parentid, false]);
                // Remove them from original collection
                profileManager.Profile.Consoles.RemoveItems(collection);
                // Sort them
                collection.Sort(new ConsolesComparer(consolesAtoZ, ConsoleCompareType.Name));
                consolesAtoZ = !consolesAtoZ;
                // Re-add add them
                profileManager.Profile.Consoles.AddRange(collection);
                // Raise event
                profileManager.Profile.OnConsolesSort();
            }
            OnEnableDisableButtons();
        }
        public override void ChangeIcon()
        {
            if (treeView1.SelectedNode == null)
                return;
            IEOElement element;
            if (treeView1.SelectedNode is TreeNodeConsolesGroup)
                element = profileManager.Profile.ConsoleGroups[((TreeNodeConsolesGroup)treeView1.SelectedNode).ID];
            else if (treeView1.SelectedNode is TreeNodeConsole)
                element = profileManager.Profile.Consoles[((TreeNodeConsole)treeView1.SelectedNode).ID];
            else
                return;

            OpenFileDialog Op = new OpenFileDialog();
            Op.Title = ls["Title_BrowseForAnIcon"];
            Op.Filter = ls["Filter_Icon"]; ;
            Op.Multiselect = false;
            if (Op.ShowDialog(this) == DialogResult.OK)
            {
                if (Path.GetExtension(Op.FileName).ToLower() == ".exe" | Path.GetExtension(Op.FileName).ToLower() == ".ico")
                { element.Icon = Icon.ExtractAssociatedIcon(Op.FileName).ToBitmap(); }
                else
                {
                    //Stream str = new FileStream(Op.FileName, FileMode.Open, FileAccess.Read);
                    //byte[] buff = new byte[str.Length];
                    //str.Read(buff, 0, (int)str.Length);
                    //str.Dispose();
                    //str.Close();

                    //element.Icon = (Bitmap)Image.FromStream(new MemoryStream(buff));
                    using (var bmpTemp = new Bitmap(Op.FileName))
                    {
                        element.Icon = new Bitmap(bmpTemp);
                    }
                }
                profileManager.Profile.OnElementIconChanged(element);
                treeView1.Invalidate();
            }
        }
        public override void ClearIcon()
        {
            base.ClearIcon();
            if (treeView1.SelectedNode == null)
                return;
            IEOElement element;
            string name = "";
            if (treeView1.SelectedNode is TreeNodeConsolesGroup)
            {
                element = profileManager.Profile.ConsoleGroups[((TreeNodeConsolesGroup)treeView1.SelectedNode).ID];
                name = ls["Status_ConsolesGroup"];
            }
            else if (treeView1.SelectedNode is TreeNodeConsole)
            {
                element = profileManager.Profile.Consoles[((TreeNodeConsole)treeView1.SelectedNode).ID];
                name = ls["Name_Console"];
            }
            else
                return;
            ManagedMessageBoxResult resul = ManagedMessageBox.ShowQuestionMessage(
                ls["Message_AreYouSureYouWantToClearIconsForSelectedItems"],
                ls["MessageCaption_ClearIcon"] + " " + ls["Word_for"] + " " + element.Name + " " + name);
            if (resul.ClickedButtonIndex == 0)
            {
                element.Icon = null;

                profileManager.Profile.OnElementIconChanged(element);
                treeView1.Invalidate();
            }
        }
        public override void ShowItemProperties()
        {
            if (treeView1.SelectedNode == null) return;

            currentlySelectedNodeParentIndex = -1;
            if (treeView1.SelectedNode.Parent != null)
                currentlySelectedNodeParentIndex = treeView1.SelectedNode.Parent.Index;
            currentlySelectedNodeIndex = treeView1.SelectedNode.Index;

            if (treeView1.SelectedNode is TreeNodeConsole)
            {
                ConsoleProperties frm = new ConsoleProperties(((TreeNodeConsole)treeView1.SelectedNode).ID, ls["Title_General"]);
                frm.ShowDialog();
            }
            else if (treeView1.SelectedNode is TreeNodeConsolesGroup)
            {
                ConsolesGroupProperties frm = new ConsolesGroupProperties(((TreeNodeConsolesGroup)treeView1.SelectedNode).ID);
                frm.ShowDialog();
            }
        }
        public void ActiveSelection()
        {
            if (treeView1.SelectedNode == null)
            {
                object _defaultStyle = settings.GetValue("Default Style", true, new EOStyle());
                treeView1.Invalidate();
                profileManager.Profile.RecentSelectedType = SelectionType.None;
                return;
            }
            if (treeView1.SelectedNode is TreeNodeConsolesGroup)
            {
                profileManager.Profile.RecentSelectedType = SelectionType.ConsolesGroup;
                profileManager.Profile.SelectedConsolesGroupID = ((TreeNodeConsolesGroup)treeView1.SelectedNode).ID;
                ApplyStyle(profileManager.Profile.ConsoleGroups[profileManager.Profile.SelectedConsolesGroupID].Style);
            }
            else if (treeView1.SelectedNode is TreeNodeConsole)
            {
                profileManager.Profile.RecentSelectedType = SelectionType.Console;
                profileManager.Profile.SelectedConsoleID = ((TreeNodeConsole)treeView1.SelectedNode).ID;
                ApplyStyle(profileManager.Profile.Consoles[profileManager.Profile.SelectedConsoleID].Style);
            }
            profileManager.Profile.OnEmulatorsRefreshRequest(new string[0]);
            treeView1.CalculateBackgroundBounds();
            OnEnableDisableButtons();
        }
        public void ExportSelectedConsole()
        {
            if (treeView1.SelectedNode == null)
                return;
            if (HighlightedConsoleID == "")
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_PleaseSelectConsoleFirst"], ls["MessageCaption_ExportConsole"]);
                return;
            }
            // Show options
            Form_ExportConsoleToFile frm = new Form_ExportConsoleToFile();
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                SaveFileDialog sav = new SaveFileDialog();
                sav.Title = ls["MessageCaption_ExportConsole"];
                sav.Filter = ls["Filter_ConsoleFile"];
                if (sav.ShowDialog(this) == DialogResult.OK)
                {
                    ConsoleFile cfile = new ConsoleFile();
                    cfile.Console = profileManager.Profile.Consoles[HighlightedConsoleID];
                    if (frm.checkBox_includeRoms.Checked)
                        cfile.Roms = profileManager.Profile.Roms[HighlightedConsoleID, false];
                    try
                    {
                        Trace.WriteLine("Saving console ..", "Profile Manager");
                        FileStream fs = new FileStream(sav.FileName, FileMode.Create, FileAccess.Write);
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(fs, cfile);
                        fs.Close();
                        Trace.WriteLine("Console saved successfully", "Profile Manager");
                        ManagedMessageBox.ShowMessage(ls["Status_Done"], ls["MessageCaption_ExportConsole"]);
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError("Unable to save console: " + ex.Message);
                        ManagedMessageBox.ShowErrorMessage(ex.Message + "\n\n" + ex.ToString(), ls["MessageCaption_ExportConsole"]);
                    }
                }
            }
        }
        public void ImportConsoleFile()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = ls["Filter_ImportConsoleFile"];
            op.Filter = ls["Filter_ConsoleFile"];
            if (op.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    // Deserialize !
                    Trace.WriteLine("Opening console file ..", "Profile Manager");
                    FileStream fs = new FileStream(op.FileName, FileMode.Open, FileAccess.Read);
                    BinaryFormatter formatter = new BinaryFormatter();
                    ConsoleFile cfile = (ConsoleFile)formatter.Deserialize(fs);
                    fs.Close();
                    Trace.WriteLine("Console opened successfully", "Profile Manager");
                    // Add console, give it and id !
                    EmulatorsOrganizer.Core.Console newConsole = cfile.Console;
                    newConsole.ID = profileManager.Profile.GenerateID();
                    newConsole.ParentGroupID = "";// No parent !
                    // Make ids
                    //        <old id, new id>
                    Dictionary<string, string> dataIDS = new Dictionary<string, string>();
                    Dictionary<string, string> icIDS = new Dictionary<string, string>();
                    // Rom Data info ***************
                    foreach (RomData inf in newConsole.RomDataInfoElements)
                    {
                        string oldID = inf.ID;
                        inf.ID = profileManager.Profile.GenerateID();
                        dataIDS.Add(oldID, inf.ID);
                    }
                    // Information containers ***************
                    foreach (InformationContainer inf in newConsole.InformationContainers)
                    {
                        string oldID = inf.ID;
                        inf.ID = profileManager.Profile.GenerateID();
                        icIDS.Add(oldID, inf.ID);
                    }

                    // Fix tabs map
                    foreach (string k in icIDS.Keys)
                    {
                        newConsole.InformationContainersMap.ReplaceID(k, icIDS[k], false);
                    }
                    // Fix columns
                    foreach (ColumnItem c in newConsole.Columns)
                    {
                        // Is it data ?
                        if (dataIDS.ContainsKey(c.ColumnID))
                            c.ColumnID = dataIDS[c.ColumnID];
                        // Is it ic ?
                        if (icIDS.ContainsKey(c.ColumnID))
                            c.ColumnID = icIDS[c.ColumnID];
                    }
                    // Roms ?
                    if (cfile.Roms != null)
                    {
                        foreach (Rom rom in cfile.Roms)
                        {
                            // Fix things !
                            rom.ID = profileManager.Profile.GenerateID();
                            rom.ParentConsoleID = newConsole.ID;
                            // Data items
                            foreach (RomDataInfoItem dataItem in rom.RomDataInfoItems)
                            {
                                if (dataIDS.ContainsKey(dataItem.ID))
                                    dataItem.ID = dataIDS[dataItem.ID];
                            }
                            // Information containers
                            foreach (InformationContainerItem icItem in rom.RomInfoItems)
                            {
                                if (icIDS.ContainsKey(icItem.ParentID))
                                    icItem.ParentID = icIDS[icItem.ParentID];
                            }
                            // Add it !!
                            profileManager.Profile.Roms.Add(rom, false);
                        }
                    }
                    // Add the console !
                    profileManager.Profile.Consoles.Add(newConsole);
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Unable to open console: " + ex.Message);
                    ManagedMessageBox.ShowErrorMessage(ex.Message + "\n\n" + ex.ToString(), ls["Filter_ImportConsoleFile"]);
                }
            }
        }
        public override bool CanDelete
        {
            get
            {
                return treeView1.SelectedNode != null;
            }
        }
        public override bool CanRename
        {
            get
            {
                return treeView1.SelectedNode != null;
            }
        }
        public override bool CanChangeIcon
        {
            get
            {
                return treeView1.SelectedNode != null;
            }
        }
        public override bool CanShowProperties
        {
            get
            {
                return (treeView1.SelectedNode != null);
            }
        }
        public bool CanAddRoms
        {
            get
            {
                if (treeView1.SelectedNode != null)
                    if (treeView1.SelectedNode is TreeNodeConsole)
                        return true;
                return false;
            }
        }
        public string HighlightedConsoleID
        {
            get
            {
                if (treeView1.SelectedNode != null)
                    if (treeView1.SelectedNode is TreeNodeConsole)
                        return ((TreeNodeConsole)treeView1.SelectedNode).ID;
                return "";
            }
        }
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                toolStrip1.BackColor = treeView1.BackColor = base.BackColor = value;
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
                treeView1.BackgroundImage = base.BackgroundImage = value;
            }
        }
        public void DetectAndDownloadFromTHeGamesDB()
        {
            ManagedMessageBox.ShowMessage("This feature is disabled temporary in this version, working on updating the api handler.");
            return;

            if (treeView1.SelectedNode == null) return;
            if (treeView1.SelectedNode is TreeNodeConsole)
            {
                string consoleID = ((TreeNodeConsole)treeView1.SelectedNode).ID;
                // Detect !
                Form_DetectAndDownloadFromTheGamesDB frm = new Form_DetectAndDownloadFromTheGamesDB(consoleID);
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    profileManager.Profile.OnInformationContainerItemsDetected();
                }
            }
        }
        public void CloneConsoleSettings()
        {
            switch (profileManager.Profile.RecentSelectedType)
            {
                case SelectionType.Console:
                    {
                        if (profileManager.Profile.Consoles.Count == 0)
                        {
                            ManagedMessageBox.ShowErrorMessage("There is no console in the profile !!");
                            return;
                        }
                        if (profileManager.Profile.Consoles.Count == 1)
                        {
                            ManagedMessageBox.ShowErrorMessage("There must be more than one console in the profile !");
                            return;
                        }
                        Form_CloneSettings frm = new Form_CloneSettings(SettingsCopyMode.CONSOLE);
                        frm.ShowDialog(this);
                        break;
                    }
                case SelectionType.ConsolesGroup:
                    {
                        if (profileManager.Profile.ConsoleGroups.Count == 0)
                        {
                            ManagedMessageBox.ShowErrorMessage("There is no consoles group in the profile !!");
                            return;
                        }
                        if (profileManager.Profile.ConsoleGroups.Count == 1)
                        {
                            ManagedMessageBox.ShowErrorMessage("There must be more than one consoles group in the profile !");
                            return;
                        }
                        Form_CloneSettings frm = new Form_CloneSettings(SettingsCopyMode.CONSOLES_GROUP);
                        frm.ShowDialog(this);
                        break;
                    }
                case SelectionType.Playlist:
                case SelectionType.PlaylistsGroup:
                case SelectionType.None: ManagedMessageBox.ShowErrorMessage("Please select a console or consoles group first."); return;
            }
        }

        protected override void OnProfileOpened()
        {
            base.OnProfileOpened();
            RefreshConsoleGroups();
            if (profileManager.Profile.RememberLatestSelectedConsoleOnProfileOpen)
            {
                switch (profileManager.Profile.RecentSelectedType)
                {
                    case SelectionType.ConsolesGroup:
                        {
                            // Look for the consoles group in the nodes ...
                            foreach (TreeNode node in treeView1.Nodes)
                            {
                                if (node is TreeNodeConsolesGroup)
                                {
                                    // Search the internal consoles

                                    if (((TreeNodeConsole)node).Console.ID == profileManager.Profile.SelectedConsolesGroupID)
                                    {
                                        treeView1.SelectedNode = node;
                                        node.Expand();
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                    case SelectionType.Console:
                        {
                            // Look for the console in the nodes ...
                            foreach (TreeNode node in treeView1.Nodes)
                            {
                                if (node is TreeNodeConsole)
                                {
                                    if (((TreeNodeConsole)node).Console.ID == profileManager.Profile.SelectedConsoleID)
                                    {
                                        treeView1.SelectedNode = node;
                                        node.Expand();
                                        break;
                                    }
                                }
                                else if (node is TreeNodeConsolesGroup)
                                {
                                    // Search the internal consoles
                                    foreach (TreeNode childnode in node.Nodes)
                                    {
                                        if (childnode is TreeNodeConsole)
                                        {
                                            if (((TreeNodeConsole)childnode).Console.ID == profileManager.Profile.SelectedConsoleID)
                                            {
                                                treeView1.SelectedNode = childnode;
                                                childnode.Expand();
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        }
                }

                ActiveSelection();
            }
        }
        protected override void OnNewProfileCreated()
        {
            base.OnNewProfileCreated();
            treeView1.Nodes.Clear();
        }
        protected override void OnEnableDisableButtons()
        {
            base.OnEnableDisableButtons();
            clearIconToolStripMenuItem.Enabled = changeIconToolStripMenuItem.Enabled = CanChangeIcon;
            deleteToolStripMenuItem.Enabled = CanDelete;
            propertiesToolStripMenuItem.Enabled = CanShowProperties;
            renameToolStripMenuItem.Enabled = CanRename;
            toolStripButton_moveDown.Enabled = toolStripButton_moveUp.Enabled = treeView1.SelectedNode != null;
        }
        protected override void OnProfileSavingStarted()
        {
            base.OnProfileSavingStarted();
            // When saving a profile, disable everything so user will be stuck with the roms browser only.
            this.Enabled = false;
        }
        protected override void OnProfileSavingFinished()
        {
            base.OnProfileSavingFinished();

            if (!this.InvokeRequired)
                this.Enabled = true;
            else
                this.Invoke(new Action(EnableThis));
        }
        private void EnableThis()
        {
            this.Enabled = true;
        }

        private void Profile_ConoslesGroupsCleared(object sender, EventArgs e)
        {
            RefreshConsoleGroups(); OnEnableDisableButtons();
        }
        private void Profile_ConoslesGroupRemoved(object sender, EventArgs e)
        {
            RefreshConsoleGroups(); OnEnableDisableButtons();
        }
        private void Profile_ConoslesGroupAdded(object sender, EventArgs e)
        {
            RefreshConsoleGroups(); OnEnableDisableButtons();
            //  if (treeView1.Nodes.Count > 0)
            //       treeView1.Nodes[treeView1.Nodes.Count - 1].BeginEdit();
        }
        private void Profile_ConoslesCleared(object sender, EventArgs e)
        {
            RefreshConsoles(); OnEnableDisableButtons();
        }
        private void Profile_ConosleRemoved(object sender, EventArgs e)
        {
            RefreshConsoles(); OnEnableDisableButtons();
        }
        private void Profile_ConosleAdded(object sender, EventArgs e)
        {
            RefreshConsoles(); OnEnableDisableButtons();
        }
        private void Profile_ConoslesGroupsMoved(object sender, EventArgs e)
        {
            RefreshConsoleGroups();
            try
            {
                treeView1.SelectedNode = treeView1.Nodes[currentlySelectedNodeIndex];
            }
            catch { }
            OnEnableDisableButtons();
        }
        private void Profile_ConsoleMoved(object sender, EventArgs e)
        {
            RefreshConsoleGroups();
            try
            {
                if (currentlySelectedNodeParentIndex >= 0)
                {
                    treeView1.Nodes[currentlySelectedNodeParentIndex].Expand();
                    treeView1.SelectedNode = treeView1.Nodes[currentlySelectedNodeParentIndex].Nodes[currentlySelectedNodeIndex];
                }
                else
                {
                    treeView1.SelectedNode = treeView1.Nodes[currentlySelectedNodeIndex];
                }
            }
            catch { }
            OnEnableDisableButtons();
        }
        private void Profile_ConoslesGroupsSorted(object sender, EventArgs e)
        {
            RefreshConsoleGroups(); OnEnableDisableButtons();
        }
        private void Profile_ConsolesSorted(object sender, EventArgs e)
        {
            RefreshConsoles();
            try
            {
                if (currentlySelectedNodeParentIndex > 0)
                {
                    treeView1.SelectedNode = treeView1.Nodes[currentlySelectedNodeParentIndex];
                    treeView1.Nodes[currentlySelectedNodeParentIndex].Expand();
                }
            }
            catch { }
            OnEnableDisableButtons();
        }
        private void Profile_ConsolePropertiesChanged(object sender, EventArgs e)
        {
            RefreshConsoles();
            try
            {
                if (currentlySelectedNodeParentIndex >= 0)
                {
                    treeView1.Nodes[currentlySelectedNodeParentIndex].Expand();
                    treeView1.SelectedNode = treeView1.Nodes[currentlySelectedNodeParentIndex].Nodes[currentlySelectedNodeIndex];
                }
                else
                {
                    treeView1.SelectedNode = treeView1.Nodes[currentlySelectedNodeIndex];
                }
            }
            catch { }
            OnEnableDisableButtons();

            if (treeView1.SelectedNode == null)
            {
                object _defaultStyle = settings.GetValue("Default Style", true, new EOStyle());
                /*if (_defaultStyle != null)
                    treeView1.BackgroundImage = ((EOStyle)_defaultStyle).image_ConsolesBrowser;
                else
                    treeView1.BackgroundImage = null;*/
                treeView1.Invalidate();
                return;
            }
            if (treeView1.SelectedNode is TreeNodeConsolesGroup)
            {
                //  treeView1.BackgroundImage = profileManager.Profile.ConsoleGroups[((TreeNodeConsolesGroup)treeView1.SelectedNode).ID].Style.image_ConsolesBrowser;
            }
            else if (treeView1.SelectedNode is TreeNodeConsole)
            {
                //  treeView1.BackgroundImage = profileManager.Profile.Consoles[((TreeNodeConsole)treeView1.SelectedNode).ID].Style.image_ConsolesBrowser;
            }
            treeView1.Invalidate();
        }
        private void Profile_ProfileCleanUpFinished(object sender, EventArgs e)
        {
            RefreshConsoles();
        }

        private void consolesGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            profileManager.Profile.AddConsolesGroup();
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            DeleteSelected();
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            RenameSelected();
        }
        private void consoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddConsole();
        }
        private void rootConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView1.SelectedNode = null;
            AddConsole();
        }
        private void treeView1_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (treeView1.SelectedNode == null)
                return;
            if (!canRename) e.CancelEdit = true;
        }
        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node == null)
                return;
            if (e.Label == "" || e.Label == null)
            {
                e.CancelEdit = true;
                return;
            }
            if (e.Node is TreeNodeConsolesGroup)
            {
                if (profileManager.Profile.ConsoleGroups.Contains(e.Label))
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_ThisNameAlreadyTakenByAnotherConsolesGroup"],
                        ls["MessageCaption_Rename"]);
                    e.CancelEdit = true;
                    return;
                }
                profileManager.Profile.RenameConsolesGroup(((TreeNodeConsolesGroup)e.Node).ID, e.Label);
            }
            else if (e.Node is TreeNodeConsole)
            {
                string parentID = "";
                if (e.Node.Parent != null)
                    parentID = ((TreeNodeConsolesGroup)e.Node.Parent).ID;
                if (profileManager.Profile.Consoles.Contains(e.Label, parentID, ((TreeNodeConsole)e.Node).ID))
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_ThisNameAlreadyTakenByAnotherConsole"],
                        ls["MessageCaption_Rename"]);
                    e.CancelEdit = true;
                    return;
                }
                profileManager.Profile.RenameConsole(((TreeNodeConsole)e.Node).ID, e.Label);
            }
            ((EOTreeNode)e.Node).RefreshText();
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            MoveSelectedItemUp();
        }
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            MoveSelectedItemDown();
        }
        private void consoleGroupsByNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SortConsolesGroupByName();
        }
        private void toolStripSplitButton2_ButtonClick(object sender, EventArgs e)
        {
            SortConsolesGroupByName();
            SortRootConsolesByName();
        }
        private void rootConsolesByNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SortRootConsolesByName();
        }
        private void consolesByNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SortConsolesByName();
        }
        private void treeView1_Enter(object sender, EventArgs e)
        {
            panel1.Visible = toolStrip1.Visible = true;
        }
        private void ConsolesBrowser_Leave(object sender, EventArgs e)
        {
            panel1.Visible = toolStrip1.Visible = false;
            if (!Focused)
                canDoDragDrop = false;
        }
        /*Draw*/
        private void treeView1_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            //clear
            if (e.Bounds.IsEmpty)
                return;
            if (e.Bounds.Y < 0)
                return;

            int h = trackBar_zoom.Value;
            int w = trackBar_zoom.Value;
            Size CharsSize = TextRenderer.MeasureText(e.Node.Text, treeView1.Font);

            Image im = null;
            int XP = 16;
            if (e.Node.GetType() == typeof(TreeNodeConsolesGroup))
            {
                ConsolesGroup gr = profileManager.Profile.ConsoleGroups[((TreeNodeConsolesGroup)e.Node).ID];
                if (gr == null)
                    return;
                if (treeView1.SelectedNode == e.Node)
                {
                    if (treeView1.Focused)
                        e.Graphics.DrawRectangle(new Pen(new SolidBrush(treeView1.ForeColor), 2),
                           e.Bounds.X + XP, e.Bounds.Y + 2, w + CharsSize.Width + 5, h);
                    else
                        e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.LightGray), 2),
                          e.Bounds.X + XP, e.Bounds.Y + 2, w + CharsSize.Width + 5, h);
                }
                if (e.Node.Nodes.Count > 0)
                {
                    if (!e.Node.IsExpanded)
                    {
                        e.Graphics.DrawImage(Properties.Resources.bullet_black,
                            e.Bounds.X + 1, e.Bounds.Y + ((e.Bounds.Height / 2) - 5), 12, 12);
                    }
                    else
                    {
                        e.Graphics.DrawImage(Properties.Resources.bullet_arrow_down,
                            e.Bounds.X + 1, e.Bounds.Y + ((e.Bounds.Height / 2) - 5), 12, 12);
                    }
                }
                im = gr.Icon;
                if (im == null)
                    im = Properties.Resources.folder;

                e.Graphics.DrawImage(im, e.Bounds.X + XP, e.Bounds.Y + 2, w, h);
                e.Graphics.DrawString(e.Node.Text, treeView1.Font, new SolidBrush(treeView1.ForeColor),
                    new PointF(e.Bounds.X + w + 20, e.Bounds.Y + ((e.Bounds.Height / 2) - 5)));
            }

            if (e.Node.GetType() == typeof(TreeNodeConsole))
            {
                EmulatorsOrganizer.Core.Console con = profileManager.Profile.Consoles[((TreeNodeConsole)e.Node).ID];
                if (con == null)
                    return;
                if (con.ParentGroupID != "")
                    XP += 16;
                if (treeView1.SelectedNode == e.Node)
                {
                    if (treeView1.Focused)
                        e.Graphics.DrawRectangle(new Pen(new SolidBrush(treeView1.ForeColor), 2),
                           e.Bounds.X + XP, e.Bounds.Y + 2, w + CharsSize.Width + 5, h);
                    else
                        e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.LightGray), 2),
                          e.Bounds.X + XP, e.Bounds.Y + 2, w + CharsSize.Width + 5, h);
                }
                if (e.Node.Nodes.Count > 0)
                {
                    if (!e.Node.IsExpanded)
                    {
                        e.Graphics.DrawImage(Properties.Resources.bullet_black,
                            e.Bounds.X + 16, e.Bounds.Y + ((e.Bounds.Height / 2) - 5), 12, 12);
                    }
                    else
                    {
                        e.Graphics.DrawImage(Properties.Resources.bullet_arrow_down,
                            e.Bounds.X + 16, e.Bounds.Y + ((e.Bounds.Height / 2) - 5), 12, 12);
                    }
                }
                im = con.Icon;
                if (im == null)
                    im = Properties.Resources.Console;

                e.Graphics.DrawImage(im, e.Bounds.X + XP, e.Bounds.Y + 2, w, h);
                e.Graphics.DrawString(e.Node.Text, treeView1.Font, new SolidBrush(treeView1.ForeColor),
                    new PointF(e.Bounds.X + w + XP, e.Bounds.Y + ((e.Bounds.Height / 2) - 5)));
            }
        }
        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                treeView1.SelectedNode = treeView1.GetNodeAt(e.Location);

                if (treeView1.SelectedNode == null)
                { OnEnableDisableButtons(); return; }
                if (treeView1.SelectedNode is TreeNodeConsolesGroup)
                {
                    if (e.Location.X < 25)
                    {
                        canRename = false;
                        if (treeView1.SelectedNode.IsExpanded)
                            treeView1.SelectedNode.Collapse();
                        else
                            treeView1.SelectedNode.Expand();
                    }
                    else
                    {
                        canRename = true;
                    }
                }
                else if (treeView1.SelectedNode is TreeNodeConsole)
                {
                    if (e.Location.X < 25 && e.Location.X > 10)
                    {
                        canRename = false;
                        if (treeView1.SelectedNode.IsExpanded)
                            treeView1.SelectedNode.Collapse();
                        else
                            treeView1.SelectedNode.Expand();
                    }
                    else
                    {
                        canRename = true;
                    }
                }
                if (!doubleClickActiveSelection)
                    ActiveSelection();
                OnEnableDisableButtons();
            }
            treeView1.Invalidate();
        }
        private void trackBar_zoom_Scroll(object sender, EventArgs e)
        {
            imageList1.ImageSize = new Size(trackBar_zoom.Value, trackBar_zoom.Value);
            treeView1.Refresh();
            // Save
            settings.AddValue(new SettingsValue("ConsolesBrowser:ZoomValue", trackBar_zoom.Value));
        }
        private void consoleGroupsByNameToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SortConsolesGroupByName();
        }
        private void rootConsolesByNameToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SortRootConsolesByName();
        }
        private void consolesByNameToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SortConsolesByName();
        }
        private void changeIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeIcon();
        }
        /*Drag and drop*/
        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (canDoDragDrop)
                DoDragDrop(e.Item, DragDropEffects.Move | DragDropEffects.Link);
        }
        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {

        }
        private void treeView1_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNodeConsolesGroup)))
            {
                Point targetPoint = treeView1.PointToClient(new Point(e.X, e.Y));
                TreeNode target = treeView1.GetNodeAt(targetPoint);
                if (target == null)
                    e.Effect = DragDropEffects.Move;
                else if (target is TreeNodeConsolesGroup)
                    e.Effect = DragDropEffects.Move;
                else if (target is TreeNodeConsole)
                {
                    // get parent
                    string parentID = "";
                    if (target.Parent != null)
                        parentID = "1";
                    if (parentID == "")
                        e.Effect = DragDropEffects.Move;
                    else
                        e.Effect = DragDropEffects.None;
                }
            }
            else if (e.Data.GetDataPresent(typeof(TreeNodeConsole)))
            {
                Point targetPoint = treeView1.PointToClient(new Point(e.X, e.Y));
                TreeNode target = treeView1.GetNodeAt(targetPoint);

                if (target is TreeNodeConsolesGroup)
                    e.Effect = DragDropEffects.Link;
                else
                    e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNodeConsolesGroup)))
            {
                TreeNodeConsolesGroup original = (TreeNodeConsolesGroup)e.Data.GetData(typeof(TreeNodeConsolesGroup));
                ConsolesGroup originalGR = profileManager.Profile.ConsoleGroups[original.ID];
                int originalIndex = profileManager.Profile.ConsoleGroups.IndexOf(originalGR);
                // Get target
                Point targetPoint = treeView1.PointToClient(new Point(e.X, e.Y));
                TreeNode target = treeView1.GetNodeAt(targetPoint);

                if (target == null)
                {
                    currentlySelectedNodeIndex = profileManager.Profile.ConsoleGroups.Count - 1;
                    profileManager.Profile.ConsoleGroups.Move(originalIndex, -1);
                }
                else if (target is TreeNodeConsole)
                {
                    if (target.Parent == null)
                    {
                        currentlySelectedNodeIndex = profileManager.Profile.ConsoleGroups.Count - 1;
                        profileManager.Profile.ConsoleGroups.Move(originalIndex, -1);
                    }
                }
                else if (target is TreeNodeConsolesGroup)
                {
                    int targetIndex = treeView1.Nodes.IndexOf(target);
                    // Then move the group into last position in the list
                    currentlySelectedNodeIndex = targetIndex;
                    profileManager.Profile.ConsoleGroups.Move(originalIndex, targetIndex);
                }
            }
            else if (e.Data.GetDataPresent(typeof(TreeNodeConsole)))
            {
                TreeNodeConsole original = (TreeNodeConsole)e.Data.GetData(typeof(TreeNodeConsole));
                Core.Console originalCon = profileManager.Profile.Consoles[original.ID];
                int originalIndex = profileManager.Profile.Consoles.IndexOf(originalCon);
                // Get target
                Point targetPoint = treeView1.PointToClient(new Point(e.X, e.Y));
                TreeNode target = treeView1.GetNodeAt(targetPoint);

                if (target == null)
                {
                    currentlySelectedNodeParentIndex = -1;
                    currentlySelectedNodeIndex = treeView1.Nodes.Count - 1;
                    originalCon.ParentGroupID = "";
                    profileManager.Profile.Consoles.Move(originalIndex, -1);
                }
                else if (target is TreeNodeConsole)
                {
                    if (target.Parent == null)
                    {
                        currentlySelectedNodeParentIndex = -1;

                        int targetIndex = profileManager.Profile.Consoles.IndexOf
                            (profileManager.Profile.Consoles[((TreeNodeConsole)target).ID]);
                        currentlySelectedNodeIndex = target.Index;
                        originalCon.ParentGroupID = "";
                        profileManager.Profile.Consoles.Move(originalIndex, targetIndex);
                    }
                    else
                    {
                        // Get parent id
                        currentlySelectedNodeParentIndex = treeView1.Nodes.IndexOf(target.Parent);
                        currentlySelectedNodeIndex = target.Index;
                        string parentID = ((TreeNodeConsolesGroup)target.Parent).ID;
                        int targetIndex = profileManager.Profile.Consoles.IndexOf
                           (profileManager.Profile.Consoles[((TreeNodeConsole)target).ID]);
                        profileManager.Profile.Consoles[originalIndex].ParentGroupID = parentID;
                        profileManager.Profile.Consoles.Move(originalIndex, targetIndex);
                    }
                }
                else if (target is TreeNodeConsolesGroup)
                {
                    string parentID = ((TreeNodeConsolesGroup)target).ID;
                    profileManager.Profile.Consoles[originalIndex].ParentGroupID = parentID;
                    profileManager.Profile.OnConsoleMoved(profileManager.Profile.Consoles[originalIndex].Name);
                }
            }
            OnEnableDisableButtons();
        }
        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            canDoDragDrop = true;
        }
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            ShowItemProperties();
        }
        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowItemProperties();
        }
        private void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (doubleClickActiveSelection)
                ActiveSelection();
        }
        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Return)
                ActiveSelection();
        }
        private void clearIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearIcon();
        }
        private void addToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            romsfolderScanToolStripMenuItem.Enabled = romsToolStripMenuItem.Enabled = CanAddRoms;
        }
        private void romsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddRoms();
        }
        private void romsfolderScanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddRomsFolderScan();
        }
        private void toolStripSplitButton1_DropDownOpening(object sender, EventArgs e)
        {
            romsfolderScanToolStripMenuItem1.Enabled = romsToolStripMenuItem1.Enabled = CanAddRoms;
        }
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            detectToolStripMenuItem.DropDownItems.Clear();
            backupToolStripMenuItem.DropDownItems.Clear();
            restoreFromFileToolStripMenuItem.DropDownItems.Clear();
            importExcelDatasheetForScoresreviewToolStripMenuItem.DropDownItems.Clear();
            exportReviewscoresAsExcelDatasheetFileToolStripMenuItem.DropDownItems.Clear();
            exportConsoleToolStripMenuItem.Enabled = false;
            if (treeView1.SelectedNode == null) return;
            if (treeView1.SelectedNode is TreeNodeConsole)
            {
                exportConsoleToolStripMenuItem.Enabled = true;
                detectToolStripMenuItem.Enabled = true;
                // Load information container items
                string consoleID = ((TreeNodeConsole)treeView1.SelectedNode).ID;
                EmulatorsOrganizer.Core.Console console = profileManager.Profile.Consoles[consoleID];
                foreach (InformationContainer con in console.InformationContainers)
                {
                    if (con.Dectable)
                    {
                        ToolStripMenuItem item = new ToolStripMenuItem();
                        item.Text = con.DisplayName;
                        item.Tag = con.ID;
                        detectToolStripMenuItem.DropDownItems.Add(item);

                        item = new ToolStripMenuItem();
                        item.Text = con.DisplayName;
                        item.Tag = con.ID;
                        backupToolStripMenuItem.DropDownItems.Add(item);

                        item = new ToolStripMenuItem();
                        item.Text = con.DisplayName;
                        item.Tag = con.ID;
                        restoreFromFileToolStripMenuItem.DropDownItems.Add(item);
                    }
                    if (con is InformationContainerReviewScore)
                    {
                        ToolStripMenuItem item = new ToolStripMenuItem();
                        item.Text = con.DisplayName;
                        item.Tag = con.ID;
                        importExcelDatasheetForScoresreviewToolStripMenuItem.DropDownItems.Add(item);

                        item = new ToolStripMenuItem();
                        item.Text = con.DisplayName;
                        item.Tag = con.ID;
                        exportReviewscoresAsExcelDatasheetFileToolStripMenuItem.DropDownItems.Add(item);
                    }
                }
            }
            else
            {
                restoreFromFileToolStripMenuItem.Enabled = backupToolStripMenuItem.Enabled = detectToolStripMenuItem.Enabled = false;
            }
        }
        // Detect
        private void detectToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // Tag of the item is the container id
            if (treeView1.SelectedNode == null) return;
            if (treeView1.SelectedNode is TreeNodeConsole)
            {
                string consoleID = ((TreeNodeConsole)treeView1.SelectedNode).ID;
                // Detect !
                Form_Detect frm = new Form_Detect(((TreeNodeConsole)treeView1.SelectedNode).ID, (string)e.ClickedItem.Tag);
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    profileManager.Profile.OnInformationContainerItemsDetected();
                }
            }
        }
        private void consolestepByStepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddConsoleStepStep();
        }
        private void backupToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // Tag of the item is the container id
            if (treeView1.SelectedNode == null) return;
            if (treeView1.SelectedNode is TreeNodeConsole)
            {
                string consoleID = ((TreeNodeConsole)treeView1.SelectedNode).ID;
                // Backup !
                try
                {
                    Form_Backup frm = new Form_Backup(((TreeNodeConsole)treeView1.SelectedNode).ID, (string)e.ClickedItem.Tag);
                    frm.ShowDialog(this);
                }
                catch { }
            }
        }
        private void restoreFromFileToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // Tag of the item is the container id
            if (treeView1.SelectedNode == null) return;
            if (treeView1.SelectedNode is TreeNodeConsole)
            {
                string consoleID = ((TreeNodeConsole)treeView1.SelectedNode).ID;
                // Backup !
                Form_RestoreBackupFile frm = new Form_RestoreBackupFile(((TreeNodeConsole)treeView1.SelectedNode).ID, (string)e.ClickedItem.Tag);
                frm.ShowDialog(this);
            }
        }
        private void exportConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportSelectedConsole();
        }
        private void importConsoleFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportConsoleFile();
        }
        private void assignEmulatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_Emulators frm = new Form_Emulators();
            frm.ShowDialog(this);
        }
        private void mAMEConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddMameConsole();
        }
        private void detectIconsForRomsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tag of the item is the container id
            if (treeView1.SelectedNode == null) return;
            if (treeView1.SelectedNode is TreeNodeConsole)
            {
                string consoleID = ((TreeNodeConsole)treeView1.SelectedNode).ID;
                // Detect !
                Form_DetectRomIcons frm = new Form_DetectRomIcons(consoleID);
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    profileManager.Profile.OnInformationContainerItemsDetected();
                }
            }
        }
        private void detectAndDownloadFromTheGamesDBnetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DetectAndDownloadFromTHeGamesDB();
        }
        private void importExcelDatasheetForScoresreviewToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (treeView1.SelectedNode == null) return;
            if (treeView1.SelectedNode is TreeNodeConsole)
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Title = "Import rating/review from Excel Datasheet file";
                op.Filter = "Excel Datasheet file (*.xlsx)|*.xlsx;";
                if (op.ShowDialog() == DialogResult.OK)
                {
                    var ds = new DataSet();
                    var connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + op.FileName + ";Extended Properties=\"Excel 12.0;IMEX=1;HDR=NO;TypeGuessRows=0;ImportMixedTypes=Text\""; ;
                    using (var conn = new OleDbConnection(connectionString))
                    {
                        conn.Open();

                        var sheets = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "SELECT * FROM [" + sheets.Rows[0]["TABLE_NAME"].ToString() + "] ";

                            var adapter = new OleDbDataAdapter(cmd);

                            adapter.Fill(ds);
                        }
                    }
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        // 1 Get the fields
                        List<string> properties = new List<string>();

                        for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                        {
                            if (ds.Tables[0].Rows[0][i].ToString() != "Name")
                            {
                                properties.Add(ds.Tables[0].Rows[0][i].ToString());
                            }
                        }
                        // Load fields
                        string consoleID = ((TreeNodeConsole)treeView1.SelectedNode).ID;
                        Core.Console console = profileManager.Profile.Consoles[consoleID];
                        InformationContainerReviewScore inf = (InformationContainerReviewScore)console.GetInformationContainer(e.ClickedItem.Tag.ToString());
                        foreach (string ff in properties)
                        {
                            if (!inf.Fields.Contains(ff))
                                inf.Fields.Add(ff);
                        }

                        // 2 Get data !
                        List<Rom> roms = new List<Rom>(profileManager.Profile.Roms[consoleID, false]);
                        for (int i = 1; i < ds.Tables[0].Rows.Count; i++)
                        {
                            for (int j = 0; j < roms.Count; j++)
                            {
                                if (Compare_RomName_EntryName(roms[j].Name, ds.Tables[0].Rows[i][0].ToString()))
                                {
                                    InformationContainerItem item = roms[j].GetInformationContainerItem(e.ClickedItem.Tag.ToString());

                                    if (item != null)
                                    {
                                        InformationContainerItemReviewScore itemIC = (InformationContainerItemReviewScore)item;
                                        foreach (string k in inf.Fields)
                                        {
                                            int score = 0;
                                            bool found = false;
                                            foreach (string p in properties)
                                            {
                                                if (p == k)
                                                {
                                                    int col_Index = properties.IndexOf(p) + 1;
                                                    if (ds.Tables[0].Rows[i][col_Index] != null)
                                                    {
                                                        string val = ds.Tables[0].Rows[i][properties.IndexOf(p) + 1].ToString().Replace(" %", "");
                                                        int.TryParse(val, out score);
                                                    }
                                                    found = true;
                                                    break;
                                                }
                                            }
                                            if (found)
                                            {
                                                if (itemIC.Scores.ContainsKey(k))
                                                    itemIC.Scores[k] = score;
                                                else
                                                    itemIC.Scores.Add(k, score);
                                            }
                                        }
                                        profileManager.Profile.Roms[roms[j].ID].Modified = true;
                                    }
                                    else
                                    {
                                        // Add new item
                                        InformationContainerItemReviewScore itemIC = new InformationContainerItemReviewScore(profileManager.Profile.GenerateID(), e.ClickedItem.Tag.ToString());
                                        foreach (string k in inf.Fields)
                                        {
                                            int score = 0; bool found = false;
                                            foreach (string p in properties)
                                            {
                                                if (p == k)
                                                {
                                                    int col_Index = properties.IndexOf(p) + 1;
                                                    if (ds.Tables[0].Rows[i][col_Index] != null)
                                                    {
                                                        string val = ds.Tables[0].Rows[i][properties.IndexOf(p) + 1].ToString().Replace(" %", "");
                                                        int.TryParse(val, out score);
                                                    }
                                                    found = true;
                                                    break;
                                                }
                                            }
                                            if (found)
                                            {
                                                if (itemIC.Scores.ContainsKey(k))
                                                    itemIC.Scores[k] = score;
                                                else
                                                    itemIC.Scores.Add(k, score);
                                            }
                                        }
                                        profileManager.Profile.Roms[roms[j].ID].RomInfoItems.Add(itemIC);
                                        profileManager.Profile.Roms[roms[j].ID].Modified = true;
                                    }
                                    //  roms.RemoveAt(j);
                                    //    j--;
                                }
                            }
                        }
                        profileManager.Profile.OnInformationContainerItemsModified(inf.DisplayName);
                    }
                }
            }
        }
        private bool Compare_RomName_EntryName(string romName, string entryName)
        {
            bool ROMMATCH = false;
            romName = romName.Replace(":", " ");
            romName = romName.Replace("|", " ");
            romName = romName.Replace(@"\", " ");
            romName = romName.Replace("/", " ");
            romName = romName.Replace("*", " ");
            romName = romName.Replace("?", " ");
            romName = romName.Replace("<", " ");
            romName = romName.Replace(">", " ");
            romName = romName.Replace("_", " ");
            romName = romName.Replace(@"""", " ");
            romName = romName.Replace(" ", "");
            string fileOfDatabase = "";
            // Check rom

            // Remove forbidden values
            fileOfDatabase = entryName.Replace(":", " ");
            fileOfDatabase = fileOfDatabase.Replace("|", " ");
            fileOfDatabase = fileOfDatabase.Replace(@"\", " ");
            fileOfDatabase = fileOfDatabase.Replace("/", " ");
            fileOfDatabase = fileOfDatabase.Replace("*", " ");
            fileOfDatabase = fileOfDatabase.Replace("?", " ");
            fileOfDatabase = fileOfDatabase.Replace("<", " ");
            fileOfDatabase = fileOfDatabase.Replace(">", " ");
            fileOfDatabase = fileOfDatabase.Replace("_", " ");
            fileOfDatabase = fileOfDatabase.Replace(@"""", " ");
            fileOfDatabase = fileOfDatabase.Replace(" ", "");

            if (romName == "" || fileOfDatabase == "")
                ROMMATCH = false;
            else if (romName.Length == 0 || fileOfDatabase.Length == 0)
                ROMMATCH = false;
            else if (romName.Length == fileOfDatabase.Length)
                ROMMATCH = fileOfDatabase.ToLower() == romName.ToLower();
            else
                ROMMATCH = romName.ToLower().StartsWith(fileOfDatabase.ToLower());

            return ROMMATCH;
        }
        private void exportReviewscoresAsExcelDatasheetFileToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // Tag of the item is the container id
            if (treeView1.SelectedNode == null) return;
            if (treeView1.SelectedNode is TreeNodeConsole)
            {
                SaveFileDialog sav = new SaveFileDialog();
                sav.Title = "Export rating/review as Excel Datasheet file";
                sav.Filter = "Excel Datasheet file (*.xlsx)|*.xlsx;";
                if (sav.ShowDialog() == DialogResult.OK)
                {
                    string consoleID = ((TreeNodeConsole)treeView1.SelectedNode).ID;

                    // 1 Create a data set
                    DataSet ds = new DataSet();
                    ds.Tables.Add("EO");

                    // Load fields
                    Core.Console console = profileManager.Profile.Consoles[consoleID];
                    InformationContainerReviewScore inf = (InformationContainerReviewScore)console.GetInformationContainer(e.ClickedItem.Tag.ToString());

                    // 1 Add the columns
                    ds.Tables[0].Columns.Add("Name");
                    foreach (string k in inf.Fields)
                    {
                        ds.Tables[0].Columns.Add(k);
                    }
                    // 2 Add data
                    Rom[] roms = profileManager.Profile.Roms[consoleID, false];
                    int j = 0;
                    foreach (Rom rom in roms)
                    {
                        InformationContainerItem item = rom.GetInformationContainerItem(e.ClickedItem.Tag.ToString());
                        ds.Tables[0].Rows.Add();
                        ds.Tables[0].Rows[j]["Name"] = rom.Name;
                        if (item != null)
                        {
                            InformationContainerItemReviewScore itemIC = (InformationContainerItemReviewScore)item;
                            foreach (string k in inf.Fields)
                            {
                                if (itemIC.Scores.ContainsKey(k))
                                {
                                    ds.Tables[0].Rows[j][k] = itemIC.Scores[k] + " %";
                                }

                            }
                        }
                        j++;
                    }

                    var wb = new XLWorkbook();

                    // Add all DataTables in the DataSet as a worksheets
                    wb.Worksheets.Add(ds);
                    wb.SaveAs(sav.FileName);
                    wb.Dispose();
                }
            }
        }

        private void copySettingsFromAnotherConsolesGroupconsoleToThisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloneConsoleSettings();
        }
    }
}
