using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatuhService.ViewModels
{
    public class UserProfileViewModel
    {
        public string userId { get; set; }
        public string password { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string phoneNo { get; set; }
        public string location { get; set; }
        public DateTime? birthday { get; set; }
        
    }
}