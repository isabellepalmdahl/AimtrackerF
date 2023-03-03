using Aimtracker.Data;
using Aimtracker.Models;
using Aimtracker.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aimtracker.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IBiathlonRepository _repo;
        private readonly IDbRepository _db;
        
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IBiathlonRepository repo, IDbRepository db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repo = repo;
            _db = db;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                //Checks database for credentials
                var userLogin = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password,false,false);
                if (userLogin.Succeeded)
                {
                    // get logged in biathlete:
                    var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
                    var ibuId = user.IbuId;
                    var biathlete = _db.GetBiathlete(ibuId);

                    // get latest shooting:
                    var latestShooting = _repo.GetShooting(biathlete.IbuID);
                    await _db.AddShootingAsync(latestShooting.Result);

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Incorrect email or password");
            }
            return View(loginViewModel);
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerNewUser)
        {
            if(ModelState.IsValid)
            {
                //Checks for duplicate user and registers new users
                var user = new ApplicationUser { UserName = registerNewUser.Email, Email = registerNewUser.Email, IbuId = registerNewUser.IbuId };
                var createUser = await _userManager.CreateAsync(user, registerNewUser.Password);
                if (createUser.Succeeded)
                {
                    var fetchshootingdata = await _repo.GetShootingDataForNewUser(user.IbuId);     
                    await _db.AddShootingsAsync(fetchshootingdata);
                    
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    await _db.SeedBiathletesAsync();
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in createUser.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(registerNewUser);
        }
        [AllowAnonymous]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
    }
}
