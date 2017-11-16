using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using EasyETL.DataSets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatabaseWriterSample
{
    public partial class DatabasePropertiesForm : Form
    {
        public string ConnectionString;
        public string DatabaseType;
        public string InsertCommand;
        public string UpdateCommand;
        public DatabasePropertiesForm()
        {
            InitializeComponent();
        }

        private void DatabasePropertiesForm_Load(object sender, EventArgs e)
        {
            foreach (DatabaseType dType in Enum.GetValues(typeof(DatabaseType)))
            {
                cmbDatabaseType.Items.Add(dType.ToString());
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ConnectionString = txtConnectionString.Text;
            DatabaseType = cmbDatabaseType.Text;
            InsertCommand = txtInsertCommand.Text;
            UpdateCommand = txtUpdateCommand.Text;
        }
    }
}
