﻿using TalanUserAccessLayer.Models;

namespace TalanAPICrashProject.Models
{
    public class UserWithToken
    {
        public User User { get; set; }
        public string Token { get; set; }
    }
}
