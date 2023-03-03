using Aimtracker.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aimtracker.Models
{
    public class ProfileViewComponent : ViewComponent         
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDbRepository _db;

        public ProfileViewComponent(IDbRepository db, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentUser = await _userManager.GetUserAsync((System.Security.Claims.ClaimsPrincipal)User);
            var biathlete = _db.GetBiathlete(currentUser.IbuId);

            UserViewModel userViewModel = new UserViewModel();

            userViewModel.FullName = biathlete.FullName;
            userViewModel.ProfilePic = biathlete.Image;

            return View(userViewModel);
        }
    }
}
