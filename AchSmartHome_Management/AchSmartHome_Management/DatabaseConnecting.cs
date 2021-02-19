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
using System.Data.Common;
using System.Collections.Generic;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AchSmartHome_Management
{
    class DatabaseConnecting
    {
        public static string smartHomeServer = "";
        public static string dbaddr = "", dbname = "", dbport = "", dbuser = "", dbpass = "";
        public static MySqlConnection sqlDb = null;
        public static void ReadDefaultDbParams()
        {
            Logging.LogEvent(0, "SQLConnectOptions", "Receiving DataBase-parameters from database.conf ...");
            try
            {
                Logging.LogEvent(1, "SQLConnectOptions", "Reading database.conf ...");
                string[] dbParamsFromFile = File.ReadAllLines("database.conf");
                Logging.LogEvent(1, "SQLConnectOptions", "Retreiving DB-parameters ...");
                if (dbParamsFromFile.Length > 1)
                {
                    dbaddr = dbParamsFromFile[0].Split(new char[] { ';' })[0];
                    dbname = dbParamsFromFile[0].Split(new char[] { ';' })[1];
                    dbport = dbParamsFromFile[0].Split(new char[] { ';' })[2];
                    dbuser = dbParamsFromFile[1].Split(new char[] { ';' })[0];
                    dbpass = dbParamsFromFile[1].Split(new char[] { ';' })[1];
                }
            }
            catch (Exception ex)
            {
                Logging.LogEvent(3, "SQLConnectOptions", "Can\'t read database params!\n" + ex.ToString());
                _ = MessageBox.Show("Error happened while reading database parameters!");
            }
        }
        public static bool ConnectToDb(string advancedServer = "")
        {
            try
            {
                smartHomeServer = ((advancedServer.Trim() != "") ? advancedServer.Trim() : dbaddr);
                sqlDb = new MySqlConnection(
                        "Server=" + smartHomeServer + ";" +
                        "Database=" + dbname + ";port=" + dbport + ";" +
                        "User Id=" + dbuser + ";password=" + dbpass);
                sqlDb.Open();
                return true;
            }
            catch (Exception ex)
            {
                Logging.LogEvent(4, "SQLConnect", "Can\'t connect to server! Exiting!\n" + ex.ToString());
                return false;
            }
        }
        public static List<object> ProcessSqlRequest(string sqlRequest, List<MySqlParameter> sqlReqParams = null)
        {
            string paramsForLog = "";
            if (sqlReqParams == null)
                sqlReqParams = new List<MySqlParameter>();
            List<object> sqlReqResult = new List<object>();
            try
            {
                MySqlCommand sqlCommand = new MySqlCommand(sqlRequest, sqlDb);
                if (sqlReqParams.Count > 0)
                {
                    foreach (MySqlParameter sqlCommandParam in sqlReqParams)
                    {
                        paramsForLog += (
                            sqlCommandParam.MySqlDbType.ToString() + " " +
                            sqlCommandParam.ParameterName + " = " + sqlCommandParam.Value + "\n"
                        );
                        sqlCommand.Parameters.Add(sqlCommandParam);
                    }
                }
                Logging.LogEvent(
                    0, "SQLExecuter", "Executing SQL-request:\n" + sqlRequest + "\nParams:\n" + paramsForLog
                );
                DbDataReader dbdr = sqlCommand.ExecuteReader();
                int read_index = 0;
                while (dbdr.Read())
                {
                    Logging.LogEvent(
                        1, "SQLExecuter",
                        "DataRead Iteration #" + read_index.ToString() +
                        ", FieldCount: " + dbdr.FieldCount
                    );
                    for (int i = 0; i < dbdr.FieldCount; i++)
                    {
                        sqlReqResult.Add(dbdr.GetValue(i));
                    }
                    read_index++;
                }
                dbdr.Close();
            }
            catch (Exception ex)
            {
                Logging.LogEvent(3, "SQLExecuter", "Error happened while executing SQL-request:\n" + ex.ToString());
            }
            return sqlReqResult;
        }
        public static List<object> ProcessSqlRequest(string sqlRequest, List<MySqlParameter> sqlReqParams, bool dontRetResult)
        {
            if (!dontRetResult)
            {
                return ProcessSqlRequest(sqlRequest, sqlReqParams);
            }
            else
            {
                string paramsForLog = "";
                if (sqlReqParams == null)
                    sqlReqParams = new List<MySqlParameter>();
                try
                {
                    MySqlCommand sqlCommand = new MySqlCommand(sqlRequest, sqlDb);
                    if (sqlReqParams.Count > 0)
                    {
                        foreach (MySqlParameter sqlCommandParam in sqlReqParams)
                        {
                            paramsForLog += (
                                sqlCommandParam.MySqlDbType.ToString() + " " +
                                sqlCommandParam.ParameterName + " = " + sqlCommandParam.Value + "\n"
                            );
                            sqlCommand.Parameters.Add(sqlCommandParam);
                        }
                    }
                    Logging.LogEvent(
                        0, "SQLExecuter", "Executing DRR SQL-request:\n" + sqlRequest + "\nParams:\n" + paramsForLog
                    );
                    sqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Logging.LogEvent(3, "SQLExecuter", "Error happened while executing DRR SQL-request!\n" + ex.ToString());
                }
                return null;
            }
        }
    }
}
