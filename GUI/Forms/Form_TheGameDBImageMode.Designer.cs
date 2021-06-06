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
    partial class Form_TheGamesDBImageMode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_TheGamesDBImageMode));
            this.radioButton_boxart_back = new System.Windows.Forms.RadioButton();
            this.radioButton_boxart_front = new System.Windows.Forms.RadioButton();
            this.radioButton_Fanart = new System.Windows.Forms.RadioButton();
            this.radioButton_Banners = new System.Windows.Forms.RadioButton();
            this.radioButton_Screenshots = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // radioButton_boxart_back
            // 
            resources.ApplyResources(this.radioButton_boxart_back, "radioButton_boxart_back");
            this.radioButton_boxart_back.Checked = true;
            this.radioButton_boxart_back.Name = "radioButton_boxart_back";
            this.radioButton_boxart_back.TabStop = true;
            this.radioButton_boxart_back.UseVisualStyleBackColor = true;
            // 
            // radioButton_boxart_front
            // 
            resources.ApplyResources(this.radioButton_boxart_front, "radioButton_boxart_front");
            this.radioButton_boxart_front.Name = "radioButton_boxart_front";
            this.radioButton_boxart_front.UseVisualStyleBackColor = true;
            // 
            // radioButton_Fanart
            // 
            resources.ApplyResources(this.radioButton_Fanart, "radioButton_Fanart");
            this.radioButton_Fanart.Name = "radioButton_Fanart";
            this.radioButton_Fanart.UseVisualStyleBackColor = true;
            // 
            // radioButton_Banners
            // 
            resources.ApplyResources(this.radioButton_Banners, "radioButton_Banners");
            this.radioButton_Banners.Name = "radioButton_Banners";
            this.radioButton_Banners.UseVisualStyleBackColor = true;
            // 
            // radioButton_Screenshots
            // 
            resources.ApplyResources(this.radioButton_Screenshots, "radioButton_Screenshots");
            this.radioButton_Screenshots.Name = "radioButton_Screenshots";
            this.radioButton_Screenshots.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form_TheGamesDBImageMode
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.radioButton_Screenshots);
            this.Controls.Add(this.radioButton_Banners);
            this.Controls.Add(this.radioButton_Fanart);
            this.Controls.Add(this.radioButton_boxart_front);
            this.Controls.Add(this.radioButton_boxart_back);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_TheGamesDBImageMode";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButton_boxart_back;
        private System.Windows.Forms.RadioButton radioButton_boxart_front;
        private System.Windows.Forms.RadioButton radioButton_Fanart;
        private System.Windows.Forms.RadioButton radioButton_Banners;
        private System.Windows.Forms.RadioButton radioButton_Screenshots;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}