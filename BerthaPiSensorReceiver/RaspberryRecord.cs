using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerthaPiSensorReceiver
{
    public class RaspberryRecord
    {
        public double Temperature { get; set; }
        public double Pressure { get; set; }
        public double Humidity { get; set; }
        public int  UserId { get; set; }

        public RaspberryRecord(double _temperature, double _pressure, double _humidity, int _userId)
        {
            Temperature = _temperature;
            Pressure = _pressure;
            Humidity = _humidity;
            UserId = _userId;
        }
    }
}
