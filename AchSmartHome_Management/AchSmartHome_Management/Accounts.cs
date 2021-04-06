/*
 * Copyright © 2020-2021 Чечкенёв Андрей
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
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace AchSmartHome_Management
{
    class Accounts
    {
        public static string regusername = "", reguserpass = "";
        public static string username = "", passhash = "";
        public static int userid = -1, userprivs = 4;

        /// <summary>
        /// Function verifies username and password, finding they in a DB. 
        /// Проверяет имя пользователя и пароль, сопоставляя с записями в БД.
        /// </summary>
        /// <param name="username">Имя пользователя для проверки</param>
        /// <param name="password">Пароль в чистом виде (не хэш!)</param>
        /// <returns>При удачной проверке - true, если пользователя с этими username и password нет - false.</returns>
        public static bool CheckUserCredentials(string username, string password)
        {
            Logging.LogEvent(
                0, "Accounts", $"Checking user credentials ...\nUsername={username}\nPassword={password}"
            );
            try
            {
                List<object> sqlReqResult = DatabaseConnecting.ProcessSqlRequest(
                    "SELECT id, name, passhash FROM users WHERE name = ?username",
                    new List<MySqlParameter>() { new MySqlParameter("username", username) }
                );
                if (sqlReqResult.Count > 0)
                {
                    if (BCrypt.Net.BCrypt.Verify(password, sqlReqResult[2].ToString()))
                    {
                        Logging.LogEvent(1, "Accounts", "Credentials verifying success!");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogEvent(3, "Accounts", $"An error happened while checking credentials!\n{ex}");
            }
            Logging.LogEvent(2, "Accounts", "Credentials verifying failed!");
            return false;
        }

        /// <summary>
        /// Function receives advanced user account data - identifier and privileges. 
        /// Получает дополнительные данные о пользователе - идентификатор и привилегии.
        /// </summary>
        public static void GetUserData()
        {
            try
            {
                userid = Convert.ToInt32(
                    DatabaseConnecting.ProcessSqlRequest(
                        "SELECT id FROM users WHERE name = ?username ORDER BY id LIMIT 1",
                        new List<MySqlParameter>() { new MySqlParameter("username", username) }
                    )[0]
                );
            }
            catch (Exception ex)
            {
                Logging.LogEvent(
                    3, "Accounts",
                    $"Can\'t receive user identifier!\n{ex}"
                );
            }

            try
            {
                userprivs = Convert.ToInt32(
                    DatabaseConnecting.ProcessSqlRequest(
                        "SELECT privs FROM users WHERE name = ?username ORDER BY id DESC LIMIT 1",
                        new List<MySqlParameter>() { new MySqlParameter("username", username) }
                    )[0]
                );
            }
            catch (Exception ex)
            {
                Logging.LogEvent(
                    3, "Accounts",
                    $"Can\'t receive user privileges!\nUsername={username}\nID={userid}\n{ex}"
                );
            }
        }

        /// <summary>
        /// Function loads username and password hash from "cookies"
        /// (Settings.cookie_username/passhash) 
        /// Подгружает имя пользователя и хэш пароля из "cookies-файлов"
        /// (которые, на самом деле, Settings.username/passhash)
        /// </summary>
        public static void LoadCredentials()
        {
            Logging.LogEvent(0, "Accounts", "Importing credentials ...");
            username = Properties.Settings.Default.cookie_username;
            passhash = Properties.Settings.Default.cookie_passhash;
            if (
                !CheckUserCredentials(
                    username,
                    System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(passhash))
                )
            )
            {
                username = "";
                passhash = "";
                return;
            }

            GetUserData();
        }

        /// <summary>
        /// Function writes username and password, stored in the corresponding variables, to "cookies". 
        /// Записывает имя пользователя и пароль, сохранённые в соответствующих переменных, в "cookie".
        /// </summary>
        public static void SaveCredentials()
        {
            Logging.LogEvent(0, "Accounts", "Saving credentials ...");
            Properties.Settings.Default.cookie_username = username;
            Properties.Settings.Default.cookie_passhash = passhash;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Function registers user. 
        /// Регистрирует пользователя.
        /// </summary>
        /// <param name="_username">Имя пользователя</param>
        /// <param name="_password">Пароль в чистом виде (не хэш!)</param>
        /// <returns>При успешной регистрации - true, иначе - false</returns>
        public static bool RegisterUser(string _username, string _password)
        {
            Logging.LogEvent(1, "Accounts", "Trying to register user ...");
            if (userprivs == 0)
            {
                DatabaseConnecting.ProcessSqlRequest(
                    "INSERT INTO users(name, passhash, privs) VALUES (?username, ?passhash, 1)",
                    new List<MySqlParameter>() {
                        new MySqlParameter("username", _username.Trim()),
                        new MySqlParameter("passhash", BCrypt.Net.BCrypt.HashPassword(_password))
                    }, true
                );
                return true;
            }
            else
            {
                Logging.LogEvent(2, "Accounts", "Insufficient privileges! (Registration)");
            }
            return false;
        }
    }
}
