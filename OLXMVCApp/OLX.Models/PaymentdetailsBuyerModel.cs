using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLX.Models
{
    public class PaymentdetailsBuyerModel
    {
        public int PaymentIdB { get; set; }
        public decimal? TotalAmountPaid { get; set; }

        public decimal? BuyerWallet { get; set; }
        public int AdvertiseId { get; set; }
        public int UserId { get; set; }
        public DateTime TransactionTimeP { get; set; }
    }

    public class PaymentdetailsSellerModel
    {
        public int PaymentIds { get; set; }

        public decimal? ReceivedAmount { get; set; }

        public int SellerWallet { get; set; }
        public int TransactionIds { get; set; }
        public int UserId { get; set; }
        public int advertiseId { get; set; }
        public DateTime TransactionTimeS { get; set; }
    }
    public class BuyerTransactionHistoryModel
    {

        public int TransactionId { get; set; }
        public decimal? TotalAmountPaid { get; set; }
        public decimal? BuyerWallet { get; set; }
        public int UserId { get; set; }
        public int advertiseId { get; set; }
        public DateTime TransactionTime { get; set; }
    }

    public class SellerTransactionHistoryModel
    {
        public int TransactionId { get; set; }
        public decimal? TotalAmountReceived { get; set; }
        public decimal? SellerWallet { get; set; }
        public int UserId { get; set; }
        public int advertiseId { get; set; }
        public DateTime TransactionTime { get; set; }
    }
}
