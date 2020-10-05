using System;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AchSmartHome_Management
{
    class DatabaseConnecting
    {
        public static string dbaddr = "", dbname = "", dbport = "", dbuser = "", dbpass = "";
        public static void ReadDefaultDbParams()
        {
            try
            {
                string[] dbParamsFromFile = File.ReadAllLines("database.conf");
                if (dbParamsFromFile.Length > 1)
                {
                    dbaddr = dbParamsFromFile[0].Split(new char[] { ';' })[0];
                    dbname = dbParamsFromFile[0].Split(new char[] { ';' })[1];
                    dbport = dbParamsFromFile[0].Split(new char[] { ';' })[2];
                    dbuser = dbParamsFromFile[1].Split(new char[] { ';' })[0];
                    dbpass = dbParamsFromFile[1].Split(new char[] { ';' })[1];
                }
            }
            catch (Exception)
            {
                _ = MessageBox.Show("Error happened while reading database parameters!");
            }
        }
        public static MySqlConnection ConnectToDb(string advancedServer = "")
        {
            string sqlConnect =
                    "Server=" + ((advancedServer.Trim() != "") ? advancedServer.Trim() : dbaddr) + ";" +
                    "Database=" + dbname + ";port=" + dbport + ";" +
                    "User Id=" + dbuser + ";password=" + dbpass;
            return new MySqlConnection(sqlConnect);
        }
    }
}
