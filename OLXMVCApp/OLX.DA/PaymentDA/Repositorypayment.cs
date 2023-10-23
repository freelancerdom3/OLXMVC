using OLX.Models;
using OLX.Models.User;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLX.DA.PaymentDA
{
  public  class Repositorypayment
    {
        private SqlConnection con;
        public void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            con = new SqlConnection(constr);

        }

        public bool IsProductAvailable(int advertiseId)
        {
            connection();
            SqlCommand com = new SqlCommand("SELECT advertiseStatus FROM tbl_MyAdvertise WHERE advertiseId = @AdvertiseId", con);
            com.Parameters.AddWithValue("@AdvertiseId", advertiseId);

            con.Open();
            SqlDataReader sqlDataReader = com.ExecuteReader();

            bool result = false;  // Initialize the result variable as false

            if (sqlDataReader.Read())  // Check if there is a row to read
            {
                result = !sqlDataReader.GetBoolean(0); // Invert the result: true for 0, false for 1
            }

            con.Close();

            return result;
        }


        public bool AddingMoneyToBuyerWallet(int userId, int amountToAdd)
        {
            connection();
            int paymentIdB = 0;
            SqlCommand getpaymentidB = new SqlCommand("select paymentIdB from PaymentdetailsBuyer where userId = @userId", con);
            getpaymentidB.Parameters.AddWithValue("@userId", userId);

           
            con.Open();                                                                                                                                                                                                                                                                             
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    

            SqlDataReader reader = getpaymentidB.ExecuteReader();

            if (reader.Read())                                                                                                                                              
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    {                                   
                paymentIdB = reader.GetInt32(0);
                reader.Close();



                if (paymentIdB > 0)
                {


                    // User exists, update Buyerwallet
                    SqlCommand updateBuyerwallet = new SqlCommand("UPDATE PaymentdetailsBuyer SET Buyerwallet = Buyerwallet + @amountToAdd WHERE userId = @userId", con);
                    updateBuyerwallet.Parameters.AddWithValue("@amountToAdd", amountToAdd);
                    updateBuyerwallet.Parameters.AddWithValue("@userId", userId);
                    int rowsAffected = updateBuyerwallet.ExecuteNonQuery();


                    if (rowsAffected > 0)
                    {
                        con.Close();

                        SqlCommand insertHistoryAddMoney = new SqlCommand("insert into BuyerTransactionHistory (UserId,BuyerWallet) values (@userId, @BuyerWallet)", con);
                        insertHistoryAddMoney.Parameters.AddWithValue("@userId", userId);
                        insertHistoryAddMoney.Parameters.AddWithValue("@Buyerwallet", amountToAdd);
                        con.Open();
                        int insertedrowsAffected = insertHistoryAddMoney.ExecuteNonQuery();

                        con.Close();
                        return true;
                    }
                }
            }
            else
            {
                reader.Close(); // Close the DataReader before proceeding

                // User does not exist, insert a new record
                SqlCommand insertUserIdWallet = new SqlCommand("INSERT INTO PaymentdetailsBuyer (userId, Buyerwallet) VALUES (@userId, @Buyerwallet)", con);

                insertUserIdWallet.Parameters.AddWithValue("@userId", userId);
                insertUserIdWallet.Parameters.AddWithValue("@Buyerwallet", amountToAdd);

                int rowsAffected = insertUserIdWallet.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    SqlCommand insertHistoryAddMoney = new SqlCommand("insert into BuyerTransactionHistory (userId,BuyerWallet) values (@userId, @BuyerWallet)", con);

                    insertHistoryAddMoney.Parameters.AddWithValue("@userId", userId);
                    insertHistoryAddMoney.Parameters.AddWithValue("@Buyerwallet", amountToAdd);

                    int insertedrowsAffected = insertHistoryAddMoney.ExecuteNonQuery();


                    con.Close();
                    return true;
                }
            }
            con.Close();
            return false;
        }







        public bool PurchaseProduct(int userId, int advertiseId)
        {
            connection();

            // Check if the product is available
            bool isAvailable = IsProductAvailable(advertiseId);

            if (!isAvailable)
            {
                // Product is not available
                con.Close();
                return false;
            }

            con.Open(); // Open the connection here

            decimal advertisePrice = 0;

            using (SqlCommand getPriceCommand = new SqlCommand("SELECT advertisePrice FROM tbl_MyAdvertise WHERE advertiseId = @AdvertiseId", con))
            {
                getPriceCommand.Parameters.AddWithValue("@AdvertiseId", advertiseId);

                using (SqlDataReader priceReader = getPriceCommand.ExecuteReader())
                {
                    if (priceReader.Read())
                    {
                        advertisePrice = priceReader.GetDecimal(0);
                    }
                }
            }

            decimal buyerWallet = 0;

            using (SqlCommand checkBalanceCommand = new SqlCommand("SELECT Buyerwallet FROM PaymentdetailsBuyer WHERE userId = @UserId", con))
            {
                checkBalanceCommand.Parameters.AddWithValue("@UserId", userId);

                using (SqlDataReader balanceReader = checkBalanceCommand.ExecuteReader())
                {
                    if (balanceReader.Read())
                    {
                        buyerWallet = balanceReader.GetDecimal(0);
                    }
                }
            }

            if (buyerWallet >= advertisePrice)
            {
                // User has sufficient balance, proceed with the purchase
                decimal totalAmountPaid = advertisePrice;

                using (SqlCommand updateTotalAmountCommand = new SqlCommand("UPDATE PaymentdetailsBuyer SET TotalamountPaid = @TotalAmountPaid, advertiseId = @AdvertiseId, buyerWallet = buyerWallet - @TotalAmountPaid WHERE userId = @UserId", con))
                {
                    updateTotalAmountCommand.Parameters.AddWithValue("@TotalAmountPaid", totalAmountPaid);
                    updateTotalAmountCommand.Parameters.AddWithValue("@UserId", userId);
                    updateTotalAmountCommand.Parameters.AddWithValue("@AdvertiseId", advertiseId);

                    int rowsAffected = updateTotalAmountCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {



                        int statusChanging = 1;

                        using (SqlCommand updateAdStatusCommand = new SqlCommand("UPDATE tbl_MyAdvertise SET advertiseStatus = @StatusChanging WHERE advertiseId = @AdvertiseId", con))
                        {
                            updateAdStatusCommand.Parameters.AddWithValue("@StatusChanging", statusChanging);
                            updateAdStatusCommand.Parameters.AddWithValue("@AdvertiseId", advertiseId);

                            int statusRowsAffected = updateAdStatusCommand.ExecuteNonQuery();
                        }


                        //this logic is about selling PaymentdetailsSeller and SellerTransactionHistory

                        int PaymentIds = 0;

                        using (SqlCommand getpaymentIds = new SqlCommand("SELECT paymentIds FROM PaymentdetailsSeller WHERE userId = @userId", con))
                        {
                            getpaymentIds.Parameters.AddWithValue("@userId", userId);

                            using (SqlDataReader reader = getpaymentIds.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    PaymentIds = reader.GetInt32(0);
                                }
                            }
                        }
                        // this storeAdvertisePrice and storeUserIdfromAd fetching and storing the value from tbl_MyAdvertise table
                        decimal storeAdvertisePrice = 0;
                        int storeUserIdfromAd = 0;

                        using (SqlCommand fetchdatafromadvert = new SqlCommand("SELECT advertisePrice, userId FROM tbl_MyAdvertise WHERE advertiseId = @advertiseId", con))
                        {
                            fetchdatafromadvert.Parameters.AddWithValue("@advertiseId", advertiseId);

                            using (SqlDataReader reader1 = fetchdatafromadvert.ExecuteReader())
                            {
                                if (reader1.Read())
                                {
                                    storeAdvertisePrice = reader1.GetDecimal(0);

                                    storeUserIdfromAd = reader1.GetInt32(1);
                                }
                            }
                        }
                        // checking here already wallet is present or not if it is then it should be update 
                        if (PaymentIds > 0)
                        {
                            // and storeAdvertisePrice and storeUserIdfromAd and other data storing in PaymentdetailsSeller table
                            using (SqlCommand updateSellingtable = new SqlCommand("UPDATE PaymentdetailsSeller SET SellerWallet = SellerWallet + @storeAdvertisePrice, ReceivedAmount =@storeAdvertisePrice,  userId = @storeUserIdfromAd, advertiseId = @advertiseId WHERE userId = @storeUserIdfromAd", con))
                            {
                                updateSellingtable.Parameters.AddWithValue("@storeAdvertisePrice", storeAdvertisePrice);
                                updateSellingtable.Parameters.AddWithValue("@storeUserIdfromAd", storeUserIdfromAd);
                                updateSellingtable.Parameters.AddWithValue("@advertiseId", advertiseId);

                                int rowsAffectedsel = updateSellingtable.ExecuteNonQuery();
                                // storing history for UPDATE PaymentdetailsSeller
                                if (rowsAffectedsel > 0)
                                {
                                    using (SqlCommand sellerHistory = new SqlCommand("insert into SellerTransactionHistory (TotalAmountReceived,SellerWallet,UserId,advertiseId) values (@storeAdvertisePrice,@storeAdvertisePrice,@storeUserIdfromAd,@advertiseId)", con))
                                    {
                                        sellerHistory.Parameters.AddWithValue("@storeAdvertisePrice", storeAdvertisePrice);
                                        sellerHistory.Parameters.AddWithValue("@storeUserIdfromAd", storeUserIdfromAd);
                                        sellerHistory.Parameters.AddWithValue("@advertiseId", advertiseId);
                                        int rowsAffectedSellerHistory = sellerHistory.ExecuteNonQuery();


                                    }
                                }
                            }
                        }
                        //if it is not then new wallet should create
                        else
                        {
                            using (SqlCommand insertintosell = new SqlCommand("INSERT INTO PaymentdetailsSeller (SellerWallet,ReceivedAmount, userId, advertiseId) VALUES (@storeAdvertisePrice,@storeAdvertisePrice, @storeUserIdfromAd, @advertiseId)", con))
                            {
                                insertintosell.Parameters.AddWithValue("@storeAdvertisePrice", storeAdvertisePrice);
                                insertintosell.Parameters.AddWithValue("@storeUserIdfromAd", storeUserIdfromAd);
                                insertintosell.Parameters.AddWithValue("@advertiseId", advertiseId);

                                int rowsAffectedsel = insertintosell.ExecuteNonQuery();

                                // storing history for INSERT INTO PaymentdetailsSeller
                                if (rowsAffectedsel > 0)
                                {
                                    using (SqlCommand sellerHistory = new SqlCommand("insert into SellerTransactionHistory (TotalAmountReceived,SellerWallet,UserId,advertiseId) values (@storeAdvertisePrice,@storeAdvertisePrice,@storeUserIdfromAd,@advertiseId)", con))
                                    {
                                        sellerHistory.Parameters.AddWithValue("@storeAdvertisePrice", storeAdvertisePrice);
                                        sellerHistory.Parameters.AddWithValue("@storeUserIdfromAd", storeUserIdfromAd);
                                        sellerHistory.Parameters.AddWithValue("@advertiseId", advertiseId);
                                        int rowsAffectedSellerHistory = sellerHistory.ExecuteNonQuery();


                                    }
                                }
                            }
                        }

                        using (SqlCommand insertinghistory = new SqlCommand("INSERT INTO BuyerTransactionHistory (UserId, BuyerWallet, TotalAmountPaid, advertiseId) VALUES (@UserId, @BuyerWallet, @TotalAmountPaid, @AdvertiseId)", con))
                        {
                            insertinghistory.Parameters.AddWithValue("@UserId", userId);
                            insertinghistory.Parameters.AddWithValue("@BuyerWallet", buyerWallet - totalAmountPaid);
                            insertinghistory.Parameters.AddWithValue("@TotalAmountPaid", totalAmountPaid);
                            insertinghistory.Parameters.AddWithValue("@AdvertiseId", advertiseId);

                            int insertinghistoryr = insertinghistory.ExecuteNonQuery();
                        }

                        // Close the connection after executing all commands
                        con.Close();

                        return true;
                    }
                }
            }

            // Insufficient balance or update failed
            con.Close(); // Close the connection in case of early return
            return false;



        }


        public List<PaymentdetailsBuyerModel> FetchRecentTransactions(int userId)
        {
            connection();
            List<PaymentdetailsBuyerModel> transactions = new List<PaymentdetailsBuyerModel>();


            con.Open();

            string query = "SELECT * FROM PaymentdetailsBuyer WHERE userId = @userId";

            using (SqlCommand recentTransaction = new SqlCommand(query, con))
            {
                recentTransaction.Parameters.AddWithValue("@userId", userId);

                using (SqlDataReader reader = recentTransaction.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PaymentdetailsBuyerModel transaction = new PaymentdetailsBuyerModel
                        {



                            PaymentIdB = reader.IsDBNull(reader.GetOrdinal("paymentIdB")) ? 0 : reader.GetInt32(reader.GetOrdinal("paymentIdB")),
                            TotalAmountPaid = reader.IsDBNull(reader.GetOrdinal("TotalAmountPaid")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("TotalAmountPaid")),
                            BuyerWallet = reader.IsDBNull(reader.GetOrdinal("BuyerWallet")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("BuyerWallet")),
                            UserId = reader.IsDBNull(reader.GetOrdinal("UserId")) ? 0 : reader.GetInt32(reader.GetOrdinal("UserId")),
                            AdvertiseId = reader.IsDBNull(reader.GetOrdinal("AdvertiseId")) ? 0 : reader.GetInt32(reader.GetOrdinal("AdvertiseId")),
                            TransactionTimeP = reader.IsDBNull(reader.GetOrdinal("TransactionTimeP")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("TransactionTimeP"))

                        };
                        transactions.Add(transaction);
                    }
                }
            }


            return transactions;
        }


        //user details
        public List<UsersModel> FetchRecentTransactions2(int userId)
        {
            connection();
            List<UsersModel> users = new List<UsersModel>();


            con.Open();

            string query = "SELECT * FROM Users WHERE userId = @userId";

            using (SqlCommand recentTransaction2 = new SqlCommand(query, con))
            {
                recentTransaction2.Parameters.AddWithValue("@userId", userId);

                using (SqlDataReader reader = recentTransaction2.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UsersModel transaction = new UsersModel
                        {
                            firstname = reader.IsDBNull(reader.GetOrdinal("firstname")) ? "0" : reader.GetString(reader.GetOrdinal("firstname")),
                            userEmail = reader.IsDBNull(reader.GetOrdinal("userEmail")) ? "0" : reader.GetString(reader.GetOrdinal("userEmail"))

                        };
                        users.Add(transaction);
                    }
                }
            }


            return users;
        }



        // invoice product price and seller detailss
        public List<MyAdvertiseModel> FetchRecentTransactions3(int advertiseId)
        {
            connection();

            List<MyAdvertiseModel> advertise = new List<MyAdvertiseModel>();

            con.Open();

            string query = "SELECT * FROM tbl_MyAdvertise WHERE advertiseId = @advertiseId;";

            using (SqlCommand recentTransaction3 = new SqlCommand(query, con))
            {
                recentTransaction3.Parameters.AddWithValue("@advertiseId", advertiseId);

                using (SqlDataReader reader = recentTransaction3.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        MyAdvertiseModel transaction3 = new MyAdvertiseModel
                        {
                            advertiseTitle = reader.IsDBNull(reader.GetOrdinal("advertiseTitle")) ? "0" : reader.GetString(reader.GetOrdinal("advertiseTitle")),
                            advertiseDescription = reader.IsDBNull(reader.GetOrdinal("advertiseDescription")) ? "0" : reader.GetString(reader.GetOrdinal("advertiseDescription"))
                        };
                        advertise.Add(transaction3);
                    }
                }
            }

            return advertise;
        }

        public List<UsersModel> FetchRecentTransactions4(int advertiseId)
        {
            connection();

            List<UsersModel> advertise2 = new List<UsersModel>();

            con.Open();

            string query = "SELECT * FROM Users u INNER JOIN tbl_MyAdvertise ma ON u.userId = ma.userId WHERE ma.advertiseId = @advertiseId;";

            using (SqlCommand recentTransaction4 = new SqlCommand(query, con))
            {
                recentTransaction4.Parameters.AddWithValue("@advertiseId", advertiseId);

                using (SqlDataReader reader = recentTransaction4.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UsersModel transaction4 = new UsersModel
                        {
                            firstname = reader.IsDBNull(reader.GetOrdinal("firstname")) ? "0" : reader.GetString(reader.GetOrdinal("firstname")),
                            userEmail = reader.IsDBNull(reader.GetOrdinal("userEmail")) ? "0" : reader.GetString(reader.GetOrdinal("userEmail")),
                            MobileNo = reader.IsDBNull(reader.GetOrdinal("MobileNo")) ? "0" : reader.GetString(reader.GetOrdinal("MobileNo")),
                        };
                        advertise2.Add(transaction4);
                    }
                }
            }

            return advertise2;
        }








        public List<PaymentdetailsSellerModel> FetchRecentTransactionsSeller(int userId)
        {
            connection();
            List<PaymentdetailsSellerModel> transactions = new List<PaymentdetailsSellerModel>();

            con.Open();

            string query = "SELECT * FROM PaymentdetailsSeller WHERE userId = @userId";

            using (SqlCommand recentTransaction = new SqlCommand(query, con))
            {
                recentTransaction.Parameters.AddWithValue("@userId", userId);

                using (SqlDataReader reader = recentTransaction.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PaymentdetailsSellerModel transaction = new PaymentdetailsSellerModel
                        {
                            PaymentIds = reader.IsDBNull(reader.GetOrdinal("PaymentIds")) ? 0 : reader.GetInt32(reader.GetOrdinal("PaymentIds")),
                            ReceivedAmount = reader.IsDBNull(reader.GetOrdinal("ReceivedAmount")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("ReceivedAmount")),
                            SellerWallet = reader.IsDBNull(reader.GetOrdinal("SellerWallet")) ? 0 : reader.GetInt32(reader.GetOrdinal("SellerWallet")),

                            UserId = reader.IsDBNull(reader.GetOrdinal("UserId")) ? 0 : reader.GetInt32(reader.GetOrdinal("UserId")),
                            advertiseId = reader.IsDBNull(reader.GetOrdinal("advertiseId")) ? 0 : reader.GetInt32(reader.GetOrdinal("advertiseId")),
                            TransactionTimeS = reader.IsDBNull(reader.GetOrdinal("TransactionTimeS")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("TransactionTimeS"))

                        };
                        transactions.Add(transaction);
                    }
                }
            }

            return transactions;
        }


        public bool TransferSellerWalletAmountToBuyer(int userId)
        {
            connection();
            int walletPriceStoreforTransfer = 0;

            using (SqlCommand fetchdatafromPaymentDetailsSeller = new SqlCommand("SELECT SellerWallet FROM PaymentdetailsSeller WHERE userId = @userId", con))
            {
                fetchdatafromPaymentDetailsSeller.Parameters.AddWithValue("@userId", userId);
                con.Open(); // Open the connection before executing the query
                using (SqlDataReader reader = fetchdatafromPaymentDetailsSeller.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        walletPriceStoreforTransfer = reader.GetInt32(0);
                    }
                }
            }

            bool res = AddingMoneyToBuyerWallet(userId, walletPriceStoreforTransfer);

            if (res == true)
            {
                using (SqlCommand minusWalletAmountOfPaymentdetailsSeller = new SqlCommand("UPDATE PaymentdetailsSeller SET SellerWallet = SellerWallet - @walletPriceStoreforTransfer WHERE userId = @userId", con))
                {
                    minusWalletAmountOfPaymentdetailsSeller.Parameters.AddWithValue("@walletPriceStoreforTransfer", walletPriceStoreforTransfer);
                    minusWalletAmountOfPaymentdetailsSeller.Parameters.AddWithValue("@userId", userId);
                    con.Open();
                    int rowsAffected = minusWalletAmountOfPaymentdetailsSeller.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {

                        con.Close();
                        return true;
                    }
                }
            }
            con.Close(); // Close the database connection
            return false;
        }

    }
}
