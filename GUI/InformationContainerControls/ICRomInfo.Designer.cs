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
namespace EmulatorsOrganizer.GUI
{
    partial class ICRomInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ICRomInfo));
            this.panel1 = new System.Windows.Forms.Panel();
            this.rating1 = new EmulatorsOrganizer.GUI.Rating();
            this.label_path = new System.Windows.Forms.Label();
            this.label_size = new System.Windows.Forms.Label();
            this.label_playCounters = new System.Windows.Forms.Label();
            this.label_name = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.imagePanel1 = new EmulatorsOrganizer.Core.ImagePanel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.timer_thumb = new System.Windows.Forms.Timer(this.components);
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rating1);
            this.panel1.Controls.Add(this.label_path);
            this.panel1.Controls.Add(this.label_size);
            this.panel1.Controls.Add(this.label_playCounters);
            this.panel1.Controls.Add(this.label_name);
            this.panel1.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // rating1
            // 
            resources.ApplyResources(this.rating1, "rating1");
            this.rating1.Name = "rating1";
            this.rating1.rating = 0;
            this.rating1.RatingChanged += new System.EventHandler(this.rating1_RatingChanged);
            // 
            // label_path
            // 
            this.label_path.AutoEllipsis = true;
            resources.ApplyResources(this.label_path, "label_path");
            this.label_path.Name = "label_path";
            // 
            // label_size
            // 
            this.label_size.AutoEllipsis = true;
            resources.ApplyResources(this.label_size, "label_size");
            this.label_size.Name = "label_size";
            // 
            // label_playCounters
            // 
            this.label_playCounters.AutoEllipsis = true;
            resources.ApplyResources(this.label_playCounters, "label_playCounters");
            this.label_playCounters.Name = "label_playCounters";
            // 
            // label_name
            // 
            this.label_name.AutoEllipsis = true;
            resources.ApplyResources(this.label_name, "label_name");
            this.label_name.Name = "label_name";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.imagePanel1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // imagePanel1
            // 
            resources.ApplyResources(this.imagePanel1, "imagePanel1");
            this.imagePanel1.ImageToView = null;
            this.imagePanel1.ImageViewMode = EmulatorsOrganizer.Core.ImageViewMode.StretchIfLarger;
            this.imagePanel1.Name = "imagePanel1";
            // 
            // timer_thumb
            // 
            this.timer_thumb.Interval = 2000;
            this.timer_thumb.Tick += new System.EventHandler(this.timer_thumb_Tick);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            resources.ApplyResources(this.dataGridView1, "dataGridView1");
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView1_CellBeginEdit);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            // 
            // Column1
            // 
            resources.ApplyResources(this.Column1, "Column1");
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            resources.ApplyResources(this.Column2, "Column2");
            this.Column2.Name = "Column2";
            // 
            // ICRomInfo
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.Name = "ICRomInfo";
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Core.ImagePanel imagePanel1;
        private System.Windows.Forms.Label label_path;
        private System.Windows.Forms.Label label_size;
        private System.Windows.Forms.Label label_playCounters;
        private System.Windows.Forms.Label label_name;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Timer timer_thumb;
        private Rating rating1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
    }
}
