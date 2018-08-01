using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.BLL;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBPay.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace IQBConsole
{
    public class PPBatchJob
    {

        int _OrderDiffMin = 60;

        public PPBatchJob()
        {
            
          
        }

        /// <summary>
        /// 批量更新代理费率
        /// </summary>
        public void UpdateAgentRate()
        {
            using (IQBContent db = new IQBContent())
            {
                var list = db.DBUserInfo.Where(a => a.UserRole == UserRole.DiamondAgent).ToList();
                foreach(EUserInfo u in list)
                {
                    /*总代*/
                    //花呗
                    EQRUser qr = db.DBQRUser.Where(a => a.OpenId == u.OpenId && a.QRType == QRReceiveType.Small).FirstOrDefault();
                    qr.Rate = (float)1.2;
                    qr.MarketRate = 2;

                    //信用卡
                    qr = db.DBQRUser.Where(a => a.OpenId == u.OpenId && a.QRType == QRReceiveType.CreditCard).FirstOrDefault();
                    qr.Rate = (float)0.35;
                    qr.MarketRate = 1;
                }
                list = db.DBUserInfo.Where(a => a.UserRole == UserRole.Agent && a.UserStatus== UserStatus.PPUser).ToList();
                foreach (EUserInfo u in list)
                {
                    /*普通代理*/
                    //花呗
                    EQRUser qr = db.DBQRUser.Where(a => a.OpenId == u.OpenId && a.QRType == QRReceiveType.Small).FirstOrDefault();
                    qr.Rate = (float)0.95;
                    qr.MarketRate = 2;

                    //信用卡
                    qr = db.DBQRUser.Where(a => a.OpenId == u.OpenId && a.QRType == QRReceiveType.CreditCard).FirstOrDefault();
                    qr.Rate = (float)0.25;
                    qr.MarketRate = 1;

                    Console.WriteLine(string.Format("调整代理：{0}", u.Name));
                }
                db.SaveChanges();
            }
        }

        public void ChangeReceiveQR()
        {
            using (IQBContent db = new IQBContent())
            {
                //var list = db.DBQRUser.Where(a => a.QRType == QRReceiveType.Small).ToList();
                var list = db.DBQRUser.Where(a => a.QRType == QRReceiveType.Small && a.OpenId== "o3nwE0jrONff65oS-_W96ErKcaa0").ToList();
                foreach (EQRUser qr in list)
                {
                    string openId = qr.OpenId;
                    EQRUser updateQr = QRManager.CreateUserUrlById(qr);
                   
                }
                db.SaveChanges();
            }
        }

        public void DeleteWaitingOrder()
        {
            using (IQBContent db = new IQBContent())
            {
                string sql = @"select count(*) as delCount from O2OOrder as o
where datediff(MINUTE,o.CreateDateTime,getdate()) >{0} and o.O2OOrderStatus between 0 and 40";
                sql = string.Format(sql, _OrderDiffMin);
                int delNum = db.Database.SqlQuery<int>(sql).FirstOrDefault();
                if(delNum>0)
                {
                    Console.WriteLine(string.Format("【{0}】：开始删除等待上传且超时{1}分钟的订单",DateTime.Now.ToString(), _OrderDiffMin));
                    sql = string.Format(@"delete from O2OOrder where datediff(MINUTE,O2OOrder.CreateDateTime,getdate()) >{0} and O2OOrder.O2OOrderStatus between 0 and 40", _OrderDiffMin);
                    int i = db.Database.ExecuteSqlCommand(sql);
                    Console.WriteLine(string.Format("【{0}】：完成删除{1}", DateTime.Now.ToString(), i));
                }
                else
                {
                    Console.WriteLine(string.Format("【{0}】：暂时没有数据删除", DateTime.Now.ToString()));
                }
            }
        }
        public void Run(int OrderDiffMin)
        {
            _OrderDiffMin = OrderDiffMin;
            DeleteWaitingOrder();
        }

     

       

      
    }
}
