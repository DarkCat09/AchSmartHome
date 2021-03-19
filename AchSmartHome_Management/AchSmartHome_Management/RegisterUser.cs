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
    public partial class RegisterUser : Form
    {
        public RegisterUser()
        {
            InitializeComponent();
            GlobalSettings.InitThemeAndLang(Controls, this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Accounts.RegisterUser(textBox1.Text, textBox2.Text))
                Close();
            else
                MessageBox.Show(
                    Languages.GetLocalizedString("InsufPrivs", "Insufficient Privileges!"), "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error
                );
        }
    }
}
