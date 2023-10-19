using OLX.DA.User;
using OLX.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OLXMVCApp.Controllers.Users
{
    public class SellController : Controller
    {
        [HttpGet]
        public ActionResult EditAdvertiseDetails(int? advertiseId)
        {
            SellDA product = new SellDA();
            MyAdvertiseModel myAdvertiseModel = product.getAdvertiseDetailsbyId(advertiseId);
            myAdvertiseModel.ImageDataList = product.GetAdvertiseImages(advertiseId);
            // Load the list of product subcategories
            // List<SelectListItem> productSubCategories = GetProductSubCategories(); // Implement this method to fetch the data

            // Set the ViewBag with the list of product subcategories
            //ViewBag.ProductSubCategories = productSubCategories;

            return View("EditAdvertiseDetails", myAdvertiseModel);
        }
               
        [HttpPost]
        public ActionResult EditAdvertiseDetails(MyAdvertiseModel myAdvertiseModel)
        {
            // Retrieve the existing product details including the image
            SellDA product = new SellDA();
            MyAdvertiseModel existingAdvertise = product.getAdvertiseDetailsbyId(myAdvertiseModel.advertiseId);

            // Update the product details
            existingAdvertise.advertiseTitle = myAdvertiseModel.advertiseTitle;
            existingAdvertise.advertiseDescription = myAdvertiseModel.advertiseDescription;
            existingAdvertise.advertisePrice = myAdvertiseModel.advertisePrice;

            // Check if a new image was uploaded
            if (Request.Files.Count > 0 && Request.Files[0] != null)
            {
                HttpPostedFileBase file = Request.Files[0];

                if (file.ContentLength > 0)
                {
                    byte[] imageData = new byte[file.ContentLength];
                    file.InputStream.Read(imageData, 0, file.ContentLength);
                    existingAdvertise.imageData = imageData;
                }
            }

            // Call the method to update product details and the image
            product.EditAdvertiseDetails(existingAdvertise);

            return RedirectToAction(nameof(ViewMyAds));
        }
        //public ActionResult EditAdvertiseDetails(MyAdvertiseModel myAdvertiseModelforeditpost, HttpPostedFileBase imageFile)
        //{
        //    if (imageFile != null && imageFile.ContentLength > 0)
        //    {
        //        // Read the image data from the uploaded file and set it in myAdvertiseModelforeditpost
        //        using (BinaryReader binaryReader = new BinaryReader(imageFile.InputStream))
        //        {
        //            myAdvertiseModelforeditpost.imageData = binaryReader.ReadBytes(imageFile.ContentLength);
        //        }
        //    }
        //    SellDA product = new SellDA();
        //    product.EditAdvertiseDetails(myAdvertiseModelforeditpost);
        //    return RedirectToAction(nameof(ViewMyAds));

        //}
        public ActionResult ViewMyAds()
        {

            SellDA product = new SellDA();
            List<MyAdvertiseModel> prc = product.GetAdvertiseDetails();
            return View(prc);

        }
        // GET: Sell
        public ActionResult Sell()
        {
            //if (Session["userid"] == null)
            //{
            //    return RedirectToAction("loginType", "User");
            //}

            //else
            //{

            //    return View();

            //}
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
        [HttpPost]
        public ActionResult Create(MyAdvertiseModel model)
        {
            // Process the form data and save it to the database

            // After data is saved, redirect to the "ViewMyAds" action
            return RedirectToAction("ViewMyAds");
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
