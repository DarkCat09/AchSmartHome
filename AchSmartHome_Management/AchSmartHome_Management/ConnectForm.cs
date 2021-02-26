/*
 * Copyright © 2020-2021 Чечкенёв Андрей
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
    public partial class ConnectForm : Form
    {
        public ConnectForm()
        {
            InitializeComponent();
            GlobalSettings.InitThemeAndLang(Controls, this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                System.Collections.Generic.List<object> sqlReqResult = DatabaseConnecting.ProcessSqlRequest(
                    $"SELECT * FROM users WHERE name = \"{textBox2.Text}\""
                );
                if (sqlReqResult.Count > 0)
                {
                    if (BCrypt.Net.BCrypt.Verify(textBox3.Text, sqlReqResult[2].ToString()))
                    {
                        MainForm.userid = Convert.ToInt32(sqlReqResult[0]);
                        MainForm.username = sqlReqResult[1].ToString();
                        MainForm.userprivs = Convert.ToInt32(sqlReqResult[3]);
                        Close();
                    }
                }
                MessageBox.Show(Languages.GetLocalizedString("UserPasswdError", "Username or password is incorrect!"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{Languages.GetLocalizedString("ErrorHappened", "An error happened!")}\n{ex.Message}"
                );
                Close();
            }
        }
    }
}
