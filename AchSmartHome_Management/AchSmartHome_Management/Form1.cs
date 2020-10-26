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

using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace AchSmartHome_Management
{
    public partial class Form1 : Form
    {
        public static string regusername = "", regpasshash = "";
        public static string username = "", passhash = "";
        public static int userid = 0, userprivs = 3;
        public static Panel panel1 = null;

        public Form1()
        {
            InitializeComponent();

            DatabaseConnecting.ReadDefaultDbParams();
            DatabaseConnecting.ConnectToDb();

            panel1 = new Panel();
            panel1.Name = "panel1";
            panel1.Location = new System.Drawing.Point(13, 28);
            panel1.AutoSize = true;
            Controls.Add(panel1);

            ReplacePanel<ControlPanel>();
            GlobalSettings.InitThemeAndLang(Controls, this);
        }

        public static void ReplacePanel<panelToAdd>() where panelToAdd : Control, new()
        {
            panel1.Controls.Clear();
            panel1.Controls.Add(new panelToAdd());
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void соединитьсяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConnectForm cf = new ConnectForm();
            cf.ShowDialog();
        }

        private void главнаяСтраницаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReplacePanel<ControlPanel>();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DatabaseConnecting.sqlDb.Close();
        }

        private void регистрацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (userprivs == 0)
            {
                RegisterUser ruForm = new RegisterUser();
                ruForm.ShowDialog();

                /*MySqlConnection sqlDb = DatabaseConnecting.ConnectToDb();
                sqlDb.Open();
                MySqlCommand sqlRequest =
                    new MySqlCommand("INSERT INTO users(name, passhash, privs) VALUES (?username, ?passhash, 1)", sqlDb);
                sqlRequest.Parameters.Add(new MySqlParameter("username", regusername));
                sqlRequest.Parameters.Add(new MySqlParameter("passhash", regpasshash));
                sqlRequest.ExecuteNonQuery();
                sqlDb.Close();*/

                DatabaseConnecting.ProcessSqlRequest(
                    "INSERT INTO users(name, passhash, privs) VALUES (?username, ?passhash, 1)",
                    new System.Collections.Generic.List<MySqlParameter>() {
                        new MySqlParameter("username", regusername), new MySqlParameter("passhash", regpasshash)
                    }, true
                );
            }
            else
            {
                _ = MessageBox.Show((Languages.curlang == "RU") ? "Недостаточно привелегий!" : "Insufficient privileges!");
            }
        }

        private void светToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReplacePanel<LightPanel>();
        }

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReplacePanel<SettingsForm>();
        }
    }
}
