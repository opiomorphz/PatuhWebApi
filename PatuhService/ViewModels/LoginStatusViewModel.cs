using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PatuhService.Models;

namespace PatuhService.ViewModels
{
    public class LoginStatusViewModel
    {
        public string userId { get; set; }
        public string userName { get; set; }
        public string userAccountType { get; set; }
        public MsMobileUserProfileViewModel userProfile { get; set; }
        public double points { get; set; }
    }
}