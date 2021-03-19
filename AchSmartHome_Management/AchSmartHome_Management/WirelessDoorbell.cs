using System;
using System.Windows.Forms;

namespace AchSmartHome_Management
{
    public partial class WirelessDoorbell : UserControl
    {
        /// <summary>
        /// Count of photos, sending from smart doorbell.
        /// Количество фотографий, отправленных с умного звонка.
        /// </summary>
        public const int picturesCount = 3;

        /// <summary>
        /// Navigation position of smart doorbell images.
        /// Позиция навигации в картинках умного звонка (те, что на этой форме отображаются).
        /// </summary>
        private int imageNavPos = 1;

        public WirelessDoorbell()
        {
            InitializeComponent();
            GlobalSettings.InitThemeAndLang(Controls, this);
            RenderDoorbellImage();
        }

        private void RenderDoorbellImage()
        {
            pictureBox1.Image = DatabaseConnecting.GetImageByRequest(
                $"SELECT picture FROM doorbell WHERE photonum = {imageNavPos} ORDER BY photosid DESC LIMIT 1"
            );
            label1.Text =
                Languages.GetLocalizedString("DoorbellDateTime", "Doorbell rang at:") +
                DatabaseConnecting.ProcessSqlRequest(
                    $"SELECT camdatetime FROM doorbell WHERE photonum = 1 ORDER BY photosid DESC LIMIT 1"
                )[0];
        }

        private void label3_Click(object sender, EventArgs e)
        {
            if (imageNavPos > 1)
            {
                imageNavPos--;
                RenderDoorbellImage();
            }
        }
        private void label4_Click(object sender, EventArgs e)
        {
            if (imageNavPos < picturesCount)
            {
                imageNavPos++;
                RenderDoorbellImage();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainForm.GoBackPage(this);
        }
    }
}
