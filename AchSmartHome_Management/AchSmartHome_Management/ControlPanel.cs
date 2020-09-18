using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AchSmartHome_Management
{
    public partial class ControlPanel : UserControl
    {
        private ControlCollection mainFormCtrls = null;

        public ControlPanel()
        {
            InitializeComponent();
        }
        public ControlPanel(ControlCollection ctrls)
        {
            InitializeComponent();
            mainFormCtrls = ctrls;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form1.ReplacePanel<LightPanel>();
        }
    }
}
