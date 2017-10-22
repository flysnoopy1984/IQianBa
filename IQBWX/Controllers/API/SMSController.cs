using IQBCore.Common.Helper;
using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.Models.SMS;
using IQBCore.IQBWX.BaseEnum;
using IQBCore.IQBWX.Models.OutParameter;
using IQBWX.Common;
using IQBWX.DataBase;
using IQBWX.DataBase.IQBPay;
using IQBWX.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IQBWX.Controllers.API
{
    public class SMSController : ApiController
    {
       

        [HttpGet]
        public string NewMemberSMSVerify(int userId)
        {
            try { 
                using (UserContent db = new UserContent())
                {
                    EUserSMSVerify uv = new EUserSMSVerify()
                    {
                        SendDateTime = DateTime.Now,
                        VerifyCode = StringHelper.GetRnd(6, true, true, false, false, ""),
                        UserId = userId,
                        VerifyStatus = UserSMSStatus.Sent,
                        SMSEvent = Convert.ToString(SMSEvent.NewMember)
                    };
                    this.DoSMS();
                    db.InsertEUserSMSVerify(uv);
                }
            }
            catch(Exception ex)
            {
                return "error:" + ex.Message;
            }
            return "OK";
        }

        /// <summary>
        /// 5分钟有效
        /// </summary>
        /// <param name="mobilePhone"></param>
        /// <param name="SMSId"></param>
        public OutSMS IQBPay_ConfirmVerifyCode(string SMSId,string Code)
        {
           
            OutSMS OutSMS = new OutSMS();
            OutSMS.SMSVerifyStatus = SMSVerifyStatus.UnKnown;
            long sId = Convert.ToInt64(SMSId);

            using (AliPayContent db = new AliPayContent())
            {
                var sms = db.DBSMSBuyerOrder.Where(s => s.ID == sId).FirstOrDefault();
                if(sms == null)
                {
                    OutSMS.SMSVerifyStatus = SMSVerifyStatus.UnKnown;
                }
                else
                {
                    TimeSpan nowtimespan = new TimeSpan(DateTime.Now.Ticks);
                    TimeSpan endtimespan = new TimeSpan(sms.SendDateTime.Ticks);
                    TimeSpan timespan = nowtimespan.Subtract(endtimespan).Duration();
                    int CurSec = Convert.ToInt32(timespan.TotalSeconds);
                    if (CurSec > 600)
                    {
                        OutSMS.SMSVerifyStatus = SMSVerifyStatus.Expired;
                    }
                    else
                    {
                        if(sms.VerifyCode == Code)
                            OutSMS.SMSVerifyStatus = SMSVerifyStatus.Success;
                        else
                            OutSMS.SMSVerifyStatus = SMSVerifyStatus.Failure;
                    }
                    db.SaveChanges();

                }


                 
            }
            return OutSMS;
        }

       
        public OutSMS IQBPay_GetVerifyingSec(string mobilePhone, int IntervalSec)
        {
            OutSMS OutSMS = new OutSMS();
            try
            {
                using (AliPayContent db = new AliPayContent())
                {
                    var sms = db.DBSMSBuyerOrder.Where(s => s.MobilePhone == mobilePhone
                    && s.SMSEvent == SMSEvent.IQB_PayOrder
                    && s.SMSVerifyStatus == SMSVerifyStatus.Sent).OrderByDescending(s => s.SendDateTime).FirstOrDefault();

                    OutSMS.RemainSec = -1;

                    if (sms != null)
                    {
                        OutSMS.SmsID = sms.ID;
                        TimeSpan nowtimespan = new TimeSpan(DateTime.Now.Ticks);
                        TimeSpan endtimespan = new TimeSpan(sms.SendDateTime.Ticks);
                        TimeSpan timespan = nowtimespan.Subtract(endtimespan).Duration();
                        int CurSec = Convert.ToInt32(timespan.TotalSeconds);
                        if (CurSec > IntervalSec)
                        {
                            return OutSMS;
                        }
                        else
                        {
                            OutSMS.RemainSec = CurSec;
                            return OutSMS;
                        }
                          
                    }
                    return OutSMS;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
          
        }

        [HttpGet]
        public OutSMS SentSMS_IQBPay_BuyerOrder(string mobilePhone, int IntervalSec)
        {
            OutSMS OutSMS = new OutSMS();
            try
            {
                OutSMS = IQBPay_GetVerifyingSec(mobilePhone, IntervalSec);

                if (OutSMS.RemainSec == -1)
                {
                    string VerifyCode = StringHelper.GenerateVerifyCode();
                    if (!this.DoSMS(VerifyCode))
                    {
                        OutSMS.SMSVerifyStatus = SMSVerifyStatus.SentFailure;
                        return OutSMS;
                    }

                    using (AliPayContent db = new AliPayContent())
                    {
                        ESMSVerification sms = new ESMSVerification()
                        {
                            VerifyCode = VerifyCode,
                            MobilePhone = mobilePhone,
                            SendDateTime = DateTime.Now,
                            SMSVerifyStatus = SMSVerifyStatus.Sent,
                            SMSEvent = SMSEvent.IQB_PayOrder,
                        };
                        db.DBSMSBuyerOrder.Add(sms);
                        db.SaveChanges();
                        OutSMS.SmsID = sms.ID;
                    }
                    OutSMS.SMSVerifyStatus = SMSVerifyStatus.Sent;
                    OutSMS.RemainSec = -1;

                }
                else
                {
                    OutSMS.RemainSec = IntervalSec - OutSMS.RemainSec;
                    OutSMS.SMSVerifyStatus = SMSVerifyStatus.Verifying;
                }
            

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return OutSMS;
        }

        private Boolean DoSMS(string VerifyCode)
        {
            return true;
        }
    }
}
