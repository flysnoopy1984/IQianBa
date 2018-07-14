using IQBCore.Common.Helper;
using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.BLL;
using IQBCore.IQBPay.Models.InParameter;
using IQBCore.IQBPay.Models.SMS;
using IQBCore.IQBWX.BaseEnum;
using IQBCore.IQBWX.Models.OutParameter;
using IQBRecharge.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IQBRecharge.Controllers.API
{
    public class SMSController : ApiController
    {
        /// <summary>
        /// 5分钟有效 验证用户输入的验证码
        /// </summary>

        [HttpGet]
        public OutSMS ConfirmVerifyCode(string mobilePhone, string Code)
        {

            OutSMS OutSMS = new OutSMS();
            OutSMS.SMSVerifyStatus = SMSVerifyStatus.UnKnown;


            using (RecDbContent db = new RecDbContent())
            {
                var sms = db.DBSMSVerification.Where(s => s.MobilePhone == mobilePhone
                                                 && (
                                                    s.SMSVerifyStatus == SMSVerifyStatus.Sent ||
                                                    s.SMSVerifyStatus == SMSVerifyStatus.Verifying ||
                                                    s.SMSVerifyStatus == SMSVerifyStatus.Failure
                                                    )
                                                 )
                    .OrderByDescending(s => s.ID)
                    .FirstOrDefault();
                if (sms == null)
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
                        if (sms.VerifyCode == Code)
                            OutSMS.SMSVerifyStatus = SMSVerifyStatus.Success;
                        else
                            OutSMS.SMSVerifyStatus = SMSVerifyStatus.Failure;
                    }
                    //  OutSMS.OrderNo = sms.OrderNo;

                    sms.SMSVerifyStatus = OutSMS.SMSVerifyStatus;
                    db.SaveChanges();

                }
            }
            return OutSMS;
        }

        /// <summary>
        /// 获取验证码间隔时间
        /// </summary>
        /// <param name="mobilePhone"></param>
        /// <param name="IntervalSec"></param>
        /// <returns></returns>
        public OutSMS IQBPay_GetVerifyingSec(string mobilePhone, int IntervalSec, SMSEvent SMSEvent = SMSEvent.O2O_BuyerPhoneVerify)
        {
            OutSMS OutSMS = new OutSMS();
            try
            {
                using (RecDbContent db = new RecDbContent())
                {
                    //获取已发送的SMS
                    var sms = db.DBSMSVerification.Where(s => s.MobilePhone == mobilePhone
                    && s.SMSEvent == SMSEvent
                    && s.SMSVerifyStatus == SMSVerifyStatus.Sent).OrderByDescending(s => s.SendDateTime).FirstOrDefault();

                    OutSMS.RemainSec = -1;

                    //存在
                    if (sms != null)
                    {
                        OutSMS.SmsID = sms.ID;
                        OutSMS.OrderNo = sms.OrderNo;

                        TimeSpan nowtimespan = new TimeSpan(DateTime.Now.Ticks);
                        TimeSpan endtimespan = new TimeSpan(sms.SendDateTime.Ticks);
                        TimeSpan timespan = nowtimespan.Subtract(endtimespan).Duration();
                        //是否超过倒计时最大值（比如90秒后重新发送，90秒为最大值）
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
        public OutSMS Sent(string mobilePhone, SMSEvent SMSEvent = SMSEvent.ReCharge)
        {
            OutSMS OutSMS = new OutSMS();
            try
            {
            
                string VerifyCode = StringHelper.GenerateVerifyCode();

                InSMS inSMS = new InSMS();
                inSMS.Init();
                inSMS.Tpl_id = Convert.ToInt32(SMSTemplate.NormalVerify).ToString();
                inSMS.PhoneNumber = mobilePhone;
                inSMS.Parameters = string.Format("{0},{1}",VerifyCode,5);

                /*调用第三方短信接口*/
                //if (!this.DoSMS(inSMS))
                //{
                //    OutSMS.SMSVerifyStatus = SMSVerifyStatus.SentFailure;
                //    return OutSMS;
                //}

                using (RecDbContent db = new RecDbContent())
                {
                    ESMSVerification sms = new ESMSVerification()
                    {
                        VerifyCode = VerifyCode,
                        MobilePhone = mobilePhone,

                        SendDateTime = DateTime.Now,
                        SMSVerifyStatus = SMSVerifyStatus.Sent,
                        SMSEvent = SMSEvent,
                    };
                    db.DBSMSVerification.Add(sms);
                    db.SaveChanges();
                    OutSMS.SmsID = sms.ID;
                }
                OutSMS.SMSVerifyStatus = SMSVerifyStatus.Sent;
                OutSMS.RemainSec = -1;



            }
            catch (Exception ex)
            {
                throw ex;
            }
            return OutSMS;
        }

        [HttpPost]
        public Boolean DoSMS([FromBody]InSMS inSMS)
        {
            Boolean result = true;
            ESMSLog smsLog = new ESMSLog();
            try
            {

                SMSManager sms = new SMSManager();

                SMSResult_API51 Response = sms.PostSMS_API51(inSMS, ref smsLog);
                if (Response.result == "0")
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
                smsLog.Exception += "DoSMS Inner Error:" + ex.InnerException.Message;
                result = false;
            }
            using (RecDbContent db = new RecDbContent())
            {
                smsLog.IsSuccess = result;
                db.DBSMSLog.Add(smsLog);
                db.SaveChanges();
            }

            return result;
        }
    }
}
