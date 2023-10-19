using OLX.DA.User;
using OLX.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OLXMVCApp.Controllers
{
    public class HomeController : Controller
    {
        UserBuyDA dataAccess = new UserBuyDA();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult newfilter(int? categoryid, int? subcategoryid, int? stateid, int? cityid, int? areaid, decimal? minprice, decimal? maxprice, int? advertiseId)
        {
            List<UserBuyModel> list = dataAccess.newFilter(categoryid, subcategoryid, stateid, cityid, areaid, minprice, maxprice, advertiseId);

            return View(list);
        }


        public ActionResult showcatsub()
        {
          
            List<UserBuyModel> categorywithsub = dataAccess.GetCategoryWithSubcategories();

            return View(categorywithsub);
        }

        public ActionResult showAdvertiseDetails(int advertiseId)
        {
      
            IEnumerable<UserBuyModel> advertise = dataAccess.GetAdvertiseById(advertiseId);

            return View(advertise);
        }

        public ActionResult showLocation()
        {

            List<UserBuyModel> location = dataAccess.GetLocation();

            return View(location);
        }

        public ActionResult filterproduct()
        {
            return View();
        }

        public ActionResult Reloadcategory()
        {

            List<UserBuyModel> categorywithsub = dataAccess.GetCategoryWithSubcategories();

            return View(categorywithsub);

        }

        public ActionResult showLocationfornavbar()
        {

            List<UserBuyModel> location = dataAccess.GetLocationstate();

            return View(location);
        }
    }
}