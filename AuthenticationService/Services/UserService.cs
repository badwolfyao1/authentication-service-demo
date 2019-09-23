using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Services
{
    public class UserService
    {
        public bool AuthenticateUser(string username, string password)
        {
            // TODO: Hook up with your own User Database
            // Here we just hard coded couple
            if(username == "username"
                && password == "password")
            {
                return true;
            }
            return false;
        }
    }
}
