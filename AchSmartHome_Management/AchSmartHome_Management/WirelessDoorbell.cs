using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
