﻿using System;
using System.IO;

namespace AchSmartHome_Management
{
    class Logging
    {
        public static void LogEvent(short level, string message)
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
                
                string preparedMessage = message.Replace("\n", "\n\t\t\t\t");
                /*
                if (preparedMessage.EndsWith("\n"))
                    preparedMessage.Remove(preparedMessage.Length - 2, preparedMessage.Length - 1);
                */
                preparedMessage =
                    DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff ") + strLevel +
                    ":\t" + preparedMessage + "\n";

                File.AppendAllText(Path.GetTempPath().ToString() + "achsmarthome_mgmt.log", preparedMessage);
            }
            catch (Exception)
            {
                // do nothing
            }
        }
        public static void DeleteLogFile()
        {
            try
            {
                File.Delete(Path.GetTempPath().ToString() + "achsmarthome_mgmt.log");
            }
            catch (Exception ex)
            {
                LogEvent(3, "Can\'t delete temporary log-file!\n" + ex.ToString());
            }
        }
    }
}
