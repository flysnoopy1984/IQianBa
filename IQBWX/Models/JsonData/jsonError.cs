using IQBWX.Common;
using System;
using System.Collections.Generic;
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
        public static jsonError GetErrorObj(Errorcode code)
        {
            jsonError obj = new jsonError();
            obj.errorCode = Convert.ToInt32(code);
            switch (code)
            {
                case Errorcode.IncorrectVerifyCode:
                    obj.errorMsg = "验证码错误";
                    break;
                case Errorcode.NotMember:
                    obj.errorMsg = "只有会员才能查看！";                     
                    break;
                case Errorcode.SystemMaintain:
                    obj.errorMsg = "系统维护中，请一会再来哦";
                    break;
                case Errorcode.OpenIdNotFound:
                    obj.errorMsg = "OpenId 未获取，请联系QQ";
                    break;
            }

            return obj;
        }
        
    }
}