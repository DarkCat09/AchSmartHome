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

namespace AchSmartHome_Management
{
    public partial class MainForm : Form
    {
        private string[] importantFiles = new string[2]
        {
            "langs.conf", "database.conf"
        };

        public static Panel panel1 = null;

        private static MyStack<int> prevPanelsType = new MyStack<int>();
        private static MyStack<int> nextPanelsType = new MyStack<int>();

        /// <summary>
        /// Hardcoded past date and time. 
        /// Захардкоженные прошедшие дата и время.
        /// </summary>
        public static DateTime lastUpdateDt = new DateTime(2021, 04, 05, 20, 24, 00);

        /// <summary>
        /// Is application running in the background?
        /// Работает ли приложение в фоне?
        /// </summary>
        public static bool backgroundMode = false;

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

            Accounts.LoadCredentials();

            #region Receiving "last update" value
            List<object> lastUpdateResult = DatabaseConnecting.ProcessSqlRequest(
                "SELECT valdatetime FROM `other_sensors` WHERE id = 0 LIMIT 1"
            );
            if (lastUpdateResult.Count < 1)
            {
                Logging.LogEvent(
                    4, "BackgroundMonitor",
                    "The server did not return last update field! Timer will be disabled."
                );
                notifyIcon1.ShowBalloonTip(
                    2000, "AchSmartHome",
                    Languages.GetLocalizedString(
                        "GetUpdateResultError",
                        "The server did not return the critical system value \"last update\"! " +
                        "Background process will be terminated."
                    ),
                    ToolTipIcon.Error);
                timer1.Enabled = false;
                timer1.Stop();
            }
            lastUpdateDt = DateTime.Parse(lastUpdateResult[0].ToString());
            #endregion

            GlobalSettings.InitThemeAndLang(Controls, this);
            saveFileDialog1.Title = Languages.GetLocalizedString("ChooseLogDest", "Choose Log-file Copying Destination");

            panel1 = new Panel();
            panel1.Name = "panel1";
            panel1.Location = new System.Drawing.Point(12, 51);
            panel1.AutoSize = true;
            panel1.ControlAdded += new ControlEventHandler(PanelChanged);
            Controls.Add(panel1);

            while (Accounts.username.Trim().Equals(""))
            {
                Logging.LogEvent(1, "WindowInit", "User is not logged in. Opening connection dialog ...");
                ConnectForm cf = new ConnectForm();
                DialogResult dr = cf.ShowDialog();
                if (dr == DialogResult.Cancel)
                {
                    CloseApp();
                }
            }

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

        /// <summary>
        /// Function replaces management panel on application main form. 
        /// Заменяет панель управления на главной форме приложения.
        /// </summary>
        /// <typeparam name="panelToAdd">Класс панели, на которую нужно заменить текущую</typeparam>
        /// <param name="appendToStack">Добавить старую панель в стек автоматически? (Полезно для GoBack/GoNext)</param>
        public static void ReplacePanel<panelToAdd>(bool appendToStack = true) where panelToAdd : Control, new()
        {
            System.Text.StringBuilder logstring = new System.Text.StringBuilder("Changing panel ");

            if (panel1.Controls.Count > 0)
            {
                if (appendToStack)
                    prevPanelsType.Push(GetPanelType(panel1.Controls[0]));

                logstring.Append($"from {panel1.Controls[0]} ");
            }

            panelToAdd pta = new panelToAdd();
            logstring.Append($"to {pta} ...\nAppendToStack={appendToStack}");
            Logging.LogEvent(0, "ReplacePanel", logstring.ToString());

            panel1.Controls.Clear();
            panel1.Controls.Add(pta);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Logging.LogEvent(1, "CloseHandler", "MainForm_FormClosing(sender, e) started!");
            if (!GlobalSettings.dontWorkInBackground)
            {
                Logging.LogEvent(0, "CloseHandler", "Application running in the background");
                e.Cancel = true;
                this.Hide();
                backgroundMode = true;
            }
            else
            {
                CloseApp();
            }
        }

        private void CloseApp()
        {
            Logging.LogEvent(0, "CloseHandler", "Closing program ...");
            notifyIcon1.Visible = false;
            DatabaseConnecting.sqlDb.Close();
            Environment.Exit(0);
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseApp();
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

        private void регистрацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegisterUser ruForm = new RegisterUser();
            ruForm.ShowDialog();
            ruForm.Dispose();
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
            MessageBox.Show(
                Properties.Settings.Default.version,
                Languages.GetLocalizedString("AboutProg", "About Program"),
                MessageBoxButtons.OK, MessageBoxIcon.Information
            );
        }

        private void оПроектеASHToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/DarkCat09/AchSmartHome");
        }

        private void копироватьЛогфайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() != DialogResult.Cancel)
            {
                Logging.LogEvent(
                    0, "Logging", $"Copying log-file from {Logging.temp_path} to {saveFileDialog1.FileName}"
                );
                File.Copy($"{Logging.temp_path}achsmarthome_mgmt.log", saveFileDialog1.FileName, true);
            }
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
                Logging.LogEvent(1, "PageChange", $"GoBackPage({form}) started!");

                int beforeBackPanel = GetPanelType(panel1.Controls[0]);
                int prevPanelType = prevPanelsType.Peek();

                if (beforeBackPanel == prevPanelType || prevPanelType < 0)
                    return;
                switch (prevPanelType)
                {
                    case 0:
                        ReplacePanel<ControlPanel>(false);
                        break;
                    case 1:
                        ReplacePanel<LightPanel>(false);
                        break;
                    case 2:
                        ReplacePanel<OtherSensorsPanel>(false);
                        break;
                    case 3:
                        panel1.Controls.Clear();
                        panel1.Controls.Add(new SettingsForm((Form)form));
                        break;
                    case 4:
                        ReplacePanel<AboutProgram>(false);
                        break;
                    case 5:
                        ReplacePanel<WirelessDoorbell>(false);
                        break;
                }
                prevPanelsType.Pop();
                nextPanelsType.Push(beforeBackPanel);

                Logging.LogEvent(0, "PageChange", "Completed!");
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
                Logging.LogEvent(1, "PageChange", $"GoNextPage({form}) started!");

                int beforeNextPanel = GetPanelType(panel1.Controls[0]);
                int nextPanelType = nextPanelsType.Peek();

                if (beforeNextPanel == nextPanelType || nextPanelType < 0)
                    return;
                switch (nextPanelType)
                {
                    case 0:
                        ReplacePanel<ControlPanel>(false);
                        break;
                    case 1:
                        ReplacePanel<LightPanel>(false);
                        break;
                    case 2:
                        ReplacePanel<OtherSensorsPanel>(false);
                        break;
                    case 3:
                        panel1.Controls.Clear();
                        panel1.Controls.Add(new SettingsForm((Form)form));
                        break;
                    case 4:
                        ReplacePanel<AboutProgram>(false);
                        break;
                    case 5:
                        ReplacePanel<WirelessDoorbell>(false);
                        break;
                }
                nextPanelsType.Pop();
                prevPanelsType.Push(beforeNextPanel);

                Logging.LogEvent(0, "PageChange", "Completed!");
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
            /* AutoSize improvment:
             * Reset form size to smaller to start auto-resizing
             */
            Logging.LogEvent(0, "ReplacePanel", "Resizing main window...");
            this.Size = new System.Drawing.Size(397, 224);

            // Changing header text
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
                panel1.Location = new System.Drawing.Point(panel1.Location.X, 51);
                panel3.Visible = true;
                panel3.Size = new System.Drawing.Size(panel3.Size.Width, 29);
            }
            else
            {
                panel1.Location = new System.Drawing.Point(panel1.Location.X, 24);
                panel3.Visible = false;
                panel3.Size = new System.Drawing.Size(panel3.Size.Width, 0); // Я не хочу удалять элемент
            }
        }

        private void панельНавигацииToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            GlobalSettings.ChangeViewSettings(панельНавигацииToolStripMenuItem.Checked);
            ChangeNavPanelVisibility(панельНавигацииToolStripMenuItem.Checked);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Checking for new values of doorbell in the DB
            List<object> newid = DatabaseConnecting.ProcessSqlRequest(
                $"SELECT `photosid`, `camdatetime` FROM `doorbell` " +
                $"WHERE `camdatetime` > '{lastUpdateDt:yyyy-MM-dd HH:mm:ss}' " +
                $"ORDER BY `camdatetime` DESC"
            );
            if (newid.Count > 0)
            {
                // Change "last update" field value
                lastUpdateDt = DateTime.Parse(newid[1].ToString());
                DatabaseConnecting.ProcessSqlRequest(
                    $"UPDATE `other_sensors` SET valdatetime='{lastUpdateDt:yyyy-MM-dd HH:mm:ss}' WHERE id = 0",
                    null, true
                );
                // Notify about it
                Logging.LogEvent(0, "BackgroundMonitor", "New doorbell value");
                notifyIcon1.ShowBalloonTip(
                    2000, "AchSmartHome",
                    Languages.GetLocalizedString("DoorbellActivated", "The doorbell rings! View photos."),
                    ToolTipIcon.Info
                );
                if (backgroundMode)
                    ReplacePanel<WirelessDoorbell>();
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            backgroundMode = false;
        }

        private void выходИзПрофиляToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Accounts.username = "";
            Accounts.passhash = "";
            Accounts.userid = 0;
            Accounts.userprivs = 4;
            Accounts.SaveCredentials();
        }

        private void перезапускToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void перезапускМониторингаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Start();
        }

        private void выходTrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseApp();
        }

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            // instead of DockStyle.Fill, that breaks all layout
            //panel1.Size = new System.Drawing.Size(this.Size.Width - 24, this.Size.Height - 51);
        }
    }
}
