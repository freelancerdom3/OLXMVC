using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLX.Models.Admin
{
    public class UserList
    {
        public int UserId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string userEmail { get; set; }
        public string Password { get; set; }
        public string MobileNo { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public DateTime createdon { get; set; }
        public DateTime updatedon { get; set; }
    }
}
