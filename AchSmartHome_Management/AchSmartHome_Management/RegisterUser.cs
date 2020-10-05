using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Form1.regusername = textBox1.Text;
            Form1.regpasshash = BCrypt.Net.BCrypt.HashPassword(textBox2.Text);
            Close();
        }
    }
}
