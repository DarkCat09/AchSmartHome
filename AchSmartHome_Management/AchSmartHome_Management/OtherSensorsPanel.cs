using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Spire.Xls;

namespace AchSmartHome_Management
{
    public partial class OtherSensorsPanel : UserControl
    {
        public OtherSensorsPanel()
        {
            InitializeComponent();
            GlobalSettings.InitThemeAndLang(Controls, this);
            toolTip1.SetToolTip(button2, Languages.GetLocalizedString("ExportToXls", "Export to Excel-spreadsheet"));
            GetSensorsValuesAndUpdate(DateTime.Now);
        }
        private void GetSensorsValuesAndUpdate(DateTime dt)
        {
            Logging.LogEvent(0, "Updating custom sensors values... DT = " + dt.ToString("dd.MM.yyyy,HH:mm"));
            try
            {
                List<object> sqlReqResult = DatabaseConnecting.ProcessSqlRequest(
                    "SELECT id, valdatetime, sensor_name, sensor_val " +
                    "FROM `other_sensors` " +
                    "WHERE valdatetime < DATE_ADD('" + dt.ToString("yyyy-MM-dd") + "', INTERVAL 1 DAY) " +
                    "ORDER BY valdatetime DESC"
                );

                List<string> sensorsNames = new List<string>();
                List<HomeSensor> otherSensors = new List<HomeSensor>();
                for (int i = 0; i < sqlReqResult.Count; i+=4)
                {
                    if (sensorsNames.Contains(sqlReqResult[i + 2].ToString()))
                        continue;
                    sensorsNames.Add(sqlReqResult[i+2].ToString());
                    otherSensors.Add(new HomeSensor(
                        Convert.ToInt32(sqlReqResult[i+0]),
                        Convert.ToDouble(sqlReqResult[i+3]),
                        Convert.ToDateTime(sqlReqResult[i+1]),
                        sqlReqResult[i+2].ToString()
                    ));
                }

                dataGridView1.Rows.Clear();
                foreach (HomeSensor shsensor in otherSensors)
                {
                    string[] sensorValArr = new string[3]
                    {
                        shsensor.Dt.ToString(), shsensor.Name, shsensor.Value.ToString()
                    };
                    dataGridView1.Rows.Add(sensorValArr);
                }
            }
            catch (Exception ex)
            {
                Logging.LogEvent(3, "Error happened while updating custom sensors values!\n" + ex.ToString());
                MessageBox.Show("Произошла ошибка!\n" + ex.Message);
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (GlobalSettings.autoUpdateSensorsVals)
                GetSensorsValuesAndUpdate(dateTimePicker1.Value);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetSensorsValuesAndUpdate(dateTimePicker1.Value);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // TODO: Exporting to Excel values and LineDataChart using NuGet-package FreeSpire.XLS
        }
    }
}
