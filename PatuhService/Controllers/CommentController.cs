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
    public class CommentController : ApiController
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
                IList<TrComment> commentList = db.TrComments.Where(x => x.ArticleId == ArticleId).ToList();


                return commentList;
            }

        }

        
        // POST api/values
        public object Post()
        {
            JsonResultViewModel result = new JsonResultViewModel();
            result.status = true;
            result.message = "Comment successfully updated";
            result.messageCode = "S";

            var httpRequest = HttpContext.Current.Request;
            long id = string.IsNullOrEmpty(httpRequest["Id"]) ? 0 : long.Parse(httpRequest["Id"]);
            long articleId = string.IsNullOrEmpty(httpRequest["ArticleId"]) ? 0 : long.Parse(httpRequest["ArticleId"]);
            string postedComment = httpRequest["Comment"];
            string userId = httpRequest["userId"];


            try
            {
                using (PatuhEntities db = new PatuhEntities())
                {

                    TrComment comment = db.TrComments.Where(x => x.Id == id).FirstOrDefault();

                    if (comment == null)
                    {
                        comment = new TrComment();
                        comment.cCreated = userId;
                        comment.dCreated = DateTime.Now;
                        db.TrComments.AddObject(comment);

                        MsPoint msPoint = db.MsPoints.Where(x => x.ActionCode == "COMMENTART").FirstOrDefault();

                        if (msPoint != null)
                        {
                            TrArticle trArticle = db.TrArticles.Where(x => x.Id == articleId).FirstOrDefault();

                            if (trArticle != null)
                            {
                                TrPoint trPoint = new TrPoint();

                                trPoint.ArticleId = trArticle.Id;
                                trPoint.UserID = trArticle.cCreated;
                                trPoint.ActionCode = "COMMENTART";
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



                    comment.ArticleId = articleId;
                    comment.Comment = postedComment;
                    
                    comment.cLastUpdated = userId;
                    comment.dLastUpdated = DateTime.Now;

                    db.SaveChanges();

                    
                }
            }
            catch (Exception e)
            {
                result.status = false;
                result.message = e.Message;
                result.messageCode = "Error in saving Comment";
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