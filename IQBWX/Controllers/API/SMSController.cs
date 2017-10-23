using IQBCore.Common.Helper;
using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.BLL;
using IQBCore.IQBPay.Models.InParameter;
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
               //    this.DoSMS();
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
        [HttpGet]
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
                    OutSMS.OrderNo = sms.OrderNo;

                    sms.SMSVerifyStatus = OutSMS.SMSVerifyStatus;
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
                        OutSMS.OrderNo = sms.OrderNo;

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
        public OutSMS SentSMS_IQBPay_BuyerOrder(string OrderNo,string mobilePhone, int IntervalSec)
        {
            OutSMS OutSMS = new OutSMS();
            try
            {
                OutSMS = IQBPay_GetVerifyingSec(mobilePhone, IntervalSec);

                if (OutSMS.RemainSec == -1)
                {
                    string VerifyCode = StringHelper.GenerateVerifyCode();

                    if (!this.DoSMS(mobilePhone, VerifyCode, OrderNo))
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
                            OrderNo = OrderNo,
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

        private Boolean DoSMS(string phoneNumber,string VerifyCode,string OrderNo)
        {
            Boolean result = true;
            ESMSLog smsLog = new ESMSLog();
            try
            {

                SMSManager sms = new SMSManager();
                InSMS inSMS = new InSMS();
                inSMS.Init();
                inSMS.PhoneNumber = phoneNumber;
                inSMS.Parameters = VerifyCode+","+ OrderNo + ",http://b.iqianba.cn/";

                SMSResult_API51 Response = sms.PostSMS_API51(inSMS,ref smsLog);
                if(Response.result == "0")
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                smsLog.Exception += "DoSMS Error:" + ex.Message;
                result = false;
            }
            using (AliPayContent db = new AliPayContent())
            {
                smsLog.IsSuccess = result;
                db.DBSMSLog.Add(smsLog);
                db.SaveChanges();
            }

           return result;
        }
    }
}
