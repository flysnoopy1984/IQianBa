using CatchWebContent;
using IQBCore.Common.Constant;
using IQBCore.Common.Helper;
using IQBCore.IQBPay.BLL;
using IQBCore.IQBPay.Models.AccountPayment;
using IQBCore.IQBPay.Models.InParameter;
using IQBCore.IQBPay.Models.Order;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBWX.BaseEnum;
using IQBPay.Core;
using IQBPay.DataBase;
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
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        public static string DoSHA1(string content, Encoding encode)
        {
            try
            {
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                byte[] bytes_in = encode.GetBytes(content);
                byte[] bytes_out = sha1.ComputeHash(bytes_in);
                sha1.Dispose();
                string result = BitConverter.ToString(bytes_out);
                result = result.Replace("-", "");
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("SHA1加密出错：" + ex.Message);
            }
        }

        static void Main(string[] args)
        {
            try
            {
                DateTime dt = DateTime.Now.AddMinutes(-10);
                //TimeSpan ts1 = new TimeSpan(dt.Ticks);
                //TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
                //TimeSpan ts3 = ts1.Subtract(ts2).Duration();

                ////你想转的格式
                //string s = ts3.TotalSeconds.ToString();
                //Console.WriteLine(s);
                //Console.Read();

                //      // float RateAmount = (float)Math.Round(5.0 * (5 / 100), 2, MidpointRounding.ToEven);
                      bool ok = DateHelper.IsOverTime(dt, 10);

                //  QRManager.CreateUserUrlById(qrUser);

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
