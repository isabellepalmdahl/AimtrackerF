using Aimtracker.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aimtracker.Models
{
    public class Shot
    {
        public Shot(ShotsDto shotDto)
        {
            ShotNumber = shotDto.ShotNr;
            Result = shotDto.Result == "hit";
            ShotXCord = shotDto.FiringCoords.X;
            ShotYCord = shotDto.FiringCoords.Y;
            FiringAngle = shotDto.FiringAngle;
            HeartRate = shotDto.HeartRate;
            DurationInSeconds = shotDto.TimeToFire;
            SeriesId = shotDto.SeriesId;
        }

        public Shot()
        {

        }

        public int Id { get; set; }
        public int ShotNumber { get; set; }
        public bool Result { get; set; }
        public double FiringAngle { get; set; }
        public int HeartRate { get; set; }
        public double ShotXCord { get; set; }
        public double ShotYCord { get; set; }
        public double DurationInSeconds { get; set; }
        public int SeriesId { get; set; }
    }
}
