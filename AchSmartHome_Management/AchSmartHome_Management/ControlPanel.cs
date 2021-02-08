﻿/*
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
    public partial class ControlPanel : UserControl
    {
        string[] sensorsText = null;
        ToolTip[] sensorsToolTips = null;
        public ControlPanel()
        {
            InitializeComponent();
            GlobalSettings.InitThemeAndLang(Controls, this);

            sensorsToolTips = new ToolTip[6];
            for (int i = 0; i < 6; i++)
            {
                sensorsToolTips[i] = new ToolTip();
            }

            foreach (ToolTip tt in sensorsToolTips)
            {
                tt.ToolTipTitle =
                    sensorsToolTips[0].ToolTipTitle = Languages.GetLocalizedString("LastUpdate", "Last Update: ");
                tt.IsBalloon = true;
            }

            sensorsText = new string[6] {
                label1.Text, label2.Text, label5.Text, linkLabel1.Text, label3.Text, ""
            };
            GetSensorsValuesAndUpdate(DateTime.Now);
        }

        private void GetSensorsValuesAndUpdate(DateTime dt)
        {
            Logging.LogEvent(0, "Updating main sensors values... DT = " + dt.ToString("dd.MM.yyyy,HH:mm"));
            try
            {
                label1.Text     = sensorsText[0];
                label2.Text     = sensorsText[1];
                label5.Text     = sensorsText[2];
                linkLabel1.Text = sensorsText[3];
                label3.Text     = sensorsText[4];

                System.Collections.Generic.List<object> sqlReqResult = DatabaseConnecting.ProcessSqlRequest(
                    "SELECT id, valdatetime, IFNULL(temp, 0.00) AS temp, IFNULL(humidity, 0.00) AS humidity " +
                    "FROM `sensors_values` " +
                    "WHERE valdatetime < DATE_ADD('" + dt.ToString("yyyy-MM-dd") + "', INTERVAL 1 DAY) " +
                    "ORDER BY id DESC LIMIT 1"
                );

                if (sqlReqResult.Count > 0)
                {
                    label1.Text =
                        Convert.ToDouble(sqlReqResult[2]).ToString() + " °C / " +
                        ((Convert.ToDouble(sqlReqResult[2]) * 9.00 / 5.00) + 32.00).ToString() + " °F";

                    sensorsToolTips[0].SetToolTip(
                        label1, sensorsToolTips[0].ToolTipTitle + sqlReqResult[1].ToString()
                    );

                    label2.Text = Convert.ToInt32(sqlReqResult[3]).ToString() + "%";

                    sensorsToolTips[1].SetToolTip(
                        label2, sensorsToolTips[1].ToolTipTitle + sqlReqResult[1].ToString()
                    );
                }

                System.Collections.Generic.List<object> sqlReqWateringResult =
                    DatabaseConnecting.ProcessSqlRequest(
                        "SELECT * FROM `watering` " +
                        "WHERE valdatetime > ('" + dt.ToString("yyyy-MM-dd") + "') " +
                        "AND valdatetime < DATE_ADD('" + dt.ToString("yyyy-MM-dd") + "', INTERVAL 1 DAY) " +
                        "ORDER BY id DESC LIMIT 1"
                    );

                if (sqlReqWateringResult.Count > 0)
                {
                    label3.Text += " " + Convert.ToDateTime(sqlReqWateringResult[1]).ToString("dd.MM.yyyy HH:mm:ss");
                }
            }
            catch (Exception ex)
            {
                Logging.LogEvent(3, "Error happened while updating main sensors values!\n" + ex.ToString());
                _ = MessageBox.Show("Произошла ошибка!\n" + ex.Message);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainForm.ReplacePanel<LightPanel>();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GetSensorsValuesAndUpdate(dateTimePicker1.Value);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainForm.ReplacePanel<OtherSensorsPanel>();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (GlobalSettings.autoUpdateSensorsVals)
                GetSensorsValuesAndUpdate(dateTimePicker1.Value);
        }
    }
}
