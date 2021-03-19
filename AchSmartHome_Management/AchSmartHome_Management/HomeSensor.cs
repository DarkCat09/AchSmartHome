using System;

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
    }
}
