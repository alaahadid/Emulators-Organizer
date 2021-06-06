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
    partial class Form_SendTo
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_SendTo));
            this.groupBox_destinationFolder = new System.Windows.Forms.GroupBox();
            this.textBox_destinationFolder = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label_info = new System.Windows.Forms.Label();
            this.groupBox_options = new System.Windows.Forms.GroupBox();
            this.checkBox_create_related_folders = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton_copy_mode_roms_related = new System.Windows.Forms.RadioButton();
            this.radioButton_copy_mode_rom = new System.Windows.Forms.RadioButton();
            this.checkBox_moveRelatedFiles = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.checkBox_compreesPreserve = new System.Windows.Forms.CheckBox();
            this.checkBox_ignoreArchiveInCompress = new System.Windows.Forms.CheckBox();
            this.textBox_compressPassword = new System.Windows.Forms.TextBox();
            this.checkBox_CompressPassword = new System.Windows.Forms.CheckBox();
            this.comboBox_compressLevel = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_compressFormat = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_extractPass = new System.Windows.Forms.TextBox();
            this.checkBox_extractPassword = new System.Windows.Forms.CheckBox();
            this.checkBox_compressRoms = new System.Windows.Forms.CheckBox();
            this.checkBox_extractCreateFolders = new System.Windows.Forms.CheckBox();
            this.checkBox_ExtractRomsIfArchive = new System.Windows.Forms.CheckBox();
            this.checkBox_OpenDestinationFolderWhenDone = new System.Windows.Forms.CheckBox();
            this.checkBox_moveRoms = new System.Windows.Forms.CheckBox();
            this.checkBox_empty = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label_progress = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.label_progress2 = new System.Windows.Forms.Label();
            this.groupBox_destinationFolder.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox_options.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_destinationFolder
            // 
            this.groupBox_destinationFolder.Controls.Add(this.textBox_destinationFolder);
            this.groupBox_destinationFolder.Controls.Add(this.button1);
            resources.ApplyResources(this.groupBox_destinationFolder, "groupBox_destinationFolder");
            this.groupBox_destinationFolder.Name = "groupBox_destinationFolder";
            this.groupBox_destinationFolder.TabStop = false;
            // 
            // textBox_destinationFolder
            // 
            resources.ApplyResources(this.textBox_destinationFolder, "textBox_destinationFolder");
            this.textBox_destinationFolder.Name = "textBox_destinationFolder";
            this.textBox_destinationFolder.ReadOnly = true;
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label_info);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // label_info
            // 
            resources.ApplyResources(this.label_info, "label_info");
            this.label_info.Name = "label_info";
            // 
            // groupBox_options
            // 
            this.groupBox_options.Controls.Add(this.checkBox_create_related_folders);
            this.groupBox_options.Controls.Add(this.groupBox1);
            this.groupBox_options.Controls.Add(this.checkBox_moveRelatedFiles);
            this.groupBox_options.Controls.Add(this.groupBox4);
            this.groupBox_options.Controls.Add(this.textBox_extractPass);
            this.groupBox_options.Controls.Add(this.checkBox_extractPassword);
            this.groupBox_options.Controls.Add(this.checkBox_compressRoms);
            this.groupBox_options.Controls.Add(this.checkBox_extractCreateFolders);
            this.groupBox_options.Controls.Add(this.checkBox_ExtractRomsIfArchive);
            this.groupBox_options.Controls.Add(this.checkBox_OpenDestinationFolderWhenDone);
            this.groupBox_options.Controls.Add(this.checkBox_moveRoms);
            this.groupBox_options.Controls.Add(this.checkBox_empty);
            resources.ApplyResources(this.groupBox_options, "groupBox_options");
            this.groupBox_options.Name = "groupBox_options";
            this.groupBox_options.TabStop = false;
            // 
            // checkBox_create_related_folders
            // 
            resources.ApplyResources(this.checkBox_create_related_folders, "checkBox_create_related_folders");
            this.checkBox_create_related_folders.Checked = true;
            this.checkBox_create_related_folders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_create_related_folders.Name = "checkBox_create_related_folders";
            this.checkBox_create_related_folders.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.radioButton_copy_mode_roms_related);
            this.groupBox1.Controls.Add(this.radioButton_copy_mode_rom);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // radioButton1
            // 
            resources.ApplyResources(this.radioButton1, "radioButton1");
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButton_copy_mode_roms_related
            // 
            resources.ApplyResources(this.radioButton_copy_mode_roms_related, "radioButton_copy_mode_roms_related");
            this.radioButton_copy_mode_roms_related.Checked = true;
            this.radioButton_copy_mode_roms_related.Name = "radioButton_copy_mode_roms_related";
            this.radioButton_copy_mode_roms_related.TabStop = true;
            this.radioButton_copy_mode_roms_related.UseVisualStyleBackColor = true;
            this.radioButton_copy_mode_roms_related.CheckedChanged += new System.EventHandler(this.radioButton_copy_mode_roms_related_CheckedChanged);
            // 
            // radioButton_copy_mode_rom
            // 
            resources.ApplyResources(this.radioButton_copy_mode_rom, "radioButton_copy_mode_rom");
            this.radioButton_copy_mode_rom.Name = "radioButton_copy_mode_rom";
            this.radioButton_copy_mode_rom.UseVisualStyleBackColor = true;
            this.radioButton_copy_mode_rom.CheckedChanged += new System.EventHandler(this.radioButton_copy_mode_rom_CheckedChanged);
            // 
            // checkBox_moveRelatedFiles
            // 
            resources.ApplyResources(this.checkBox_moveRelatedFiles, "checkBox_moveRelatedFiles");
            this.checkBox_moveRelatedFiles.Name = "checkBox_moveRelatedFiles";
            this.checkBox_moveRelatedFiles.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkBox_compreesPreserve);
            this.groupBox4.Controls.Add(this.checkBox_ignoreArchiveInCompress);
            this.groupBox4.Controls.Add(this.textBox_compressPassword);
            this.groupBox4.Controls.Add(this.checkBox_CompressPassword);
            this.groupBox4.Controls.Add(this.comboBox_compressLevel);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.comboBox_compressFormat);
            this.groupBox4.Controls.Add(this.label2);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // checkBox_compreesPreserve
            // 
            resources.ApplyResources(this.checkBox_compreesPreserve, "checkBox_compreesPreserve");
            this.checkBox_compreesPreserve.Name = "checkBox_compreesPreserve";
            this.checkBox_compreesPreserve.UseVisualStyleBackColor = true;
            // 
            // checkBox_ignoreArchiveInCompress
            // 
            resources.ApplyResources(this.checkBox_ignoreArchiveInCompress, "checkBox_ignoreArchiveInCompress");
            this.checkBox_ignoreArchiveInCompress.Checked = true;
            this.checkBox_ignoreArchiveInCompress.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_ignoreArchiveInCompress.Name = "checkBox_ignoreArchiveInCompress";
            this.checkBox_ignoreArchiveInCompress.UseVisualStyleBackColor = true;
            // 
            // textBox_compressPassword
            // 
            resources.ApplyResources(this.textBox_compressPassword, "textBox_compressPassword");
            this.textBox_compressPassword.Name = "textBox_compressPassword";
            // 
            // checkBox_CompressPassword
            // 
            resources.ApplyResources(this.checkBox_CompressPassword, "checkBox_CompressPassword");
            this.checkBox_CompressPassword.Name = "checkBox_CompressPassword";
            this.checkBox_CompressPassword.UseVisualStyleBackColor = true;
            // 
            // comboBox_compressLevel
            // 
            this.comboBox_compressLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_compressLevel.FormattingEnabled = true;
            resources.ApplyResources(this.comboBox_compressLevel, "comboBox_compressLevel");
            this.comboBox_compressLevel.Name = "comboBox_compressLevel";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // comboBox_compressFormat
            // 
            this.comboBox_compressFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_compressFormat.FormattingEnabled = true;
            resources.ApplyResources(this.comboBox_compressFormat, "comboBox_compressFormat");
            this.comboBox_compressFormat.Name = "comboBox_compressFormat";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // textBox_extractPass
            // 
            resources.ApplyResources(this.textBox_extractPass, "textBox_extractPass");
            this.textBox_extractPass.Name = "textBox_extractPass";
            // 
            // checkBox_extractPassword
            // 
            resources.ApplyResources(this.checkBox_extractPassword, "checkBox_extractPassword");
            this.checkBox_extractPassword.Name = "checkBox_extractPassword";
            this.checkBox_extractPassword.UseVisualStyleBackColor = true;
            // 
            // checkBox_compressRoms
            // 
            resources.ApplyResources(this.checkBox_compressRoms, "checkBox_compressRoms");
            this.checkBox_compressRoms.Name = "checkBox_compressRoms";
            this.checkBox_compressRoms.UseVisualStyleBackColor = true;
            this.checkBox_compressRoms.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBox_extractCreateFolders
            // 
            resources.ApplyResources(this.checkBox_extractCreateFolders, "checkBox_extractCreateFolders");
            this.checkBox_extractCreateFolders.Checked = true;
            this.checkBox_extractCreateFolders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_extractCreateFolders.Name = "checkBox_extractCreateFolders";
            this.checkBox_extractCreateFolders.UseVisualStyleBackColor = true;
            // 
            // checkBox_ExtractRomsIfArchive
            // 
            resources.ApplyResources(this.checkBox_ExtractRomsIfArchive, "checkBox_ExtractRomsIfArchive");
            this.checkBox_ExtractRomsIfArchive.Name = "checkBox_ExtractRomsIfArchive";
            this.checkBox_ExtractRomsIfArchive.UseVisualStyleBackColor = true;
            this.checkBox_ExtractRomsIfArchive.CheckedChanged += new System.EventHandler(this.checkBox_ExtractRomsIfArchive_CheckedChanged);
            // 
            // checkBox_OpenDestinationFolderWhenDone
            // 
            resources.ApplyResources(this.checkBox_OpenDestinationFolderWhenDone, "checkBox_OpenDestinationFolderWhenDone");
            this.checkBox_OpenDestinationFolderWhenDone.Name = "checkBox_OpenDestinationFolderWhenDone";
            this.checkBox_OpenDestinationFolderWhenDone.UseVisualStyleBackColor = true;
            // 
            // checkBox_moveRoms
            // 
            resources.ApplyResources(this.checkBox_moveRoms, "checkBox_moveRoms");
            this.checkBox_moveRoms.Name = "checkBox_moveRoms";
            this.checkBox_moveRoms.UseVisualStyleBackColor = true;
            // 
            // checkBox_empty
            // 
            resources.ApplyResources(this.checkBox_empty, "checkBox_empty");
            this.checkBox_empty.Checked = true;
            this.checkBox_empty.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_empty.Name = "checkBox_empty";
            this.checkBox_empty.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // progressBar1
            // 
            resources.ApplyResources(this.progressBar1, "progressBar1");
            this.progressBar1.Name = "progressBar1";
            // 
            // label_progress
            // 
            resources.ApplyResources(this.label_progress, "label_progress");
            this.label_progress.Name = "label_progress";
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // progressBar2
            // 
            resources.ApplyResources(this.progressBar2, "progressBar2");
            this.progressBar2.Name = "progressBar2";
            // 
            // label_progress2
            // 
            resources.ApplyResources(this.label_progress2, "label_progress2");
            this.label_progress2.Name = "label_progress2";
            // 
            // Form_SendTo
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label_progress2);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.label_progress);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox_options);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox_destinationFolder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_SendTo";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_SendTo_FormClosing);
            this.groupBox_destinationFolder.ResumeLayout(false);
            this.groupBox_destinationFolder.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox_options.ResumeLayout(false);
            this.groupBox_options.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_destinationFolder;
        private System.Windows.Forms.TextBox textBox_destinationFolder;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label_info;
        private System.Windows.Forms.GroupBox groupBox_options;
        private System.Windows.Forms.CheckBox checkBox_empty;
        private System.Windows.Forms.ComboBox comboBox_compressFormat;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox_OpenDestinationFolderWhenDone;
        private System.Windows.Forms.CheckBox checkBox_moveRoms;
        private System.Windows.Forms.CheckBox checkBox_extractCreateFolders;
        private System.Windows.Forms.TextBox textBox_compressPassword;
        private System.Windows.Forms.CheckBox checkBox_CompressPassword;
        private System.Windows.Forms.ComboBox comboBox_compressLevel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox checkBox_ExtractRomsIfArchive;
        private System.Windows.Forms.CheckBox checkBox_compressRoms;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label_progress;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textBox_extractPass;
        private System.Windows.Forms.CheckBox checkBox_extractPassword;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.Label label_progress2;
        private System.Windows.Forms.CheckBox checkBox_ignoreArchiveInCompress;
        private System.Windows.Forms.CheckBox checkBox_compreesPreserve;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox checkBox_moveRelatedFiles;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton_copy_mode_roms_related;
        private System.Windows.Forms.RadioButton radioButton_copy_mode_rom;
        private System.Windows.Forms.CheckBox checkBox_create_related_folders;
    }
}