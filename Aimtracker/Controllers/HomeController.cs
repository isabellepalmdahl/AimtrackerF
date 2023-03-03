using Aimtracker.Models;
using Aimtracker.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
namespace Aimtracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly IDbRepository _db;

        public HomeController(ILogger<HomeController> logger, IDbRepository db, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _logger = logger;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            // get logged in biathlete:
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            SessionViewModel sessionViewModel = new();
            sessionViewModel.Sessions = _db.GetShootingsByDate(DateTime.Now, sessionViewModel.GetToDate(Dates.week), user.IbuId);
            sessionViewModel.SessionsVS = _db.GetShootingsByDate(
                sessionViewModel.GetToDate(Dates.week),
                sessionViewModel.GetToDate(sessionViewModel.GetTimeValueInUnix(Dates.week) * 2), user.IbuId);
            sessionViewModel.HitStatistic = sessionViewModel.CalcHitStatistic(sessionViewModel.Sessions);

            sessionViewModel.HitStatisticVS = sessionViewModel.CalcHitStatistic(sessionViewModel.SessionsVS);

            sessionViewModel.HitStatPrChange = sessionViewModel.CalcPercent(sessionViewModel.HitStatistic, sessionViewModel.HitStatisticVS);

            sessionViewModel.SessionsPrChange = sessionViewModel.CalcPercent(sessionViewModel.Sessions.Count(), sessionViewModel.SessionsVS.Count());

            return View(sessionViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Time(string value)
        {
            SessionViewModel model = new();
            var user = await _userManager.GetUserAsync(User);
            model.Sessions = _db.GetShootingsByDate(DateTime.Now, model.GetToDate(model.ConvertJsStringToDates(value)), user.IbuId);
            model.SessionsVS = _db.GetShootingsByDate(
                model.GetToDate(model.ConvertJsStringToDates(value)),
                model.GetToDate(model.GetTimeValueInUnix(model.ConvertJsStringToDates(value))*2), user.IbuId);
            model.HitStatistic = model.CalcHitStatistic(model.Sessions);
            model.HitStatisticVS = model.CalcHitStatistic(model.SessionsVS);
            model.HitStatPrChange = model.CalcPercent(model.HitStatistic, model.HitStatisticVS);
            model.SessionsPrChange = model.CalcPercent(model.Sessions.Count(),model.SessionsVS.Count());
            return Json(model);
        }
        [HttpGet]
        public async Task<IActionResult> Chart(string value, int datevalue)
        {

            SessionViewModel session = new();

            var user = await _userManager.GetUserAsync(User);
            var result = _db.GetShootingsByDate(new DateTime(datevalue, 12, 31), new DateTime(datevalue, 1, 1), user.IbuId);
            var data = session.GetStatistics(session.GetGraphVisuals(value), result); 
            return Json(data);
        }

            public IActionResult Privacy()
            {
                return View();
            }

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }


    }

