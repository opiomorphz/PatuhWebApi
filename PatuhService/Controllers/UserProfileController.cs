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
    public class UserProfileController : ApiController
    {
       

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

       

        // GET api/values/5
        public object Get(string UserID)
        {
            using (PatuhEntities db = new PatuhEntities())
            {
                MsMobileUserProfile profile = db.MsMobileUserProfiles.Where(x => x.UserID == UserID).First();

                return profile;
            }

        }

        public HttpResponseMessage GetProfilePic(string UserID, string userAccountType)
        {
            using (PatuhEntities db = new PatuhEntities())
            {
                MsMobileUserProfile profile = db.MsMobileUserProfiles.Where(x => x.UserID == UserID).FirstOrDefault();

                try
                {

                    FileStream fileStream = File.OpenRead(profile.ProfilePicPath);
                    long fileLength = new FileInfo(profile.ProfilePicPath).Length;

                    var response = new HttpResponseMessage();
                    response.Content = new StreamContent(fileStream);
                    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    response.Content.Headers.ContentDisposition.FileName = "image";
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response.Content.Headers.ContentLength = fileLength;
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
            result.message = "User Profile successfully updated";
            result.messageCode = "S";

            var httpRequest = HttpContext.Current.Request;
            string userId = httpRequest["userId"];
            string password = httpRequest["password"];
            string userName = httpRequest["userName"];
            string email = httpRequest["email"];
            string phoneNo = httpRequest["phoneNo"];
            string location = httpRequest["location"];
            string birthday = httpRequest["birthday"];


            try
            {
                using (PatuhEntities db = new PatuhEntities())
                {
                    MsMobileUserProfile profile = db.MsMobileUserProfiles.Where(x => x.UserID == userId).FirstOrDefault();

                          

                    Guid userGuid = System.Guid.NewGuid();
                    string hashedPassword = Security.HashSHA1(password + userGuid.ToString());

                    string profilePicPath = "";
                    try
                    {
                        if (httpRequest.Files.Count > 0)
                        {
                            string extention = "";
                            string guid = "";
                            string[] supportedTypes = new string[] { "jpg", "jpeg", "bmp", "png" };

                            foreach (string file in httpRequest.Files)
                            {
                                var postedFile = httpRequest.Files[file];
                                Type fileType = postedFile.GetType();
                                if (postedFile != null)
                                {
                                    if (postedFile.FileName != "")
                                    {
                                        extention = (Path.GetExtension(postedFile.FileName).TrimStart('.')).ToLower();

                                        if (supportedTypes.Contains(extention))
                                        {
                                            string filePath = Path.Combine(httpRequest.MapPath("~/PhotoUploads"), Path.GetFileName(postedFile.FileName));
                                            postedFile.SaveAs(filePath);
                                            guid = DateTime.Now.ToString("yyyyMMddhhmmss") + System.Guid.NewGuid().ToString("n") + Path.GetExtension(postedFile.FileName);

                                            FileInfo TheFile = new FileInfo(filePath);
                                            if (TheFile.Exists)
                                            {
                                                TheFile.MoveTo(Path.Combine(httpRequest.MapPath("~/PhotoUploads"), Path.GetFileName(guid)));

                                            }

                                            profilePicPath = TheFile.FullName;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {

                    }

                    if (profile == null)
                    {
                        profile = new MsMobileUserProfile();                        
                        db.MsMobileUserProfiles.AddObject(profile);
                    }

                    profile.UserID = userId;
                    profile.FullName = userName;
                    profile.Email = email;
                    profile.PhoneNo = phoneNo;
                    profile.Location = location;
                    profile.DOB = DateTime.Now;//DateTime.Parse(birthday);
                    profile.Pwd = hashedPassword;
                    profile.UserGuid = userGuid;
                    profile.ProfilePicPath = profilePicPath;
                                        
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                result.status = false;
                result.message = e.Message;
                result.messageCode = "Error in saving User Profile";
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