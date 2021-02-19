using System;
using System.Windows.Forms;

namespace AchSmartHome_Management
{
    public partial class WirelessDoorbell : UserControl
    {
        public WirelessDoorbell()
        {
            InitializeComponent();
            GlobalSettings.InitThemeAndLang(Controls, this);
        }
    }
}
