using System;
using System.Windows.Forms;

namespace AchSmartHome_Management
{
    public partial class AboutProgram : UserControl
    {
        public AboutProgram()
        {
            InitializeComponent();
            GlobalSettings.InitThemeAndLang(Controls, this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MainForm.GoBackPage(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!textBox1.Visible)
            {
                textBox1.Visible = true;
                button1.Text = Languages.GetLocalizedString("HideDescr", "Hide Description");
            }
            else
            {
                textBox1.Visible = false;
                button1.Text = Languages.GetLocalizedString("ShowDescr", "Show Description");
            }
        }
    }
}
