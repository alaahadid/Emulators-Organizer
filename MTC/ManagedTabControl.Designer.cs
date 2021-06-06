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
namespace MTC
{
    partial class ManagedTabControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.managedTabControlPanel1 = new MTC.ManagedTabControlPanel();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.panel_controls = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.managedTabControlPanel1);
            this.panel1.Controls.Add(this.hScrollBar1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(451, 22);
            this.panel1.TabIndex = 1;
            // 
            // managedTabControlPanel1
            // 
            this.managedTabControlPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.managedTabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.managedTabControlPanel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.managedTabControlPanel1.Location = new System.Drawing.Point(0, 0);
            this.managedTabControlPanel1.Name = "managedTabControlPanel1";
            this.managedTabControlPanel1.Size = new System.Drawing.Size(425, 22);
            this.managedTabControlPanel1.TabIndex = 0;
            this.managedTabControlPanel1.Text = "managedTabControlPanel1";
            this.managedTabControlPanel1.SelectedTabPageIndexChanged += new System.EventHandler(this.managedTabControlPanel1_SelectedTabPageIndexChanged);
            this.managedTabControlPanel1.TabPageCloseRequest += new System.EventHandler<MTC.MTCTabPageCloseArgs>(this.managedTabControlPanel1_TabPageCloseRequest);
            this.managedTabControlPanel1.RefreshScrollBar += new System.EventHandler(this.managedTabControlPanel1_RefreshScrollBar);
            this.managedTabControlPanel1.ShowTabPageToolTipRequest += new System.EventHandler<MTC.MTCTabPageShowToolTipArgs>(this.managedTabControlPanel1_ShowTabPageToolTipRequest);
            this.managedTabControlPanel1.ClearToolTip += new System.EventHandler(this.managedTabControlPanel1_ClearToolTip);
            this.managedTabControlPanel1.AfterTabPageReorder += new System.EventHandler(this.managedTabControlPanel1_AfterTabPageReorder);
            this.managedTabControlPanel1.TabPageDrag += new System.EventHandler<MTC.MTCTabPageDragArgs>(this.managedTabControlPanel1_TabPageDrag_1);
            this.managedTabControlPanel1.ScrollToSelectedTabPage += new System.EventHandler(this.managedTabControlPanel1_ScrollToSelectedTabPage);
            this.managedTabControlPanel1.BeforeAutoDragAndDrop += new System.EventHandler(this.managedTabControlPanel1_BeforeAutoDragAndDrop);
            this.managedTabControlPanel1.AfterAutoDragAndDrop += new System.EventHandler(this.managedTabControlPanel1_AfterAutoDragAndDrop);
            this.managedTabControlPanel1.TabPagesCleared += new System.EventHandler(this.managedTabControlPanel1_TabPagesCleared);
            this.managedTabControlPanel1.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.managedTabControlPanel1_GiveFeedback);
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.hScrollBar1.Location = new System.Drawing.Point(425, 0);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(26, 22);
            this.hScrollBar1.SmallChange = 10;
            this.hScrollBar1.TabIndex = 0;
            this.hScrollBar1.Visible = false;
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
            // 
            // panel_controls
            // 
            this.panel_controls.BackColor = System.Drawing.SystemColors.Control;
            this.panel_controls.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_controls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_controls.Location = new System.Drawing.Point(0, 22);
            this.panel_controls.Name = "panel_controls";
            this.panel_controls.Size = new System.Drawing.Size(451, 409);
            this.panel_controls.TabIndex = 2;
            this.panel_controls.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.panel_controls_GiveFeedback);
            // 
            // ManagedTabControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_controls);
            this.Controls.Add(this.panel1);
            this.Name = "ManagedTabControl";
            this.Size = new System.Drawing.Size(451, 431);
            this.Resize += new System.EventHandler(this.ManagedTabControl_Resize);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.Panel panel_controls;
        private System.Windows.Forms.ToolTip toolTip1;
        private ManagedTabControlPanel managedTabControlPanel1;
    }
}
