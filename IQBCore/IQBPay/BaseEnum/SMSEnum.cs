using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.BaseEnum
{
    public class SMSEnum
    {
    }

    public enum SMSVerifyStatus
    {
        //基本不用。。。
        Verifying =1,
        //已发送
        Sent = 2,
        //发送失败
        SentFailure = 6,

        //校验成功
        Success =3,
        //校验失败
        Failure= 4,

        //过期
        Expired=5,
        UnKnown=-1,


        
    }

    public enum SMSEvent
    {
        //QMMD
        NewMember=0,
        //QMMD
        UpdateMember=1,

        IQB_PayOrder=100,

        O2O_BuyerPhoneVerify = 200,

        ReCharge = 300,

        OO_Register = 400,
    }
}
