using Aimtracker.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aimtracker.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public Location(ShootingDto shootingDto)
        {
            Name = shootingDto.Location;
            Latitude = shootingDto.Geometry.Coordinates[0];
            Longitude = shootingDto.Geometry.Coordinates[1];
        }

        public Location()
        {

        }
    }
}
