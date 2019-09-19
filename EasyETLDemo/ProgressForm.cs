using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyXmlSample
{
    public partial class ProgressForm : Form
    {
        public int MaximumItems = 100;
        public string ActionName = "";
        public ProgressForm()
        {
            InitializeComponent();
        }

        public void SetCurrentIndex(int itemIndex)
        {
            pbProgress.Maximum = MaximumItems;
            lblActionName.Text = ActionName;
            pbProgress.Value = itemIndex;
            lblProgressText.Text = String.Format("Processing {0}/{1}",itemIndex,MaximumItems);
        }


    }
}
