using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Dto.WeatherApi
{
    public record ForecastDay(DateTime Date, DayInfo Day);
}

