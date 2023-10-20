using OLX.DA.PaymentDA;
using OLX.Models;
using OLX.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OLXMVCApp.Controllers.Users
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult CheckAvailability()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CheckAvailability(int advertiseId)
        {
            // Call the IsProductAvailable method from Repositorypayment to check product availability
            Repositorypayment repository = new Repositorypayment();
            bool isAvailable = repository.IsProductAvailable(advertiseId);

            // Return a JSON response indicating whether the product is available
            return Json(new { IsAvailable = isAvailable }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PurchaseProduct()
        {
           
            return View();
        }
        [HttpPost]
        public ActionResult PurchaseProduct(int advertiseId)
        {
            if (Session["userid"] != null)
            {
                int userId = (int)Session["userid"];

                // Now you can use the userId in your logic.
                Repositorypayment repository = new Repositorypayment();
                bool purchaseResult = repository.PurchaseProduct(userId, advertiseId);

                if (purchaseResult)
                {
                    // Fetch recent transactions
                    List<PaymentdetailsBuyerModel> recentTransactions = repository.FetchRecentTransactions(userId);
                    ViewData["Transactions"] = recentTransactions;
                    ViewBag.Message = "Purchase was successful!";
                    // Pass the recent transactions to the view
                    return View("PurchaseProduct"); // Updated view name to "PurchaseSuccess"
                }
                else
                {
                    ViewBag.Message = "Purchase failed. Insufficient balance or product unavailable.";
                    return View("PurchaseProduct");
                }
            }
            else
            {
                return RedirectToAction("loginType", "User");
            }
        }



        public ActionResult AddMoney()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddMoney(int amountToAdd)
        {
            if (Session["userid"] != null)
            {
                int userId = (int)Session["userid"];

                // You can replace "Repositorypayment" with your actual repository class
                Repositorypayment repository = new Repositorypayment();

                // Assuming AddingMoneyToBuyerWallet returns a boolean indicating success or failure
                bool addMoneyResult = repository.AddingMoneyToBuyerWallet(userId, amountToAdd);

                if (addMoneyResult)
                {
                    List<PaymentdetailsBuyerModel> recentTransactions = repository.FetchRecentTransactions(userId);
                    ViewData["Transactions"] = recentTransactions;
                    ViewBag.Message = "Money added successfully!";
                }
                else
                {
                    ViewBag.Message = "Failed to add money. Please try again.";
                }

                // Return a view or redirect to another action as needed.
                return View("AddMoney");
            }
            else
            {
                // Handle the case where the user is not logged in or the session has expired.
                // You might want to redirect to a login page or show an error message.
                return RedirectToAction("loginType", "User");
            }
        }

        public ActionResult checkBuyerWallet()
        {
            if (Session["userid"] != null)
            {
                int userId = (int)Session["userId"];

                Repositorypayment repository = new Repositorypayment();
                List<PaymentdetailsBuyerModel> recentTransactions = repository.FetchRecentTransactions(userId);
                ViewData["Transactions"] = recentTransactions;

                return View("AddMoney");
            }
            else
            {
                return RedirectToAction("loginType", "User");
            }
        }

        public ActionResult checkSellerWallet()
        {
            if (Session["userid"] != null)
            {
                int userId = (int)Session["userid"];

                Repositorypayment repository = new Repositorypayment();
                List<PaymentdetailsSellerModel> recentTransactions = repository.FetchRecentTransactionsSeller(userId);
                ViewData["Transactions"] = recentTransactions;

                return View("AddMoney");
            }
           
               else
                {
                    // Handle the case where the user is not logged in or the session has expired.
                    // You might want to redirect to a login page or show an error message.
                    return View("login");
                }
            }
       


        [HttpPost]
        public ActionResult TransferSellerWalletToBuyer()
        {
            if (Session["userid"] != null)
            {
                int userId = (int)Session["userid"];
                bool transferResult = false;

           

                // You can replace "Repositorypayment" with your actual repository class
                Repositorypayment repository = new Repositorypayment();

                // Assuming TransferSellerWalletAmountToBuyer returns a boolean indicating success or failure
                transferResult = repository.TransferSellerWalletAmountToBuyer(userId);


                if (transferResult)
                {
                    ViewBag.Message = "Money transferred to buyer successfully!";
                }
                else
                {
                    ViewBag.Message = "Failed to transfer money. Please try again.";
                }
            }
            else
            {
                // Handle the case where the user is not logged in or the session has expired.
                // You might want to redirect to a login page or show an error message.
                return View("login");
            }
            return View("AddMoney"); // Replace "YourViewName" with the appropriate view name.
        }



        public ActionResult BuyerStatement(DateTime fromDate, DateTime toDate)
        {
            if (Session["userid"] != null)
            {
                int userId = (int)Session["userid"];

                // Your code to fetch data from the BuyerTransactionHistory table
                RepositoryInvoice invoice = new RepositoryInvoice();
                List<BuyerTransactionHistoryModel> transactions = invoice.FetchHistoryofBuyer(userId, fromDate, toDate);

                ViewData["Transactions"] = transactions;


                // Pass the transactions to the view
                return View("AddMoney");

            }
            else
            {
                // Handle the case where the user is not logged in or the session has expired.
                // You might want to redirect to a login page or show an error message.
                return View("login");
            }

        }

        public ActionResult SellerStatement(DateTime fromDate, DateTime toDate)
        {
            if (Session["userid"] != null)
            {
                int userId = (int)Session["userid"];
                // Your code to fetch data from the SellerTransactionHistory table
                RepositoryInvoice invoice = new RepositoryInvoice();
                List<SellerTransactionHistoryModel> transactions = invoice.FetchHistoryofSeller(userId, fromDate, toDate);

                ViewData["Transactions"] = transactions;

                // Pass the transactions to the view
                return View("AddMoney"); // You might need to change the view name as per your application
            }

            else
            {
                // Handle the case where the user is not logged in or the session has expired.
                // You might want to redirect to a login page or show an error message.
                return View("login");
            }
        }

    }
}