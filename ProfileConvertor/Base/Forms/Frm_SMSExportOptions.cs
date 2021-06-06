using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AHD.EO.Base
{
    public partial class Frm_SMSExportOptions : Form
    {
        public Frm_SMSExportOptions(DB_SMS sms)
        {
            this.sms = sms;
            InitializeComponent();
            checkBox_ignoreEmptyFields.Checked = sms._IgnoreEmptyFields;
            checkBox_addAllDataItems.Checked = sms._AddAllDataItems;
        }
        DB_SMS sms;
        public bool _IgnoreEmptyFields { get { return checkBox_ignoreEmptyFields.Checked; } }
        public bool _AddAllDataItems { get { return checkBox_addAllDataItems.Checked; } }
        public bool _CalculateCRCInsideOfArchive { get { return checkBox_calculateCRC.Checked; } }
        public string Platform { get { return textBox_platform.Text; } }
        public string Gametype { get { return textBox_gametype.Text; } }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            checkBox_ignoreEmptyFields.Checked = checkBox_addAllDataItems.Checked = false;
        }
    }
}
