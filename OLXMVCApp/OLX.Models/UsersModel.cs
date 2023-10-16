using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLX.Models
{
    public class UsersModel
    {
        public int userId { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string userEmail { get; set; }
        public string Password { get; set; }

        public string MobileNo { get; set; }
        public string Gender { get; set; }
        public DateTime Dob { get; set; }
        public string Roles { get; set; }
    }
}
