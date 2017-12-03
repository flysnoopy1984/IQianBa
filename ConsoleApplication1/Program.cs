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

                EQRUser qrUser = new EQRUser();
                using (AliPayContent db = new AliPayContent())
                {
                    qrUser = db.DBQRUser.Where(a => a.ID == 12).FirstOrDefault();
                    QRManager.CreateUserUrlById(qrUser);
                }
                   

                Console.Read();

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
