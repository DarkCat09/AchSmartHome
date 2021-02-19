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

using System.Drawing;
using System.Windows.Forms;

namespace AchSmartHome_Management
{
    class GlobalSettings
    {
        public static Color theme = Properties.Settings.Default.theme;
        public static Color fontcol = Properties.Settings.Default.font_color;
        public static bool minimizeToTray = Properties.Settings.Default.min_to_tray;
        public static bool dontWorkInBackground = Properties.Settings.Default.dont_work_in_bg;
        public static bool autoUpdateSensorsVals = Properties.Settings.Default.auto_upd_values;

        public static void ChangeSettings(
            string _language,
            Color _theme,
            Color _fontcol,
            bool _minToTray,
            bool _dontWorkInBg,
            bool _autoUpdSensorsVals
        )
        {
            // update local settings
            Languages.curlang = _language;
            theme = _theme;
            fontcol = _fontcol;
            minimizeToTray = _minToTray;
            dontWorkInBackground = _dontWorkInBg;
            autoUpdateSensorsVals = _autoUpdSensorsVals;

            // update settings in special file
            Properties.Settings.Default.theme = _theme;
            Properties.Settings.Default.font_color = _fontcol;
            Properties.Settings.Default.min_to_tray = _minToTray;
            Properties.Settings.Default.dont_work_in_bg = _dontWorkInBg;
            Properties.Settings.Default.auto_upd_values = _autoUpdSensorsVals;
            Properties.Settings.Default.language = _language;
            Properties.Settings.Default.Save();
        }

        public static void InitThemeAndLang(Control.ControlCollection ctrls, Control f)
        {
            System.Collections.Generic.Dictionary<string, string> langFiles = Languages.InitLangs();
            int errcode = Languages.LoadLang(langFiles[Languages.curlang], langFiles);
            Logging.LogEvent(3, "ThemeInitializer", $"Error happened while loading languages! LoadLang() exit code = {errcode}.");

            f.BackColor = theme;
            foreach (Control ctrl in ctrls)
            {
                if (DoesThisElemColorChanging(ctrl))
                    ctrl.ForeColor = fontcol;
                if (ctrl is LinkLabel)
                    ((LinkLabel)ctrl).LinkColor = fontcol;

                //for Panel
                if (ctrl is Panel)
                {
                    foreach (Control panelctrl in ((Panel)ctrl).Controls)
                    {
                        if (DoesThisElemColorChanging(panelctrl))
                            panelctrl.ForeColor = fontcol;
                        if (panelctrl is LinkLabel)
                            ((LinkLabel)panelctrl).LinkColor = fontcol;

                        if (panelctrl.Tag != null)
                        {
                            if (Languages.Lang.ContainsKey(panelctrl.Tag.ToString()))
                            {
                                panelctrl.Text = Languages.Lang[panelctrl.Tag.ToString()];
                            }
                        }
                    }
                }

                //for TableLayoutPanel
                if (ctrl is TableLayoutPanel)
                {
                    foreach (Control tablectrl in ((TableLayoutPanel)ctrl).Controls)
                    {
                        //for Panel
                        if (tablectrl is Panel)
                        {
                            foreach (Control panelctrl in ((Panel)tablectrl).Controls)
                            {
                                if (DoesThisElemColorChanging(panelctrl))
                                    panelctrl.ForeColor = fontcol;
                                if (panelctrl is LinkLabel)
                                    ((LinkLabel)panelctrl).LinkColor = fontcol;

                                if (panelctrl.Tag != null)
                                {
                                    if (Languages.Lang.ContainsKey(panelctrl.Tag.ToString()))
                                    {
                                        panelctrl.Text = Languages.Lang[panelctrl.Tag.ToString()];
                                    }
                                }
                            }
                        }

                        if (DoesThisElemColorChanging(tablectrl))
                            tablectrl.ForeColor = fontcol;
                        if (tablectrl is LinkLabel)
                            ((LinkLabel)tablectrl).LinkColor = fontcol;

                        if (tablectrl.Tag != null)
                        {
                            if (Languages.Lang.ContainsKey(tablectrl.Tag.ToString()))
                            {
                                tablectrl.Text = Languages.Lang[tablectrl.Tag.ToString()];
                            }
                        }
                    }
                }

                if (ctrl.Tag != null)
                {
                    if (Languages.Lang.ContainsKey(ctrl.Tag.ToString()))
                    {
                        ctrl.Text = Languages.Lang[ctrl.Tag.ToString()];
                    }
                }
            }
        }

        private static bool DoesThisElemColorChanging(Control ctrlToCheck)
        {
            return (
                !(ctrlToCheck is Button) &&
                !(ctrlToCheck is ComboBox) &&
                !(ctrlToCheck is MenuStrip) &&
                !(ctrlToCheck is TableLayoutPanel) &&
                !(ctrlToCheck is Panel) &&
                !(ctrlToCheck is DataGridView) &&
                !(ctrlToCheck is TextBox)
            );
        }

        public static Color ThemeStringToColor(object value)
        {
            return
                (value != null) ? (
                (value.ToString() == "Light")  ? Color.FromName("Control")     :
                (value.ToString() == "Dark")   ? Color.FromArgb(64, 64, 64)    :
                (value.ToString() == "Green")  ? Color.FromName("SeaGreen")    :
                (value.ToString() == "Cyan")   ? Color.FromName("DarkCyan")    :
                (value.ToString() == "Pink")   ? Color.FromName("Pink")        :
                (value.ToString() == "Orange") ? Color.FromName("DarkOrange")  :
                (value.ToString() == "Maroon") ? Color.FromName("Maroon")      :
                Color.FromName("Control")) : theme;
        }
        public static string ColorToThemeName(Color value)
        {
            string colorStr = (value != null) ? value.ToString() : "Control";
            return (
                (colorStr == "Control")     ? "Light"   :
                (value.R == 64 && value.G == 64 && value.B == 64) ? "Dark" :
                (colorStr == "SeaGreen")    ? "Green"   :
                (colorStr == "DarkCyan")    ? "Cyan"    :
                (colorStr == "Pink")        ? "Pink"    :
                (colorStr == "DarkOrange")  ? "Orange"  :
                (colorStr == "Maroon")      ? "Maroon"  :
                "Light"
            );
        }
    }
}
