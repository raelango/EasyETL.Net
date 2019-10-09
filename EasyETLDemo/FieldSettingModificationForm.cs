using EasyETL.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyXmlSample
{
    public partial class FieldSettingModificationForm : Form
    {
        public string FieldValue;
        public EasyFieldAttribute EFA = null;

        public FieldSettingModificationForm()
        {
            InitializeComponent();
        }

        private void FieldSettingModificationForm_Load(object sender, EventArgs e)
        {
        }

        public void SetFields(string actionName, string fieldValue, EasyFieldAttribute easyFieldAttribute)
        {
            lblActionName.Text = actionName;
            FieldValue = fieldValue;
            txtFieldValue.Text = FieldValue;
            EFA = easyFieldAttribute;
            //txtFieldValue.UseSystemPasswordChar = EFA.IsPassword;
            if (EFA.IsPassword) txtFieldValue.PasswordChar = '*';
            lblFieldName.Text = EFA.FieldName;
            lblDescription.Text = EFA.FieldDescription;
            if (EFA.PossibleValues.Count > 0)
            {
                cmbFieldValue.DataSource = EFA.PossibleValues;
                cmbFieldValue.Text = FieldValue;
                txtFieldValue.Visible = false;
                cmbFieldValue.Visible = true;
                tableLayoutPanel1.Controls.Remove(txtFieldValue);
                tableLayoutPanel1.Controls.Remove(cmbFieldValue);
                tableLayoutPanel1.Controls.Add(cmbFieldValue, 1, 4);
            }
            else
            {
                cmbFieldValue.Visible = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string fieldValue = txtFieldValue.Text;
            if (cmbFieldValue.Visible) fieldValue = cmbFieldValue.Text;
            if ((EFA != null) && (!String.IsNullOrWhiteSpace(EFA.RegexMatch)) && (!Regex.IsMatch(fieldValue, "^(" + EFA.RegexMatch + ")$")))
            {
                MessageBox.Show("The input data does not match the expected format.  The expected format is " + EFA.RegexMatch);
                return;
            }
            FieldValue = fieldValue;
            this.Hide();
        }

    }
}
