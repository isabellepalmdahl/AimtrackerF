using Aimtracker.Models.Dtos;
using System.Collections.Generic;

namespace Aimtracker.Models
{
    public class Biathlete
    {
        public int Id { get; set; }
        public string IbuID { get; set; }
        public string FullName { get; set; }
        public int MaxHeartRate { get; set; }
        public string Image { get; set; }

        public Biathlete(BiathleteDto biathleteDto)
        {
            IbuID = biathleteDto.IbuID;
            FullName = biathleteDto.FullName;
            MaxHeartRate = biathleteDto.MaxHeartRate;
            Image = biathleteDto.Image;
        }

        public Biathlete()
        {

        }
    }


}
