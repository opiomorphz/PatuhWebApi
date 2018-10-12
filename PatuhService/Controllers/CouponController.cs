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
    public class CouponController : ApiController
    {
       

        // GET api/values
        public IEnumerable<CouponViewModel> Get()
        {
            IList<CouponViewModel> listCouponViewModel = new List<CouponViewModel>();

            using (PatuhEntities db = new PatuhEntities())
            {
                IList<MsCoupon> couponList = db.MsCoupons.OrderBy(x=>x.Id).ToList();

                if (couponList != null && couponList.Count > 0)
                {
                    foreach (MsCoupon coupon in couponList)
                    {
                        CouponViewModel row = new CouponViewModel();
                        row.Id = coupon.Id;
                        row.Title = coupon.Title;
                        row.Benefit = coupon.Benefit;
                        row.Usage = coupon.Usage;
                        row.Tnc = coupon.Tnc;
                        row.PointNeeded = coupon.PointNeeded ?? 0;
                        row.ValidUntil = coupon.ValidUntil;
                        //row.CouponImage = coupon.CouponImage;
                        row.cStatus = coupon.cStatus;
                        row.cCreated = coupon.cCreated;
                        row.dCreated = coupon.dCreated;
                        row.cLastUpdated = coupon.cLastUpdated; 
                        row.dLastUpdated = coupon.dLastUpdated;

                        listCouponViewModel.Add(row);
                    }

                }

                return listCouponViewModel;
            }
        }

       

        // GET api/values/5
        public object Get(string UserID)
        {
            IList<UserCouponViewModel> listUserCouponViewModel = new List<UserCouponViewModel>();

            using (PatuhEntities db = new PatuhEntities())
            {
                IList<TrUserCoupon> userCouponList = db.TrUserCoupons.Where(x=>x.UserID == UserID).OrderBy(x => x.Id).ToList();

                if (userCouponList != null && userCouponList.Count > 0)
                {
                    foreach (TrUserCoupon userCoupon in userCouponList)
                    {
                        MsCoupon coupon = db.MsCoupons.Where(y => y.Id == userCoupon.MsCouponId).FirstOrDefault();

                        UserCouponViewModel row = new UserCouponViewModel();
                        row.Id = userCoupon.Id;
                        row.UserID = userCoupon.UserID;
                        row.MsCouponId = userCoupon.MsCouponId ?? 0;
                        row.CouponCode = userCoupon.CouponCode;
                        row.Title = coupon.Title;
                        row.Benefit = coupon.Benefit;
                        row.Usage = coupon.Usage;
                        row.Tnc = coupon.Tnc;
                        row.PointNeeded = coupon.PointNeeded ?? 0;
                        row.ValidUntil = coupon.ValidUntil;
                        //row.CouponImage = coupon.CouponImage;
                        row.cStatus = coupon.cStatus;
                        row.cCreated = coupon.cCreated;
                        row.dCreated = coupon.dCreated;
                        row.cLastUpdated = coupon.cLastUpdated;
                        row.dLastUpdated = coupon.dLastUpdated;

                        listUserCouponViewModel.Add(row);
                    }

                }

                return listUserCouponViewModel;
            }

        }

        public HttpResponseMessage GetCouponImage(long msCouponId)
        {
            using (PatuhEntities db = new PatuhEntities())
            {
                MsCoupon image = db.MsCoupons.Where(x => x.Id == msCouponId).FirstOrDefault();

                try
                {

                    var response = new HttpResponseMessage();
                    //response.Content = new StreamContent(fileStream);
                    response.Content = new ByteArrayContent(image.CouponImage);

                    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline");
                    response.Content.Headers.ContentDisposition.FileName = "image";
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
                    response.Content.Headers.ContentLength = image.CouponImage.Length;//fileLength;
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
            result.message = "Coupon successfully claimed";
            result.messageCode = "S";

            var httpRequest = HttpContext.Current.Request;
            long id = string.IsNullOrEmpty(httpRequest["msCouponId"]) ? 0 : long.Parse(httpRequest["msCouponId"]);
            string userId = httpRequest["UserId"];


            try
            {
                using (PatuhEntities db = new PatuhEntities())
                {
                    MsCoupon msCoupon = db.MsCoupons.Where(x => x.Id == id).FirstOrDefault();

                    TrUserCoupon userCoupon = db.TrUserCoupons.Where(x => x.MsCouponId == id).FirstOrDefault();

                    if (userCoupon == null)
                    {
                        userCoupon = new TrUserCoupon();
                        userCoupon.MsCouponId = id;
                        userCoupon.UserID = userId;
                        userCoupon.cCreated = userId;
                        userCoupon.dCreated = DateTime.Now;
                        db.TrUserCoupons.AddObject(userCoupon);
                    }
                    
                    userCoupon.cStatus = "Y";
                    /*
                    if (httpRequest.Files.Count > 0)
                        {                             
                           
                            foreach (string file in httpRequest.Files)
                            {
                                var postedFile = httpRequest.Files[file];
                                Type fileType = postedFile.GetType();
                                byte[] couponImage = new byte[postedFile.ContentLength];

                                postedFile.InputStream.Read(couponImage, 0, postedFile.ContentLength);
                                userCoupon.c
                            }
                    */
                    db.SaveChanges();                    
                }
            }
            catch (Exception e)
            {
                result.status = false;
                result.message = e.Message;
                result.messageCode = "Error in claiming Coupon";
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