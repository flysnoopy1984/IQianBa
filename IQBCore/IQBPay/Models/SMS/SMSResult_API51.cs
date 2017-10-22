using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.SMS
{
    public class SMSResult_API51
    {
        public string result { get; set; }
        public string errmsg { get; set; }
        public string ext { get; set; }

        public string sid { get; set; }

        public string fee { get; set; }
    }
}
