/*
 * Copyright © 2020 Чечкенёв Андрей
 * 
 * This file is part of AchSmartHome.
 * 
 * AchSmartHome is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 2 of the License, or
 * (at your option) any later version.
 * 
 * AchSmartHome is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with AchSmartHome.  If not, see <https://www.gnu.org/licenses/>.
 */

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
            comboBox1.Items.AddRange(new string[5] {
                Languages.GetLocalizedString("PanelName0", "Main Control Panel"),
                Languages.GetLocalizedString("PanelName1", "Light Control Panel"),
                Languages.GetLocalizedString("PanelName2", "Custom sensors Panel"),
                Languages.GetLocalizedString("PanelName3", "Settings Panel"),
                Languages.GetLocalizedString("PanelName4", "About Program Panel")
            });
            f = callingForm;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    MainForm.ReplacePanel<ControlPanel>();
                    break;
                case 1:
                    MainForm.ReplacePanel<LightPanel>();
                    break;
                case 2:
                    MainForm.ReplacePanel<OtherSensorsPanel>();
                    break;
                case 3:
                    MainForm.panel1.Controls.Clear();
                    MainForm.panel1.Controls.Add(new SettingsForm(f));
                    break;
                case 4:
                    MainForm.ReplacePanel<AboutProgram>();
                    break;
                default:
                    Logging.LogEvent(2, "ChoosePanel", "Invalid selected index!");
                    break;
            }
            Close();
        }
    }
}
