using Aimtracker.Infrastructure;
using Aimtracker.Models.Dtos;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Aimtracker.Repositories
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly IApiClient _apiClient;
        private readonly string _baseEndpointWeather = "http://api.openweathermap.org/data/2.5/onecall/timemachine?appid=";
        
        public IConfiguration Configuration { get; }

        public WeatherRepository(IApiClient apiClient, IConfiguration configuration)
        {
            _apiClient = apiClient;
            Configuration = configuration;
        }

        public async Task<GetWeatherDto> GetWeather(string latitude, string longitude, string timestamp)
        {
            string key = Configuration["ConnectionStrings:Weather"];

            try
            {
                await Task.Delay(0);
                var weather = await _apiClient.GetAsync<GetWeatherDto>($"{_baseEndpointWeather}{key}&lat={latitude}&lon={longitude}&dt={timestamp}");
                return weather;
            }
            catch (System.Exception)
            {
                var weather = new GetWeatherDto();
                return weather;
            }
        }

        // gets mock weather for historical trainingSessions:
        public async Task<GetWeatherDto> GetWeatherMock()
        {
            await Task.Delay(0);
            var x = GetTestData<GetWeatherDto>();
            return x;
        }

        private T GetTestData<T>()
        {
            var data = File.ReadAllText("Mockdata/weather.json");
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}
