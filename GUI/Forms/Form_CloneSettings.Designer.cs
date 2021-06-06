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
    partial class Form_CloneSettings
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
            this.label_copy_from = new System.Windows.Forms.Label();
            this.comboBox_copy_from = new System.Windows.Forms.ComboBox();
            this.label_copy_to = new System.Windows.Forms.Label();
            this.textBox_copy_to = new System.Windows.Forms.TextBox();
            this.checkBox__copy_win_size_and_splitters = new System.Windows.Forms.CheckBox();
            this.checkBox_copy_archive_settings = new System.Windows.Forms.CheckBox();
            this.checkBox_copy_rchive_extensions = new System.Windows.Forms.CheckBox();
            this.checkBox_copy_copy_rom_settings = new System.Windows.Forms.CheckBox();
            this.checkBox_copy_console_extensions = new System.Windows.Forms.CheckBox();
            this.checkBox_copy_ics = new System.Windows.Forms.CheckBox();
            this.checkBox_copy_rom_data_types = new System.Windows.Forms.CheckBox();
            this.checkBox_copy_launch_options = new System.Windows.Forms.CheckBox();
            this.checkBox_copy_tab_priority_settings = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.checkBox_copy_style = new System.Windows.Forms.CheckBox();
            this.checkBox_copy_icon = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.checkBox_clear_rom_data_first = new System.Windows.Forms.CheckBox();
            this.checkBox_copy_ics_map = new System.Windows.Forms.CheckBox();
            this.checkBox_clear_ics_first = new System.Windows.Forms.CheckBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.checkBox_copy_columns_settings = new System.Windows.Forms.CheckBox();
            this.checkBox_copy_filters = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button_copy_console_only = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_copy_from
            // 
            this.label_copy_from.AutoSize = true;
            this.label_copy_from.Location = new System.Drawing.Point(12, 9);
            this.label_copy_from.Name = "label_copy_from";
            this.label_copy_from.Size = new System.Drawing.Size(102, 13);
            this.label_copy_from.TabIndex = 0;
            this.label_copy_from.Text = "Copy settings from:";
            // 
            // comboBox_copy_from
            // 
            this.comboBox_copy_from.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_copy_from.FormattingEnabled = true;
            this.comboBox_copy_from.Location = new System.Drawing.Point(12, 25);
            this.comboBox_copy_from.Name = "comboBox_copy_from";
            this.comboBox_copy_from.Size = new System.Drawing.Size(363, 21);
            this.comboBox_copy_from.TabIndex = 1;
            // 
            // label_copy_to
            // 
            this.label_copy_to.AutoSize = true;
            this.label_copy_to.Location = new System.Drawing.Point(12, 49);
            this.label_copy_to.Name = "label_copy_to";
            this.label_copy_to.Size = new System.Drawing.Size(82, 13);
            this.label_copy_to.TabIndex = 2;
            this.label_copy_to.Text = "To this console:";
            // 
            // textBox_copy_to
            // 
            this.textBox_copy_to.Location = new System.Drawing.Point(12, 65);
            this.textBox_copy_to.Name = "textBox_copy_to";
            this.textBox_copy_to.ReadOnly = true;
            this.textBox_copy_to.Size = new System.Drawing.Size(363, 20);
            this.textBox_copy_to.TabIndex = 3;
            // 
            // checkBox__copy_win_size_and_splitters
            // 
            this.checkBox__copy_win_size_and_splitters.AutoSize = true;
            this.checkBox__copy_win_size_and_splitters.Checked = true;
            this.checkBox__copy_win_size_and_splitters.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox__copy_win_size_and_splitters.Location = new System.Drawing.Point(6, 6);
            this.checkBox__copy_win_size_and_splitters.Name = "checkBox__copy_win_size_and_splitters";
            this.checkBox__copy_win_size_and_splitters.Size = new System.Drawing.Size(225, 17);
            this.checkBox__copy_win_size_and_splitters.TabIndex = 4;
            this.checkBox__copy_win_size_and_splitters.Text = "Copy window size and splitters distances.";
            this.checkBox__copy_win_size_and_splitters.UseVisualStyleBackColor = true;
            // 
            // checkBox_copy_archive_settings
            // 
            this.checkBox_copy_archive_settings.AutoSize = true;
            this.checkBox_copy_archive_settings.Location = new System.Drawing.Point(6, 6);
            this.checkBox_copy_archive_settings.Name = "checkBox_copy_archive_settings";
            this.checkBox_copy_archive_settings.Size = new System.Drawing.Size(134, 17);
            this.checkBox_copy_archive_settings.TabIndex = 5;
            this.checkBox_copy_archive_settings.Text = "Copy archive settings.";
            this.checkBox_copy_archive_settings.UseVisualStyleBackColor = true;
            // 
            // checkBox_copy_rchive_extensions
            // 
            this.checkBox_copy_rchive_extensions.AutoSize = true;
            this.checkBox_copy_rchive_extensions.Location = new System.Drawing.Point(6, 29);
            this.checkBox_copy_rchive_extensions.Name = "checkBox_copy_rchive_extensions";
            this.checkBox_copy_rchive_extensions.Size = new System.Drawing.Size(148, 17);
            this.checkBox_copy_rchive_extensions.TabIndex = 6;
            this.checkBox_copy_rchive_extensions.Text = "Copy archive extensions.";
            this.checkBox_copy_rchive_extensions.UseVisualStyleBackColor = true;
            // 
            // checkBox_copy_copy_rom_settings
            // 
            this.checkBox_copy_copy_rom_settings.AutoSize = true;
            this.checkBox_copy_copy_rom_settings.Location = new System.Drawing.Point(6, 6);
            this.checkBox_copy_copy_rom_settings.Name = "checkBox_copy_copy_rom_settings";
            this.checkBox_copy_copy_rom_settings.Size = new System.Drawing.Size(263, 17);
            this.checkBox_copy_copy_rom_settings.TabIndex = 0;
            this.checkBox_copy_copy_rom_settings.Text = "Copy rom copy settings (rom copy before launch)";
            this.checkBox_copy_copy_rom_settings.UseVisualStyleBackColor = true;
            // 
            // checkBox_copy_console_extensions
            // 
            this.checkBox_copy_console_extensions.AutoSize = true;
            this.checkBox_copy_console_extensions.Location = new System.Drawing.Point(6, 6);
            this.checkBox_copy_console_extensions.Name = "checkBox_copy_console_extensions";
            this.checkBox_copy_console_extensions.Size = new System.Drawing.Size(149, 17);
            this.checkBox_copy_console_extensions.TabIndex = 0;
            this.checkBox_copy_console_extensions.Text = "Copy console extensions.";
            this.checkBox_copy_console_extensions.UseVisualStyleBackColor = true;
            // 
            // checkBox_copy_ics
            // 
            this.checkBox_copy_ics.AutoSize = true;
            this.checkBox_copy_ics.Location = new System.Drawing.Point(6, 29);
            this.checkBox_copy_ics.Name = "checkBox_copy_ics";
            this.checkBox_copy_ics.Size = new System.Drawing.Size(275, 17);
            this.checkBox_copy_ics.TabIndex = 1;
            this.checkBox_copy_ics.Text = "Copy information containers (just names and types)";
            this.checkBox_copy_ics.UseVisualStyleBackColor = true;
            this.checkBox_copy_ics.CheckedChanged += new System.EventHandler(this.checkBox_copy_ics_CheckedChanged);
            // 
            // checkBox_copy_rom_data_types
            // 
            this.checkBox_copy_rom_data_types.AutoSize = true;
            this.checkBox_copy_rom_data_types.Location = new System.Drawing.Point(6, 150);
            this.checkBox_copy_rom_data_types.Name = "checkBox_copy_rom_data_types";
            this.checkBox_copy_rom_data_types.Size = new System.Drawing.Size(278, 17);
            this.checkBox_copy_rom_data_types.TabIndex = 2;
            this.checkBox_copy_rom_data_types.Text = "Copy rom data info elements (just names and types)";
            this.checkBox_copy_rom_data_types.UseVisualStyleBackColor = true;
            this.checkBox_copy_rom_data_types.CheckedChanged += new System.EventHandler(this.checkBox_copy_rom_data_types_CheckedChanged);
            // 
            // checkBox_copy_launch_options
            // 
            this.checkBox_copy_launch_options.AutoSize = true;
            this.checkBox_copy_launch_options.Location = new System.Drawing.Point(6, 196);
            this.checkBox_copy_launch_options.Name = "checkBox_copy_launch_options";
            this.checkBox_copy_launch_options.Size = new System.Drawing.Size(123, 17);
            this.checkBox_copy_launch_options.TabIndex = 4;
            this.checkBox_copy_launch_options.Text = "Copy launch options";
            this.checkBox_copy_launch_options.UseVisualStyleBackColor = true;
            // 
            // checkBox_copy_tab_priority_settings
            // 
            this.checkBox_copy_tab_priority_settings.AutoSize = true;
            this.checkBox_copy_tab_priority_settings.Location = new System.Drawing.Point(6, 219);
            this.checkBox_copy_tab_priority_settings.Name = "checkBox_copy_tab_priority_settings";
            this.checkBox_copy_tab_priority_settings.Size = new System.Drawing.Size(152, 17);
            this.checkBox_copy_tab_priority_settings.TabIndex = 5;
            this.checkBox_copy_tab_priority_settings.Text = "Copy tab priority settings.";
            this.checkBox_copy_tab_priority_settings.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Location = new System.Drawing.Point(12, 91);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(363, 295);
            this.tabControl1.TabIndex = 11;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.checkBox_copy_style);
            this.tabPage1.Controls.Add(this.checkBox_copy_icon);
            this.tabPage1.Controls.Add(this.checkBox__copy_win_size_and_splitters);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(355, 192);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Style and visual";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // checkBox_copy_style
            // 
            this.checkBox_copy_style.AutoSize = true;
            this.checkBox_copy_style.Location = new System.Drawing.Point(6, 52);
            this.checkBox_copy_style.Name = "checkBox_copy_style";
            this.checkBox_copy_style.Size = new System.Drawing.Size(77, 17);
            this.checkBox_copy_style.TabIndex = 6;
            this.checkBox_copy_style.Text = "Copy style";
            this.checkBox_copy_style.UseVisualStyleBackColor = true;
            // 
            // checkBox_copy_icon
            // 
            this.checkBox_copy_icon.AutoSize = true;
            this.checkBox_copy_icon.Location = new System.Drawing.Point(6, 29);
            this.checkBox_copy_icon.Name = "checkBox_copy_icon";
            this.checkBox_copy_icon.Size = new System.Drawing.Size(73, 17);
            this.checkBox_copy_icon.TabIndex = 5;
            this.checkBox_copy_icon.Text = "Copy icon";
            this.checkBox_copy_icon.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.checkBox_copy_archive_settings);
            this.tabPage2.Controls.Add(this.checkBox_copy_rchive_extensions);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(355, 192);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Archive";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.checkBox_copy_copy_rom_settings);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(355, 192);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Rom copying";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.checkBox_clear_rom_data_first);
            this.tabPage4.Controls.Add(this.checkBox_copy_ics_map);
            this.tabPage4.Controls.Add(this.checkBox_clear_ics_first);
            this.tabPage4.Controls.Add(this.checkBox_copy_tab_priority_settings);
            this.tabPage4.Controls.Add(this.checkBox_copy_console_extensions);
            this.tabPage4.Controls.Add(this.checkBox_copy_launch_options);
            this.tabPage4.Controls.Add(this.checkBox_copy_ics);
            this.tabPage4.Controls.Add(this.checkBox_copy_rom_data_types);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(355, 269);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Console";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // checkBox_clear_rom_data_first
            // 
            this.checkBox_clear_rom_data_first.AutoSize = true;
            this.checkBox_clear_rom_data_first.Enabled = false;
            this.checkBox_clear_rom_data_first.Location = new System.Drawing.Point(21, 173);
            this.checkBox_clear_rom_data_first.Name = "checkBox_clear_rom_data_first";
            this.checkBox_clear_rom_data_first.Size = new System.Drawing.Size(190, 17);
            this.checkBox_clear_rom_data_first.TabIndex = 8;
            this.checkBox_clear_rom_data_first.Text = "Clear rom data info elements first.";
            this.checkBox_clear_rom_data_first.UseVisualStyleBackColor = true;
            // 
            // checkBox_copy_ics_map
            // 
            this.checkBox_copy_ics_map.AutoSize = true;
            this.checkBox_copy_ics_map.Enabled = false;
            this.checkBox_copy_ics_map.Location = new System.Drawing.Point(21, 75);
            this.checkBox_copy_ics_map.Name = "checkBox_copy_ics_map";
            this.checkBox_copy_ics_map.Size = new System.Drawing.Size(273, 69);
            this.checkBox_copy_ics_map.TabIndex = 7;
            this.checkBox_copy_ics_map.Text = "Copy tabs map if possible.\r\nNOTE: with this option, you may need to show and \r\nhi" +
    "de the tabs in the console properties window.\r\nAnyway, the map will be copied as" +
    " it self but the\r\ntabs may be hidden.";
            this.checkBox_copy_ics_map.UseVisualStyleBackColor = true;
            // 
            // checkBox_clear_ics_first
            // 
            this.checkBox_clear_ics_first.AutoSize = true;
            this.checkBox_clear_ics_first.Enabled = false;
            this.checkBox_clear_ics_first.Location = new System.Drawing.Point(21, 52);
            this.checkBox_clear_ics_first.Name = "checkBox_clear_ics_first";
            this.checkBox_clear_ics_first.Size = new System.Drawing.Size(313, 17);
            this.checkBox_clear_ics_first.TabIndex = 6;
            this.checkBox_clear_ics_first.Text = "Clear information containers first (this may take a long time)";
            this.checkBox_clear_ics_first.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.checkBox_copy_columns_settings);
            this.tabPage5.Controls.Add(this.checkBox_copy_filters);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(355, 192);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "MISC";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // checkBox_copy_columns_settings
            // 
            this.checkBox_copy_columns_settings.AutoSize = true;
            this.checkBox_copy_columns_settings.Location = new System.Drawing.Point(6, 29);
            this.checkBox_copy_columns_settings.Name = "checkBox_copy_columns_settings";
            this.checkBox_copy_columns_settings.Size = new System.Drawing.Size(137, 17);
            this.checkBox_copy_columns_settings.TabIndex = 1;
            this.checkBox_copy_columns_settings.Text = "Copy columns settings.";
            this.checkBox_copy_columns_settings.UseVisualStyleBackColor = true;
            // 
            // checkBox_copy_filters
            // 
            this.checkBox_copy_filters.AutoSize = true;
            this.checkBox_copy_filters.Location = new System.Drawing.Point(6, 6);
            this.checkBox_copy_filters.Name = "checkBox_copy_filters";
            this.checkBox_copy_filters.Size = new System.Drawing.Size(81, 17);
            this.checkBox_copy_filters.TabIndex = 0;
            this.checkBox_copy_filters.Text = "Copy filters";
            this.checkBox_copy_filters.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 389);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(340, 39);
            this.label1.TabIndex = 12;
            this.label1.Text = "*Note: some settings may become grayed out, due to these settings \r\nare only avai" +
    "lable on certin types (for example, console settings only \r\nworks with consoles." +
    "... etc)";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(213, 506);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "&OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(300, 506);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 14;
            this.button2.Text = "&Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 436);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(162, 23);
            this.button3.TabIndex = 15;
            this.button3.Text = "Copy &everything";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(12, 465);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(162, 23);
            this.button4.TabIndex = 16;
            this.button4.Text = "Copy &nothing";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(12, 506);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(162, 23);
            this.button5.TabIndex = 17;
            this.button5.Text = "Default";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button_copy_console_only
            // 
            this.button_copy_console_only.Enabled = false;
            this.button_copy_console_only.Location = new System.Drawing.Point(213, 436);
            this.button_copy_console_only.Name = "button_copy_console_only";
            this.button_copy_console_only.Size = new System.Drawing.Size(162, 23);
            this.button_copy_console_only.TabIndex = 18;
            this.button_copy_console_only.Text = "Copy console settings only";
            this.button_copy_console_only.UseVisualStyleBackColor = true;
            this.button_copy_console_only.Click += new System.EventHandler(this.button6_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(213, 465);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(162, 23);
            this.button6.TabIndex = 19;
            this.button6.Text = "Copy style and visual only";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click_1);
            // 
            // Form_CloneSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 541);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button_copy_console_only);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.textBox_copy_to);
            this.Controls.Add(this.label_copy_to);
            this.Controls.Add(this.comboBox_copy_from);
            this.Controls.Add(this.label_copy_from);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_CloneSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Clone Settings";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_copy_from;
        private System.Windows.Forms.ComboBox comboBox_copy_from;
        private System.Windows.Forms.Label label_copy_to;
        private System.Windows.Forms.TextBox textBox_copy_to;
        private System.Windows.Forms.CheckBox checkBox__copy_win_size_and_splitters;
        private System.Windows.Forms.CheckBox checkBox_copy_archive_settings;
        private System.Windows.Forms.CheckBox checkBox_copy_rchive_extensions;
        private System.Windows.Forms.CheckBox checkBox_copy_copy_rom_settings;
        private System.Windows.Forms.CheckBox checkBox_copy_console_extensions;
        private System.Windows.Forms.CheckBox checkBox_copy_rom_data_types;
        private System.Windows.Forms.CheckBox checkBox_copy_ics;
        private System.Windows.Forms.CheckBox checkBox_copy_launch_options;
        private System.Windows.Forms.CheckBox checkBox_copy_tab_priority_settings;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.CheckBox checkBox_copy_style;
        private System.Windows.Forms.CheckBox checkBox_copy_icon;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.CheckBox checkBox_copy_filters;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBox_copy_columns_settings;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button_copy_console_only;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.CheckBox checkBox_clear_rom_data_first;
        private System.Windows.Forms.CheckBox checkBox_copy_ics_map;
        private System.Windows.Forms.CheckBox checkBox_clear_ics_first;
    }
}