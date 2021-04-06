using System;
using System.Collections.Generic;
using System.Windows.Forms;
/*using Spire.Xls;*/

namespace AchSmartHome_Management
{
    public partial class OtherSensorsPanel : UserControl
    {
        private bool showMainSensors = false;
        public OtherSensorsPanel()
        {
            InitializeComponent();
            GlobalSettings.InitThemeAndLang(Controls, this);
            Column1.HeaderText = Languages.GetLocalizedString("DateText", "Date");
            Column2.HeaderText = Languages.GetLocalizedString("Name", "Sensor Name");
            Column3.HeaderText = Languages.GetLocalizedString("Value", "Value");
            toolTip1.SetToolTip(button2, Languages.GetLocalizedString("ExportToXls", "Export to Excel-spreadsheet"));
            toolTip2.SetToolTip(button3, Languages.GetLocalizedString("MainSensorsToolTip1", "Show main sensors"));
            GetSensorsValuesAndUpdate(DateTime.Now);
        }

        private void GetSensorsValuesAndUpdate(DateTime dt, bool clearTable = true)
        {
            Logging.LogEvent(
                0, "OtherSensors",
                $"Updating custom sensors values... DT = {dt:dd.MM.yyyy,HH:mm}"
            );
            try
            {
                List<object> sqlReqResult = DatabaseConnecting.ProcessSqlRequest(
                    $"SELECT id, valdatetime, sensor_name, sensor_val " +
                    $"FROM `other_sensors` " +
                    $"WHERE valdatetime < DATE_ADD('{dt:yyyy-MM-dd}', INTERVAL 1 DAY) " +
                    $"AND id > 0 ORDER BY valdatetime DESC"
                );

                List<string> sensorsNames = new List<string>();
                List<HomeSensor> otherSensors = new List<HomeSensor>();
                for (int i = 0; i < sqlReqResult.Count; i+=4)
                {
                    if (sensorsNames.Contains(sqlReqResult[i+2].ToString()))
                        continue;
                    sensorsNames.Add(sqlReqResult[i+2].ToString());
                    otherSensors.Add(new HomeSensor(
                        Convert.ToInt32(sqlReqResult[i+0]),
                        Convert.ToDouble(sqlReqResult[i+3]),
                        Convert.ToDateTime(sqlReqResult[i+1]),
                        sqlReqResult[i+2].ToString()
                    ));
                }

                if (clearTable)
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
                Logging.LogEvent(3, "OtherSensors", $"An error happened while updating custom sensors values!\n{ex}");
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
            /*
            Workbook wbStream = new Workbook();
            Worksheet sheet = wbStream.Worksheets[0];
            sheet.Range["A1"].Text = "Date";
            sheet.Range["B1"].Text = "Name";
            sheet.Range["C1"].Text = "Value";
            */
        }

        private void ChangeMainSensorsButtonToolTip()
        {
            if (showMainSensors)
            {
                toolTip2.SetToolTip(
                    button3, Languages.GetLocalizedString("MainSensorsToolTip2", "Hide main sensors")
                );
            }
            else
            {
                toolTip2.SetToolTip(
                    button3, Languages.GetLocalizedString("MainSensorsToolTip1", "Show main sensors")
                );
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (showMainSensors)
            {
                Logging.LogEvent(1, "OtherSensors", "Hiding main sensors...");
                GetSensorsValuesAndUpdate(dateTimePicker1.Value);
                showMainSensors = false;
                ChangeMainSensorsButtonToolTip();
            }
            else
            {
                Logging.LogEvent(1, "OtherSensors", "Showing main sensors...");
                try
                {
                    List<HomeSensor> mainSensors = new List<HomeSensor>();

                    Dictionary<string, object> reqResult = HomeSensor.GetMainSensorsValues(dateTimePicker1.Value);

                    mainSensors.Add(new HomeSensor(
                        -1,
                        Convert.ToDouble(reqResult["temperature"]),
                        Convert.ToDateTime(reqResult["datetime"]),
                        Languages.GetLocalizedString("Temperature", "Temperature")
                    ));
                    mainSensors.Add(new HomeSensor(
                        -2,
                        Convert.ToDouble(reqResult["humidity"]),
                        Convert.ToDateTime(reqResult["datetime"]),
                        Languages.GetLocalizedString("Humidity", "Humidity")
                    ));

                    dataGridView1.Rows.Clear();
                    foreach (HomeSensor shsensor in mainSensors)
                    {
                        string[] sensorValArr = new string[3]
                        {
                        shsensor.Dt.ToString(), shsensor.Name, shsensor.Value.ToString()
                        };
                        dataGridView1.Rows.Add(sensorValArr);
                    }
                    GetSensorsValuesAndUpdate(dateTimePicker1.Value, false);
                    showMainSensors = true;
                    ChangeMainSensorsButtonToolTip();
                }
                catch (Exception ex)
                {
                    Logging.LogEvent(
                        3, "OtherSensors",
                        $"An error happened while updating table with main and custom sensors!\n{ex}"
                    );
                }
            }
        }
    }
}
