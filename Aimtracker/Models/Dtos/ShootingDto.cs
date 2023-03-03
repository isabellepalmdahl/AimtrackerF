using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aimtracker.Models.Dtos
{
    public class ShootingDto
    {
        public string Id { get; set; }
        public string Shooter { get; set; }
        public DateTime Date { get; set; }
        public GeometryDto Geometry { get; set; }
        public string Location { get; set; }
        public string IbuID { get; set; }
        public ICollection<SeriesDto> Results { get; set; }
        public string Comments { get; set; }  
    }
}
