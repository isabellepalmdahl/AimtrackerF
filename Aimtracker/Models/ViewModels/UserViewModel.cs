using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aimtracker.Repositories;

namespace Aimtracker.Models
{  
    public class UserViewModel
    {
        public string FullName { get; set; }
        public string ProfilePic { get; set; }        

        public UserViewModel()
        {       
            
        }
    }
}
