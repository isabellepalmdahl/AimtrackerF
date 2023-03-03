using Aimtracker.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aimtracker.Repositories
{
    public interface IBiathlonRepository
    {
        Task<List<BiathleteDto>> GetBiathletes();
        Task<ShootingDto> GetShooting(string ibuId);
        Task<List<ShootingDto>> GetShootingDataForNewUser(string ibuId);
    }
}