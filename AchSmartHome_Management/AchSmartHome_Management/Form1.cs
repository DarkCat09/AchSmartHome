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
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace AchSmartHome_Management
{
    public partial class Form1 : Form
    {
        public static string regusername = "", regpasshash = "";
        public static string username = "", passhash = "";
        public static int userid = 0, userprivs = 3;
        public static Panel panel1 = null;

        private static MyStack<int> prevPanelsType = new MyStack<int>();
        private static MyStack<int> nextPanelsType = new MyStack<int>();

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
            saveFileDialog1.Title = Languages.GetLocalizedString("ChooseLogDest", "Choose Log-file Copying Destination");
            ReplacePanel<ControlPanel>();
        }

        public static void ReplacePanel<panelToAdd>() where panelToAdd : Control, new()
        {
            System.Text.StringBuilder logstring = new System.Text.StringBuilder("Changing panel ");

            if (panel1.Controls.Count > 0)
            {
                prevPanelsType.Push(GetPanelType(panel1.Controls[0]));
                logstring.Append($"from {panel1.Controls[0]} ");
            }

            panelToAdd pta = new panelToAdd();
            logstring.Append($"to {pta} ...");
            Logging.LogEvent(0, logstring.ToString());

            panel1.Controls.Clear();
            panel1.Controls.Add(pta);
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
                    new List<MySqlParameter>() {
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

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReplacePanel<AboutProgram>();
        }

        private void копироватьЛогфайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() != DialogResult.Cancel)
                File.Copy($"{Path.GetTempPath()}achsmarthome_mgmt.log", saveFileDialog1.FileName);
        }

        /// <summary>
        /// Go to the previous page
        /// Перейти на предыдущую страницу
        /// </summary>
        /// <param name="form">Компонент, вызывающий функцию</param>
        public static void GoBackPage(Control form)
        {
            try
            {
                int beforeBackPanel = GetPanelType(panel1.Controls[0]);
                int prevPanelType = prevPanelsType.Peek();

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
                        panel1.Controls.Add(new SettingsForm((Form)form));
                        break;
                }
                prevPanelsType.Pop();
                nextPanelsType.Push(beforeBackPanel);
            }
            catch (InvalidOperationException) { }
        }
        /// <summary>
        /// Go to the next page
        /// Перейти на следующую страницу
        /// </summary>
        /// <param name="form">Компонент, вызывающий функцию</param>
        public void GoNextPage(Control form)
        {
            try
            {
                int beforeNextPanel = GetPanelType(panel1.Controls[0]);
                int nextPanelType = nextPanelsType.Peek();

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
                        panel1.Controls.Add(new SettingsForm((Form)form));
                        break;
                }
                nextPanelsType.Pop();
                prevPanelsType.Push(beforeNextPanel);
            }
            catch (InvalidOperationException) { }
        }

        private void goBackLabel_Click(object sender, EventArgs e)
        {
            GoBackPage(this);
        }

        private void goNextLabel_Click(object sender, EventArgs e)
        {
            GoNextPage(this);
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
