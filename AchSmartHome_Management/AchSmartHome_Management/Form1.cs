using System;
using System.Windows.Forms;

namespace AchSmartHome_Management
{
    public partial class Form1 : Form
    {
        public static Panel panel1 = null;
        public Form1()
        {
            InitializeComponent();

            panel1 = new Panel();
            panel1.Name = "panel1";
            panel1.Location = new System.Drawing.Point(13, 28);
            panel1.AutoSize = true;
            Controls.Add(panel1);

            ReplacePanel<ControlPanel>();
        }

        public static void ReplacePanel<panelToAdd>() where panelToAdd : Control, new()
        {
            panel1.Controls.Clear();
            panel1.Controls.Add(new panelToAdd());
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

        private void главнаяСтраницаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReplacePanel<ControlPanel>();
        }

        private void светToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReplacePanel<LightPanel>();
        }
    }
}
