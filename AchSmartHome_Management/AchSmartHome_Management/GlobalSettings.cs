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
        public static System.Drawing.Color theme = new System.Drawing.Color();
        public static System.Drawing.Color fontcol = new System.Drawing.Color();
        public static bool minimizeToTray = false;
        public static bool dontWorkInBackground = false;

        public static void InitThemeAndLang(Control.ControlCollection ctrls, Control f)
        {
            System.Collections.Generic.Dictionary<string, string> langFiles = Languages.InitLangs();
            _ = Languages.LoadLang(langFiles[Languages.curlang], langFiles);
            //int errcode = Languages.LoadLang(langFiles[Languages.curlang], langFiles);
            //_ = MessageBox.Show(errcode.ToString());

            f.BackColor = theme;
            foreach (Control ctrl in ctrls)
            {
                if (!(ctrl is Button) && !(ctrl is ComboBox) && !(ctrl is TableLayoutPanel))
                    ctrl.ForeColor = GlobalSettings.fontcol;

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
                                if (!(panelctrl is Button) && !(panelctrl is ComboBox) && !(panelctrl is TableLayoutPanel))
                                    panelctrl.ForeColor = GlobalSettings.fontcol;

                                if (panelctrl.Tag != null)
                                {
                                    if (Languages.Lang.ContainsKey(panelctrl.Tag.ToString()))
                                    {
                                        panelctrl.Text = Languages.Lang[panelctrl.Tag.ToString()];
                                    }
                                }
                            }
                        }

                        if (!(tablectrl is Button) && !(tablectrl is ComboBox) && !(tablectrl is TableLayoutPanel))
                            tablectrl.ForeColor = GlobalSettings.fontcol;

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
