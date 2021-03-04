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
                if (!Accounts.CheckUserCredentials(textBox2.Text.Trim(), textBox3.Text))
                    MessageBox.Show(
                        Languages.GetLocalizedString("UserPasswdError", "Username or password is incorrect!")
                    );
                else
                {
                    Accounts.username = textBox2.Text.Trim();
                    Accounts.passhash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(textBox3.Text));
                    Accounts.GetUserData();
                    Accounts.SaveCredentials();
                    Close();
                }
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
