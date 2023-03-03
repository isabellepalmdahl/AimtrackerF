using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Aimtracker.Infrastructure
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _client = new HttpClient();
        public async Task<T> GetAsync<T>(string endpoint)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
            try
            {
                using var response = await _client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<T>(responseJson);
                    return data;
                }
                throw new Exception("No connection with API");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}