using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatuhService.ViewModels
{
    public class ArticleViewModel
    {
        public long Id { get; set; }
        public string Category {get;set;}
        public string Title {get;set;}
        public string Story {get;set;}
        public string GPSLocation { get; set; }
        public double? GPSLong {get;set;}
        public double? GPSLat {get;set;}
        public long[] ImageIds { get; set; }

        public string cCreated { get; set; }
        public DateTime? dCreated { get; set; }
        public string cLastUpdated { get; set; }
        public DateTime? dLastUpdated { get; set; }
    }
}