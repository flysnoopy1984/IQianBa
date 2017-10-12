using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQBCore.IQBPay.BaseEnum
{
    public class IQBPayEnum
    {
    }

    /// <summary>
    /// 表明用户是普通用户还是管理员
    /// </summary>
    public enum UserRole
    {
        
        /// <summary>
        /// 普通用户，就是进来看看
        /// </summary>
        NormalUser=1,

        Agent = 2,

        /// <summary>
        /// 可以在平台二维码的基础上生成自己的二维码并发放
        /// </summary>
        StoreVendor = 3,

        /// <summary>
        /// 提供密钥,可使用自己的二维码
        /// </summary>
        StoreMaster =99,

        Administrator=100,

    }

    /// <summary>
    /// 0/100
    /// </summary>
    public enum UserStatus
    {
        /// <summary>
        /// 仅仅注册不能支付不能提现
        /// </summary>
        
        JustRegister =0,


        PPUser= 1,


    }

    public enum QRType
    {
        AR =1,
        StoreAuth =2,
        ARAuth = 3,
        Temp =0,
    }

    public enum Channel
    {
        PP=0,
        League = 1,
        PPAuto = 99,
        All =-1,
    }

    public enum RecordStatus
    {
        Normal=0,
       
        Blocked=1,

        Process = 2,
    }

    public enum OrderStatus
    {
        WaitingAliPayNotify =0,

        Paid =1,

        Closed= 2,
        
        Exception =-1,
    }

    public enum  OrderType
    {
        Normal,
        UnKnow,
    }


}