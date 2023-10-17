using OLX.DA.User;
using OLX.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace OLXMVCApp.Controllers.Users
{
    public class SellController : Controller
    {
        public ActionResult ViewMyAds()
        {

            SellDA product = new SellDA();
            List<MyAdvertiseModel> prc = product.GetAdvertiseDetails();
            return View(prc);

        }
        // GET: Sell
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
            string successMessage = TempData["SuccessMessage"] as string;

            if (!string.IsNullOrEmpty(successMessage))
            {
                ViewBag.SuccessMessage = successMessage;
            }
            return View();
        }

    }
}