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

using System.IO;
using System.Collections.Generic;

namespace AchSmartHome_Management
{
    class Languages
    {
        public static string curlang = Properties.Settings.Default.language;
        public static Dictionary<string, string> Lang = new Dictionary<string, string>();

        public static Dictionary<string, string> InitLangs()
        {
            Dictionary<string, string> langFiles = new Dictionary<string, string>();

            StreamReader s = new StreamReader("langs.conf");
            string line;
            while ((line = s.ReadLine()) != null)
            {
                if (!line.StartsWith("#") && line != "")
                {
                    string[] langOpts = line.Split(new char[] { ' ' });
                    if (File.Exists("langs\\" + langOpts[1]))
                    {
                        langFiles.Add(langOpts[0], "langs\\" + langOpts[1]);
                    }
                }
            }

            return langFiles;
        }

        public static int LoadLang(string filename, Dictionary<string, string> langFiles)
        {
            try
            {
                Lang.Clear();
                StreamReader s = new StreamReader(filename);
                string line;
                while ((line = s.ReadLine()) != null)
                {
                    if (!line.StartsWith("#") && line != "")
                    {
                        string[] lang_value = line.Split(new char[] { '=' });
                        if (lang_value.Length > 1)
                        {
                            Lang.Add(lang_value[0], lang_value[1]);
                        }
                    }
                }

                foreach (string langCode in langFiles.Keys)
                {
                    curlang = (langFiles[langCode] == filename) ? langCode : curlang;
                }
            }
            catch (FileNotFoundException)
            {
                return 1;
            }
            catch (DirectoryNotFoundException)
            {
                return 2;
            }
            catch (IOException)
            {
                return 3;
            }
            catch (System.Exception)
            {
                return -1;
            }

            return 0;
        }

        public static string GetLocalizedString(string dicKey, string defaultString = "")
        {
            return (Languages.Lang.ContainsKey(dicKey)) ? Languages.Lang[dicKey] : defaultString;
        }
    }
}
