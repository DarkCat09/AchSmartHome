using System;
using System.Windows.Forms;

namespace AchSmartHome_Management
{
    public partial class LightPanel : UserControl
    {
        public LightPanel()
        {
            InitializeComponent();
            GlobalSettings.InitThemeAndLang(Controls, this);
            GetLightData();
        }

        private void GetLightData()
        {
            CheckBox[] lampCheckboxes = new CheckBox[4]
            {
                checkBox1, checkBox2, checkBox3, checkBox4
            };
            for (int i = 1; i <= 4; i++)
            {
                System.Collections.Generic.Dictionary<int, object> lampResult = DatabaseConnecting.ProcessSqlRequest(
                    "SELECT * from `light` WHERE (lampnum = " + i + ") ORDER BY id DESC LIMIT 1"
                );
                if (lampResult.Count > 0)
                    lampCheckboxes[i-1].Checked = Convert.ToBoolean(lampResult[3]);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form1.ReplacePanel<ControlPanel>();
        }
    }
}
