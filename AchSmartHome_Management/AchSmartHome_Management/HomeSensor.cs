using System;
using System.Collections.Generic;

namespace AchSmartHome_Management
{
    class HomeSensor
    {
        public int Id = 0;
        public DateTime Dt = new DateTime();
        public string Name = "SmartHomeSensor";
        public double Value = 0.00;

        public HomeSensor(int _id, double _value, DateTime _dt, string _name = "")
        {
            Id = _id;
            Value = _value;
            Name = (_name.Trim().Equals("")) ? ("SmartHomeSensor #" + Id.ToString()) : _name.Trim();
            Dt = _dt;
        }

        public static Dictionary<string, object> GetMainSensorsValues(DateTime dt)
        {
            Logging.LogEvent(1, "HomeSensorApi", "GetMainSensorsValues(DateTime) started!");
            Dictionary<string, object> sensors_vals = new Dictionary<string, object>();

            try
            {
                List<object> sqlReqResult = DatabaseConnecting.ProcessSqlRequest(
                    $"SELECT id, valdatetime, IFNULL(temp, 0.00) AS temp, IFNULL(humidity, 0.00) AS humidity " +
                    $"FROM `sensors_values` " +
                    $"WHERE valdatetime < DATE_ADD('{dt:yyyy-MM-dd}', INTERVAL 1 DAY) " +
                    $"ORDER BY id DESC LIMIT 1"
                );

                sensors_vals.Add("temperature", Convert.ToDouble(sqlReqResult[2]));
                sensors_vals.Add("humidity", Convert.ToDouble(sqlReqResult[3]));
                sensors_vals.Add("datetime", sqlReqResult[1]);
            }
            catch (Exception ex)
            {
                Logging.LogEvent(
                    3, "HomeSensorApi",
                    $"An error happened while receiving main sensors values!\n{ex}"
                );
            }

            return sensors_vals;
        }
    }
}
