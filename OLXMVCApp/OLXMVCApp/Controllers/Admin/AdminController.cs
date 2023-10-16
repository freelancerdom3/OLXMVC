using OLX.DA.Admin;
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
        ProductListDA dataAccess = new ProductListDA();
        UserList_Data_Access uda = new UserList_Data_Access();
        AdminDA productCatRepository = new AdminDA();     


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




        public ActionResult UserIndex()
        {
            IEnumerable<UserList> ul = uda.GetAllUser();
            return View("UserIndex", "admin_layout", ul);
        }

        public ActionResult UserDetails(int? id)
        {
            UserList product = uda.GetUserData(id);
            return View("UserDetails", "admin_layout", product);
        }

        public ActionResult UserEdit(int id)
        {

            UserList user = uda.GetUserData(id);
            TempData["AlertMessage"] = "User Edited successfully......";
            return View("UserEdit", "admin_layout", user);
        }

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

        public ActionResult UserDelete(int? id)
        {
            UserList userList = uda.GetUserData(id);
            return View("UserDelete", "admin_layout", userList);
        }

        [HttpPost]
        public ActionResult UserDelete(int id)
        {
            try
            {
                uda.DeleteUser(id);
                TempData["AlertMessage"] = "user deleted successfully......";
                return RedirectToAction(nameof(UserIndex));
            }
            catch
            {
                return View();
            }
        }




        public ActionResult ProductList(string SearchItem, int? i)
        {
            IEnumerable<ProductListModel> products = dataAccess.GetAllProductList();
            return View("ProductList", "admin_layout", products);
        }

        public ActionResult ProductListDetails(int advertiseId)
        {
            ProductListModel product = dataAccess.GetProductList(advertiseId);
            return View("ProductListDetails", "admin_layout", product);
        }
        public ActionResult ProductlistEdit(int advertiseId)
        {
            ProductListModel product = dataAccess.GetProductList(advertiseId);
            return View("ProductlistEdit", "admin_layout", product);
        }

        [HttpPost]
        public ActionResult ProductlistEdit(ProductListModel product)
        {
            try
            {
                TempData["AlertMessage"] = "Product-List Edited successfully......";
                dataAccess.UpdateProductList(product);
                return RedirectToAction(nameof(ProductList));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ProductListDelete(int advertiseId)
        {
            TempData["AlertMessage"] = "Product-List Data Deleted successfully......";
            ProductListModel product = dataAccess.GetProductList(advertiseId);
            return View("ProductListDelete", "admin_layout", product);
        }

        [HttpPost]
        public ActionResult ProductListDelete(ProductListModel product)
        {
            try
            {
                dataAccess.DeleteProductList(product.advertiseId);
                return RedirectToAction(nameof(ProductList));
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Advertise()
        {
            AdvertiseDA d1 = new AdvertiseDA();
            AdvertiseModel viewModel = new AdvertiseModel();

            viewModel.Products = d1.GetProductsFromDatabase();
            viewModel.SubCategories = d1.GetProductsFromDatabase1();

            foreach (var subCategory in viewModel.SubCategories)
            {
                subCategory.ImageBytes = subCategory.imageData;
            }


            return View(viewModel);
        }
        [HttpPost] 
        public ActionResult Delete(int advertiseId, int advertiseImageId)
        {
            AdvertiseDA d = new AdvertiseDA();
            d.Deleteproduct(advertiseId, advertiseImageId);

            return RedirectToAction("Advertise");
        }


    }

}
