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
using MySql.Data.MySqlClient;

namespace AchSmartHome_Management
{
    public partial class Form1 : Form
    {
        public static string regusername = "", regpasshash = "";
        public static string username = "", passhash = "";
        public static int userid = 0, userprivs = 3;
        public static Panel panel1 = null;

        private static int prevPanelType = -1;
        private static int nextPanelType = -1;

        public Form1()
        {
            InitializeComponent();

            DatabaseConnecting.ReadDefaultDbParams();
            if (!DatabaseConnecting.ConnectToDb())
            {
                MessageBox.Show(
                    "Error happened while connecting to database! Application will be closed.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error
                );
                Close();
            };

            panel1 = new Panel();
            panel1.Name = "panel1";
            panel1.Location = new System.Drawing.Point(12, 51);
            panel1.AutoSize = true;
            panel1.ControlAdded += new ControlEventHandler(PanelChanged);
            Controls.Add(panel1);

            GlobalSettings.InitThemeAndLang(Controls, this);
            ReplacePanel<ControlPanel>();
        }

        public static void ReplacePanel<panelToAdd>() where panelToAdd : Control, new()
        {
            if (panel1.Controls.Count > 0)
                prevPanelType = GetPanelType(panel1.Controls[0]);
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
            cf.Dispose();
        }

        private void главнаяСтраницаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReplacePanel<ControlPanel>();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Logging.DeleteLogFile();
            DatabaseConnecting.sqlDb.Close();
        }

        private void регистрацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (userprivs == 0)
            {
                RegisterUser ruForm = new RegisterUser();
                ruForm.ShowDialog();
                ruForm.Dispose();

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
            panel1.Controls.Clear();
            panel1.Controls.Add(new SettingsForm(this));
        }

        private void goBackLabel_Click(object sender, EventArgs e)
        {
            int beforeBackPanel = GetPanelType(panel1.Controls[0]);
            if (beforeBackPanel == prevPanelType || prevPanelType < 0)
                return;
            switch (prevPanelType)
            {
                case 0:
                    ReplacePanel<ControlPanel>();
                    break;
                case 1:
                    ReplacePanel<LightPanel>();
                    break;
                case 2:
                    ReplacePanel<OtherSensorsPanel>();
                    break;
                case 3:
                    panel1.Controls.Clear();
                    panel1.Controls.Add(new SettingsForm(this));
                    break;
            }
            nextPanelType = beforeBackPanel;
        }

        private void goNextLabel_Click(object sender, EventArgs e)
        {
            int beforeNextPanel = GetPanelType(panel1.Controls[0]);
            if (beforeNextPanel == nextPanelType || nextPanelType < 0)
                return;
            switch (nextPanelType)
            {
                case 0:
                    ReplacePanel<ControlPanel>();
                    break;
                case 1:
                    ReplacePanel<LightPanel>();
                    break;
                case 2:
                    ReplacePanel<OtherSensorsPanel>();
                    break;
                case 3:
                    panel1.Controls.Clear();
                    panel1.Controls.Add(new SettingsForm(this));
                    break;
            }
            prevPanelType = beforeNextPanel;
        }

        private void panelNameLabel_Click(object sender, EventArgs e)
        {
            ChoosePanel cp = new ChoosePanel();
            cp.ShowDialog();
            cp.Dispose();
        }

        private void PanelChanged(object sender, ControlEventArgs e)
        {
            panelNameLabel.Text = Languages.GetLocalizedString("PanelName" + GetPanelType(e.Control), "Panel");
        }

        public static int GetPanelType(Control ctrl)
        {
            if (ctrl is ControlPanel)
                return 0;

            else if (ctrl is LightPanel)
                return 1;

            else if (ctrl is OtherSensorsPanel)
                return 2;

            else if (ctrl is SettingsForm)
                return 3;

            else
                return 4;
        }
    }
}
