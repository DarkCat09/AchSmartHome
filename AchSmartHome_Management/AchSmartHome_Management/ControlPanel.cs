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
    public partial class ControlPanel : UserControl
    {
        public ControlPanel()
        {
            InitializeComponent();
            GlobalSettings.InitThemeAndLang(Controls, this);
            GetSensorsValuesAndUpdate(DateTime.Now);
        }

        private void GetSensorsValuesAndUpdate(DateTime dt)
        {
            try
            {
                System.Collections.Generic.Dictionary<int, object> sqlReqResult = DatabaseConnecting.ProcessSqlRequest(
                    "SELECT * FROM `sensors_values` " +
                    "WHERE valdatetime > ('" + dt.ToString("yyyy-MM-dd") + "') " +
                    "AND valdatetime < DATE_ADD('" + dt.ToString("yyyy-MM-dd") + "', INTERVAL 1 DAY)");

                if (sqlReqResult.Count > 0)
                {
                    label1.Text = Convert.ToDouble(sqlReqResult[2]).ToString() + " °C / " +
                        ((Convert.ToDouble(sqlReqResult[2]) * 9.00 / 5.00) + 32.00).ToString() + " °F";

                    label2.Text = Convert.ToInt32(sqlReqResult[3]).ToString() + "%";
                    //label3.Text = label3.Text + " " + sqlReqResult[4].ToString();
                }

                System.Collections.Generic.Dictionary<int, object> sqlReqWateringResult = DatabaseConnecting.ProcessSqlRequest(
                    "SELECT * FROM `watering` " +
                    "WHERE valdatetime > ('" + dt.ToString("yyyy-MM-dd") + "') " +
                    "AND valdatetime < DATE_ADD('" + dt.ToString("yyyy-MM-dd") + "', INTERVAL 1 DAY)");

                if (sqlReqWateringResult.Count > 0)
                {
                    label3.Text += " " + Convert.ToDateTime(sqlReqWateringResult[1]).ToString("dd.MM.yyyy HH:mm:ss");
                }
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show("Произошла ошибка!\n" + ex.Message);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form1.ReplacePanel<LightPanel>();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GetSensorsValuesAndUpdate(dateTimePicker1.Value);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1.ReplacePanel<OtherSensorsPanel>();
        }
    }
}
