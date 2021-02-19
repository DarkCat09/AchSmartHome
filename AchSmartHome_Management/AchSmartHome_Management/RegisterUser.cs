using System;
using System.Windows.Forms;

namespace AchSmartHome_Management
{
    public partial class RegisterUser : Form
    {
        public RegisterUser()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainForm.regusername = textBox1.Text;
            MainForm.regpasshash = BCrypt.Net.BCrypt.HashPassword(textBox2.Text);
            Close();
        }
    }
}
