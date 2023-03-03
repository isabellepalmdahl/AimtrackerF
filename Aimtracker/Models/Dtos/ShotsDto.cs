using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aimtracker.Models.Dtos
{
    public class ShotsDto
    {
        public int ShotNr { get; set; }
        public string Result { get; set; }
        public int FiringIndex { get; set; }
        public double FiringAngle { get; set; }
        public FiringCoordsDto FiringCoords { get; set; }
        public List<ShotXyDto> ShotXy { get; set; }
        public int HeartRate { get; set; }
        public double TimeToFire { get; set; }
        public int SeriesId { get; set; }
    }
}
