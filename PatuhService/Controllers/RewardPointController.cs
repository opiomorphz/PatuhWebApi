﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.IO;
using PatuhService.ViewModels;
using PatuhService.Models;
using PatuhService.Utils;
using System.Net.Http.Headers;

namespace PatuhService.Controllers
{
    public class RewardPointController : ApiController
    {
       

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }



        public object Get(string UserID)
        {
            using (PatuhEntities db = new PatuhEntities())
            {

                Int64 pointsEarned = db.TrPoints.Where(x => x.UserID == UserID).DefaultIfEmpty(new TrPoint()).Select(x => x.PointValue.Value).Sum();


                return pointsEarned;
            }

        }
        
        

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}