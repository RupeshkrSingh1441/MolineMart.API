﻿using Microsoft.AspNetCore.Identity;

namespace MolineMart.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
