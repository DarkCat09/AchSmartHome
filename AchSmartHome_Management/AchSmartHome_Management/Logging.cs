using System;
using System.IO;

namespace AchSmartHome_Management
{
    class Logging
    {
        public static string temp_path = Path.GetTempPath();
        /// <summary>
        /// Записать событие в лог-файл.
        /// Write event to log-file.
        /// </summary>
        /// <param name="level">Уровень события (0=Information,1=Debug,2=Warning,3=Error,4=Fatal)</param>
        /// <param name="app">Название части программы, логирующей событие</param>
        /// <param name="message">Описание события (сообщение)</param>
        public static void LogEvent(short level, string app, string message)
        {
            try
            {
                string strLevel;
                switch (level)
                {
                    case 0:
                        strLevel = "I";
                        break;
                    case 1:
                        strLevel = "D";
                        break;
                    case 2:
                        strLevel = "W";
                        break;
                    case 3:
                        strLevel = "E";
                        break;
                    case 4:
                        strLevel = "F";
                        break;
                    default:
                        strLevel = "I";
                        break;
                }

                string preparedMessage = message.Replace("\n", "\n\t\t\t\t\t\t");
                string tabulations = (app.Length < 13) ? "\t\t" : "\t";
                preparedMessage =
                    $"{DateTime.Now:dd.MM.yyyy HH:mm:ss.fff} {strLevel}/{app}:{tabulations}{preparedMessage}\n";

                File.AppendAllText($"{temp_path}achsmarthome_mgmt.log", preparedMessage);
            }
            catch (Exception) {}
        }
        /// <summary>
        /// Function deletes temporary log-file.
        /// Удаляет временный лог-файл.
        /// </summary>
        public static void DeleteLogFile()
        {
            try
            {
                File.Delete($"{temp_path}achsmarthome_mgmt.log");
            }
            catch (Exception ex)
            {
                LogEvent(3, "Logging", $"Can\'t delete temporary log-file!\n{ex}");
            }
        }
    }
}
