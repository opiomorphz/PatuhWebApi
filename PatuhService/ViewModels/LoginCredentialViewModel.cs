using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatuhService.ViewModels
{
    public class LoginCredentialViewModel
    {
        public string userId { get; set; }
        public string password { get; set; }
        public string userAccountType { get; set; }
    }
}