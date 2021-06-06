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
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using EmulatorsOrganizer.Services;
using EmulatorsOrganizer.Services.DefaultServices;
using EmulatorsOrganizer.Core;
using MMB;
namespace EmulatorsOrganizer.GUI
{
    public partial class CommandlinesEditor : UserControl
    {
        public CommandlinesEditor()
        {
            InitializeComponent();
        }

        private List<CommandlinesGroup> commandlineGroups;
        private LanguageResourcesService ls = (LanguageResourcesService)ServicesManager.GetService("EO Language Resources");
        private bool allowEdit = true;

        public event EventHandler CommandlinesEnableChange;
        /// <summary>
        /// Get or set the commandlines groups collection. When set, the groups get cloned.
        /// </summary>
        public List<CommandlinesGroup> CommandlineGroups
        {
            set
            {
                // Clone collection
                this.commandlineGroups = new List<CommandlinesGroup>();
                if (value != null)
                {
                    foreach (CommandlinesGroup gr in value)
                        this.commandlineGroups.Add(gr.Clone());
                }
                //reload
                optimizedTreeview1.Nodes.Clear();
                RefreshGroups();
            }
            get
            {
                return commandlineGroups;
            }
        }
        /// <summary>
        /// Get or set if commandlines edit is allowed
        /// </summary>
        public bool AllowEdit
        {
            get { return allowEdit; }
            set
            {
                allowEdit = value;
                toolStrip1.Visible = value;
                optimizedTreeview1.ContextMenuStrip = value ? contextMenuStrip1 : null;
            }
        }

        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                optimizedTreeview1.ForeColor = base.ForeColor = value;
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
                optimizedTreeview1.BackColor = textBox_show.BackColor = base.BackColor = value;
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
        public ImageViewMode BackgroundImageMode
        {
            get
            {
                return optimizedTreeview1.BackgroundImageMode;
            }
            set
            {
                optimizedTreeview1.BackgroundImageMode = value;
            }
        }
        private void RefreshGroups()
        {
            if (commandlineGroups == null)
                return;
            optimizedTreeview1.Nodes.Clear();
            foreach (CommandlinesGroup gr in commandlineGroups)
            {
                TreeNode_CommandlinesGroup TR = new TreeNode_CommandlinesGroup();
                TR.SelectedImageIndex = TR.ImageIndex = 0;// folder
                TR.CommandlinesGroup = gr;
                TR.RefreshCommandlines(1, 1, 2, 2);
                optimizedTreeview1.Nodes.Add(TR);
            }
            RefreshScriptView();
        }
        private void RefreshScriptView()
        {
            string text = "";
            foreach (CommandlinesGroup gr in commandlineGroups)
            {
                if (gr.Enabled)
                {
                    foreach (Commandline cm in gr.Commandlines)
                    {
                        if (cm.Enabled)
                        {
                            if (cm.Code.ToLower() != "<killspace>")
                            {
                                text += cm.Code + " ";
                            }
                            else
                            {
                                text = text.Substring(0, text.Length - 1);
                            }
                            foreach (Parameter par in cm.Parameters)
                            {
                                if (par.Enabled)
                                {
                                    if (par.Code.ToLower() != "<killspace>")
                                    {
                                        text += par.Code + " ";
                                    }
                                    else
                                    {
                                        text = text.Substring(0, text.Length - 1);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            textBox_show.Text = text;
        }
        public void Reset()
        {
            ManagedMessageBoxResult result = ManagedMessageBox.ShowQuestionMessage(ls["Message_AreYouSure"],
                ls["MessageCaption_Delete"]);
            if (result.ClickedButtonIndex == 1)
                return;
            commandlineGroups = new List<CommandlinesGroup>();

            CommandlinesGroup gr = new CommandlinesGroup();
            gr.Name = "<default>";
            gr.IsReadOnly = true;
            gr.Enabled = true;

            Commandline cm = new Commandline();
            cm.Name = "<default>";
            cm.Code = "<rompath>";
            cm.Enabled = true;
            cm.IsReadOnly = true;
            gr.Commandlines.Add(cm);

            commandlineGroups.Add(gr);
            RefreshScriptView();
            RefreshGroups();
        }
        public void MoveSelectedUp()
        {
            if (optimizedTreeview1.SelectedNode == null)
                return;
            if (optimizedTreeview1.SelectedNode.GetType() == typeof(TreeNode_CommandlinesGroup))
            {
                TreeNode_CommandlinesGroup GR = (TreeNode_CommandlinesGroup)optimizedTreeview1.SelectedNode;
                int index = GR.Index;

                optimizedTreeview1.Nodes.Remove(GR);

                if (index > 0)
                    index--;

                optimizedTreeview1.Nodes.Insert(index, GR);
                optimizedTreeview1.SelectedNode = GR;
                commandlineGroups = new List<CommandlinesGroup>();
                foreach (TreeNode_CommandlinesGroup tr in optimizedTreeview1.Nodes)
                {
                    commandlineGroups.Add(tr.CommandlinesGroup);
                }
            }
            else if (optimizedTreeview1.SelectedNode.GetType() == typeof(TreeNode_Commandline))
            {
                TreeNode_CommandlinesGroup GR = (TreeNode_CommandlinesGroup)optimizedTreeview1.SelectedNode.Parent;
                TreeNode_Commandline CM = (TreeNode_Commandline)optimizedTreeview1.SelectedNode;
                int index = CM.Index;

                GR.Nodes.Remove(CM);
                GR.CommandlinesGroup.Commandlines.RemoveAt(index);

                if (index > 0)
                    index--;

                GR.Nodes.Insert(index, CM);
                GR.CommandlinesGroup.Commandlines.Insert(index, CM.Commandline);

                optimizedTreeview1.SelectedNode = CM;
                GR.CommandlinesGroup.Commandlines = new List<Commandline>();
                foreach (TreeNode_Commandline tr in GR.Nodes)
                    GR.CommandlinesGroup.Commandlines.Add(tr.Commandline);
            }
            else if (optimizedTreeview1.SelectedNode.GetType() == typeof(TreeNode_Parameter))
            {
                TreeNode_Commandline CM = (TreeNode_Commandline)optimizedTreeview1.SelectedNode.Parent;
                TreeNode_Parameter PA = (TreeNode_Parameter)optimizedTreeview1.SelectedNode;
                int index = PA.Index;

                CM.Nodes.Remove(PA);
                CM.Commandline.Parameters.RemoveAt(index);

                if (index > 0)
                    index--;

                CM.Nodes.Insert(index, PA);
                CM.Commandline.Parameters.Insert(index, PA.Parameter);

                optimizedTreeview1.SelectedNode = PA;
                CM.Commandline.Parameters = new List<Parameter>();
                foreach (TreeNode_Parameter tr in CM.Nodes)
                    CM.Commandline.Parameters.Add(tr.Parameter);
            }
            RefreshScriptView();
        }
        public void MoveSelectedDown()
        {
            if (optimizedTreeview1.SelectedNode == null)
                return;
            if (optimizedTreeview1.SelectedNode.GetType() == typeof(TreeNode_CommandlinesGroup))
            {
                TreeNode_CommandlinesGroup GR = (TreeNode_CommandlinesGroup)optimizedTreeview1.SelectedNode;
                int index = GR.Index;

                optimizedTreeview1.Nodes.Remove(GR);

                if (index < optimizedTreeview1.Nodes.Count)
                    index++;

                optimizedTreeview1.Nodes.Insert(index, GR);
                optimizedTreeview1.SelectedNode = GR;
                commandlineGroups = new List<CommandlinesGroup>();
                foreach (TreeNode_CommandlinesGroup tr in optimizedTreeview1.Nodes)
                {
                    commandlineGroups.Add(tr.CommandlinesGroup);
                }
            }
            else if (optimizedTreeview1.SelectedNode.GetType() == typeof(TreeNode_Commandline))
            {
                TreeNode_CommandlinesGroup GR = (TreeNode_CommandlinesGroup)optimizedTreeview1.SelectedNode.Parent;
                TreeNode_Commandline CM = (TreeNode_Commandline)optimizedTreeview1.SelectedNode;
                int index = CM.Index;

                GR.Nodes.Remove(CM);
                GR.CommandlinesGroup.Commandlines.RemoveAt(index);

                if (index < GR.Nodes.Count)
                    index++;

                GR.Nodes.Insert(index, CM);
                GR.CommandlinesGroup.Commandlines.Insert(index, CM.Commandline);

                optimizedTreeview1.SelectedNode = CM;
                GR.CommandlinesGroup.Commandlines = new List<Commandline>();
                foreach (TreeNode_Commandline tr in GR.Nodes)
                    GR.CommandlinesGroup.Commandlines.Add(tr.Commandline);
            }
            else if (optimizedTreeview1.SelectedNode.GetType() == typeof(TreeNode_Parameter))
            {
                TreeNode_Commandline CM = (TreeNode_Commandline)optimizedTreeview1.SelectedNode.Parent;
                TreeNode_Parameter PA = (TreeNode_Parameter)optimizedTreeview1.SelectedNode;
                int index = PA.Index;

                CM.Nodes.Remove(PA);
                CM.Commandline.Parameters.RemoveAt(index);

                if (index < CM.Nodes.Count)
                    index++;

                CM.Nodes.Insert(index, PA);
                CM.Commandline.Parameters.Insert(index, PA.Parameter);

                optimizedTreeview1.SelectedNode = PA;
                CM.Commandline.Parameters = new List<Parameter>();
                foreach (TreeNode_Parameter tr in CM.Nodes)
                    CM.Commandline.Parameters.Add(tr.Parameter);
            }
            RefreshScriptView();
        }
        private void AddCommandlinesGroup(object sender, EventArgs e)
        {
            Form_EnterName frm = new Form_EnterName(ls["Title_TheGroupName"], ls["NewGroup"], true, false);
            frm.StartPosition = FormStartPosition.Manual;
            frm.Location = Cursor.Position;
            frm.OkPressed += new EventHandler<EnterNameFormOkPressedArgs>(frm_OkPressed);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                CommandlinesGroup gr = new CommandlinesGroup();
                gr.Name = frm.EnteredName;
                commandlineGroups.Add(gr);

                TreeNode_CommandlinesGroup TR = new TreeNode_CommandlinesGroup();
                TR.SelectedImageIndex = TR.ImageIndex = 0;
                TR.CommandlinesGroup = gr;
                TR.RefreshCommandlines(1, 1, 2, 2);
                optimizedTreeview1.Nodes.Add(TR);
                RefreshScriptView();
            }
        }
        private void AddCommandline(object sender, EventArgs e)
        {
            if (optimizedTreeview1.SelectedNode == null)
                return;
            if (optimizedTreeview1.SelectedNode.GetType() != typeof(TreeNode_CommandlinesGroup))
                return;
            CommandlinesGroup gr = ((TreeNode_CommandlinesGroup)optimizedTreeview1.SelectedNode).CommandlinesGroup;
            if (gr.IsReadOnly)
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_YouCantAddItemsToThisGroup"], ls["MessageCaption_Error"]);
                return;
            }
            Commandline cm = new Commandline();
            Form_CommandlineProperties frm = new Form_CommandlineProperties(cm);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                cm.Enabled = true;
                cm.Code = frm.EnteredCode;
                cm.Name = frm.EnteredName;
                gr.Commandlines.Add(cm);
                ((TreeNode_CommandlinesGroup)optimizedTreeview1.SelectedNode).RefreshCommandlines(1, 1, 2, 2);
                RefreshScriptView();
            }
        }
        private void EditSelected(object sender, EventArgs e)
        {
            if (optimizedTreeview1.SelectedNode == null)
                return;
            if (optimizedTreeview1.SelectedNode.GetType() == typeof(TreeNode_CommandlinesGroup))
            {
                CommandlinesGroup gr = ((TreeNode_CommandlinesGroup)optimizedTreeview1.SelectedNode).CommandlinesGroup;

                List<string> originalNames = new List<string>();
                foreach (TreeNode_CommandlinesGroup tr in optimizedTreeview1.Nodes)
                {
                    originalNames.Add(tr.CommandlinesGroup.Name.ToLower());
                }
                originalNames.Remove(gr.Name.ToLower());

                Form_EnterName frm = new Form_EnterName(ls["Title_TheGroupName"], gr.Name, true, false);
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    if (!originalNames.Contains(frm.EnteredName.ToLower()))
                    {
                        gr.Name = frm.EnteredName;
                        ((TreeNode_CommandlinesGroup)optimizedTreeview1.SelectedNode).RefreshName();
                    }
                    else
                    {
                        ManagedMessageBox.ShowErrorMessage(ls["Message_ThisNameAlreadyTaken"],
                        ls["MessageCaption_Error"]);
                    }
                }
            }
            else if (optimizedTreeview1.SelectedNode.GetType() == typeof(TreeNode_Commandline))
            {
                Commandline cm = ((TreeNode_Commandline)optimizedTreeview1.SelectedNode).Commandline;

                Form_CommandlineProperties frm = new Form_CommandlineProperties(cm);
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    cm.Name = frm.EnteredName;
                    cm.Code = frm.EnteredCode;
                    ((TreeNode_Commandline)optimizedTreeview1.SelectedNode).RefreshName();
                }
            }
            else if (optimizedTreeview1.SelectedNode.GetType() == typeof(TreeNode_Parameter))
            {
                Parameter par = ((TreeNode_Parameter)optimizedTreeview1.SelectedNode).Parameter;

                Form_CommandlineProperties frm = new Form_CommandlineProperties(par);
                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    par.Name = frm.EnteredName;
                    par.Code = frm.EnteredCode;
                    ((TreeNode_Parameter)optimizedTreeview1.SelectedNode).RefreshName();
                }
            }
            RefreshScriptView();
        }
        private void AddParameter(object sender, EventArgs e)
        {
            if (optimizedTreeview1.SelectedNode == null)
                return;
            if (optimizedTreeview1.SelectedNode.GetType() != typeof(TreeNode_Commandline))
                return;
            Commandline cm = ((TreeNode_Commandline)optimizedTreeview1.SelectedNode).Commandline;
            if (cm.IsReadOnly)
            {
                ManagedMessageBox.ShowErrorMessage(ls["Message_YouCantAddItemsToThisCommandline"], ls["MessageCaption_Error"]);
                return;
            }
            Parameter par = new Parameter();
            Form_CommandlineProperties frm = new Form_CommandlineProperties(par);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                par.Enabled = true;
                par.Code = frm.EnteredCode;
                par.Name = frm.EnteredName;
                cm.Parameters.Add(par);
                ((TreeNode_Commandline)optimizedTreeview1.SelectedNode).RefreshParameters(2, 2);
                RefreshScriptView();
            }
        }
        private void DeleteSelected(object sender, EventArgs e)
        {
            if (optimizedTreeview1.SelectedNode == null)
                return;
            ManagedMessageBoxResult result = ManagedMessageBox.ShowQuestionMessage(ls["Message_AreYouSure"], ls["MessageCaption_Delete"]);
            if (result.ClickedButtonIndex == 1)
                return;
            if (optimizedTreeview1.SelectedNode.GetType() == typeof(TreeNode_CommandlinesGroup))
            {
                CommandlinesGroup gr = ((TreeNode_CommandlinesGroup)optimizedTreeview1.SelectedNode).CommandlinesGroup;
                if (gr.IsReadOnly)
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_YouCantDeleteThisItem"], ls["MessageCaption_Error"]);
                    return;
                }
                commandlineGroups.Remove(gr);
                RefreshGroups();
            }
            else if (optimizedTreeview1.SelectedNode.GetType() == typeof(TreeNode_Commandline))
            {
                CommandlinesGroup gr = ((TreeNode_CommandlinesGroup)optimizedTreeview1.SelectedNode.Parent).CommandlinesGroup;
                Commandline cm = ((TreeNode_Commandline)optimizedTreeview1.SelectedNode).Commandline;
                if (gr.IsReadOnly)
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_YouCantDeleteThisItem"], ls["MessageCaption_Error"]);
                    return;
                }
                gr.Commandlines.Remove(cm);
                ((TreeNode_CommandlinesGroup)optimizedTreeview1.SelectedNode.Parent).RefreshCommandlines(1, 1, 2, 2);
            }
            else if (optimizedTreeview1.SelectedNode.GetType() == typeof(TreeNode_Parameter))
            {
                Commandline cm = ((TreeNode_Commandline)optimizedTreeview1.SelectedNode.Parent).Commandline;
                Parameter par = ((TreeNode_Parameter)optimizedTreeview1.SelectedNode).Parameter;
                if (cm.IsReadOnly)
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_YouCantDeleteThisItem"], ls["MessageCaption_Error"]);
                    return;
                }
                cm.Parameters.Remove(par);
                ((TreeNode_Commandline)optimizedTreeview1.SelectedNode.Parent).RefreshParameters(2, 2);
            }
            RefreshScriptView();
        }
        private void SaveAsXML(object sender, EventArgs e)
        {
            SaveFileDialog sav = new SaveFileDialog();
            sav.Filter = "XML (*xml)|*.xml";
            if (sav.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                CommandLinesXML xmll = new CommandLinesXML();
                foreach (TreeNode_CommandlinesGroup GR in optimizedTreeview1.Nodes)
                {
                    xmll.CommandLinesGroups.Add(GR.CommandlinesGroup);
                }

                XmlSerializer SER = new XmlSerializer(typeof(CommandLinesXML));
                Stream STR = new FileStream(sav.FileName, FileMode.Create);
                SER.Serialize(STR, xmll);
                STR.Close();
            }
        }
        private void OpenXML(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "XML (*xml)|*.xml";
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                XmlSerializer SER = new XmlSerializer(typeof(CommandLinesXML));
                Stream STR = new FileStream(op.FileName, FileMode.Open, FileAccess.Read);
                CommandLinesXML LOADED = (CommandLinesXML)SER.Deserialize(STR);
                STR.Close();

                if (LOADED != null)
                {
                    if (MessageBox.Show(ls["Message_AreYouSureYouWantToLoadThisFileAndDiscardCurrentChanges"],
                       ls["MessageCaption_LoadXml"], MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        //CommandlineGroups = LOADED.CommandLinesGroups;
                        commandlineGroups.Clear();
                        foreach (CommandlinesGroup gr in LOADED.CommandLinesGroups)
                        {
                            commandlineGroups.Add(gr.Clone());
                        }
                        //load the groups
                        RefreshGroups();
                    }
                }
            }
        }

        private void frm_OkPressed(object sender, EnterNameFormOkPressedArgs e)
        {
            foreach (CommandlinesGroup gr in commandlineGroups)
            {
                if (gr.Name.ToLower() == e.NameEntered.ToLower())
                {
                    ManagedMessageBox.ShowErrorMessage(ls["Message_ThisNameAlreadyTakenByAnotherGroup"], ls["MessageCaption_Error"]);
                    e.Cancel = true;
                }
            }
        }
        private void toolStripSplitButton1_DropDownOpening(object sender, EventArgs e)
        {
            if (optimizedTreeview1.SelectedNode == null)
            {
                commandlineToolStripMenuItem.Enabled = parameterToolStripMenuItem.Enabled = false;
                return;
            }
            commandlineToolStripMenuItem.Enabled = optimizedTreeview1.SelectedNode.GetType() == typeof(TreeNode_CommandlinesGroup);
            parameterToolStripMenuItem.Enabled = optimizedTreeview1.SelectedNode.GetType() == typeof(TreeNode_Commandline);
        }
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (optimizedTreeview1.SelectedNode == null)
            {
                deleteToolStripMenuItem.Enabled = commandlineToolStripMenuItem1.Enabled = parameterToolStripMenuItem1.Enabled = false;
                return;
            }
            else
            {
                deleteToolStripMenuItem.Enabled = true;
            }
            commandlineToolStripMenuItem1.Enabled = optimizedTreeview1.SelectedNode.GetType() == typeof(TreeNode_CommandlinesGroup);
            parameterToolStripMenuItem1.Enabled = optimizedTreeview1.SelectedNode.GetType() == typeof(TreeNode_Commandline);
        }
        private void treeView1_Click(object sender, EventArgs e)
        {
            DeleteButton.Enabled = (optimizedTreeview1.SelectedNode != null);
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Reset();
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            MoveSelectedUp();
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            MoveSelectedDown();
        }
        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null)
                return;

            if (e.Node.GetType() == typeof(TreeNode_CommandlinesGroup))
            {
                TreeNode_CommandlinesGroup gr = (TreeNode_CommandlinesGroup)e.Node;
                ((TreeNode_CommandlinesGroup)e.Node).CommandlinesGroup.Enabled = e.Node.Checked;
            }
            else if (e.Node.GetType() == typeof(TreeNode_Commandline))
            {
                TreeNode_CommandlinesGroup gr = (TreeNode_CommandlinesGroup)e.Node.Parent;
                ((TreeNode_Commandline)e.Node).Commandline.Enabled = e.Node.Checked;
            }
            else if (e.Node.GetType() == typeof(TreeNode_Parameter))
            {
                TreeNode_CommandlinesGroup gr = (TreeNode_CommandlinesGroup)e.Node.Parent.Parent;
                ((TreeNode_Parameter)e.Node).Parameter.Enabled = e.Node.Checked;
            }
            RefreshScriptView();

            if (CommandlinesEnableChange != null)
                CommandlinesEnableChange(this, new EventArgs());
        }
        private void optimizedTreeview1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
    }
    public class TreeNode_CommandlinesGroup : TreeNode
    {
        CommandlinesGroup gr;
        public CommandlinesGroup CommandlinesGroup
        { get { return gr; } set { gr = value; RefreshName(); } }
        public void RefreshName()
        {
            this.Text = gr.Name;
            this.Checked = gr.Enabled;
        }
        public void RefreshCommandlines(int imgIndex, int SimgIndex, int imgIndexForCMD, int SimgIndexForCMD)
        {
            Nodes.Clear();
            foreach (Commandline cm in gr.Commandlines)
            {
                TreeNode_Commandline TR = new TreeNode_Commandline();
                TR.Commandline = cm;
                TR.ImageIndex = imgIndex;
                TR.SelectedImageIndex = SimgIndex;
                TR.RefreshParameters(imgIndexForCMD, SimgIndexForCMD);
                Nodes.Add(TR);
            }
        }
    }
    public class TreeNode_Commandline : TreeNode
    {
        Commandline cm;
        public Commandline Commandline
        { get { return cm; } set { cm = value; RefreshName(); } }
        public void RefreshName()
        {
            this.Text = cm.Name + " (" + cm.Code + ")";
            this.Checked = cm.Enabled;
        }
        public void RefreshParameters(int imgIndex, int SimgIndex)
        {
            Nodes.Clear();
            foreach (Parameter pr in cm.Parameters)
            {
                TreeNode_Parameter TR = new TreeNode_Parameter();
                TR.Parameter = pr;
                TR.ImageIndex = imgIndex;
                TR.SelectedImageIndex = SimgIndex;
                Nodes.Add(TR);
            }
        }
    }
    public class TreeNode_Parameter : TreeNode
    {
        Parameter pr;
        public Parameter Parameter
        { get { return pr; } set { pr = value; RefreshName(); } }
        public void RefreshName()
        {
            this.Text = pr.Name + " (" + pr.Code + ")";
            this.Checked = pr.Enabled;
        }
    }
}
