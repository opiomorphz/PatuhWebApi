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
using PagedList;

namespace PatuhService.Controllers
{
    public class NewsController : ApiController
    {
       

        // GET api/values
        public IList<ArticleViewModel> Get()
        {
            IList<ArticleViewModel> articleViewModels = new List<ArticleViewModel>();
            using (PatuhEntities db = new PatuhEntities())
            {
                IList<TrArticle> articles = db.TrArticles.Where(x => x.Id != 0 && x.Category == "NEWS").ToList();


                foreach (TrArticle article in articles)
                {
                    ArticleViewModel articleViewModel = new ArticleViewModel();
                    articleViewModel.Id = article.Id;
                    articleViewModel.Title = article.Title;
                    articleViewModel.Category = article.Category;
                    articleViewModel.Story = article.Story;
                    articleViewModel.GPSLocation = article.GPSLocation;
                    articleViewModel.GPSLong = article.GPSLong;
                    articleViewModel.GPSLat = article.GPSLat;
                    articleViewModel.ImageIds = db.TrImageAttachments.Where(x => x.HeaderId == article.Id).Select(y => y.Id).ToArray();

                    articleViewModel.cCreated = article.cCreated;
                    articleViewModel.dCreated = article.dCreated;
                    articleViewModel.cLastUpdated = article.cLastUpdated;
                    articleViewModel.dLastUpdated = article.dLastUpdated;

                    articleViewModels.Add(articleViewModel);
                }
            }
            return articleViewModels;
        }


        public IList<ArticleViewModel> Get(string UserID)
        {
            IList<ArticleViewModel> articleViewModels = new List<ArticleViewModel>();
            using (PatuhEntities db = new PatuhEntities())
            {
                IList<TrArticle> articles = db.TrArticles.Where(x => x.cCreated == UserID && x.Category == "NEWS").ToList();

                foreach (TrArticle article in articles)
                {
                    ArticleViewModel articleViewModel = new ArticleViewModel();
                    articleViewModel.Id = article.Id;
                    articleViewModel.Title = article.Title;
                    articleViewModel.Category = article.Category;
                    articleViewModel.Story = article.Story;
                    articleViewModel.GPSLocation = article.GPSLocation;
                    articleViewModel.GPSLong = article.GPSLong;
                    articleViewModel.GPSLat = article.GPSLat;
                    articleViewModel.ImageIds = db.TrImageAttachments.Where(x => x.HeaderId == article.Id).Select(y => y.Id).ToArray();

                    articleViewModel.cCreated = article.cCreated;
                    articleViewModel.dCreated = article.dCreated;
                    articleViewModel.cLastUpdated = article.cLastUpdated;
                    articleViewModel.dLastUpdated = article.dLastUpdated;

                    articleViewModels.Add(articleViewModel);

                    
                }
            }
            return articleViewModels;
        }

       
        public HttpResponseMessage GetArticleImage(long imageId)
        {
            using (PatuhEntities db = new PatuhEntities())
            {
                TrImageAttachment image = db.TrImageAttachments.Where(x => x.Id == imageId).FirstOrDefault();

                try
                {
                   
                    var response = new HttpResponseMessage();
                    //response.Content = new StreamContent(fileStream);
                    response.Content = new ByteArrayContent(image.Image);

                    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline");
                    response.Content.Headers.ContentDisposition.FileName = "image";
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
                    response.Content.Headers.ContentLength = image.Image.Length;//fileLength;
                    return response;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

        }

        // POST api/values
        public object Post()
        {
            JsonResultViewModel result = new JsonResultViewModel();
            result.status = true;
            result.message = "News successfully updated";
            result.messageCode = "S";

            var httpRequest = HttpContext.Current.Request;
            long id = string.IsNullOrEmpty(httpRequest["Id"]) ? 0 : long.Parse(httpRequest["Id"]);           
            string title = httpRequest["title"];
            string story = httpRequest["story"];
            string location = httpRequest["location"];
            double latitude = string.IsNullOrEmpty(httpRequest["latitude"]) ? 0 : double.Parse(httpRequest["latitude"]);
            double longitude = string.IsNullOrEmpty(httpRequest["longitude"]) ? 0 : double.Parse(httpRequest["longitude"]); 
            string userId = httpRequest["userId"];


            try
            {
                using (PatuhEntities db = new PatuhEntities())
                {
                    TrArticle article = db.TrArticles.Where(x => x.Id == id).FirstOrDefault();

                    if (article == null)
                    {
                        article = new TrArticle();     
                        article.cCreated = userId;
                        article.dCreated = DateTime.Now;
                        db.TrArticles.AddObject(article);
                    }


                    article.Category = "ARTICLE";
                    article.Title = title;
                    article.Story = story;
                    article.GPSLocation = location;
                    article.GPSLong = longitude;
                    article.GPSLat = latitude;
                   
                    article.cLastUpdated = userId;
                    article.dLastUpdated = DateTime.Now;

                    db.SaveChanges();

                    try
                    {
                        if (httpRequest.Files.Count > 0)
                        {
                            IList<TrImageAttachment> currentImages = db.TrImageAttachments.Where(x => x.HeaderId == article.Id).ToList();
                            if (currentImages != null && currentImages.Count > 0)
                            {
                                foreach(TrImageAttachment dbImg in currentImages)
                                {
                                    db.TrImageAttachments.DeleteObject(dbImg);
                                }

                            }

                            string extention = "";
                            string guid = "";
                            
                            string[] supportedTypes = new string[] { "jpg", "jpeg", "bmp", "png" };
                            int fileSequence = 0;

                            foreach (string file in httpRequest.Files)
                            {
                                var postedFile = httpRequest.Files[file];
                                Type fileType = postedFile.GetType();
                                if (postedFile != null)
                                {
                                    if (postedFile.FileName != "")
                                    {
                                        byte[] theImage = new byte[postedFile.ContentLength];
                                        extention = (Path.GetExtension(postedFile.FileName).TrimStart('.')).ToLower();

                                        if (supportedTypes.Contains(extention))
                                        {
                                           
                                            postedFile.InputStream.Read(theImage, 0, postedFile.ContentLength);


                                            TrImageAttachment imageAtt = new TrImageAttachment();// db.TrImageAttachments.Where(x => x.Id == id).FirstOrDefault();


                                            imageAtt = new TrImageAttachment();
                                            imageAtt.HeaderId = article.Id;

                                            imageAtt.cCreated = userId;
                                            imageAtt.dCreated = DateTime.Now;
                                            
                                            imageAtt.Sequence = ++fileSequence;
                                            imageAtt.Image = theImage;
                                            imageAtt.cLastUpdated = userId;
                                            imageAtt.dLastUpdated = DateTime.Now;

                                            db.TrImageAttachments.AddObject(imageAtt);
                                            db.SaveChanges();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        result.status = false;
                        result.message = e.Message;
                        result.messageCode = "Error in saving article";
                        return result;
                    }

                    
                }
            }
            catch (Exception e)
            {
                result.status = false;
                result.message = e.Message;
                result.messageCode = "Error in saving Article";
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