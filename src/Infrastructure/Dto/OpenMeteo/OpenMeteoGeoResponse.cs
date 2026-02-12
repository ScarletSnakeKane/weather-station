using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Dto.OpenMeteo
{
    public record OpenMeteoGeoResponse(List<GeoResult>? Results);
}
