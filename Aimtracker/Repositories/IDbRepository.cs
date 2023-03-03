using Aimtracker.Models;
using Aimtracker.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aimtracker.Repositories
{
    public interface IDbRepository
    {
        Task<TrainingSession> AddShootingsAsync(List<ShootingDto> shootingDto);
        List<TrainingSession> GetAllShootings();
        Biathlete GetBiathlete(string ibuId);
        TrainingSession GetShootingByShootingId(string shootingId);
        List<TrainingSession> GetShootingsByDate(DateTime fromDate, DateTime toDate, string ibuId);
        void AddCommentsToDatabase(string id, string comment);
        Task<List<Biathlete>> SeedBiathletesAsync();
        Task<TrainingSession> AddShootingAsync(ShootingDto shootingDto);
        List<TrainingSession> GetSessionsByLocation(int locationId);
        List<TrainingSession> GetSessionByHeartrate(int min, int max);
        List<TrainingSession> GetSessionsByHitstatistic(double lowerHit, double higherHit);
        List<Shot> ShotsWithHeartRate(int min, int max);
        Weather GetWeatherById(int weatherId);
        List<TrainingSession> GetSessionsByTemperature(int min, int max);
        List<TrainingSession> GetSessionsByWindspeed(int min, int max);
    }
}
