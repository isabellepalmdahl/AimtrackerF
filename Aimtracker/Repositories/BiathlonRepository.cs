using Aimtracker.Infrastructure;
using Aimtracker.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aimtracker.Repositories
{
    public class BiathlonRepository : IBiathlonRepository
    {
        private readonly IApiClient _apiClient;
        private readonly string _baseEndpoint = "https://grupp8.dsvkurs.miun.se/api/";


        public BiathlonRepository(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<ShootingDto> GetShooting(string ibuId)
        {
            await Task.Delay(0);
            var shooting = await _apiClient.GetAsync<ShootingDto>($"{_baseEndpoint}Training/{ibuId}");
            return shooting;
        }

        public async Task<List<BiathleteDto>> GetBiathletes()
        {
            await Task.Delay(0);
            var athletes = await _apiClient.GetAsync<List<BiathleteDto>>($"{_baseEndpoint}athletes");
            return athletes;
        }

        public async Task<List<ShootingDto>> GetShootingDataForNewUser(string ibuId)
        {
            await Task.Delay(0);
            var shootings = await _apiClient.GetAsync<List<ShootingDto>>($"{_baseEndpoint}history/date/{ibuId}?startDate=200807&endDate=220211");
            return shootings;
        }
    }
}
