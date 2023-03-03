using Aimtracker.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aimtracker.Models
{
    public class Series
    {
        public Series(SeriesDto seriesDto)
        {
            Stance = seriesDto.Stance;
            DateTime = seriesDto.DateTime;
            Shots = new List<Shot>();

            foreach (var s in seriesDto.Shots)
            {
                var shot = new Shot(s);
                Shots.Add(shot);
            }
        }

        public Series()
        {

        }

        public int Id { get; set; }
        public string Stance { get; set; }
        public DateTime DateTime { get; set; }
        public Weather Weather { get; set; }
        

        // nav props
        public virtual ICollection<Shot> Shots { get; set; }  
        public int WeatherId { get; set; }
        public int TrainingSessionId { get; set; }
    }

}
