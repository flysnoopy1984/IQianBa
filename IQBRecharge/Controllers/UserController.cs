using IQBRecharge.DataBase;
using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.IQBRecharge.Models;
using IQBCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IQBRecharge.Controllers.API;
using IQBCore.IQBWX.Models.OutParameter;
using IQBCore.IQBRecharge.Models.In;

namespace IQBRecharge.Controllers
{
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Home()
        {
            // var rtnUrl = System.Web.HttpUtility.UrlEncode("http://rc.iqianba.cn/User/Index", System.Text.Encoding.UTF8); 

            //var url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxcc6a60fdad3ef777&redirect_uri="+ rtnUrl+"&response_type=code&scope=snsapi_userinfo&state=1#wechat_redirect";
            // return Redirect(url);
            return View(); 
         
        }

        public ActionResult Register(string phone,string pwd)
        {
            return View();
        }

        public ActionResult SellFuelCard()
        {
            return View();
        }
        public ActionResult SellPhoneCard()
        {
            return View();
        }

        public ActionResult DoRegister(string phone, string pwd,string verifyCode)
        {
            OutAPIResult result = new OutAPIResult();
            try
            {
                if(string.IsNullOrEmpty(phone))
                {
                    result.IsSuccess = false;
                    result.ErrorMsg = "请填写手机号";
                    return Json(result);
                }

                if (string.IsNullOrEmpty(pwd))
                {
                    result.IsSuccess = false;
                    result.ErrorMsg = "请填写密码";
                    return Json(result);

                }

                if (string.IsNullOrEmpty(verifyCode))
                {
                    result.IsSuccess = false;
                    result.ErrorMsg = "请填写验证码";
                    return Json(result);

                }


                SMSController sms = new SMSController();
                OutSMS outSMS= sms.ConfirmVerifyCode(phone, verifyCode);

                if(outSMS.SMSVerifyStatus == IQBCore.IQBPay.BaseEnum.SMSVerifyStatus.Success)
                {
                    using (RecDbContent db = new RecDbContent())
                    {
                        EUserInfo ui = db.DBUserInfo.Where(a => a.LoginPhone == phone).FirstOrDefault();
                        if (ui != null)
                        {
                            result.IsSuccess = false;
                            result.ErrorMsg = "手机号已经存在";
                        }
                        else
                        {
                            ui = new EUserInfo
                            {
                                Name = phone,
                                LoginPhone = phone,
                                Pwd = pwd,
                                RegisterDateTime = DateTime.Now,
                                LastLoginDateTime = DateTime.Now

                            };
                            db.DBUserInfo.Add(ui);
                            db.SaveChanges();
                        }
                    }
                }
                else if(outSMS.SMSVerifyStatus == IQBCore.IQBPay.BaseEnum.SMSVerifyStatus.Expired)
                {
                    result.IsSuccess = false;
                    result.ErrorMsg = "验证码已过期，请重新获取";
                }
                else
                {
                    result.IsSuccess = false;
                    result.ErrorMsg = "验证码错误";
                }
            }
            catch (Exception ex)
            {
                result.ErrorMsg = result.ErrorMsg;
                result.IsSuccess = false;
            }
            return Json(result);
        }

        public ActionResult Login(string phone,string pwd)
        {
            NResult<EUserInfo> result = new NResult<EUserInfo>();
            EUserInfo ui = null;
            try
            {
                using (RecDbContent db = new RecDbContent())
                {
                    ui = db.DBUserInfo.Where(a => a.LoginPhone == phone && a.Pwd == pwd).FirstOrDefault();
                    if (ui == null)
                    {
                        result.IsSuccess = false;
                        result.ErrorMsg = "手机号或密码不正确";
                    }
                    else
                        result.resultObj = ui;
                }
            }
            catch(Exception ex)
            {
                result.ErrorMsg = result.ErrorMsg;
                result.IsSuccess = false;
            }
            return Json(result);
        }

        public ActionResult AccountBalance()
        {

            EUserAccountBalance ub = new EUserAccountBalance();
            using (RecDbContent db = new RecDbContent())
            {
                //string openId = UserSession.OpenId;
                //ub = db.DBUserAccountBalance.Where(a => a.OpenId == openId && a.UserAccountType == UserAccountType.Agent).FirstOrDefault();
                //if (ub == null)
                //{
                //    ub = new EUserAccountBalance()
                //    {
                //        OpenId = openId,
                //        UserAccountType = UserAccountType.Agent,
                //        O2OShipBalance = 0,
                //        O2OShipInCome = 0,
                //        O2OShipOutCome = 0,
                //    };
                //    db.DBUserAccountBalance.Add(ub);
                //    db.SaveChanges();
                //}
            }

            return View(ub);
        }

        [HttpPost]
        public ActionResult DoCreateOrder(InOrder inOrder)
        {
            OutAPIResult result = new OutAPIResult();
            ERcOrderInfo order = new ERcOrderInfo();
            try
            {
                using (RecDbContent db = new RecDbContent())
                {
                    
                }
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }
            return Json(result);
        }
    }
}