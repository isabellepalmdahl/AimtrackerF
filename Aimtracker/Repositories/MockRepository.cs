using Aimtracker.Models.Dtos;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Aimtracker.Repositories
{
    public class MockRepository : IWeatherRepository
    {
        private readonly string _basePath;
        private readonly string _jsonFileName;

        public MockRepository(IWebHostEnvironment environment)
        {
            _basePath = $@"{environment.ContentRootPath}\Mockdata\";
            _jsonFileName = "ApiSessionData.json";
        }
        public MockRepository()//Constructor for NUnit test
        {
            _basePath =new Uri(@"Mockdata\",UriKind.Relative).ToString();
            _jsonFileName = "Shootings2.json";
        }
        public async Task<List<ShootingDto>> FilterSessions(DateTime fromDate,DateTime toDate)
        {
            //Filters
            List<ShootingDto> shootings = FilterByDate(await GetSessions(),fromDate,toDate); 
            return shootings;
        }
        private static List<ShootingDto> FilterByDate(List<ShootingDto> shootings, DateTime fromDate, DateTime toDate)
        {
            //Convert for session.Date because it is System.ReadOnlySpan<char> and can not be parsed

            shootings = shootings.Where(
                series => DateTime.Parse(Convert.ToString(series.Date)) < fromDate
                && DateTime.Parse(Convert.ToString(series.Date)) > toDate).ToList();
            
            return shootings;
        }
        
        public async Task<ShootingDto> GetSessionById(string id)
        {
            var sessions = await GetSessions();
            foreach (var shooting in sessions)
            {
                if(shooting.Id == id)
                {
                    return shooting;
                }
            }
            return null;
        }
        public async Task<List<ShootingDto>> GetSessions()
        {
            await Task.Delay(0);
            var x = GetTestData<List<ShootingDto>>(_jsonFileName);
            return x;
        }

        private T GetTestData<T>(string testfile)
        {
            var path = $@"{_basePath}{testfile}";
            var data = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(data);
        }

        public Task<ShootingDto> GetShooting(string ibuId)
        {
            throw new NotImplementedException();
        }

        public Task<GetWeatherDto> GetWeather(string latitude, string longitude, string timestamp)
        {
            throw new NotImplementedException();
        }

        public async Task<GetWeatherDto> GetWeatherMock()
        {
            await Task.Delay(0);
            var x = GetTestData<GetWeatherDto>("weather.json");
            return x;
        }
    }
}
