using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Infrastructure.Dto.OpenMeteo
{
    public record GeoResult([property: JsonPropertyName("latitude")] double Latitude, 
        [property: JsonPropertyName("longitude")] double Longitude);

}
