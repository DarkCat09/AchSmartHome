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
    class GlobalSettings
    {
        public static System.Drawing.Color theme = Properties.Settings.Default.theme;
        public static System.Drawing.Color fontcol = Properties.Settings.Default.font_color;
        public static bool minimizeToTray = Properties.Settings.Default.min_to_tray;
        public static bool dontWorkInBackground = Properties.Settings.Default.dont_work_in_bg;

        public static void ChangeSettings(
            string _language,
            System.Drawing.Color _theme,
            System.Drawing.Color _fontcol,
            bool _minToTray,
            bool _dontWorkInBg
        )
        {
            // update local settings
            Languages.curlang = _language;
            theme = _theme;
            fontcol = _fontcol;
            minimizeToTray = _minToTray;
            dontWorkInBackground = _dontWorkInBg;

            // update settings in special file
            Properties.Settings.Default.theme = _theme;
            Properties.Settings.Default.font_color = _fontcol;
            Properties.Settings.Default.min_to_tray = _minToTray;
            Properties.Settings.Default.dont_work_in_bg = _dontWorkInBg;
            Properties.Settings.Default.language = _language;
            Properties.Settings.Default.Save();
        }

        public static void InitThemeAndLang(Control.ControlCollection ctrls, Control f)
        {
            System.Collections.Generic.Dictionary<string, string> langFiles = Languages.InitLangs();
            _ = Languages.LoadLang(langFiles[Languages.curlang], langFiles);
            //int errcode = Languages.LoadLang(langFiles[Languages.curlang], langFiles);
            //_ = MessageBox.Show(errcode.ToString());

            f.BackColor = theme;
            foreach (Control ctrl in ctrls)
            {
                if (!(ctrl is Button) && !(ctrl is ComboBox) && !(ctrl is MenuStrip) && !(ctrl is TableLayoutPanel) && !(ctrl is Panel))
                    ctrl.ForeColor = GlobalSettings.fontcol;
                if (ctrl is LinkLabel)
                    ((LinkLabel)ctrl).LinkColor = GlobalSettings.fontcol;

                //for Panel
                if (ctrl is Panel)
                {
                    foreach (Control panelctrl in ((Panel)ctrl).Controls)
                    {
                        if (!(panelctrl is Button) && !(panelctrl is ComboBox) && !(panelctrl is MenuStrip) && !(panelctrl is TableLayoutPanel))
                            panelctrl.ForeColor = GlobalSettings.fontcol;
                        if (panelctrl is LinkLabel)
                            ((LinkLabel)panelctrl).LinkColor = GlobalSettings.fontcol;

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
                                if (!(panelctrl is Button) && !(panelctrl is ComboBox) && !(panelctrl is MenuStrip) && !(panelctrl is TableLayoutPanel))
                                    panelctrl.ForeColor = GlobalSettings.fontcol;
                                if (panelctrl is LinkLabel)
                                    ((LinkLabel)panelctrl).LinkColor = GlobalSettings.fontcol;

                                if (panelctrl.Tag != null)
                                {
                                    if (Languages.Lang.ContainsKey(panelctrl.Tag.ToString()))
                                    {
                                        panelctrl.Text = Languages.Lang[panelctrl.Tag.ToString()];
                                    }
                                }
                            }
                        }

                        if (!(tablectrl is Button) && !(tablectrl is ComboBox) && !(tablectrl is MenuStrip) && !(tablectrl is TableLayoutPanel))
                            tablectrl.ForeColor = GlobalSettings.fontcol;
                        if (tablectrl is LinkLabel)
                            ((LinkLabel)tablectrl).LinkColor = GlobalSettings.fontcol;

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
    }
}
