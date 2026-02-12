using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class WeatherForecast
    {
        public string Provider { get; set; }
        public DateTime Date { get; set; }
        public double TemperatureC { get; set; }
        public string Description { get; set; }
    }

}
