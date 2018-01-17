using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBWX.BaseEnum
{
    public class IQBWxEnum
    {
    }

    public enum PaymentState
    {
        None = 0,
        Paying = 1,
        paid = 2,
    }

    public enum Errorcode
    {
        IncorrectVerifyCode = 1001,
        NotMember = 1002,
        OpenIdNotFound = 1003,
        
        /// <summary>
        /// 系统刚维护
        /// </summary>
        SystemMaintain = 9999,
        
        NormalErrorNoButton = 2000,
        /// <summary>
        /// 支付错误
        /// </summary>
        AliPay_PayError = 2001,
        NotAuthorized = 2002,

        /// <summary>
        /// 大额支付错误
        /// </summary>
        QRHugeError = 2003,
        QRHugeQRUserMiss = 2004,
        QRHugeBlock = 2005,

        /// <summary>
        /// 客户端错误
        /// </summary>
        NotAliPayClient = 3000,
        NotWXClient = 3001,

    }


    public enum UserSMSStatus
    {
        None = 0,
        Sent = 1,
        Pass = 2,
        Expire = -1

    }


    public enum MemberType
    {
        City = 2,
        Channel = 1,
    }

    public enum ARTransType
    {
        pop,
        loan,
    }

    public enum LoginStatus
    {
        QRCreated = 0,
        QRScaned = 1,
        Login = 2,
    }

    public enum ExternalWeb
    {
        Book = 1,
        Pay = 2,
    }

    public enum SMSTemplate
    {
        //验证码：您的验证码为:{1}. 支付成功后。为保证商家发货给您，请注意确认短信。
        VerifyCode = 60627,
        //收款确认：您已支付成功，您的收款确认码为：{1}。若代理商家已打款，请到以下地址进行收款确认：http://b.iqianba.cn/。
        ReceiveConfirm = 60977,
    }
}
