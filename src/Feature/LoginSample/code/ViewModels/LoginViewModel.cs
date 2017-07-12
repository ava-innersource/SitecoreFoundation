using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.LoginSample
{
    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Persistant { get; set; }
    }
}