using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aimtracker.Models.Dtos
{
    public class CurrentDto
    {
        public int Dt { get; set; }
        public double Temp { get; set; }
        public double Wind_speed { get; set; }
        public int Wind_deg { get; set; }
        public List<WeatherDto> Weather { get; set; }
    }
}
