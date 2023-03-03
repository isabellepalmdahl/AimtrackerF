using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aimtracker.Models.Dtos
{
    public class GeometryDto
    {
        public string Type { get; set; }
        public double[] Coordinates { get; set; } 
    }
}
