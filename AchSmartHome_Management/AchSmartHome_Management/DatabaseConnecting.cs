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
        public static string dbaddr = "", dbname = "", dbport = "", dbuser = "", dbpass = "";
        public static MySqlConnection sqlDb = null;
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
        public static void ConnectToDb(string advancedServer = "")
        {
            sqlDb = new MySqlConnection(
                    "Server=" + ((advancedServer.Trim() != "") ? advancedServer.Trim() : dbaddr) + ";" +
                    "Database=" + dbname + ";port=" + dbport + ";" +
                    "User Id=" + dbuser + ";password=" + dbpass);
            sqlDb.Open();
        }
        public static Dictionary<int, object> ProcessSqlRequest(string sqlRequest, List<MySqlParameter> sqlReqParams = null)
        {
            if (sqlReqParams == null)
                sqlReqParams = new List<MySqlParameter>();
            Dictionary<int, object> sqlReqResult = new Dictionary<int, object>();
            MySqlCommand sqlCommand = new MySqlCommand(sqlRequest, sqlDb);
            if (sqlReqParams.Count > 0)
            {
                foreach (MySqlParameter sqlCommandParam in sqlReqParams)
                {
                    sqlCommand.Parameters.Add(sqlCommandParam);
                }
            }
            DbDataReader dbdr = sqlCommand.ExecuteReader();
            dbdr.Read();
            if (dbdr.HasRows)
            {
                for (int i = 0; i < dbdr.FieldCount; i++)
                {
                    sqlReqResult.Add(i, dbdr.GetValue(i));
                }
            }
            dbdr.Close();
            return sqlReqResult;
        }
        public static Dictionary<int, object> ProcessSqlRequest(string sqlRequest, List<MySqlParameter> sqlReqParams, bool dontRetResult)
        {
            if (!dontRetResult)
            {
                return ProcessSqlRequest(sqlRequest, sqlReqParams);
            }
            else
            {
                if (sqlReqParams == null)
                    sqlReqParams = new List<MySqlParameter>();
                MySqlCommand sqlCommand = new MySqlCommand(sqlRequest, sqlDb);
                if (sqlReqParams.Count > 0)
                {
                    foreach (MySqlParameter sqlCommandParam in sqlReqParams)
                    {
                        sqlCommand.Parameters.Add(sqlCommandParam);
                    }
                }
                sqlCommand.ExecuteNonQuery();
                return null;
            }
        }
    }
}
