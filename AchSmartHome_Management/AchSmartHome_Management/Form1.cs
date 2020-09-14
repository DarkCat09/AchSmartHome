using System;
using System.Windows.Forms;

namespace AchSmartHome_Management
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ControlPanel controlPanel1 = new ControlPanel();
            controlPanel1.Location = new System.Drawing.Point(0, 24);
            Controls.Add(controlPanel1);
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void соединитьсяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConnectForm cf = new ConnectForm();
            cf.ShowDialog();
        }
    }
}
