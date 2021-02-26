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
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace AchSmartHome_Management
{
    public partial class MainForm : Form
    {
        private string[] importantFiles = new string[2]
        {
            "langs.conf", "database.conf"
        };

        public static string regusername = "", regpasshash = "";
        public static string username = "", passhash = "";
        public static int userid = 0, userprivs = 3;
        public static Panel panel1 = null;

        private static MyStack<int> prevPanelsType = new MyStack<int>();
        private static MyStack<int> nextPanelsType = new MyStack<int>();

        public MainForm()
        {
            InitializeComponent();

            Logging.DeleteLogFile();

            #region Checking configuration files
            foreach (string impfile in importantFiles)
            {
                if (!File.Exists(impfile))
                {
                    Logging.LogEvent(2, "ConfFilesCheck", $"File {impfile} not found!");
                    ExtractFiles();
                    break;
                }
            }
            #endregion

            #region Connecting to DB
            DatabaseConnecting.ReadDefaultDbParams();
            if (!DatabaseConnecting.ConnectToDb())
            {
                MessageBox.Show(
                    "An error happened while connecting to database! Application will be closed.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error
                );
                Close();
            };
            #endregion

            GlobalSettings.InitThemeAndLang(Controls, this);
            saveFileDialog1.Title = Languages.GetLocalizedString("ChooseLogDest", "Choose Log-file Copying Destination");

            panel1 = new Panel();
            panel1.Name = "panel1";
            panel1.Location = new System.Drawing.Point(12, 51);
            panel1.AutoSize = true;
            panel1.ControlAdded += new ControlEventHandler(PanelChanged);
            Controls.Add(panel1);

            ReplacePanel<ControlPanel>();
            панельНавигацииToolStripMenuItem.Checked = GlobalSettings.showNavigationPanel;
            ChangeNavPanelVisibility(GlobalSettings.showNavigationPanel);
        }

        private void ExtractFiles()
        {
            try
            {
                Logging.LogEvent(1, "ExtractConfFiles", "Extracting archive files.zip from Resources ...");
                #region Extracting files.zip
                FileStream fzfs = File.Open("files.zip", FileMode.Create);
                BinaryWriter fzbw = new BinaryWriter(fzfs);
                fzbw.Write(Properties.Resources.files);
                fzbw.Close();
                fzfs.Close();
                fzbw.Dispose();
                fzfs.Dispose();
                #endregion

                Logging.LogEvent(1, "ExtractConfFiles", "Extracting 7za.exe from Resources ...");
                #region Extracting 7-Zip Standalone Executable
                FileStream szfs = File.Open("7za.exe", FileMode.Create);
                BinaryWriter szbw = new BinaryWriter(szfs);
                szbw.Write(Properties.Resources._7za);
                szbw.Close();
                szfs.Close();
                szbw.Dispose();
                szfs.Dispose();
                #endregion

                Logging.LogEvent(1, "ExtractConfFiles", "Extracting files using 7-Zip ...");
                #region Executing 7-Zip to extract files.zip
                ProcessStartInfo szpinfo = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c \"\"{Path.GetFullPath("7za.exe")}\" x \"{Path.GetFullPath("files.zip")}\" \"",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
                Process szp = Process.Start(szpinfo);
                StreamReader szsrout = szp.StandardOutput;
                string line;
                while ((line = szsrout.ReadLine()) != null)
                {
                    Logging.LogEvent(0, "7-Zip", line);
                }
                szsrout.Close();
                szp.WaitForExit(10000);
                szp.Close();
                szp.Dispose();
                #endregion
            }
            catch (Exception ex) {
                Logging.LogEvent(4, "ExtractConfFiles", "An error happened while extracting files!\n" + ex.ToString());
                DialogResult extractErrRes = MessageBox.Show(
                    Languages.GetLocalizedString("ExtractError", "An error happened while extracting files!"),
                    Languages.GetLocalizedString("Error", "Error"),
                    MessageBoxButtons.AbortRetryIgnore,
                    MessageBoxIcon.Error
                );
                switch (extractErrRes)
                {
                    case DialogResult.Abort:
                        Close();
                        break;
                    case DialogResult.Retry:
                        Logging.LogEvent(0, "ErrorHandling", "Restarting application ...");
                        Application.Restart();
                        break;
                    case DialogResult.Ignore:
                        Logging.LogEvent(2, "ErrorHandling", "Extracting-files-error ignored!");
                        break;
                }
            }
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
            Logging.LogEvent(0, "ReplacePanel", logstring.ToString());

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

        private void пользовательскиеДатчикиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReplacePanel<OtherSensorsPanel>();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Logging.LogEvent(0, "SimpleEventHandler", "Closing program ...");
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
                _ = MessageBox.Show(Languages.GetLocalizedString("InsufPrivs", "Insufficient Privileges!"));
            }
        }

        private void светToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReplacePanel<LightPanel>();
        }

        private void умныйЗвонокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReplacePanel<WirelessDoorbell>();
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
                File.Copy($"{Path.GetTempPath()}achsmarthome_mgmt.log", saveFileDialog1.FileName, true);
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
                    case 4:
                        ReplacePanel<AboutProgram>();
                        break;
                    case 5:
                        ReplacePanel<WirelessDoorbell>();
                        break;
                }
                prevPanelsType.Pop();
                nextPanelsType.Push(beforeBackPanel);
            }
            catch (InvalidOperationException) {}
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
                    case 4:
                        ReplacePanel<AboutProgram>();
                        break;
                    case 5:
                        ReplacePanel<WirelessDoorbell>();
                        break;
                }
                nextPanelsType.Pop();
                prevPanelsType.Push(beforeNextPanel);
            }
            catch (InvalidOperationException) {}
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

            else if (ctrl is AboutProgram)
                return 4;

            else if (ctrl is WirelessDoorbell)
                return 5;

            else
                return 6;
        }

        private void ChangeNavPanelVisibility(bool showPanel)
        {
            if (showPanel)
            {
                panel3.Visible = true;
                panel3.Size = new System.Drawing.Size(panel3.Size.Width, 29);
            }
            else
            {
                panel3.Visible = false;
                panel3.Size = new System.Drawing.Size(panel3.Size.Width, 0); // Я не хочу удалять элемент
            }
        }

        private void панельНавигацииToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            GlobalSettings.ChangeViewSettings(панельНавигацииToolStripMenuItem.Checked);
            ChangeNavPanelVisibility(панельНавигацииToolStripMenuItem.Checked);
        }
    }
}
