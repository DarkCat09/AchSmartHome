using System;
using System.Net;
using System.Windows.Forms;

namespace AchSmartHome_Management
{
    public partial class LightPanel : UserControl
    {
        CheckBox[] lampCheckboxes = null;
        private bool contentInitialized = false;
        public LightPanel()
        {
            InitializeComponent();
            GlobalSettings.InitThemeAndLang(Controls, this);
            GetLightData();
            contentInitialized = true;
        }

        private void GetLightData()
        {
            lampCheckboxes = new CheckBox[4]
            {
                checkBox1, checkBox2, checkBox3, checkBox4
            };
            for (int i = 1; i <= 4; i++)
            {
                System.Collections.Generic.List<object> lampResult = DatabaseConnecting.ProcessSqlRequest(
                    "SELECT * from `light` WHERE (lampnum = " + i + ") ORDER BY id DESC LIMIT 1"
                );
                if (lampResult.Count > 0)
                    lampCheckboxes[i-1].Checked = Convert.ToBoolean(lampResult[3]);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainForm.ReplacePanel<ControlPanel>();
        }

        private void ChangeLightState(int _lampnum)
        {
            if (!contentInitialized)
                return;

            try
            {
                byte[] byteReqData = System.Text.Encoding.UTF8.GetBytes(
                    "action=light&num=" + _lampnum + "&state=" +
                    Convert.ToInt32(lampCheckboxes[_lampnum + 1].Checked).ToString()
                );

                WebRequest wr = WebRequest.Create("http://" + DatabaseConnecting.smartHomeServer + ":80/app_request.php");
                wr.Method = "POST";
                wr.ContentType = "application/x-www-form-urlencoded";
                wr.ContentLength = byteReqData.Length;

                System.IO.Stream reqStream = wr.GetRequestStream();
                reqStream.Write(byteReqData, 0, byteReqData.Length);
                reqStream.Close();

                _ = wr.GetResponse();
            }
            catch (Exception ex)
            {
                Logging.LogEvent(3, "LightPanel", "An error happened while sending SH-request!\n" + ex.ToString());
                MessageBox.Show("Произошла ошибка!\n" + ex.Message);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ChangeLightState(1);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            ChangeLightState(2);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            ChangeLightState(3);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            ChangeLightState(4);
        }
    }
}
