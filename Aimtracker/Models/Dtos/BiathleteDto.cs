using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aimtracker.Models.Dtos
{
    public class BiathleteDto
    {
        public string IbuID { get; set; }
        public string FullName { get; set; }
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public string Country { get; set; }
        public string Nat { get; set; }
        public string GenderId { get; set; }
        public int MaxHeartRate { get; set; }
        public string Image { get; set; }
        public List<string> StatSeasons { get; set; }
        public List<string> StatShootingProne { get; set; }
        public List<string> StatShootingStanding { get; set; }
    }
}
