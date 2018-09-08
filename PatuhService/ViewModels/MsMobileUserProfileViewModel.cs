using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatuhService.ViewModels
{
    public class MsMobileUserProfileViewModel
    {
        public string UserID { get; set; }
        public string Pwd { get; set; }
        public Guid? UserGuid { get; set; }
        public string UserAccountType { get; set; }
        public string FullName { get; set; }        
        public string DOB { get; set; }
        public string Location { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public long PointReward { get; set; }
    }
}