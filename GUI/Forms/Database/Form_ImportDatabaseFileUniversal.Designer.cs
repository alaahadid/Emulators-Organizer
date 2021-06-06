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
    partial class Form_ImportDatabaseFileUniversal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_ImportDatabaseFileUniversal));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_dbType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox_dbFile = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox_add_filters = new System.Windows.Forms.CheckBox();
            this.checkBox_import_category = new System.Windows.Forms.CheckBox();
            this.button3 = new System.Windows.Forms.Button();
            this.checkBox_turboe = new System.Windows.Forms.CheckBox();
            this.checkBox_cahceOndisk = new System.Windows.Forms.CheckBox();
            this.checkBox_archive_perfect_match = new System.Windows.Forms.CheckBox();
            this.checkBox_forNesArchiveTweeks = new System.Windows.Forms.CheckBox();
            this.textBox_archive_extesnions = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_archivePassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox_check_inside_archive = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox_parent_not_match = new System.Windows.Forms.GroupBox();
            this.radioButton_parent_not_match_keep_parent_if_one_child_match = new System.Windows.Forms.RadioButton();
            this.radioButton_parent_not_mathc_free_matched_children = new System.Windows.Forms.RadioButton();
            this.checkBox_parent_not_match_keep_on_one_child_match_dont_keep_unmathced_children = new System.Windows.Forms.CheckBox();
            this.groupBox_parent_match = new System.Windows.Forms.GroupBox();
            this.radioButton_parent_match_keep_all_children = new System.Windows.Forms.RadioButton();
            this.radioButton_parent_matche_keep_matched_only = new System.Windows.Forms.RadioButton();
            this.checkBox_delete_not_matched_related = new System.Windows.Forms.CheckBox();
            this.checkBox_delete_not_matched_files = new System.Windows.Forms.CheckBox();
            this.checkBox_delete_roms_nott_found = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.radioButton_compare_rom_file_name = new System.Windows.Forms.RadioButton();
            this.radioButton_compare_sha1 = new System.Windows.Forms.RadioButton();
            this.radioButton_compare_md5 = new System.Windows.Forms.RadioButton();
            this.radioButton_compare_crc = new System.Windows.Forms.RadioButton();
            this.radioButton_compare_name = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.panel_db_options = new System.Windows.Forms.Panel();
            this.groupBox_status = new System.Windows.Forms.GroupBox();
            this.progressBar_slave = new System.Windows.Forms.ProgressBar();
            this.label_status_sub = new System.Windows.Forms.Label();
            this.progressBar_master = new System.Windows.Forms.ProgressBar();
            this.label_status_master = new System.Windows.Forms.Label();
            this.button_start = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBox_rename_parent = new System.Windows.Forms.CheckBox();
            this.checkBox_apply_rom_data_info = new System.Windows.Forms.CheckBox();
            this.checkBox_rename_rom = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox_parent_not_match.SuspendLayout();
            this.groupBox_parent_match.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox_status.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.comboBox_dbType);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.textBox_dbFile);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // comboBox_dbType
            // 
            this.comboBox_dbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_dbType.FormattingEnabled = true;
            resources.ApplyResources(this.comboBox_dbType, "comboBox_dbType");
            this.comboBox_dbType.Name = "comboBox_dbType";
            this.toolTip1.SetToolTip(this.comboBox_dbType, resources.GetString("comboBox_dbType.ToolTip"));
            this.comboBox_dbType.SelectedIndexChanged += new System.EventHandler(this.comboBox_dbType_SelectedIndexChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.changeDatabaseFile);
            // 
            // textBox_dbFile
            // 
            resources.ApplyResources(this.textBox_dbFile, "textBox_dbFile");
            this.textBox_dbFile.Name = "textBox_dbFile";
            this.textBox_dbFile.ReadOnly = true;
            this.toolTip1.SetToolTip(this.textBox_dbFile, resources.GetString("textBox_dbFile.ToolTip"));
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox_add_filters);
            this.groupBox2.Controls.Add(this.checkBox_import_category);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.checkBox_turboe);
            this.groupBox2.Controls.Add(this.checkBox_cahceOndisk);
            this.groupBox2.Controls.Add(this.checkBox_archive_perfect_match);
            this.groupBox2.Controls.Add(this.checkBox_forNesArchiveTweeks);
            this.groupBox2.Controls.Add(this.textBox_archive_extesnions);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.textBox_archivePassword);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.checkBox_check_inside_archive);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // checkBox_add_filters
            // 
            resources.ApplyResources(this.checkBox_add_filters, "checkBox_add_filters");
            this.checkBox_add_filters.Checked = true;
            this.checkBox_add_filters.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_add_filters.Name = "checkBox_add_filters";
            this.toolTip1.SetToolTip(this.checkBox_add_filters, resources.GetString("checkBox_add_filters.ToolTip"));
            this.checkBox_add_filters.UseVisualStyleBackColor = true;
            // 
            // checkBox_import_category
            // 
            resources.ApplyResources(this.checkBox_import_category, "checkBox_import_category");
            this.checkBox_import_category.Checked = true;
            this.checkBox_import_category.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_import_category.Name = "checkBox_import_category";
            this.toolTip1.SetToolTip(this.checkBox_import_category, resources.GetString("checkBox_import_category.ToolTip"));
            this.checkBox_import_category.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.toolTip1.SetToolTip(this.button3, resources.GetString("button3.ToolTip"));
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // checkBox_turboe
            // 
            resources.ApplyResources(this.checkBox_turboe, "checkBox_turboe");
            this.checkBox_turboe.Name = "checkBox_turboe";
            this.toolTip1.SetToolTip(this.checkBox_turboe, resources.GetString("checkBox_turboe.ToolTip"));
            this.checkBox_turboe.UseVisualStyleBackColor = true;
            // 
            // checkBox_cahceOndisk
            // 
            resources.ApplyResources(this.checkBox_cahceOndisk, "checkBox_cahceOndisk");
            this.checkBox_cahceOndisk.Name = "checkBox_cahceOndisk";
            this.toolTip1.SetToolTip(this.checkBox_cahceOndisk, resources.GetString("checkBox_cahceOndisk.ToolTip"));
            this.checkBox_cahceOndisk.UseVisualStyleBackColor = true;
            // 
            // checkBox_archive_perfect_match
            // 
            resources.ApplyResources(this.checkBox_archive_perfect_match, "checkBox_archive_perfect_match");
            this.checkBox_archive_perfect_match.Name = "checkBox_archive_perfect_match";
            this.toolTip1.SetToolTip(this.checkBox_archive_perfect_match, resources.GetString("checkBox_archive_perfect_match.ToolTip"));
            this.checkBox_archive_perfect_match.UseVisualStyleBackColor = true;
            // 
            // checkBox_forNesArchiveTweeks
            // 
            resources.ApplyResources(this.checkBox_forNesArchiveTweeks, "checkBox_forNesArchiveTweeks");
            this.checkBox_forNesArchiveTweeks.Name = "checkBox_forNesArchiveTweeks";
            this.toolTip1.SetToolTip(this.checkBox_forNesArchiveTweeks, resources.GetString("checkBox_forNesArchiveTweeks.ToolTip"));
            this.checkBox_forNesArchiveTweeks.UseVisualStyleBackColor = true;
            this.checkBox_forNesArchiveTweeks.CheckedChanged += new System.EventHandler(this.checkBox_forNesArchiveTweeks_CheckedChanged);
            // 
            // textBox_archive_extesnions
            // 
            resources.ApplyResources(this.textBox_archive_extesnions, "textBox_archive_extesnions");
            this.textBox_archive_extesnions.Name = "textBox_archive_extesnions";
            this.toolTip1.SetToolTip(this.textBox_archive_extesnions, resources.GetString("textBox_archive_extesnions.ToolTip"));
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // textBox_archivePassword
            // 
            resources.ApplyResources(this.textBox_archivePassword, "textBox_archivePassword");
            this.textBox_archivePassword.Name = "textBox_archivePassword";
            this.toolTip1.SetToolTip(this.textBox_archivePassword, resources.GetString("textBox_archivePassword.ToolTip"));
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // checkBox_check_inside_archive
            // 
            resources.ApplyResources(this.checkBox_check_inside_archive, "checkBox_check_inside_archive");
            this.checkBox_check_inside_archive.Name = "checkBox_check_inside_archive";
            this.toolTip1.SetToolTip(this.checkBox_check_inside_archive, resources.GetString("checkBox_check_inside_archive.ToolTip"));
            this.checkBox_check_inside_archive.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox_parent_not_match);
            this.groupBox3.Controls.Add(this.groupBox_parent_match);
            this.groupBox3.Controls.Add(this.checkBox_delete_not_matched_related);
            this.groupBox3.Controls.Add(this.checkBox_delete_not_matched_files);
            this.groupBox3.Controls.Add(this.checkBox_delete_roms_nott_found);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // groupBox_parent_not_match
            // 
            this.groupBox_parent_not_match.Controls.Add(this.radioButton_parent_not_match_keep_parent_if_one_child_match);
            this.groupBox_parent_not_match.Controls.Add(this.radioButton_parent_not_mathc_free_matched_children);
            this.groupBox_parent_not_match.Controls.Add(this.checkBox_parent_not_match_keep_on_one_child_match_dont_keep_unmathced_children);
            resources.ApplyResources(this.groupBox_parent_not_match, "groupBox_parent_not_match");
            this.groupBox_parent_not_match.Name = "groupBox_parent_not_match";
            this.groupBox_parent_not_match.TabStop = false;
            // 
            // radioButton_parent_not_match_keep_parent_if_one_child_match
            // 
            resources.ApplyResources(this.radioButton_parent_not_match_keep_parent_if_one_child_match, "radioButton_parent_not_match_keep_parent_if_one_child_match");
            this.radioButton_parent_not_match_keep_parent_if_one_child_match.Checked = true;
            this.radioButton_parent_not_match_keep_parent_if_one_child_match.Name = "radioButton_parent_not_match_keep_parent_if_one_child_match";
            this.radioButton_parent_not_match_keep_parent_if_one_child_match.TabStop = true;
            this.toolTip1.SetToolTip(this.radioButton_parent_not_match_keep_parent_if_one_child_match, resources.GetString("radioButton_parent_not_match_keep_parent_if_one_child_match.ToolTip"));
            this.radioButton_parent_not_match_keep_parent_if_one_child_match.UseVisualStyleBackColor = true;
            this.radioButton_parent_not_match_keep_parent_if_one_child_match.CheckedChanged += new System.EventHandler(this.radioButton_parent_not_match_keep_parent_if_one_child_match_CheckedChanged);
            // 
            // radioButton_parent_not_mathc_free_matched_children
            // 
            resources.ApplyResources(this.radioButton_parent_not_mathc_free_matched_children, "radioButton_parent_not_mathc_free_matched_children");
            this.radioButton_parent_not_mathc_free_matched_children.Name = "radioButton_parent_not_mathc_free_matched_children";
            this.toolTip1.SetToolTip(this.radioButton_parent_not_mathc_free_matched_children, resources.GetString("radioButton_parent_not_mathc_free_matched_children.ToolTip"));
            this.radioButton_parent_not_mathc_free_matched_children.UseVisualStyleBackColor = true;
            // 
            // checkBox_parent_not_match_keep_on_one_child_match_dont_keep_unmathced_children
            // 
            resources.ApplyResources(this.checkBox_parent_not_match_keep_on_one_child_match_dont_keep_unmathced_children, "checkBox_parent_not_match_keep_on_one_child_match_dont_keep_unmathced_children");
            this.checkBox_parent_not_match_keep_on_one_child_match_dont_keep_unmathced_children.Checked = true;
            this.checkBox_parent_not_match_keep_on_one_child_match_dont_keep_unmathced_children.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_parent_not_match_keep_on_one_child_match_dont_keep_unmathced_children.Name = "checkBox_parent_not_match_keep_on_one_child_match_dont_keep_unmathced_children";
            this.toolTip1.SetToolTip(this.checkBox_parent_not_match_keep_on_one_child_match_dont_keep_unmathced_children, resources.GetString("checkBox_parent_not_match_keep_on_one_child_match_dont_keep_unmathced_children.To" +
            "olTip"));
            this.checkBox_parent_not_match_keep_on_one_child_match_dont_keep_unmathced_children.UseVisualStyleBackColor = true;
            // 
            // groupBox_parent_match
            // 
            this.groupBox_parent_match.Controls.Add(this.radioButton_parent_match_keep_all_children);
            this.groupBox_parent_match.Controls.Add(this.radioButton_parent_matche_keep_matched_only);
            resources.ApplyResources(this.groupBox_parent_match, "groupBox_parent_match");
            this.groupBox_parent_match.Name = "groupBox_parent_match";
            this.groupBox_parent_match.TabStop = false;
            // 
            // radioButton_parent_match_keep_all_children
            // 
            resources.ApplyResources(this.radioButton_parent_match_keep_all_children, "radioButton_parent_match_keep_all_children");
            this.radioButton_parent_match_keep_all_children.Name = "radioButton_parent_match_keep_all_children";
            this.toolTip1.SetToolTip(this.radioButton_parent_match_keep_all_children, resources.GetString("radioButton_parent_match_keep_all_children.ToolTip"));
            this.radioButton_parent_match_keep_all_children.UseVisualStyleBackColor = true;
            // 
            // radioButton_parent_matche_keep_matched_only
            // 
            resources.ApplyResources(this.radioButton_parent_matche_keep_matched_only, "radioButton_parent_matche_keep_matched_only");
            this.radioButton_parent_matche_keep_matched_only.Checked = true;
            this.radioButton_parent_matche_keep_matched_only.Name = "radioButton_parent_matche_keep_matched_only";
            this.radioButton_parent_matche_keep_matched_only.TabStop = true;
            this.toolTip1.SetToolTip(this.radioButton_parent_matche_keep_matched_only, resources.GetString("radioButton_parent_matche_keep_matched_only.ToolTip"));
            this.radioButton_parent_matche_keep_matched_only.UseVisualStyleBackColor = true;
            // 
            // checkBox_delete_not_matched_related
            // 
            resources.ApplyResources(this.checkBox_delete_not_matched_related, "checkBox_delete_not_matched_related");
            this.checkBox_delete_not_matched_related.Name = "checkBox_delete_not_matched_related";
            this.toolTip1.SetToolTip(this.checkBox_delete_not_matched_related, resources.GetString("checkBox_delete_not_matched_related.ToolTip"));
            this.checkBox_delete_not_matched_related.UseVisualStyleBackColor = true;
            // 
            // checkBox_delete_not_matched_files
            // 
            resources.ApplyResources(this.checkBox_delete_not_matched_files, "checkBox_delete_not_matched_files");
            this.checkBox_delete_not_matched_files.Name = "checkBox_delete_not_matched_files";
            this.toolTip1.SetToolTip(this.checkBox_delete_not_matched_files, resources.GetString("checkBox_delete_not_matched_files.ToolTip"));
            this.checkBox_delete_not_matched_files.UseVisualStyleBackColor = true;
            // 
            // checkBox_delete_roms_nott_found
            // 
            resources.ApplyResources(this.checkBox_delete_roms_nott_found, "checkBox_delete_roms_nott_found");
            this.checkBox_delete_roms_nott_found.Name = "checkBox_delete_roms_nott_found";
            this.toolTip1.SetToolTip(this.checkBox_delete_roms_nott_found, resources.GetString("checkBox_delete_roms_nott_found.ToolTip"));
            this.checkBox_delete_roms_nott_found.UseVisualStyleBackColor = true;
            this.checkBox_delete_roms_nott_found.CheckedChanged += new System.EventHandler(this.checkBox_delete_roms_nott_found_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.radioButton_compare_rom_file_name);
            this.groupBox4.Controls.Add(this.radioButton_compare_sha1);
            this.groupBox4.Controls.Add(this.radioButton_compare_md5);
            this.groupBox4.Controls.Add(this.radioButton_compare_crc);
            this.groupBox4.Controls.Add(this.radioButton_compare_name);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // radioButton_compare_rom_file_name
            // 
            resources.ApplyResources(this.radioButton_compare_rom_file_name, "radioButton_compare_rom_file_name");
            this.radioButton_compare_rom_file_name.Name = "radioButton_compare_rom_file_name";
            this.toolTip1.SetToolTip(this.radioButton_compare_rom_file_name, resources.GetString("radioButton_compare_rom_file_name.ToolTip"));
            this.radioButton_compare_rom_file_name.UseVisualStyleBackColor = true;
            // 
            // radioButton_compare_sha1
            // 
            resources.ApplyResources(this.radioButton_compare_sha1, "radioButton_compare_sha1");
            this.radioButton_compare_sha1.Name = "radioButton_compare_sha1";
            this.toolTip1.SetToolTip(this.radioButton_compare_sha1, resources.GetString("radioButton_compare_sha1.ToolTip"));
            this.radioButton_compare_sha1.UseVisualStyleBackColor = true;
            // 
            // radioButton_compare_md5
            // 
            resources.ApplyResources(this.radioButton_compare_md5, "radioButton_compare_md5");
            this.radioButton_compare_md5.Name = "radioButton_compare_md5";
            this.toolTip1.SetToolTip(this.radioButton_compare_md5, resources.GetString("radioButton_compare_md5.ToolTip"));
            this.radioButton_compare_md5.UseVisualStyleBackColor = true;
            // 
            // radioButton_compare_crc
            // 
            resources.ApplyResources(this.radioButton_compare_crc, "radioButton_compare_crc");
            this.radioButton_compare_crc.Checked = true;
            this.radioButton_compare_crc.Name = "radioButton_compare_crc";
            this.radioButton_compare_crc.TabStop = true;
            this.toolTip1.SetToolTip(this.radioButton_compare_crc, resources.GetString("radioButton_compare_crc.ToolTip"));
            this.radioButton_compare_crc.UseVisualStyleBackColor = true;
            // 
            // radioButton_compare_name
            // 
            resources.ApplyResources(this.radioButton_compare_name, "radioButton_compare_name");
            this.radioButton_compare_name.Name = "radioButton_compare_name";
            this.toolTip1.SetToolTip(this.radioButton_compare_name, resources.GetString("radioButton_compare_name.ToolTip"));
            this.radioButton_compare_name.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.panel_db_options);
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // panel_db_options
            // 
            resources.ApplyResources(this.panel_db_options, "panel_db_options");
            this.panel_db_options.Name = "panel_db_options";
            // 
            // groupBox_status
            // 
            this.groupBox_status.Controls.Add(this.progressBar_slave);
            this.groupBox_status.Controls.Add(this.label_status_sub);
            this.groupBox_status.Controls.Add(this.progressBar_master);
            this.groupBox_status.Controls.Add(this.label_status_master);
            resources.ApplyResources(this.groupBox_status, "groupBox_status");
            this.groupBox_status.Name = "groupBox_status";
            this.groupBox_status.TabStop = false;
            // 
            // progressBar_slave
            // 
            resources.ApplyResources(this.progressBar_slave, "progressBar_slave");
            this.progressBar_slave.Name = "progressBar_slave";
            this.toolTip1.SetToolTip(this.progressBar_slave, resources.GetString("progressBar_slave.ToolTip"));
            // 
            // label_status_sub
            // 
            resources.ApplyResources(this.label_status_sub, "label_status_sub");
            this.label_status_sub.Name = "label_status_sub";
            // 
            // progressBar_master
            // 
            resources.ApplyResources(this.progressBar_master, "progressBar_master");
            this.progressBar_master.Name = "progressBar_master";
            this.toolTip1.SetToolTip(this.progressBar_master, resources.GetString("progressBar_master.ToolTip"));
            // 
            // label_status_master
            // 
            resources.ApplyResources(this.label_status_master, "label_status_master");
            this.label_status_master.Name = "label_status_master";
            // 
            // button_start
            // 
            resources.ApplyResources(this.button_start, "button_start");
            this.button_start.Name = "button_start";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // checkBox_rename_parent
            // 
            resources.ApplyResources(this.checkBox_rename_parent, "checkBox_rename_parent");
            this.checkBox_rename_parent.Checked = true;
            this.checkBox_rename_parent.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_rename_parent.Name = "checkBox_rename_parent";
            this.toolTip1.SetToolTip(this.checkBox_rename_parent, resources.GetString("checkBox_rename_parent.ToolTip"));
            this.checkBox_rename_parent.UseVisualStyleBackColor = true;
            // 
            // checkBox_apply_rom_data_info
            // 
            resources.ApplyResources(this.checkBox_apply_rom_data_info, "checkBox_apply_rom_data_info");
            this.checkBox_apply_rom_data_info.Checked = true;
            this.checkBox_apply_rom_data_info.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_apply_rom_data_info.Name = "checkBox_apply_rom_data_info";
            this.toolTip1.SetToolTip(this.checkBox_apply_rom_data_info, resources.GetString("checkBox_apply_rom_data_info.ToolTip"));
            this.checkBox_apply_rom_data_info.UseVisualStyleBackColor = true;
            // 
            // checkBox_rename_rom
            // 
            resources.ApplyResources(this.checkBox_rename_rom, "checkBox_rename_rom");
            this.checkBox_rename_rom.Checked = true;
            this.checkBox_rename_rom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_rename_rom.Name = "checkBox_rename_rom";
            this.toolTip1.SetToolTip(this.checkBox_rename_rom, resources.GetString("checkBox_rename_rom.ToolTip"));
            this.checkBox_rename_rom.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label6);
            this.groupBox6.Controls.Add(this.label5);
            this.groupBox6.Controls.Add(this.checkBox_apply_rom_data_info);
            this.groupBox6.Controls.Add(this.checkBox_rename_rom);
            this.groupBox6.Controls.Add(this.checkBox_rename_parent);
            resources.ApplyResources(this.groupBox6, "groupBox6");
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.TabStop = false;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.ForeColor = System.Drawing.Color.OliveDrab;
            this.label6.Name = "label6";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.ForeColor = System.Drawing.Color.OliveDrab;
            this.label5.Name = "label5";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.richTextBox1);
            resources.ApplyResources(this.groupBox7, "groupBox7");
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.TabStop = false;
            // 
            // richTextBox1
            // 
            resources.ApplyResources(this.richTextBox1, "richTextBox1");
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            // 
            // Form_ImportDatabaseFileUniversal
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button_start);
            this.Controls.Add(this.groupBox_status);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_ImportDatabaseFileUniversal";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormImportDatabaseFileUniversal_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox_parent_not_match.ResumeLayout(false);
            this.groupBox_parent_not_match.PerformLayout();
            this.groupBox_parent_match.ResumeLayout(false);
            this.groupBox_parent_match.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox_status.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBox_dbType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox_dbFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox_archivePassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBox_check_inside_archive;
        private System.Windows.Forms.TextBox textBox_archive_extesnions;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBox_forNesArchiveTweeks;
        private System.Windows.Forms.CheckBox checkBox_archive_perfect_match;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBox_delete_roms_nott_found;
        private System.Windows.Forms.CheckBox checkBox_delete_not_matched_related;
        private System.Windows.Forms.CheckBox checkBox_delete_not_matched_files;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton radioButton_compare_sha1;
        private System.Windows.Forms.RadioButton radioButton_compare_md5;
        private System.Windows.Forms.RadioButton radioButton_compare_crc;
        private System.Windows.Forms.RadioButton radioButton_compare_name;
        private System.Windows.Forms.RadioButton radioButton_compare_rom_file_name;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox_status;
        private System.Windows.Forms.ProgressBar progressBar_slave;
        private System.Windows.Forms.Label label_status_sub;
        private System.Windows.Forms.ProgressBar progressBar_master;
        private System.Windows.Forms.Label label_status_master;
        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBox_turboe;
        private System.Windows.Forms.Panel panel_db_options;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox checkBox_cahceOndisk;
        private System.Windows.Forms.RadioButton radioButton_parent_not_mathc_free_matched_children;
        private System.Windows.Forms.RadioButton radioButton_parent_not_match_keep_parent_if_one_child_match;
        private System.Windows.Forms.RadioButton radioButton_parent_matche_keep_matched_only;
        private System.Windows.Forms.RadioButton radioButton_parent_match_keep_all_children;
        private System.Windows.Forms.CheckBox checkBox_parent_not_match_keep_on_one_child_match_dont_keep_unmathced_children;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox_parent_not_match;
        private System.Windows.Forms.GroupBox groupBox_parent_match;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckBox checkBox_rename_parent;
        private System.Windows.Forms.CheckBox checkBox_import_category;
        private System.Windows.Forms.CheckBox checkBox_add_filters;
        private System.Windows.Forms.CheckBox checkBox_rename_rom;
        private System.Windows.Forms.CheckBox checkBox_apply_rom_data_info;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}