using System;
using System.Windows.Forms;

namespace AchSmartHome_Management
{
    public partial class ChoosePanel : Form
    {
        Form f = null;
        public ChoosePanel(Form callingForm = null)
        {
            InitializeComponent();
            GlobalSettings.InitThemeAndLang(Controls, this);
            comboBox1.Items.AddRange(new string[4] {
                Languages.GetLocalizedString("PanelName0", "Main Control Panel"),
                Languages.GetLocalizedString("PanelName1", "Light Control Panel"),
                Languages.GetLocalizedString("PanelName2", "Custom sensors Panel"),
                Languages.GetLocalizedString("PanelName3", "Settings Panel")
            });
            f = callingForm;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    Form1.ReplacePanel<ControlPanel>();
                    break;
                case 1:
                    Form1.ReplacePanel<LightPanel>();
                    break;
                case 2:
                    Form1.ReplacePanel<OtherSensorsPanel>();
                    break;
                case 3:
                    Form1.panel1.Controls.Clear();
                    Form1.panel1.Controls.Add(new SettingsForm(f));
                    break;
                default:
                    Logging.LogEvent(2, "Invalid selected index in ChoosePanel!");
                    break;
            }
            Close();
        }
    }
}
