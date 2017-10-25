using CatchWebContent;
using IQBCore.Common.Constant;
using IQBCore.IQBPay.BLL;
using IQBCore.IQBPay.Models.AccountPayment;
using IQBCore.IQBPay.Models.InParameter;
using IQBCore.IQBPay.Models.Order;
using IQBCore.IQBPay.Models.QR;
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

                using (AliPayContent db = new AliPayContent())
                {
                    AliPayManager pay = new AliPayManager();
                    EOrderInfo order = db.DBOrder.FirstOrDefault();
                    EQRUser qrUser = db.DBQRUser.Where(s => s.ID ==3).FirstOrDefault();
                    for(int i=1;i<44;i++)
                    { 
                        EAgentCommission obj = pay.InitAgentCommission(order, qrUser);
                        if (i % 2 == 0)
                            obj.AgentCommissionStatus = IQBCore.IQBPay.BaseEnum.AgentCommissionStatus.Paid;
                        else
                            obj.AgentCommissionStatus = IQBCore.IQBPay.BaseEnum.AgentCommissionStatus.Open;
                        db.DBAgentCommission.Add(obj);
                  
                    }
                    db.SaveChanges();
                }


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
