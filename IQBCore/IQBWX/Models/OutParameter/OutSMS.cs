using IQBCore.IQBPay.BaseEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBWX.Models.OutParameter
{
    public class OutSMS
    {

        public long SmsID { get; set; }
        public SMSVerifyStatus SMSVerifyStatus { get; set; }

        public int RemainSec { get; set; }

      

    }
}
