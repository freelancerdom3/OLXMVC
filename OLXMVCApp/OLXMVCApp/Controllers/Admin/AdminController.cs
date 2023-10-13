﻿using OLX.DA.Admin;
using OLX.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OLXMVCApp.Controllers.Admin
{
    public class AdminController : Controller
    {
        AdminDA productCatRepository = null;
        public AdminController()
        {
            productCatRepository = new AdminDA();
        }


        public ActionResult SubCategoryList()
        {
            IEnumerable<ProductSubCategoryModeljoin> productDetails = productCatRepository.GetProductDetailsLists();
            

            return View("SubCategoryList", "Admin_Layout", productDetails);
        }

        public ActionResult SubCategoryListDetails(int productSubCategoryId)
        {
            ProductSubCategoryModeljoin productDetails = productCatRepository.GetProductDetails(productSubCategoryId);
            return View("SubCategoryListDetails", "Admin_Layout", productDetails);
        }

        public ActionResult SubCategoryListCreate()
        {
            return View("SubCategoryListCreate", "Admin_Layout");
        }

        [HttpPost]
        public ActionResult SubCategoryListCreate(ProductSubCategoryModeljoin productDetails)
        {
            try
            {
                TempData["AlertMessage"] = "Product Subcategory Added successfully......";
                productCatRepository.AddProductDetails(productDetails);

                return RedirectToAction(nameof(SubCategoryList));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult SubCategoryListEdit(int productSubCategoryId)
        {
            ProductSubCategoryModeljoin productDetails = productCatRepository.GetProductDetails(productSubCategoryId);
            return View("SubCategoryListEdit", "Admin_Layout", productDetails);
        }

        [HttpPost]
        public ActionResult SubCategoryListEdit(ProductSubCategoryModeljoin productDetails)
        {
            try
            {
                TempData["AlertMessage"] = "Product Subcategory Edited successfully......";
                productCatRepository.UpdateProductDetails(productDetails);
                return RedirectToAction(nameof(SubCategoryList));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult SubCategoryListDelete(int productSubCategoryId)
        {
            ProductSubCategoryModeljoin productDetails = productCatRepository.GetProductDetails(productSubCategoryId);
            return View("SubCategoryListDelete", "Admin_Layout", productDetails);
        }

        [HttpPost]
        public ActionResult SubCategoryListDelete(ProductSubCategoryModeljoin productDetails)
        {
            try
            {
                TempData["AlertMessage"] = "Product Subcategory Deleted successfully......";
                productCatRepository.DeleteProductDetails(productDetails.productSubCategoryId);
                return RedirectToAction(nameof(SubCategoryList));
            }
            catch
            {
                return View();
            }
        }

        UserList_Data_Access uda = new UserList_Data_Access();

        // GET: olx
        public ActionResult UserIndex()
        {
            IEnumerable<UserList> ul = uda.GetAllUser();
            return View("UserIndex", "admin_layout", ul);
            // return View(ul);
        }

        //// GET: olx/Details/5
        public ActionResult UserDetails(int? id)
        {
            UserList product = uda.GetUserData(id);
            return View("UserDetails", "admin_layout", product);
            // return View();
        }

        // GET: olx/Create

        // GET: olx/Edit/5
        public ActionResult UserEdit(int id)
        {

            UserList user = uda.GetUserData(id);
            TempData["AlertMessage"] = "User Edited successfully......";
            return View("UserEdit", "admin_layout", user);
        }

        //// POST: olx/Edit/5
        [HttpPost]
        public ActionResult UserEdit(UserList ul)
        {
            try
            {
                uda.Updateuser(ul);

                return RedirectToAction(nameof(UserIndex));
            }
            catch
            {
                return View();
            }
        }

        
        


        // GET: olx/Delete/5
        public ActionResult UserDelete(int? id)
        {
            UserList userList = uda.GetUserData(id);
            return View("UserDelete", "admin_layout", userList);
        }

        // POST: olx/Delete/5
        [HttpPost]
        public ActionResult UserDelete(int id)
        {
            try
            {

                // TODO: Add delete logic here
                uda.DeleteUser(id);
                TempData["AlertMessage"] = "user deleted successfully......";
                return RedirectToAction(nameof(UserIndex));
            }
            catch
            {
                return View();
            }
        }

       


       
    }

}