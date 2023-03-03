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
    public class SessionController : Controller
    {
        private readonly IBiathlonRepository _repo;
        private readonly IWeatherRepository _weatherRepository;
        private readonly IDbRepository _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public SessionController(IBiathlonRepository repo, IWeatherRepository weatherRepository, IDbRepository dbRepository, UserManager<ApplicationUser> userManager)
        {
            _repo = repo;
            _weatherRepository = weatherRepository;
            _db = dbRepository;
            _userManager = userManager;
        }

        
        public async Task<IActionResult> Session(string id)
        {
            await Task.Delay(0);
            SessionViewModel sessionViewModel = new();
            sessionViewModel.Session = _db.GetShootingByShootingId(id);
            return View(sessionViewModel);
        }

        [HttpPost]
        public IActionResult AddCommentToShooting(string ShootingId, string comment)
        {
            _db.AddCommentsToDatabase(ShootingId, comment);
            return Json("Done");
        }
        public async Task<IActionResult> Filter()
        {
            var user = await _userManager.GetUserAsync(User);
            SessionViewModel sessionViewModel = new();
           
            sessionViewModel.Sessions = _db.GetShootingsByDate(DateTime.Now, sessionViewModel.GetToDate(Dates.twoYear), user.IbuId);// This for Db data
           
            return View(sessionViewModel);
        }

        [HttpGet]
        public IActionResult GetWeather(int weatherId)
        {
            Weather weather = new();
            Random random = new();
            weather = _db.GetWeatherById(weatherId);
            weather.Temp += random.Next(1, 4); // adds randomly to temp to simulate different weather conditions for each shot
            weather.Wind_deg += random.Next(1, 90); // adds randomly to wind degree to simulate different weather conditions for each shot
            weather.Wind_speed += random.Next(1, 7); // adds randomly to wind speed to simulate different weather conditions for each shot
            weather.Wind_speed = Math.Round(weather.Wind_speed, 2);
            return Json(weather);
        }
    }
}
