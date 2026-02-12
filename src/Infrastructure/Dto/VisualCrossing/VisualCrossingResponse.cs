using Infrastructure.Dto.OpenMeteo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Dto.VisualCrossing
{
    public record VisualCrossingResponse(List<DayData> Days);
}

