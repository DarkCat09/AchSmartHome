using System;
using System.Windows.Forms;

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
            if (textBox3.Text != "")
            {
                bool passMatching = BCrypt.Net.BCrypt.Verify(
                    textBox3.Text,
                    "$2y$10$1mVLdb6P4G8NebmECg8ugeBI6zR.uen9k9qINqvZLGGjCFBabb7g6" // I will receive this value (now is Admin123) from database
                );
                _ = MessageBox.Show(passMatching.ToString());
            }
            Close();
        }
    }
}
