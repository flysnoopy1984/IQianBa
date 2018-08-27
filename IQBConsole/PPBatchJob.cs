using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.BLL;
using IQBCore.IQBPay.Models.Order;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBPay.Models.Simple;
using IQBCore.IQBPay.Models.Sys;
using IQBCore.IQBPay.Models.User;
using IQBCore.IQBWX.Models.WX.Template.InviteCode;
using IQBCore.IQBWX.Models.WX.Template.PaySuccessTellAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using WxPayAPI;

namespace IQBConsole
{
    public class PPBatchJob
    {

        int _OrderDiffMin = 60;
        int _UserNum;
        static EGlobalConfig _config;
        List<string> block = new List<string>();

        public static EGlobalConfig Config
        {
            get
            {
                if(_config == null)
                {
                    using (IQBContent db = new IQBContent())
                    {
                        _config = db.DBGlobalConfig.FirstOrDefault();
                    }
                }
                return _config;
            }
        }

        public PPBatchJob()
        {

           
            block.Add("o3nwE0oE32A4jA-bGu9t59Ob4Qzc");
            block.Add("o3nwE0i_Z9mpbZ22KdOTWeALXaus");
        }

        private void testchild(IQBContent db, string parentOpenId)
        {
            var list = db.DBUserInfo.Where(a => a.UserRole == UserRole.Agent &&
                                  a.UserStatus == UserStatus.PPUser &&
                                  a.parentOpenId == parentOpenId).ToList();

            foreach (EUserInfo u in list)
            {
                parentOpenId = u.OpenId;
                testchild(db, parentOpenId);

                _UserNum++;
                Console.WriteLine(string.Format("【{0}】{1}", _UserNum, parentOpenId));
            }

       }
        public void Test()
        {
            int i = 0;
            using (IQBContent db = new IQBContent())
            {
                var list = db.DBUserInfo.Where(a => a.UserRole == UserRole.DiamondAgent || a.UserRole == UserRole.Administrator
                && a.UserStatus == UserStatus.PPUser).ToList();
                foreach(var u in list)
                {
                    string parentOpenId = u.OpenId;
                   
                    _UserNum++;
                    Console.WriteLine(string.Format("【{0}】{1}", _UserNum,parentOpenId));

                    testchild(db, parentOpenId);
                }
            }
        }
       
        /// <summary>
        /// 费率变动
        /// </summary>
        /// <param name="list"></param>
        /// <param name="openId"></param>
        public void WXNTAgentFeeRate(List<SFee> list,string openId)
        {
            var accessToken = JsApiPay.GetAccessToken();
            if (block.IndexOf(openId)>-1) return;

            PPInviteCodeNT notice = new PPInviteCodeNT(accessToken, list, openId);
            notice.Push();
        }

        private void AnalyUser(IQBContent db,string parentOpenId,int level)
        {
            var list = db.DBUserInfo.Where(a => a.UserRole == UserRole.Agent && 
                                    a.UserStatus == UserStatus.PPUser && 
                                    a.parentOpenId== parentOpenId).ToList();
            level++;
            foreach (EUserInfo u in list)
            {
              //  if (u.OpenId == "o3nwE0oE32A4jA-bGu9t59Ob4Qzc") continue;

                List<SFee> feeList = new List<SFee>();
                SFee sfee = null;
                /*普通代理*/
                //花呗 1
                EQRUser qr = db.DBQRUser.Where(a => a.OpenId == u.OpenId && a.QRType == QRReceiveType.Small).FirstOrDefault();
                if (qr != null)
                {
                    var fee = 0.75+ Config.ChildFixRate* (level - 1);

                    sfee = new SFee();
                    sfee.OrigFeeRate = Convert.ToSingle((qr.MarketRate - qr.Rate).ToString("0.00"));
                    sfee.QRType = qr.QRType;

                    qr.Rate = Convert.ToSingle((qr.MarketRate - fee).ToString("0.00"));

                    sfee.AdjustedFeeRate = Convert.ToSingle((qr.MarketRate - qr.Rate).ToString("0.00"));
                    feeList.Add(sfee);
                }


                //信用卡 0.63/0.75
                qr = db.DBQRUser.Where(a => a.OpenId == u.OpenId && a.QRType == QRReceiveType.CreditCard).FirstOrDefault();
                if (qr != null)
                {
                    var fee = 0.63+Config.CCChildFixRate*(level-1);

                    sfee = new SFee();
                    sfee.OrigFeeRate = Convert.ToSingle((qr.MarketRate - qr.Rate).ToString("0.00"));
                    sfee.QRType = qr.QRType;

                    qr.Rate = Convert.ToSingle((qr.MarketRate - fee).ToString("0.00"));

                    sfee.AdjustedFeeRate = Convert.ToSingle((qr.MarketRate - qr.Rate).ToString("0.00"));
                    feeList.Add(sfee);
                }


                WXNTAgentFeeRate(feeList, u.OpenId);

               
                AnalyUser(db, u.OpenId, level);

                _UserNum++;

                Console.WriteLine(string.Format("{1}调整代理：{0}", u.Name, _UserNum));
            }
        }
        /// <summary>
        /// 批量更新代理费率
        /// </summary>
        public void UpdateAgentRate()
        {
          
            using (IQBContent db = new IQBContent())
            {
                var list = db.DBUserInfo.Where(a => a.UserRole == UserRole.DiamondAgent || a.UserRole == UserRole.Administrator
              && a.UserStatus == UserStatus.PPUser).ToList();

                foreach (EUserInfo u in list)
                {
                    List<SFee> feeList = new List<SFee>();
                    SFee sfee = null;
                    /*总代*/
                    //花呗(0.75)/0.8
                    EQRUser qr = db.DBQRUser.Where(a => a.OpenId == u.OpenId && a.QRType == QRReceiveType.Small).FirstOrDefault();
                    var fee = 0.75;

                    sfee = new SFee();
                    sfee.OrigFeeRate = Convert.ToSingle((qr.MarketRate - qr.Rate).ToString("0.00"));
                    sfee.QRType = qr.QRType;

                    qr.Rate = Convert.ToSingle((qr.MarketRate - fee).ToString("0.00"));
                    sfee.AdjustedFeeRate = Convert.ToSingle((qr.MarketRate - qr.Rate).ToString("0.00"));
                    feeList.Add(sfee);

                    //信用卡0.63/0.65
                    qr = db.DBQRUser.Where(a => a.OpenId == u.OpenId && a.QRType == QRReceiveType.CreditCard).FirstOrDefault();
                    fee = 0.63;

                    sfee = new SFee();
                    sfee.OrigFeeRate = Convert.ToSingle((qr.MarketRate - qr.Rate).ToString("0.00"));
                    sfee.QRType = qr.QRType;

                    qr.Rate = Convert.ToSingle((qr.MarketRate - fee).ToString("0.00"));

                    sfee.AdjustedFeeRate = Convert.ToSingle((qr.MarketRate - qr.Rate).ToString("0.00"));
                    feeList.Add(sfee);

                    Console.WriteLine(string.Format("总代：{0}", u.Name));

                 //   WXNTAgentFeeRate(feeList, u.OpenId);

                    AnalyUser(db, u.OpenId,1);
                }


                db.SaveChanges();
                Console.WriteLine(string.Format("调整代理Done"));
            }
        }

        public void UpdateAgentForSpecial()
        {
            using (IQBContent db = new IQBContent())
            {
                var list = db.DBUserInfo.Where(a => a.parentOpenId == "o3nwE0vaY07Rr2RJRgb9JRKci_KI").ToList();
                foreach (EUserInfo u in list)
                {
                    List<SFee> feeList = new List<SFee>();
                    SFee sfee = null;
                    /*总代*/
                    //花呗(0.75)/0.8
                    EQRUser qr = db.DBQRUser.Where(a => a.OpenId == u.OpenId && a.QRType == QRReceiveType.Small).FirstOrDefault();
                    var fee = 0.75;

                    sfee = new SFee();
                    sfee.OrigFeeRate = Convert.ToSingle((qr.MarketRate - qr.Rate).ToString("0.00"));
                    sfee.QRType = qr.QRType;

                    qr.Rate = Convert.ToSingle((qr.MarketRate - fee).ToString("0.00"));
                    sfee.AdjustedFeeRate = Convert.ToSingle((qr.MarketRate - qr.Rate).ToString("0.00"));
                    feeList.Add(sfee);

                    //信用卡0.63/0.65
                    qr = db.DBQRUser.Where(a => a.OpenId == u.OpenId && a.QRType == QRReceiveType.CreditCard).FirstOrDefault();
                    if (u.OpenId == "o3nwE0snE94bXggy2K8ZfHuyypVs" || u.OpenId == "o3nwE0og1j5cLMTVdg0XSjtJ88E8")
                        fee = 0.63;
                    else
                        fee = 0.65;

                    sfee = new SFee();
                    sfee.OrigFeeRate = Convert.ToSingle((qr.MarketRate - qr.Rate).ToString("0.00"));
                    sfee.QRType = qr.QRType;

                    qr.Rate = Convert.ToSingle((qr.MarketRate - fee).ToString("0.00"));

                    sfee.AdjustedFeeRate = Convert.ToSingle((qr.MarketRate - qr.Rate).ToString("0.00"));
                    feeList.Add(sfee);

                    Console.WriteLine(string.Format("代理：{0}", u.Name));

                   // WXNTAgentFeeRate(feeList, u.OpenId);
                }
                db.SaveChanges();
                Console.WriteLine(string.Format("调整代理Done"));
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


        public void WXNTPayTellAdmin(string openId)
        {
            var accessToken = JsApiPay.GetAccessToken();
            EOrderInfo order = null;
            using (IQBContent db = new IQBContent())
            {
                order = db.DBOrder.Where(a => a.OrderType == OrderType.WX).FirstOrDefault();
            }
            PaySuccessTellAdminNT notice = new PaySuccessTellAdminNT(accessToken, openId, order);
            notice.Push();
        }


        public void CreateQR(string url)
        {

        }



    }
}
