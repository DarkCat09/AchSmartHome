using System;
using System.Data.Common;
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
                /*
                MySqlConnection sqlDb = DatabaseConnecting.ConnectToDb(textBox1.Text);
                sqlDb.Open();
                MySqlCommand sqlRequest =
                    new MySqlCommand("SELECT * FROM users WHERE name = \"" + textBox2.Text + "\"", sqlDb);

                DbDataReader dbdr = sqlRequest.ExecuteReader();
                if (dbdr.HasRows)
                {
                    dbdr.Read();
                    if (BCrypt.Net.BCrypt.Verify(textBox3.Text, dbdr.GetString(2)))
                    {
                        Form1.userid = dbdr.GetInt32(0);
                        Form1.username = dbdr.GetString(1);
                        Form1.userprivs = dbdr.GetInt32(3);
                    }
                    else
                    {
                        _ = MessageBox.Show("Username or password is incorrect!");
                    }
                }
                else
                {
                    _ = MessageBox.Show("Username or password is incorrect!");
                }
                dbdr.Close();
                sqlDb.Close();
                */
                System.Collections.Generic.Dictionary<int, object> sqlReqResult = DatabaseConnecting.ProcessSqlRequest("SELECT * FROM users WHERE name = \"" + textBox2.Text + "\"");
                if (sqlReqResult.Count > 0)
                {
                    if (BCrypt.Net.BCrypt.Verify(textBox3.Text, sqlReqResult[2].ToString()))
                    {
                        Form1.userid = Convert.ToInt32(sqlReqResult[0]);
                        Form1.username = sqlReqResult[1].ToString();
                        Form1.userprivs = Convert.ToInt32(sqlReqResult[3]);
                    }
                    else
                    {
                        _ = MessageBox.Show("Username or password is incorrect!");
                    }
                }
                else
                {
                    _ = MessageBox.Show("Username or password is incorrect!");
                }
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show("Error happened!\n" + ex.Message);
            }
            Close();
        }
    }
}
