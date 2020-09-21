using System;
using System.Windows.Forms;

namespace AchSmartHome_Management
{
    public partial class ControlPanel : UserControl
    {
        public ControlPanel()
        {
            InitializeComponent();
            GlobalSettings.InitThemeAndLang(Controls, this);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form1.ReplacePanel<LightPanel>();
        }
    }
}
