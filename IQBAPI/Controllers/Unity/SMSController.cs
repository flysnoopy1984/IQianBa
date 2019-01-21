using IQBCore.Common.Extension;
using IQBCore.Common.Helper;
using IQBCore.DataBase;
using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.BLL;
using IQBCore.IQBPay.Models.InParameter;
using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.IQBPay.Models.SMS;
using IQBCore.IQBWX.BaseEnum;
using IQBCore.OO.Models.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IQBAPI.Controllers.Unity
{
    public class SMSController : ApiController
    {
        [HttpPost]
        public OutAPIResult  GetMessage(string Phone)
        {
            OutAPIResult result = new OutAPIResult();
            try
            {
                if(IsVerifiedPhone(Phone))
                {
                    result.ErrorMsg = ("手机已经注册，请直接登陆");
                    result.IntMsg = 100;
                    return result;
                }
                int LastInterval = GetLastMessageInterval(Phone);
                if(LastInterval == -1)
                {
                    ESMSVerification sms = SendSMSToUser(Phone);
                    if(sms == null || sms.SMSVerifyStatus == SMSVerifyStatus.SentFailure)
                    {
                        result.ErrorMsg = ("短信发送未成功，若尝试几次任然不成功，请联系客服！");
                    }
                }
                else
                {
                    result.ErrorMsg = string.Format("请不要重复操作，先查看已发送的验证码，{0}秒后再尝试",LastInterval);
                }
            }
            catch(Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }

            return result;
        }

        [HttpPost]
        public OutAPIResult ConfirmVerification(string Phone,string VerifyCode)
        {
            OutAPIResult result = new OutAPIResult();
            try
            {
                if (string.IsNullOrEmpty(VerifyCode))
                {
                    result.ErrorMsg = "验证码不能为空！";
                    return result;
                }
                using (PPContent db = new PPContent())
                {
                   ESMSVerification sms =  db.DBSMSVerification.Where(a=>a.MobilePhone == Phone &&
                                              a.SMSVerifyStatus == SMSVerifyStatus.Sent &&
                                              a.SMSEvent == SMSEvent.OO_Register
                                             )
                   .OrderByDescending(s => s.ID)
                   .FirstOrDefault();

                    if(sms == null)
                    {
                        result.ErrorMsg = "验证码还没有成功获取！";
                        return result;
                    }
                    else{
                       
                       
                        if( sms.VerifyCode != VerifyCode)
                        {
                            result.ErrorMsg = "验证码不正确！请仔细查看收到的短信信息";
                        }
                        //校验成功
                        else
                        {
                            int SMSMaxIntervalSec = Convert.ToInt32(ConfigurationManager.AppSettings["SMSMaxIntervalSec"]);
                            if (sms.SendDateTime.GetSecInterval() > SMSMaxIntervalSec)
                            {
                                result.ErrorMsg = "验证码已失效，请重新获取！";
                                result.IntMsg = -100;
                                return result;
                            }
                            else
                            {
                                sms.SMSVerifyStatus = SMSVerifyStatus.Success;
                                db.SaveChanges();
                            }
                         
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        private ESMSVerification SendSMSToUser(string Phone)
        {
            string VerifyCode = StringHelper.GenerateVerifyCode();
            int SMSMaxIntervalSec = Convert.ToInt32(ConfigurationManager.AppSettings["SMSMaxIntervalSec"]);

            InSMS inSMS = new InSMS();
            inSMS.Init();
            inSMS.Tpl_id = Convert.ToInt32(SMSTemplate.NormalVerify).ToString();
            inSMS.PhoneNumber = Phone;
            inSMS.Parameters = VerifyCode + ","+ SMSMaxIntervalSec/60;

            bool sentResult = this.DoSMS(inSMS);
            ESMSVerification sms = null;
            using (PPContent db = new PPContent())
            {
                sms = new ESMSVerification()
                {
                    VerifyCode = VerifyCode,
                    MobilePhone = Phone,

                    SendDateTime = DateTime.Now,
                    SMSVerifyStatus = SMSVerifyStatus.Sent,
                    SMSEvent = SMSEvent.OO_Register,
                };
                if (sentResult == false)
                    sms.SMSVerifyStatus = SMSVerifyStatus.SentFailure;

                db.DBSMSVerification.Add(sms);
                db.SaveChanges();
              
            }
            return sms;

        }

        /// <summary>
        /// 已经存在的手机不要发送
        /// </summary>
        /// <param name="Phone"></param>
        /// <returns></returns>
        private bool IsVerifiedPhone(string Phone)
        {
            using (OOContent db = new OOContent())
            {
                EUserInfo user = db.DBUserInfo.Where(a => a.Phone == Phone).FirstOrDefault();
                return user != null;

                
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>单位：秒。没有-1</returns>
        private int GetLastMessageInterval(string phone)
        {


            using (PPContent db = new PPContent())
            {

                //查找手机号 上次请求的SMS验证消息
                var sms = db.DBSMSVerification.Where(s => s.MobilePhone == phone
                && s.SMSEvent == SMSEvent.OO_Register
                && s.SMSVerifyStatus == SMSVerifyStatus.Sent).OrderByDescending(s => s.SendDateTime).FirstOrDefault();

                
                if (sms!=null)
                {
                    int SMSMaxIntervalSec = Convert.ToInt32(ConfigurationManager.AppSettings["SMSMaxIntervalSec"]);
                    int CurrentSec = sms.SendDateTime.GetSecInterval();

                    if (CurrentSec >= SMSMaxIntervalSec)
                        return -1;
                    else
                        return  SMSMaxIntervalSec- CurrentSec;
                }
               

            }

            return -1;
        }

        private Boolean DoSMS(InSMS inSMS)
        {
            Boolean result = true;
            ESMSLog smsLog = new ESMSLog();
            try
            {

                SMSManager smsManger = new SMSManager();

                SMSResult_API51 Response = smsManger.PostSMS_API51(inSMS, ref smsLog);
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
            using (PPContent db = new PPContent())
            {
                smsLog.IsSuccess = result;
                db.DBSMSLog.Add(smsLog);
                db.SaveChanges();
            }

            return result;
        }
    }
}
