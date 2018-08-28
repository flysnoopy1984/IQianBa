using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.BLL;
using IQBCore.IQBPay.Models.Store;
using IQBCore.IQBPay.Models.Sys;
using IQBPay.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBConsole
{
    public class PayTest
    {
        /// <summary>
        ///  EZS_ydj1125
        /// </summary>
        private string AppId = "2018082261169057";

        public PayTest()
        {

        }
        public void Pay()
        {
            AliPayManager payMag = new AliPayManager();
            AliPayResult result;
            using (AliPayContent db = new AliPayContent())
            {
                EAliPayApplication app =  db.DBAliPayApp.Where(a => a.AppId == AppId).FirstOrDefault();
                EStoreInfo store = db.DBStoreInfo.Where(a => a.ID == 119).FirstOrDefault();
                string code = payMag.PayTest(app, store, out result);

                Console.WriteLine("Result:" + code);

            }
        }
    }
}
