using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQBWX.Models.JsonData
{
    public class jsonApplyMember
    {
        public string userName { get; set; }
        public string userPhone { get; set; }
        public int WXNum { get; set; }
        public string WXRange { get; set; }
        public int Province { get; set; }
        public string ProvinceValue { get; set; }
        public int seltc { get; set; }

        public int UserId { get; set; }
        public string userVerifyCode { get; set; }
    }
}