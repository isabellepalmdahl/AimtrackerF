using Aimtracker.Data;
using Aimtracker.Converters;
using Aimtracker.Models;
using Aimtracker.Models.Dtos;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System;

using Microsoft.EntityFrameworkCore;

namespace Aimtracker.Repositories
{
    public class DbRepository : IDbRepository
    {
        #region FIELDS
        private readonly AppDbContext _db;
        private readonly IBiathlonRepository _repo;
        private readonly IWeatherRepository _wRepo;
        TimeConverter _timeConverter = new TimeConverter();
        #endregion

        #region CONSTRUCTORS
        public DbRepository(AppDbContext db, IBiathlonRepository repo, IWeatherRepository wRepo)
        {
            _db = db;
            _repo = repo;
            _wRepo = wRepo;
        }
        #endregion

        #region CREATE
        /// <summary>
        /// Adds shooting to database if not already existing
        /// </summary>
        /// <param name="shootingDto"></param>
        /// <returns>Shooting</returns>
        private double CalcHitStatistic(TrainingSession trainingSession)
        {
            double hitStatistic = 0;
            int numberOfShots = 0;
            foreach (var series in trainingSession.Results)
            {
                foreach (var shot in series.Shots)
                {
                    if (shot.Result)
                        hitStatistic++;
                    numberOfShots++;
                }
            }
            return Math.Round((hitStatistic/numberOfShots)*100);
        }
        public async Task<TrainingSession> AddShootingAsync(ShootingDto shootingDto)
        {
            if (!_db.Shootings.Any(a => a.ShootingId == shootingDto.Id))
            {
                var location = await AddLocationAsync(shootingDto);
                var shooting = new TrainingSession(shootingDto, location);
                shooting.HitStatistic = CalcHitStatistic(shooting);
                await _db.AddAsync(shooting);
                await AddWeatherToSerieAsync(shooting, location);
                _db.SaveChanges();
                return shooting;
            }
            return null;
        }

        /// <summary>
        /// Adds historic shootings to database
        /// </summary>
        /// <param name="shootingDto"></param>
        /// <returns>TrainingSession</returns>
        public async Task<TrainingSession> AddShootingsAsync(List<ShootingDto> shootingDtos)
        {
            var Sessions = new List<TrainingSession>();            
            foreach (var shootingDto in shootingDtos)
            {
                if (!_db.Shootings.Any(a => a.ShootingId == shootingDto.Id))
                {
                    var location = await AddLocationAsync(shootingDto);
                    var shooting = new TrainingSession(shootingDto, location);

                    shooting.HitStatistic = CalcHitStatistic(shooting);
                    Sessions.Add(shooting);
                    await AddWeatherToHistoricalSerieAsync(shooting);
                }
            }
            await _db.AddRangeAsync(Sessions);
            _db.SaveChanges();
            return null;
        }

        /// <summary>
        /// Seeds biathletes to database if none there
        /// </summary>
        /// <returns></returns>
        public async Task<List<Biathlete>> SeedBiathletesAsync()
        {
            if (!_db.Biathletes.Any())
            {
                var biathleteDtos = await _repo.GetBiathletes();
                var biathletes = new List<Biathlete>();
                foreach (var biathleteDto in biathleteDtos)
                {
                    var biathlete = new Biathlete(biathleteDto);
                    biathletes.Add(biathlete);
                }
                _db.AddRange(biathletes);
                _db.SaveChanges();
            }
            return null;
        }

        /// <summary>
        /// Adds weather data to each series in shooting
        /// </summary>
        /// <param name="shooting"></param>
        /// <returns>Weather</returns>
        private async Task<Weather> AddWeatherToSerieAsync(TrainingSession shooting, Location location)
        {
            foreach (var series in shooting.Results)
            {
                var time = _timeConverter.ConvertDateTimeToUnix(series.DateTime).ToString();
                var lat = location.Latitude.ToString();
                var lon = location.Longitude.ToString();
                var w = _wRepo.GetWeather(lat, lon, time); // this for weather api
                //var w = _wRepo.GetWeatherMock(); // this for mock weather
                if (w.Result.Current == null)
                {
                    continue;
                }
                var weather = new Weather(w.Result);
                await _db.AddAsync(weather);
                series.Weather = weather;
            }
            return null;
        }

        /// <summary>
        /// Adds mock weather data to each series in historical shooting
        /// </summary>
        /// <param name="shooting"></param>
        /// <returns>Weather</returns>
        private async Task<Weather> AddWeatherToHistoricalSerieAsync(TrainingSession shooting)
        {
            foreach (var series in shooting.Results)
            {
                var w = _wRepo.GetWeatherMock(); 
                if (w.Result.Current == null)
                {
                    continue;
                }
                var weather = new Weather(w.Result);
                await _db.AddAsync(weather);
                series.Weather = weather;
            }
            return null;
        }

        /// <summary>
        /// Adds location to database if not already existing
        /// </summary>
        /// <param name="shootingDto"></param>
        /// <returns></returns>
        private async Task<Location> AddLocationAsync(ShootingDto shootingDto)
        {
            var result = GetLocationByName(shootingDto.Location);
            if (result == null)
            {
                var location = new Location(shootingDto);
                var newLocation = await _db.AddAsync(location);
                _db.SaveChanges();
                result = newLocation.Entity;
            }
            return result;
        }       
        #endregion

        #region READ
        public List<TrainingSession> GetAllShootings()
        {
            var shootings =  _db.Shootings.ToList();
           
            return shootings;
        }
        


        public List<TrainingSession> GetShootingsByDate(DateTime fromDate, DateTime toDate, string user)

        {
            var shootings = _db.Shootings
                .Where(series => series.Date < fromDate
                && series.Date > toDate && series.IbuID == user).Include(s => s.Results).Include("Results.Shots");
            return shootings.ToList();
        }

        private Location GetLocationByName(string locationName)
        {
            var location = _db.Locations
                .Where(x => x.Name == locationName);
            return location.FirstOrDefault();
        }

        public List<TrainingSession> GetSessionsByLocation(int locationId)
        {
            List<TrainingSession> shootings = new List<TrainingSession>();
            var session = _db.Shootings
                .Where(x => x.Location.Id == locationId);
            foreach (var shooting in session)
            {
                shootings.Add(shooting);
            }
            return shootings;
        }

        public List<Shot> ShotsWithHeartRate(int min, int max)
        {
            var shotsWithDesiredHeartrate = _db.Shots
                .Where(x => x.HeartRate >= min
                && x.HeartRate <= max);
            return shotsWithDesiredHeartrate.ToList();
        }

        public List<TrainingSession> GetSessionsByHitstatistic(double lowerHit, double higherHit)
        {
                var session = _db.Shootings
                .Where(x => x.HitStatistic > lowerHit
                && x.HitStatistic < higherHit);
                return session.ToList();
        }

        public List<TrainingSession> GetSessionByHeartrate(int min, int max)
        {
            List<TrainingSession> shootings = new List<TrainingSession>();
            var trainingSessions = _db.Shootings;
            var series = _db.Series;
            var listOfRelevantShots = ShotsWithHeartRate(min, max);

            List<Series> WantedSeries = new List<Series>();

            foreach (var shot in listOfRelevantShots)
            {
                foreach (var serie in series)
                {
                    if (shot.SeriesId == serie.Id)
                    {
                        WantedSeries.Add(serie);
                    }
                }
            }

            foreach (var serie in WantedSeries)
            {
                foreach (var Session in trainingSessions)
                {
                    if (serie.TrainingSessionId == Session.Id)
                    {
                        if (shootings.Contains(Session))    
                        {
                            continue;
                        }
                        else
                        {
                            shootings.Add(Session);
                        }
                    }
                }
            }
            return shootings;
        }

        public List<TrainingSession> GetSessionsByTemperature(int min, int max)
        {
            var seriesWithDesiredTemp = _db.Series
                .Where(x => x.Weather.Temp > min
                && x.Weather.Temp < max);
            List<TrainingSession> relevantTrainingSessions = new List<TrainingSession>();
            var desiredShootings = _db.Shootings;

            foreach (var serie in seriesWithDesiredTemp)
            {
                foreach (var shooting in desiredShootings)
                {
                    if (serie.TrainingSessionId == shooting.Id)
                    {
                        relevantTrainingSessions.Add(shooting);
                    }
                }
            }
            return relevantTrainingSessions;
        }

        public List<TrainingSession> GetSessionsByWindspeed(int min, int max)
        {
            var seriesWithDesiredWindspeed = _db.Series
                .Where(x => x.Weather.Wind_speed > min
                && x.Weather.Wind_speed < max);
            List<TrainingSession> relevantTrainingSessions = new List<TrainingSession>();
            var desiredShootings = _db.Shootings;

            foreach (var serie in seriesWithDesiredWindspeed)
            {
                foreach (var shooting in desiredShootings)
                {
                    if (serie.TrainingSessionId == shooting.Id)
                    {
                        relevantTrainingSessions.Add(shooting);
                    }
                }
            }
            return relevantTrainingSessions;
        }
        /// <summary>
        /// Gets a shooting from the database
        /// </summary>
        /// <param name="shootingId"></param>
        /// <returns>TrainingSession</returns>
        public TrainingSession GetShootingByShootingId(string shootingId)
        {
            var shooting = _db.Shootings
                .Include(s => s.Results)//Gets the series
                .Include("Results.Shots")//Gets the shots
                .Include("Results.Weather")
                .Include(s => s.Location)
                .Where(x => x.ShootingId == shootingId);

            return shooting.FirstOrDefault();
        }

        public Weather GetWeatherById(int weatherId)
        {
            var weather = _db.Weathers
                .Where(x => x.Id == weatherId);
            return weather.FirstOrDefault();
        }

        /// <summary>
        /// Gets a biathlete from the database
        /// </summary>
        /// <param name="ibuId"></param>
        /// <returns>Biathlete</returns>
        public Biathlete GetBiathlete(string ibuId)
        {
            var biathlete = _db.Biathletes
                .Where(x => x.IbuID == ibuId);
            return biathlete.FirstOrDefault();
        }
        #endregion

        #region UPDATE
        public void AddCommentsToDatabase(string id, string comment)
        {
            var x = GetShootingByShootingId(id);
            x.Comments = comment;
            _db.Entry(x).Property("Comments").IsModified = true;
            _db.SaveChanges();
        }
        #endregion

        #region DELETE

        #endregion  
    }
}
