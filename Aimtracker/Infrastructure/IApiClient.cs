using System.Threading.Tasks;

namespace Aimtracker.Infrastructure
{
    public interface IApiClient
    {
        Task<T> GetAsync<T>(string endpoint);
    }
}