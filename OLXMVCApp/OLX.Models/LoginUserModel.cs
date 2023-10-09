using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLX.Models
{
    public class LoginUserModel
    {
        public int LoginUserId { get; set; }
        public int LoginOtp { get; set; }

        public DateTime ExpirationLoginTime { get; set; }
        public int userIdByLoginUser { get; set; }
    }
}
