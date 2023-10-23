using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLX.Models.User
{
   public class showChatModel
    {
        public string firstName { get; set; }
        public int AdvertiseId { get; set; }
        public bool BuyOrSellId { get; set; }
        public int userId { get; set; }
        public string Chat { get; set; }

        public List<showChatModel> model { get; set; }
    }
}
