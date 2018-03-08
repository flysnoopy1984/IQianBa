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
        Verifying =1,
        Sent = 2,
        SentFailure = 6,
        Success =3,
        Failure= 4,
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
    }
}
