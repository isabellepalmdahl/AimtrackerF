using Aimtracker.Models;
using Aimtracker.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aimtracker.Controllers
{
    public class FilterController : Controller
    {
        private readonly IBiathlonRepository _repo;
        private readonly IWeatherRepository _weatherRepository;
        private readonly IDbRepository _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public FilterController(IBiathlonRepository repo, IWeatherRepository weatherRepository, IDbRepository dbRepository, UserManager<ApplicationUser> userManager)
        {
            _repo = repo;
            _weatherRepository = weatherRepository;
            _db = dbRepository;
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> GetSessionBasedOnHeartrate(string heartrateFrom, string heartrateTo, string windfrom, string windto, string tempfrom, string tempto)
        {
            var user = await _userManager.GetUserAsync(User);
            return Json(_db.ShotsWithHeartRate(int.Parse(heartrateFrom), int.Parse(heartrateTo)));
        }

    }
}
