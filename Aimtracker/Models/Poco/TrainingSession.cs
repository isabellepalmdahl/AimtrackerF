using Aimtracker.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aimtracker.Models
{
    public class TrainingSession
    {

        public int Id { get; set; }
        public string ShootingId { get; set; }
        public Location Location { get; set; }
        public string IbuID { get; set; }
        public DateTime Date { get; set; }
        public double HitStatistic { get; set; }
        public string Comments { get; set; }
        // navigation properties
        public virtual List<Series> Results { get; set; } = new List<Series>();

        public TrainingSession(ShootingDto shootingDto, Location location)
        {
            ShootingId = shootingDto.Id;
            Location = location;
            IbuID = shootingDto.IbuID;
            Date = shootingDto.Date;
            
            Results = new List<Series>();
            foreach (var s in shootingDto.Results)
            {
                var series = new Series(s);
                Results.Add(series);
            }

            double nmbrOfShots = 0;
            double nmbrOfHits = 0;

            foreach (var serie in shootingDto.Results)
            {
                foreach (var shot in serie.Shots)
                {
                    nmbrOfShots++;
                    if (shot.Result == "hit")
                    {
                        nmbrOfHits++;
                    }
                }
            }
            
            double HitStat = Math.Round((nmbrOfHits/nmbrOfShots)*100,1);            

            HitStatistic = HitStat;
        }

        public TrainingSession()
        {

        }
    }
}
