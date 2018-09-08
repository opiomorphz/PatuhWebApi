using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PatuhService.Models;
using PatuhService.Utils;
using PatuhService.ViewModels;

namespace PatuhService.Controllers
{
    public class LoginController : ApiController
    {

        // GET api/values
        public object Get()
        {
            try
            {

                using (PatuhEntities db = new PatuhEntities())
                {
                    var obj = db.MsMobileUserProfiles.ToList();

                    return obj;
                }

            }
            catch (Exception e)
            {
                return e;
            }

            
             
        }

        // GET api/values
        public object Get(string userId, string password, string userAccountType)
        {
            return new LoginStatusViewModel() { userId = userId, userName = "Nama User", userAccountType = " STANDARD" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public object Post([FromBody]LoginCredentialViewModel loginCredential)
        {
            //string hashedPassword = Security.HashSHA1(loginCredential.password + userGuid.ToString());
            LoginStatusViewModel loginStatus = new LoginStatusViewModel();
            loginStatus.userId = "";
            loginStatus.userName = "";
            loginStatus.userAccountType = "";
            
            
            MsMobileUserProfile matchUser;

            try
            {
                using (PatuhEntities db = new PatuhEntities())
                {
                    //MsMobileUserProfile listProfile =  db.MsMobileUserProfiles.Where(x => x.UserID == loginCredential.userId).FirstOrDefault();

                    matchUser = db.MsMobileUserProfiles.Where(x => x.UserID == loginCredential.userId).FirstOrDefault();//db.MsMobileUserProfiles.Where(x => x.UserID == loginCredential.userId).DefaultIfEmpty(new MsMobileUserProfile()).FirstOrDefault();

                    if (matchUser != null)
                    {
                        string hashedPassword = Security.HashSHA1(loginCredential.password + matchUser.UserGuid);

                        if (matchUser.Pwd == hashedPassword)
                        {
                            // The password is correct
                            loginStatus.userId = matchUser.UserID ?? "";
                            loginStatus.userName = matchUser.FullName ?? "";
                            loginStatus.userAccountType = matchUser.UserAccountType ?? "";

                            MsMobileUserProfileViewModel msMobileUserProfileViewModel = new MsMobileUserProfileViewModel();
                            msMobileUserProfileViewModel.UserID = matchUser.UserID;
                            msMobileUserProfileViewModel.Pwd = matchUser.Pwd;
                            msMobileUserProfileViewModel.UserGuid = matchUser.UserGuid;
                            msMobileUserProfileViewModel.UserAccountType = matchUser.UserAccountType;
                            msMobileUserProfileViewModel.FullName = matchUser.FullName;
                            msMobileUserProfileViewModel.DOB = (matchUser.DOB != null ? matchUser.DOB.Value.ToString("dd-MM-yyyy") : "");
                            msMobileUserProfileViewModel.Location = matchUser.Location;
                            msMobileUserProfileViewModel.PhoneNo = matchUser.PhoneNo;
                            msMobileUserProfileViewModel.Email = matchUser.Email;


                            IList<TrPoint> listPoint = db.TrPoints.Where(x => x.UserID == matchUser.UserID).ToList();

                            if (listPoint != null)
                            {
                                msMobileUserProfileViewModel.PointReward = (listPoint.Select(x => x.PointValue.Value).Sum());
                            }
                            loginStatus.userProfile = msMobileUserProfileViewModel;
                        }

                    }

                }
            }
            catch (Exception e)
            {
                return e;

            }

            return loginStatus;

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