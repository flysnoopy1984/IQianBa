using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQBWX.Models.Results
{
    public class RMemberQR
    {
        public string HeaderImg { get; set; }
        public string NickName { get; set; }
        public string BKImg { get; set; }

        public string openId { get; set; }
        public int UserId { get; set; }
    }
}