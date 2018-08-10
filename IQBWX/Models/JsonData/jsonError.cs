using IQBCore.IQBWX.BaseEnum;
using IQBWX.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace IQBWX.Models.JsonData
{
    public class jsonError
    {
        /// <summary>
        /// 1001 验证码出错
        /// 1002 没有权限
        /// 9999 系统维护
        /// </summary>
        public int errorCode { get; set; }
        public string errorMsg { get; set; }
        public string btnText { get; set; }

        private string _btnUrl;
        public string btnUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_btnUrl))
                    _btnUrl = "#";
                return _btnUrl;
            }
            set
            {
                _btnUrl = value;

            }
        }
      
        public static jsonError GetErrorObj(Errorcode code,string errorMsg = "",string returnUrl="")
        {
            jsonError obj = new jsonError();
            obj.errorCode = Convert.ToInt32(code);
            string PaySite = ConfigurationManager.AppSettings["Site_IQBPay"];
            switch (code)
            {
                case Errorcode.IncorrectVerifyCode:
                    obj.errorMsg = "出错啦！验证码错误";
                    break;
                case Errorcode.NotMember:
                    obj.errorMsg = "只有会员才能查看！";                     
                    break;
                case Errorcode.SystemMaintain:
                    obj.errorMsg = errorMsg;
                    break;
                case Errorcode.OpenIdNotFound:
                    obj.errorMsg = "OpenId 未获取，请联系QQ";
                    break;
                case Errorcode.NormalErrorNoButton:
                    obj.errorMsg = errorMsg;
                    break;
                case Errorcode.NormalError:
                    obj.errorMsg = errorMsg;
                    break;
                case Errorcode.NotAuthorized:
                    obj.errorMsg = "您没有权限！";
                    break;
                case Errorcode.NoStoreAuthorized:
                    obj.errorMsg = "码商需要联系平台单独开通！";
                    break;
                case Errorcode.AliPay_PayError:
                    obj.btnText = "返回支付";
                    obj.errorMsg = errorMsg;
                    obj.btnUrl = "/PP/Pay";
                    break;
                case Errorcode.QRHugeError:
                    obj.btnText = "返回支付";
                    obj.errorMsg = errorMsg;
                    obj.btnUrl = PaySite+ "Wap/PayHuge";
                    break;
                case Errorcode.QRHugeQRUserMiss:
                    obj.errorMsg = "您的大额码配置未找到或被禁用！";
                    break;
                case Errorcode.QRHugeBlock:
                    obj.errorMsg = "您的大额码配置暂时被禁用！";
                    break;
                case Errorcode.NotAliPayClient:
                    obj.errorMsg = "请用支付宝扫描";
                  
                    break;
                case Errorcode.NotWXClient:
                    obj.errorMsg = "请用微信扫描";
                    break;
               

            }

            return obj;
        }
        
    }
}