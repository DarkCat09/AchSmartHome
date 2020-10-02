using System;
using System.Data.Common;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AchSmartHome_Management
{
    public partial class ConnectForm : Form
    {
        public ConnectForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*if (textBox3.Text != "")
            {
                bool passMatching = BCrypt.Net.BCrypt.Verify(
                    textBox3.Text,
                    "$2y$10$1mVLdb6P4G8NebmECg8ugeBI6zR.uen9k9qINqvZLGGjCFBabb7g6" // I will receive this value (now is Admin123) from database
                );
                _ = MessageBox.Show(passMatching.ToString());
            }*/
            try
            {
                #region receiving information about db from file
                string dbaddr = "", dbname = "", dbport = "", dbuser = "", dbpass = "";
                string[] dbParamsFromFile = File.ReadAllLines("database.conf");
                if (dbParamsFromFile.Length > 1)
                {
                    dbaddr = dbParamsFromFile[0].Split(new char[] { ';' })[0];
                    dbname = dbParamsFromFile[0].Split(new char[] { ';' })[1];
                    dbport = dbParamsFromFile[0].Split(new char[] { ';' })[2];
                    dbuser = dbParamsFromFile[1].Split(new char[] { ';' })[0];
                    dbpass = dbParamsFromFile[1].Split(new char[] { ';' })[1];
                }
                #endregion
                string sqlConnect =
                    "Server=" + ((textBox1.Text.Trim() != "") ? textBox1.Text.Trim() : dbaddr) + ";" +
                    "Database=" + dbname + ";port=" + dbport + ";" +
                    "User Id=" + dbuser + ";password=" + dbpass;
                MySqlConnection sqlDb = new MySqlConnection(sqlConnect);
                sqlDb.Open();
                MySqlCommand sqlRequest =
                    new MySqlCommand("SELECT * FROM users WHERE name = \"" + textBox2.Text + "\"", sqlDb);
                DbDataReader dbdr = sqlRequest.ExecuteReader();
                if (dbdr.HasRows)
                {
                    dbdr.Read();
                    if (BCrypt.Net.BCrypt.Verify(textBox3.Text, dbdr.GetString(2)))
                    {
                        _ = MessageBox.Show("Password is correct! \\/");
                    }
                    else
                    {
                        _ = MessageBox.Show("Password is incorrect! ><");
                    }
                }
                sqlDb.Close();
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show("Эксепшн!\n" + ex.Message);
            }
            Close();
        }
    }
}
