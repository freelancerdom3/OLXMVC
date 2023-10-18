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
    public class RepositoryInvoice
    {
        private SqlConnection con;
        public void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            con = new SqlConnection(constr);

        }

        public List<BuyerTransactionHistoryModel> FetchHistoryofBuyer(int userId, DateTime fromDate, DateTime toDate)
        {
            connection();
            List<BuyerTransactionHistoryModel> transactions = new List<BuyerTransactionHistoryModel>();
            con.Open();
            string query = "SELECT * FROM BuyerTransactionHistory WHERE UserId = @userId AND TransactionTime BETWEEN @fromDate AND @toDate";
            using (SqlCommand transactionhis = new SqlCommand(query, con))
            {
                transactionhis.Parameters.AddWithValue("@userId", userId);
                transactionhis.Parameters.AddWithValue("@fromDate", fromDate);
                transactionhis.Parameters.AddWithValue("@toDate", toDate);

                using (SqlDataReader reader = transactionhis.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        BuyerTransactionHistoryModel buyerTransaction = new BuyerTransactionHistoryModel
                        {
                            TransactionId = reader.IsDBNull(reader.GetOrdinal("transactionId")) ? 0 : reader.GetInt32(reader.GetOrdinal("transactionId")),
                            TotalAmountPaid = reader.IsDBNull(reader.GetOrdinal("totalamountPaid")) ? 0 : reader.GetDecimal(reader.GetOrdinal("totalamountPaid")),
                            BuyerWallet = reader.IsDBNull(reader.GetOrdinal("Buyerwallet")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Buyerwallet")),
                            UserId = reader.IsDBNull(reader.GetOrdinal("userId")) ? 0 : reader.GetInt32(reader.GetOrdinal("userId")),
                            advertiseId = reader.IsDBNull(reader.GetOrdinal("advertiseId")) ? 0 : reader.GetInt32(reader.GetOrdinal("advertiseId")),
                            TransactionTime = reader.IsDBNull(reader.GetOrdinal("TransactionTime")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("TransactionTime"))
                        };



                        transactions.Add(buyerTransaction);
                    }
                }


            }
            con.Close();
            return transactions;
        }

        public List<SellerTransactionHistoryModel> FetchHistoryofSeller(int userId, DateTime fromDate, DateTime toDate)
        {
            connection();
            List<SellerTransactionHistoryModel> transactions = new List<SellerTransactionHistoryModel>();
            con.Open();
            string query = "SELECT * FROM SellerTransactionHistory WHERE UserId = @userId AND TransactionTime BETWEEN @fromDate AND @toDate";
            using (SqlCommand transactionhis = new SqlCommand(query, con))
            {
                transactionhis.Parameters.AddWithValue("@userId", userId);
                transactionhis.Parameters.AddWithValue("@fromDate", fromDate);
                transactionhis.Parameters.AddWithValue("@toDate", toDate);

                using (SqlDataReader reader = transactionhis.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SellerTransactionHistoryModel sellerTransaction = new SellerTransactionHistoryModel
                        {
                            TransactionId = reader.IsDBNull(reader.GetOrdinal("transactionId")) ? 0 : reader.GetInt32(reader.GetOrdinal("transactionId")),
                            TotalAmountReceived = reader.IsDBNull(reader.GetOrdinal("totalamountReceived")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("totalamountReceived")),
                            SellerWallet = reader.IsDBNull(reader.GetOrdinal("Sellerwallet")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Sellerwallet")),
                            UserId = reader.IsDBNull(reader.GetOrdinal("userId")) ? 0 : reader.GetInt32(reader.GetOrdinal("userId")),
                            advertiseId = reader.IsDBNull(reader.GetOrdinal("advertiseId")) ? 0 : reader.GetInt32(reader.GetOrdinal("advertiseId")),
                            TransactionTime = reader.IsDBNull(reader.GetOrdinal("TransactionTime")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("TransactionTime"))
                        };

                        transactions.Add(sellerTransaction);
                    }
                }
            }

            con.Close();
            return transactions;
        }

    }
}
