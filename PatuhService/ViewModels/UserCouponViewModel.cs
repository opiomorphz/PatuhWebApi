using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatuhService.ViewModels
{
    public class UserCouponViewModel
    {
        public long Id { get; set; }
        public string UserID { get; set; }
        public long MsCouponId { get; set; }
        public string CouponCode { get; set; }

        public string Title { get; set; }
        public string Benefit { get; set; }
        public string Usage { get; set; }
        public string Tnc { get; set; }

        public long PointNeeded { get; set; }
        public DateTime? ValidUntil { get; set; }
        //public byte[] CouponImage { get; set; }

        public string cStatus { get; set; }

        public string cCreated { get; set; }
        public DateTime? dCreated { get; set; }
        public string cLastUpdated { get; set; }
        public DateTime? dLastUpdated { get; set; }
    }
}