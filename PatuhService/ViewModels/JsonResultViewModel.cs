using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatuhService.ViewModels
{
    public class JsonResultViewModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string messageCode { get; set; }
    }
}