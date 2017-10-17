using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.BLL
{
    public class SMSManager
    {
        private const String host = "http://smsapi.api51.cn";
        private const String path = "/single_sms/";
        private const String method = "POST";
        private const String appcode = "d595292b73f8415691cd09b90ca04d17";

        public Boolean PostSMS()
        {
            try
            {
                return true;
            }
            catch(Exception ex)
            {

            }
            return false;
        }

        public Boolean PostAliSMS()
        {
            return true;
        }
    }
}
