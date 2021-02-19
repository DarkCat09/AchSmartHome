using System;
using System.IO;

namespace AchSmartHome_Management
{
    class Logging
    {
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

                File.AppendAllText($"{Path.GetTempPath()}achsmarthome_mgmt.log", preparedMessage);
            }
            catch (Exception) {}
        }
        public static void DeleteLogFile()
        {
            try
            {
                File.Delete($"{Path.GetTempPath()}achsmarthome_mgmt.log");
            }
            catch (Exception ex)
            {
                LogEvent(3, "Logging", $"Can\'t delete temporary log-file!\n{ex}");
            }
        }
    }
}
