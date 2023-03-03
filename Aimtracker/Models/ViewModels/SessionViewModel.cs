using Aimtracker.Models.Dtos;
using Aimtracker.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Aimtracker.Models
{
    public enum Dates
    {
        week, month, year, twoYear
    }
    public enum GraphStatistics
    {
        precision, passes 
    }

    public class SessionViewModel
    {
        #region Db Data
        [DisplayName("Sessions")]

        public List<TrainingSession> Sessions { get; set; }
        public List<TrainingSession> SessionsVS { get; set; }
        public double SessionsPrChange { get; set; }
        public TrainingSession Session { get; set; }


        #endregion
        public double HitStatistic { get; set; }
        public double HitStatisticVS { get; set; }

        public double HitStatPrChange { get; set; }

        public SessionViewModel()
        {

        }

        public double CalcPercent(double share,double whole)
        {
            if (share == 0 || whole == 0)
                return 100;
            return Math.Round(((share / whole)-1) * 100,1);
        }
      
        public double CalcHitStatistic(List<TrainingSession> shootings)
        {
            if (shootings.Count > 0)
            {
                double total = 0;

                foreach (var shooting in shootings)
                {
                    total += shooting.HitStatistic;
                }
                if (total == 0)
                    return 0;
                double hitStatistic = Math.Round(total / shootings.Count, 1);
                return hitStatistic;
            }
            else
                return 0;
        }
        public DateTime GetToDate(Dates dates)
        {
            TimeSpan timeSpan = GetTimeValueInUnix(dates);
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(DateTimeOffset.Now.ToUnixTimeSeconds() - timeSpan.TotalSeconds).ToLocalTime();



        }
        public DateTime GetToDate(TimeSpan timeSpan)
        {

            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(DateTimeOffset.Now.ToUnixTimeSeconds() - timeSpan.TotalSeconds).ToLocalTime();

        }
        public Dates ConvertJsStringToDates(string value)
        {
            switch (value)
            {
                case "week":
                    return Dates.week;
                case "month":
                    return Dates.month;
                case "year":
                    return Dates.year;

            }
            return Dates.week;
        }

        public TimeSpan GetTimeValueInUnix(Dates dates)
        {
            switch (dates)
            {
                case Dates.week:
                    return new DateTime(1970, 1, 8) - new DateTime(1970, 1, 1);
                case Dates.month:
                    return new DateTime(1970, 2, 1) - new DateTime(1970, 1, 1);
                case Dates.year:
                    return new DateTime(1971, 1, 1) - new DateTime(1970, 1, 1);
                case Dates.twoYear:
                    return new DateTime(1972, 1, 1) - new DateTime(1970, 1, 1);

            }
            return new DateTime(1970, 1, 1) - new DateTime(1970, 1, 1);
        }
        #region ChartViewModel Partial

        public double[] MonthlyShootings { get; set; } = new double[12];
        public double[] GetMonthlyShootings(ICollection<TrainingSession> shootingDb)
        {
            double[] passes = new double[12];
            foreach (var session in shootingDb)
            {
                var date = session.Date.Month - 1;
                passes[date] += 1;

            }
            return passes;
        }

        private double[] GetHitStatistic(ICollection<TrainingSession> shootingDb)

        {
            double[] statistic = new double[12];
            double[] amountOfSessions = new double[12];

            foreach (var session in shootingDb)
            {
                var date = session.Date.Month - 1;
                statistic[date] += session.HitStatistic;
                amountOfSessions[date]++;
            }
            for (int i = 0; i <= statistic.Count() - 1; i++)
            {
                if (statistic[i] != 0 || amountOfSessions[i] != 0)
                {
                    statistic[i] = statistic[i] / amountOfSessions[i];
                }
            }
            return statistic;
        }

        public GraphStatistics GetGraphVisuals(string value)
        {
            switch (value)
            {
                case "precision":
                    return GraphStatistics.precision;
                case "passes":
                    return GraphStatistics.passes;
            }
            return GraphStatistics.precision;

        }
        public double[] GetStatistics(GraphStatistics statistics, ICollection<TrainingSession> shootings)
        {
            switch (statistics)
            {
                case GraphStatistics.precision:
                    return GetHitStatistic(shootings);

                case GraphStatistics.passes:
                    return GetMonthlyShootings(shootings);
            }
            return MonthlyShootings;
        }

        #endregion
    }
}

