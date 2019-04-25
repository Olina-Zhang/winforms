using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoreTesting
{
    public partial class MenuForm : Form
    {
        public MenuForm()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            StripControls stripControls = new StripControls();
            stripControls.Show();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            ToolStripContainer toolStripContainer = new ToolStripContainer();
            toolStripContainer.Show();
        }
    }
}
