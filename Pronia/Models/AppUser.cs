﻿using Microsoft.AspNetCore.Identity;

namespace Pronia.Models
{
    public class AppUser:IdentityUser
    {
        public string FullName {get;set;}
        public bool IsAdmin {get;set;}
        public string Address {get;set;}

    }
}
