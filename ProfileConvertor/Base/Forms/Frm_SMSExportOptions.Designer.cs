namespace AHD.EO.Base
{
    partial class Frm_SMSExportOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_SMSExportOptions));
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox_addAllDataItems = new System.Windows.Forms.CheckBox();
            this.checkBox_ignoreEmptyFields = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_platform = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_gametype = new System.Windows.Forms.TextBox();
            this.checkBox_calculateCRC = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBox_addAllDataItems
            // 
            resources.ApplyResources(this.checkBox_addAllDataItems, "checkBox_addAllDataItems");
            this.checkBox_addAllDataItems.Name = "checkBox_addAllDataItems";
            this.checkBox_addAllDataItems.UseVisualStyleBackColor = true;
            // 
            // checkBox_ignoreEmptyFields
            // 
            resources.ApplyResources(this.checkBox_ignoreEmptyFields, "checkBox_ignoreEmptyFields");
            this.checkBox_ignoreEmptyFields.Name = "checkBox_ignoreEmptyFields";
            this.checkBox_ignoreEmptyFields.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // textBox_platform
            // 
            resources.ApplyResources(this.textBox_platform, "textBox_platform");
            this.textBox_platform.Name = "textBox_platform";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // textBox_gametype
            // 
            resources.ApplyResources(this.textBox_gametype, "textBox_gametype");
            this.textBox_gametype.Name = "textBox_gametype";
            // 
            // checkBox_calculateCRC
            // 
            resources.ApplyResources(this.checkBox_calculateCRC, "checkBox_calculateCRC");
            this.checkBox_calculateCRC.Name = "checkBox_calculateCRC";
            this.checkBox_calculateCRC.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // Frm_SMSExportOptions
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkBox_calculateCRC);
            this.Controls.Add(this.textBox_gametype);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_platform);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBox_addAllDataItems);
            this.Controls.Add(this.checkBox_ignoreEmptyFields);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Frm_SMSExportOptions";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox_addAllDataItems;
        private System.Windows.Forms.CheckBox checkBox_ignoreEmptyFields;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_platform;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_gametype;
        private System.Windows.Forms.CheckBox checkBox_calculateCRC;
        private System.Windows.Forms.Label label3;
    }
}