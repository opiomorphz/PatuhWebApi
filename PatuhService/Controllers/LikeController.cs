using System;
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
    public class LikeController : ApiController
    {
       

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

       

        // GET api/values/5
        public object Get(long ArticleId)
        {
            using (PatuhEntities db = new PatuhEntities())
            {
                IList<TrLike> likeList = db.TrLikes.Where(x => x.ArticleId == ArticleId).ToList();


                return likeList;
            }

        }

        public object Get(long ArticleId, string UserID)
        {
            using (PatuhEntities db = new PatuhEntities())
            {
                 
                IList<TrLike> likeList = db.TrLikes.Where(x => x.ArticleId == ArticleId && x.cCreated == UserID).ToList();


                return likeList;
            }

        }
        // POST api/values
        public object Post()
        {
            JsonResultViewModel result = new JsonResultViewModel();
            result.status = true;
            result.message = "Like successfully updated";
            result.messageCode = "S";

            var httpRequest = HttpContext.Current.Request;
            string id = httpRequest["Id"];
            long articleId = string.IsNullOrEmpty(httpRequest["ArticleId"]) ? 0 : long.Parse(httpRequest["ArticleId"]);
            string status = httpRequest["Status"];
            string userId = httpRequest["userId"];


            try
            {
                using (PatuhEntities db = new PatuhEntities())
                {

                    TrLike like = db.TrLikes.Where(x => x.ArticleId == articleId && x.cCreated == userId).FirstOrDefault();

                    if (like == null)
                    {
                        like = new TrLike();
                        like.cCreated = userId;
                        like.dCreated = DateTime.Now;
                        db.TrLikes.AddObject(like);

                        MsPoint msPoint = db.MsPoints.Where(x => x.ActionCode == "LIKEART").FirstOrDefault();

                        if (msPoint != null)
                        {
                            TrArticle trArticle = db.TrArticles.Where(x => x.Id == articleId).FirstOrDefault();

                            if (trArticle != null)
                            {
                                TrPoint trPoint = new TrPoint();

                                trPoint.ArticleId = trArticle.Id;
                                trPoint.UserID = trArticle.cCreated;
                                trPoint.ActionCode = "LIKEART";
                                trPoint.PointValue = msPoint.RewardPoint;
                                trPoint.cCreated = userId;
                                trPoint.dCreated = DateTime.Now;
                                trPoint.cLastUpdated = userId;
                                trPoint.dLastUpdated = DateTime.Now;

                                db.TrPoints.AddObject(trPoint);
                                //db.SaveChanges();
                            }
                        }
                    }



                    like.ArticleId = articleId;
                    like.cStatus = status;

                    like.cLastUpdated = userId;
                    like.dLastUpdated = DateTime.Now;

                    db.SaveChanges();

                    
                }
            }
            catch (Exception e)
            {
                result.status = false;
                result.message = e.Message;
                result.messageCode = "Error in saving Like";
            }

            return result;

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