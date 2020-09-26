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
using System.Drawing;
using System.Windows.Forms;

namespace AchSmartHome_Management
{
    public partial class SettingsForm : UserControl
    {
        public SettingsForm()
        {
            InitializeComponent();
            this.BackColor = GlobalSettings.theme;
            GlobalSettings.InitThemeAndLang(Controls, this);

            System.Collections.Generic.Dictionary<string, string> langFiles = Languages.InitLangs();
            comboBox1.Items.Clear();
            foreach (string key in langFiles.Keys)
            {
                comboBox1.Items.Add(key);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
            Languages.curlang =
                (comboBox1.SelectedItem != null) ? comboBox1.Text.ToString() : Languages.curlang;

            GlobalSettings.theme =
                (comboBox2.SelectedItem != null) ? (
                (comboBox2.SelectedItem.ToString() == "Light")	? Color.FromName("Control")		:
                (comboBox2.SelectedItem.ToString() == "Dark")	? Color.FromArgb(64, 64, 64)	:
                (comboBox2.SelectedItem.ToString() == "Green")	? Color.FromName("SeaGreen")	:
                (comboBox2.SelectedItem.ToString() == "Cyan")	? Color.FromName("DarkCyan")	:
                (comboBox2.SelectedItem.ToString() == "Pink")	? Color.FromName("Pink")		:
                (comboBox2.SelectedItem.ToString() == "Orange") ? Color.FromName("DarkOrange")	:
                (comboBox2.SelectedItem.ToString() == "Maroon") ? Color.FromName("Maroon")		:
                Color.FromName("Control")) : GlobalSettings.theme;

            GlobalSettings.fontcol =
                (comboBox2.SelectedItem != null) ? (
                (comboBox2.SelectedItem.ToString() == "Light")	? Color.FromName("ControlText")		:
                (comboBox2.SelectedItem.ToString() == "Dark")	? Color.FromName("HighlightText")	:
                (comboBox2.SelectedItem.ToString() == "Green")	? Color.FromName("ControlText")		:
                (comboBox2.SelectedItem.ToString() == "Cyan")	? Color.FromName("HighlightText")	:
                (comboBox2.SelectedItem.ToString() == "Pink")	? Color.FromName("ControlText")		:
                (comboBox2.SelectedItem.ToString() == "Orange")	? Color.FromName("HighlightText")	:
                (comboBox2.SelectedItem.ToString() == "Maroon")	? Color.FromName("HighlightText")	:
                Color.FromName("ControlText")) : GlobalSettings.fontcol;

            GlobalSettings.minimizeToTray = checkBox1.Checked;
            GlobalSettings.dontWorkInBackground = checkBox2.Checked;
            */

            GlobalSettings.ChangeSettings(
                (comboBox1.SelectedItem != null) ? comboBox1.Text.ToString() : Languages.curlang,

                (comboBox2.SelectedItem != null) ? (
                (comboBox2.SelectedItem.ToString() == "Light")  ? Color.FromName("Control")     :
                (comboBox2.SelectedItem.ToString() == "Dark")   ? Color.FromArgb(64, 64, 64)    :
                (comboBox2.SelectedItem.ToString() == "Green")  ? Color.FromName("SeaGreen")    :
                (comboBox2.SelectedItem.ToString() == "Cyan")   ? Color.FromName("DarkCyan")    :
                (comboBox2.SelectedItem.ToString() == "Pink")   ? Color.FromName("Pink")        :
                (comboBox2.SelectedItem.ToString() == "Orange") ? Color.FromName("DarkOrange")  :
                (comboBox2.SelectedItem.ToString() == "Maroon") ? Color.FromName("Maroon")      :
                Color.FromName("Control")) : GlobalSettings.theme,

                (comboBox2.SelectedItem != null) ? (
                (comboBox2.SelectedItem.ToString() == "Light")  ? Color.FromName("ControlText")     :
                (comboBox2.SelectedItem.ToString() == "Dark")   ? Color.FromName("HighlightText")   :
                (comboBox2.SelectedItem.ToString() == "Green")  ? Color.FromName("ControlText")     :
                (comboBox2.SelectedItem.ToString() == "Cyan")   ? Color.FromName("HighlightText")   :
                (comboBox2.SelectedItem.ToString() == "Pink")   ? Color.FromName("ControlText")     :
                (comboBox2.SelectedItem.ToString() == "Orange") ? Color.FromName("HighlightText")   :
                (comboBox2.SelectedItem.ToString() == "Maroon") ? Color.FromName("HighlightText")   :
                Color.FromName("ControlText")) : GlobalSettings.fontcol,

                checkBox1.Checked, checkBox2.Checked
            );
            MessageBox.Show(
                (Languages.curlang == "RU") ?
                "Для полного изменения темы и языка\n" +
                "нажмите в этом сообщении OK и\nперезапустите приложение." :
                "To completely change the theme and language\n" +
                "click in this message OK and\nrestart the application.",
                (Languages.curlang == "RU") ? "Изменение настроек" : "Changing settings",
                MessageBoxButtons.OK, MessageBoxIcon.Warning
            );
            Form1.ReplacePanel<ControlPanel>();
        }
    }
}
