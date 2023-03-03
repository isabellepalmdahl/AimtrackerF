using Aimtracker.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aimtracker.Repositories
{
    public interface IWeatherRepository
    {        
        Task<GetWeatherDto> GetWeather(string latitude, string longitude, string timestamp);
        Task<GetWeatherDto> GetWeatherMock();
    }
}
