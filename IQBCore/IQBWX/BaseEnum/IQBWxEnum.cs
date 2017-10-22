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
        SystemMaintain = 9999,
        OpenIdNotFound = 1003,
        NormalErrorNoButton = 2000,
        AliPay_PayError = 2001,

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
}
