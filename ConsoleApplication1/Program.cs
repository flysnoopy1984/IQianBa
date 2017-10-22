using CatchWebContent;
using IQBCore.Common.Constant;
using IQBCore.IQBPay.BLL;
using IQBCore.IQBPay.Models.InParameter;
using IQBWX.Common;
using IQBWX.DataBase;
using IQBWX.Models.Order;
using IQBWX.Models.Results;
using IQBWX.Models.Transcation;
using IQBWX.Models.User;
using IQBWX.Models.WX;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                //SMSManager sms = new SMSManager();
                //InSMS inSMS = new InSMS();
                //inSMS.Init();
                //inSMS.PhoneNumber = "13482710060";
                //inSMS.Parameters = "212341,IQBSO320201019,http://b.iqianba.cn/";

                //sms.PostSMS(inSMS);


            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            Console.Read();
        }

      
    }
}
