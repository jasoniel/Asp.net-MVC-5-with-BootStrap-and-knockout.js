using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BootstrapIntroduction.Models
{
    public class AuthenticatedUsers
    {

        private static List<User> _users = new List<User>
        {
            new User("jasoniel","123",null,new List<string>{ "::1"})
        };

        public static List<User> Users { get { return _users; } }
    }   
}