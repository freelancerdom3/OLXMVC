using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OLX.DA.User;
using OLX.Models;
using OLX.Models.User;
using WebApplication1.Models;

namespace OLXMVCApp.Controllers.Users
{

    public class UserController : Controller
    {
        LoginDA access = new LoginDA();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

       
        public ActionResult loginType()
        {
            return View();
        }
        public ActionResult login()
        {
            return View();

        }

        [HttpPost]
        public ActionResult login(UsersModel loginModel)
        {
            
            bool result = access.authLogin(loginModel, out string msg,out int id);
            if (result)
            {


                if (access.IsAdmin(loginModel.userEmail))
                {
                    // return RedirectToAction("Index", "Home");
                    return Json(2);
                }
                else
                {

                    //return RedirectToAction("About", "Home");
                    Session["userid"] = id;
                    return Json(1);
                }

            }

            else
            {
                ViewBag.m = msg;
                //return View("Index");
                return Json(3);
            }


        }

        public ActionResult sendotp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult sendotp(string Mobileno)
        {
           
            bool ismobile = access.MobileNumberExists(Mobileno);
            if (!ismobile)
            {
                //return Json(new { success = false, message = "Invalid phone number format." });
                return Json(2);
            }
            Random rand = new Random();
            int value = rand.Next(100001, 999999);
            //string address = "+918237382320";
            string otp = $"Your otp is:{value}";


            int userid = access.GetUserIdByMobileNumber(Mobileno);
            DateTime expiretime = DateTime.Now.AddMinutes(5);
            access.InsertOtp(userid, value, expiretime);

            using (var wb = new WebClient())
            {
                byte[] response = wb.UploadValues("https://api.textlocal.in/send/", new NameValueCollection()
                {
                    {"apikey","NTA2ODRhNGU0ZDUyNmY0MzQ2NmQ1YTczNjQ1YTM2N2E=" },
                    {"numbers","91"+Mobileno },
                    {"sender","TXTLCL" }
                });
                string result = System.Text.Encoding.UTF8.GetString(response);
                Session["otp"] = value;

            }
            // return RedirectToAction("verify", "User");
            // return Json(new { success = true, message = "OTP sent successfully." });
            return Json(0);
        }
        public ActionResult MatchOtp()
        {
            return View();
        }

        [HttpPost]

        public ActionResult MatchOtp(int LoginOtp)
        {

            //LoginModel loginModel = new LoginModel();

           

            int userid = access.getidfromOtp(LoginOtp, out string msg);

            if (userid > 0)
            {   //string stored = user.GetOtp(userid);

                int stored = access.GetOtp(userid, out string message);
                int id = access.getuserid(userid);

                //bool otp = user.verifyOTP(userotp,out stored);
                if (LoginOtp == stored)
                {
                    Session["idbyotp"] = id;
                    return Json(1);
                    //return Json(new { message = "login" });
                }
                else
                {
                    //ViewBag.userid = message;
                    return Json(0);
                }
                //return Json(1);}




            }
            else
            {
               // ViewBag.msg = msg;
                return Json(2);
               // return View("verify");
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return View("logintype");
        }

        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registration(RegistrationModel model)
        {
            RegistrationDA repo = new RegistrationDA();
            var isemailalreadyexists = repo.IsEmailAlreadyExists(model.@userEmail);
            if (isemailalreadyexists)
            {
                ModelState.AddModelError("useremail", "this email already exists.");
            }
            else
            {
                bool registrationResult = repo.InsertUser(model);
                if (registrationResult)
                {
                    //ModelState.AddModelError(string.Empty, "Registration Success");
                    return RedirectToAction("loginType");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
                    return View(model);
                }
            }
            return View(model);
        }
        public ActionResult Sell()
        {
          
            return View();
        }
        [HttpPost]
        public ActionResult Sell(MyAdvertiseModel advertise, AdvertiseImagesModel image)
        {
            SellDA dataAccess = new SellDA();
            try
            {
                int advertiseId = dataAccess.InsertAdvertise(advertise);

       
                if (Request.Files.Count > 0)
                {

                    image.advertiseId = advertiseId;
                    image.ImageDataList = new List<byte[]>();

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase file = Request.Files[i];

                        if (file != null && file.ContentLength > 0)
                        {
                            byte[] imageData = new byte[file.ContentLength];
                            file.InputStream.Read(imageData, 0, file.ContentLength);
                            image.ImageDataList.Add(imageData);
                        }
                    }

                    dataAccess.InsertAdvertiseImage(image);
                }

                
                return RedirectToAction("Success");
            }
            catch (Exception ex)
            {
               
                ViewBag.ErrorMessage = "An error occurred while submitting the data: " + ex.Message;
                return View(); 
            }
        }
        public ActionResult Success()
        {
            return View();
        }

    }
}