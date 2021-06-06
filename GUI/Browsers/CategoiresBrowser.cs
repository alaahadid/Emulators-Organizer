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
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EmulatorsOrganizer.GUI
{
    public partial class CategoiresBrowser : IBrowserControl
    {
        public CategoiresBrowser()
        {
            InitializeComponent();
            base.InitializeEvents();
            //   InitializeEvents();
        }
        private bool isChangingCheck;
        private bool AToZ;
        private List<string> rom_shown_ids = new List<string>();
        private bool isLoadingRoms;
        private delegate void AddRomCategoriesDelegate(Core.RomShowedArgs e);

        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                optimizedTreeview1.BackColor = base.BackColor = value;
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
                optimizedTreeview1.BackgroundImage = base.BackgroundImage = value;
            }
        }
        public override void InitializeEvents()
        {
            profileManager.Profile.RomShowed += Profile_RomShowed;
            profileManager.Profile.ConsoleSelectionChanged += Profile_ConsoleSelectionChanged;
            profileManager.Profile.ConsolesGroupSelectionChanged += Profile_ConsoleSelectionChanged;
            profileManager.Profile.PlaylistSelectionChanged += Profile_ConsoleSelectionChanged;
            profileManager.Profile.PlaylistsGroupSelectionChanged += Profile_ConsoleSelectionChanged;
            profileManager.Profile.RequestCategoriesListClear += Profile_RequestCategoriesListClear;
            profileManager.Profile.RomsLoadingStarted += Profile_RomsLoadingStarted;
            profileManager.Profile.RomsLoadingFinished += Profile_RomsLoadingFinished;
        }
        public override void DisposeEvents()
        {
            profileManager.Profile.RomShowed -= Profile_RomShowed;
            profileManager.Profile.ConsoleSelectionChanged -= Profile_ConsoleSelectionChanged;
            profileManager.Profile.ConsolesGroupSelectionChanged -= Profile_ConsoleSelectionChanged;
            profileManager.Profile.PlaylistSelectionChanged -= Profile_ConsoleSelectionChanged;
            profileManager.Profile.PlaylistsGroupSelectionChanged -= Profile_ConsoleSelectionChanged;
            profileManager.Profile.RequestCategoriesListClear -= Profile_RequestCategoriesListClear;
            profileManager.Profile.RomsLoadingStarted -= Profile_RomsLoadingStarted;
            profileManager.Profile.RomsLoadingFinished -= Profile_RomsLoadingFinished;
        }
        public override void ApplyStyle(EOStyle style)
        {
            base.ApplyStyle(style);
            this.BackColor = style.bkgColor_CategoriesBrowser;
            this.BackgroundImage = style.image_CategoriesBrowser;
            switch (style.imageMode_CategoriesBrowser)
            {
                case BackgroundImageMode.NormalStretchNoAspectRatio: this.optimizedTreeview1.BackgroundImageMode = ImageViewMode.Normal; break;
                case BackgroundImageMode.StretchIfLarger: this.optimizedTreeview1.BackgroundImageMode = ImageViewMode.StretchIfLarger; break;
                case BackgroundImageMode.StretchToFit: this.optimizedTreeview1.BackgroundImageMode = ImageViewMode.StretchToFit; break;
            }
            optimizedTreeview1.ForeColor = style.txtColor_CategoriesBrowser;
            // Font
            try
            {
                FontConverter conv = new FontConverter();
                optimizedTreeview1.Font = (Font)conv.ConvertFromString(style.font_CategoriesBrowser);
            }
            catch { }
        }
        protected override void OnProfileOpened()
        {
            base.OnProfileOpened();
            optimizedTreeview1.Nodes.Clear();
            profileManager.Profile.ActiveCategories = new List<string>();
            rom_shown_ids = new List<string>();

        }
        private void AddCategoriesFromRom(Core.RomShowedArgs e)
        {
            //System.Diagnostics.Trace.WriteLine("ADD");
            if (!rom_shown_ids.Contains(e.Rom.ID))
                rom_shown_ids.Add(e.Rom.ID);
            foreach (string cat in e.Rom.Categories)
            {
                bool hasParent = cat.Contains('/');
                string parent = "";
                string child = "";
                if (hasParent)
                {
                    string[] codes = cat.Split('/');
                    if (codes.Length > 1)
                    {
                        parent = codes[0];
                        child = codes[1];
                    }
                    else
                    {
                        hasParent = false;
                        child = cat;
                    }
                }

                if (hasParent)
                {
                    // Look for the father
                    if (!IsCategoryExist(null, parent))
                    {
                        // Add it as new father
                        TreeNode pnode = new TreeNode();
                        pnode.Text = parent;
                        pnode.Checked = profileManager.Profile.ActiveCategories.Contains(parent);
                        pnode.Tag = parent;
                        pnode.SelectedImageIndex = pnode.ImageIndex = 1;
                        optimizedTreeview1.Nodes.Add(pnode);
                        // Add the child !!
                        TreeNode node = new TreeNode();
                        node.Text = child;
                        node.Checked = profileManager.Profile.ActiveCategories.Contains(cat);
                        node.Tag = cat;// Child !!
                        node.SelectedImageIndex = node.ImageIndex = 0;
                        pnode.Nodes.Add(node);
                    }
                    else// We have the parent !!
                    {
                        foreach (TreeNode pnode in optimizedTreeview1.Nodes)
                        {
                            if (pnode.Tag.ToString() == parent)
                            {
                                // This is the father !!
                                // Look for children
                                if (!IsCategoryExist(pnode, cat))
                                {
                                    // Add the child !!
                                    TreeNode node = new TreeNode();
                                    node.Text = child;
                                    node.Checked = profileManager.Profile.ActiveCategories.Contains(cat);
                                    node.Tag = cat;// Child !!
                                    node.SelectedImageIndex = node.ImageIndex = 0;
                                    pnode.Nodes.Add(node);
                                }
                                break;
                            }
                        }
                    }
                }
                else
                {
                    // Just add the category as it is !
                    if (!IsCategoryExist(null, cat))
                    {
                        // Add the child !!
                        TreeNode node = new TreeNode();
                        node.Text = cat;
                        node.Checked = profileManager.Profile.ActiveCategories.Contains(cat);
                        node.Tag = cat;// Child !!
                        node.SelectedImageIndex = node.ImageIndex = 0;
                        optimizedTreeview1.Nodes.Add(node);
                    }
                }
            }
        }
        private void EnableMySelf()
        {
            this.Enabled = true;
        }
        private void DisableMySelf()
        {
            this.Enabled = false;
        }
        private bool IsCategoryExist(TreeNode pnode, string category)
        {
            if (pnode == null)
            {
                foreach (TreeNode node in optimizedTreeview1.Nodes)
                {
                    if (node.Tag.ToString() == category) return true;
                }
            }
            else
            {
                foreach (TreeNode node in pnode.Nodes)
                {
                    if (node.Tag.ToString() == category) return true;
                }
            }
            return false;
        }
        private void Profile_ConsoleSelectionChanged(object sender, EventArgs e)
        {
            optimizedTreeview1.Nodes.Clear();
            profileManager.Profile.ActiveCategories = new List<string>();
            rom_shown_ids = new List<string>();
            //System.Diagnostics.Trace.WriteLine("CLEAR");
        }
        private void Profile_RomShowed(object sender, Core.RomShowedArgs e)
        {
            if (!this.InvokeRequired)
                AddCategoriesFromRom(e);
            else
                Invoke(new AddRomCategoriesDelegate(AddCategoriesFromRom), e);
        }
        private void optimizedTreeview1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (isChangingCheck) return;
            if (e.Node == null)
                return;
            isChangingCheck = true;
            profileManager.Profile.ActiveCategories = new List<string>();
            if (e.Node.Nodes != null)
                if (e.Node.Nodes.Count > 0)
                {
                    // This is a parent node, check all as same as the parent !!
                    foreach (TreeNode t in e.Node.Nodes)
                    {
                        t.Checked = e.Node.Checked;
                    }
                }
            foreach (TreeNode tr in optimizedTreeview1.Nodes)
            {
                // Add as normal category
                if (tr.Checked)
                    profileManager.Profile.ActiveCategories.Add(tr.Tag.ToString());
                if (tr.Nodes != null)
                    if (tr.Nodes.Count > 0)
                    {
                        // This node is a parent node !!
                        foreach (TreeNode t in tr.Nodes)
                        {
                            if (t.Checked)
                                profileManager.Profile.ActiveCategories.Add(t.Tag.ToString());
                        }
                    }
            }
            profileManager.Profile.OnRomsRefreshRequest();
            isChangingCheck = false;
        }
        private void Profile_RequestCategoriesListClear(object sender, EventArgs e)
        {
            optimizedTreeview1.Nodes.Clear();
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            isChangingCheck = true;
            foreach (TreeNode tr in optimizedTreeview1.Nodes)
            {
                // Add as normal category
                tr.Checked = true;
                if (tr.Nodes != null)
                    if (tr.Nodes.Count > 0)
                    {
                        // This node is a parent node !!
                        foreach (TreeNode t in tr.Nodes)
                        {
                            t.Checked = true;
                        }
                    }
            }
            // Apply
            profileManager.Profile.ActiveCategories = new List<string>();
            foreach (TreeNode tr in optimizedTreeview1.Nodes)
            {
                // Add as normal category
                if (tr.Checked)
                    profileManager.Profile.ActiveCategories.Add(tr.Text);
                if (tr.Nodes != null)
                    if (tr.Nodes.Count > 0)
                    {
                        // This node is a parent node !!
                        foreach (TreeNode t in tr.Nodes)
                        {
                            if (t.Checked)
                                profileManager.Profile.ActiveCategories.Add(t.Text);
                        }
                    }
            }
            profileManager.Profile.OnRomsRefreshRequest();
            isChangingCheck = false;
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            isChangingCheck = true;
            foreach (TreeNode tr in optimizedTreeview1.Nodes)
            {
                // Add as normal category
                tr.Checked = false;
                if (tr.Nodes != null)
                    if (tr.Nodes.Count > 0)
                    {
                        // This node is a parent node !!
                        foreach (TreeNode t in tr.Nodes)
                        {
                            t.Checked = false;
                        }
                    }
            }
            // Apply
            profileManager.Profile.ActiveCategories = new List<string>();
            foreach (TreeNode tr in optimizedTreeview1.Nodes)
            {
                // Add as normal category
                if (tr.Checked)
                    profileManager.Profile.ActiveCategories.Add(tr.Text);
                if (tr.Nodes != null)
                    if (tr.Nodes.Count > 0)
                    {
                        // This node is a parent node !!
                        foreach (TreeNode t in tr.Nodes)
                        {
                            if (t.Checked)
                                profileManager.Profile.ActiveCategories.Add(t.Text);
                        }
                    }
            }
            profileManager.Profile.OnRomsRefreshRequest();
            isChangingCheck = false;
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            AToZ = !AToZ;
            optimizedTreeview1.TreeViewNodeSorter = new CategoryNodesSorter(AToZ);
            //optimizedTreeview1.Sort();
        }
        private void optimizedTreeview1_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {

        }
        private void optimizedTreeview1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node == null)
                return;
            if (e.Label == "" || e.Label == null)
            {
                e.CancelEdit = true;
                return;
            }
            bool isParent = e.Node.Parent == null;
            string full_category = e.Node.Tag.ToString();
            if (full_category == "")
            {
                e.CancelEdit = true;
                return;
            }
            foreach (string id in rom_shown_ids)
            {
                Rom rom = profileManager.Profile.Roms[id];
                if (rom.Categories.Count > 0)
                    if (rom.Categories.Contains(full_category))
                    {
                        int i = rom.Categories.IndexOf(full_category);
                        string parent = "";
                        string child = "";
                        bool parentAndChildCat = false;
                        string[] codes = full_category.Split('/');
                        if (codes.Length > 1)
                        {
                            parent = codes[0];
                            child = codes[1];
                            parentAndChildCat = true;
                        }
                        else
                        {
                            child = full_category;
                            parentAndChildCat = false;
                        }

                        if (isParent)
                        {
                            if (parentAndChildCat)
                            {
                                // Update it using the parent !
                                if (parent.Length != rom.Categories[i].Length)
                                { rom.Categories[i].Replace(parent, e.Label); rom.Modified = true; }
                                else
                                { rom.Categories[i] = e.Label; rom.Modified = true; }
                            }
                            else
                            {
                                // Update it using the child; since the category is not parent/child!
                                if (child.Length != rom.Categories[i].Length)
                                { rom.Categories[i].Replace(child, e.Label); rom.Modified = true; }
                                else
                                { rom.Categories[i] = e.Label; rom.Modified = true; }
                            }
                        }
                        else
                        {
                            // Update it anyway (using the child always) !
                            if (child.Length != rom.Categories[i].Length)
                            { rom.Categories[i].Replace(child, e.Label); rom.Modified = true; }
                            else
                            { rom.Categories[i] = e.Label; rom.Modified = true; }
                        }
                        e.Node.Tag = rom.Categories[i];
                    }
            }
            profileManager.Profile.OnRomMultiplePropertiesChanged();
        }
        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (optimizedTreeview1.SelectedNode == null)
                return;
            optimizedTreeview1.SelectedNode.BeginEdit();
        }
        private void editCateogriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RomPropertiesMultibleEdit frm = new RomPropertiesMultibleEdit(profileManager.Profile.SelectedRomIDS.ToArray(), null);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                profileManager.Profile.OnRequestCategoriesListClear();

                foreach (string id in profileManager.Profile.SelectedRomIDS)
                {
                    this.Profile_RomShowed(null, new RomShowedArgs(profileManager.Profile.Roms[id]));
                }
            }
        }
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            renameToolStripMenuItem.Enabled = rom_shown_ids.Count > 0;
            editCateogriesToolStripMenuItem.Enabled = profileManager.Profile.SelectedRomIDS.Count > 0;
        }
        private void Profile_RomsLoadingFinished(object sender, EventArgs e)
        {
            isLoadingRoms = false;
            if (!this.InvokeRequired)
                EnableMySelf();
            else
                Invoke(new Action(EnableMySelf));
        }
        private void Profile_RomsLoadingStarted(object sender, EventArgs e)
        {
            isLoadingRoms = true;
            if (!this.InvokeRequired)
                DisableMySelf();
            else
                Invoke(new Action(DisableMySelf));
        }
    }
    public class CategoryNodesSorter : IComparer
    {
        public CategoryNodesSorter(bool AToZ)
        {
            this.AToZ = AToZ;
        }
        private bool AToZ;
        // Compare the length of the strings, or the strings 
        // themselves, if they are the same length. 
        public int Compare(object x, object y)
        {
            TreeNode tx = x as TreeNode;
            TreeNode ty = y as TreeNode;
            string xText = tx.Tag.ToString();
            string yText = ty.Tag.ToString();

            // If they are the same length, call Compare. 
            return AToZ ? string.Compare(xText, yText) : (-1 * string.Compare(xText, yText));
        }
    }
}
