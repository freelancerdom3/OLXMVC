using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLX.Models.User
{
    public class ChatMappingModel
    {
        public int Mapid { get; set; }

        public int Buyerid { get; set; }

        public int Sellerid { get; set; }

        public int advertiseid { get; set; }

        public DateTime CreatedOn { get; set; }
    }

    public class ChatsModel
    {
        public int Chatsec { get; set; }
        public int Mapid { get; set; }

        public bool BuyOrSellId { get; set; }
        public string Chat { get; set; }

        public DateTime CreatedOn { get; set; }
    }

    public class ChatMddual : users
    {

        public int ChatId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int BuyOrSellId { get; set; }
        public string Chat1 { get; set; }

        public List<ChatMddual> model { get; set; }

        //public string fristname { get; set; }
    }

    public class users
    {
        public int userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string userEmail { get; set; }
        public string Password { get; set; }
        public string MobileNo { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string City { get; set; }


        }
    }

