using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IQBCore.IQBPay.BaseEnum
{
    [NotMapped()]
    public class IQBPayEnum
    {
        public static string GetPayMethod(PayMethod pm)
        {
            string result = "";
            switch(pm)
            {
                case PayMethod.HuaBei:
                    result = "花呗";
                    break;
                case PayMethod.BaiTiao:
                    result = "京东白条";
                    break;
                case PayMethod.HuaBei_FK:
                    result = "花呗(跳风控)";
                    break;
            }
            return result;
        }
        public static string  GetO2OName(O2OOrderStatus os)
        {
            string O2OOrderStatusStr= "";

            switch (os)
            {
                case O2OOrderStatus.All:
                    O2OOrderStatusStr = "全部";
                    break;
                case O2OOrderStatus.Complete:
                    O2OOrderStatusStr = "完成";
                    break;
                case O2OOrderStatus.OpenOrder:
                    O2OOrderStatusStr = "初始化";
                    break;
                case O2OOrderStatus.OrderRefused:
                    O2OOrderStatusStr = "审核拒绝";
                    break;
                case O2OOrderStatus.OrderReview:
                    O2OOrderStatusStr = "等待审核";
                    break;
                case O2OOrderStatus.ComfirmSign:
                    O2OOrderStatusStr = "等待确认签收";
                    break;
                case O2OOrderStatus.Payment:
                    O2OOrderStatusStr = "等待支付用户";
                    break;
                case O2OOrderStatus.Settlement:
                    O2OOrderStatusStr = "等待平台结算";
                    break;
                case O2OOrderStatus.WaitingSendSMS:
                    O2OOrderStatusStr = "等待发送短信";
                    break;
                case O2OOrderStatus.SignCodeInfo:
                    O2OOrderStatusStr = "快递柜信息";
                    break;

                //case O2OOrderStatus.WaitingDeliver:
                //    O2OOrderStatusStr = "等待发货";
                //    break;
                case O2OOrderStatus.WaitingUpload:
                    O2OOrderStatusStr = "等待订单上传";
                    break;
                case O2OOrderStatus.UserClose:
                    O2OOrderStatusStr = "用户关闭";
                    break;
            }
            return O2OOrderStatusStr;
        }

    }

  
    public enum PayWebStatus
    {
        Running = 0,
        Stop = -1,
    }

    public enum QRHugeEntry
    {
        Running = 0,
        Stop = -1,
    }

    public enum AliPayResult
    {
        SUCCESS = 0,
        FAILED = 1,
        AUTHERROR =2,
    }

    public enum O2OUserRole
    {
        //没有O2O入口权限
        User =0,
        //有O2O入口权限
        Agent = 1,
        
        Mall =5,
        //出库商
        Shippment = 10, 
        
        Shippment_Shop = 12,      
    }

    /// <summary>
    /// 表明用户是普通用户还是管理员
    /// </summary>
    public enum UserRole
    {
        
        /// <summary>
        /// 过客，就是进来看看
        /// </summary>
        NormalUser=1,

        /// <summary>
        /// 代理用户
        /// </summary>
        Agent = 2,

        DiamondAgent = 3,

        /// <summary>
        /// 可以对接自己的商户码，且有代理功能
        /// </summary>
        StoreVendor = 10,

        /// <summary>
        /// 提供密钥,可使用自己的二维码
        /// </summary>
        StoreMaster =99,

        /// <summary>
        /// 超级管理员
        /// </summary>
        Administrator=100,

        All = -1,

       

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
        All = 99,


    }

    public enum UserVerifyStatus
    {
        /// <summary>
        /// 仅扫描了二维码
        /// </summary>
        Scaned = 0,

        /// <summary>
        /// 提交审核单
        /// </summary>
        Submit =1,

        /// <summary>
        /// 审核驳回
        /// </summary>
        Refused = -1,

        /// <summary>
        /// 审核通过
        /// </summary>
        Pass =100
    }

    public enum QRReceiveType
    {
        Small = 1,
        CreditCard =2,
        Huge = 4,
        O2O = 10

    }

    public enum QRType
    {
        /// <summary>
        /// 普通收款码
        /// </summary>
        AR =1,
        /// <summary>
        /// 大额收款码
        /// </summary>
        ARHuge = 4,

        StoreAuth =2,
        /// <summary>
        /// 邀请码
        /// </summary>
        ARAuth = 3,
        Temp =0,
        O2O = 10,
       
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
        Init  = -1,

        Normal=0,
       
        Blocked=1,

        Process = 2,

        WaitingReview = 10,

        All = 99,
    }

    public enum QRHugeTransStatus
    {
        Open =1,
        Closed = 2,  
    }

    public enum QRHugeStatus
    {
        /// <summary>
        /// 创建
        /// </summary>
        Created =0,
        /// <summary>
        /// 失效
        /// </summary>
        InValid = 1,

        Closed = 100

    }

    public enum AgentCommissionStatus
    {
        /// <summary>
        /// 下单后的状态
        /// </summary>
        Open=2,
        /// <summary>
        /// 暂时不用
        /// </summary>
        Paid =3,
        /// <summary>
        /// 已转账给上级代理
        /// </summary>
        Closed =4,
    }

    public enum OrderStatus
    {
        /// <summary>
        /// 等待用户确认是否收到款
        /// </summary>
        WaitingBuyerConfirm =-2,

        /// <summary>
        /// 系统关闭交易，用户未支付
        /// </summary>
        SystemClose = -3,
        /// <summary>
        /// 等待支付
        /// </summary>
        WaitingAliPayNotify =0,

        /// <summary>
        /// 用户已支付
        /// </summary>
        Paid =1,

        /// <summary>
        /// 成功交易
        /// </summary>
        Closed= 2,

        
        /// <summary>
        /// 异常
        /// </summary>
        Exception =-1,

        ALL = 99,
    }

    public enum ConditionDataType
    {
        Today =1,
        Week =2,
        Month =3,
        All = 99,
    }

    public enum  OrderType
    {
        Normal=1,
        Huge = 4,
        UnKnow=-1,
        All = 99,
    }

    public enum ExistAction
    {
        sessionlost,
        notAdmin,
        noRegister,
    }

    public enum TransferTarget
    {
        User =0,
        Agent =1,
        ParentAgent=2,
        Internal =3,
        L3Agent = 4,
        MidStore = 5,

        /// <summary>
        /// 出库商获得押金
        /// </summary>
        O2OWareHouse =20,
        
       
        
       /// <summary>
       /// 充值
       /// </summary>
        ReCharge = 50,
        /// <summary>
        /// 平台从用户账户扣减余额
        /// </summary>
        PP = 100,

        /*查询使用*/
        All = 999, 
    }

    public enum PayTargetMode
    {
        AliPayId =0,
        AliPayAccount = 1,
    }

    public enum TransferStatus
    {
        Open =0 ,
        Success = 1,
        Failure= -1,

    }

    public enum TransactionType
    {
        Agent_Order_Comm = 0,
        Parent_Comm = 1,
        L3_Comm = 2,

        /// <summary>
        /// 提款
        /// </summary>
        GetCash = 3,

        Store_Comm = 10,
        Store_L2 = 11,
        Store_L3 = 12,

    }

    public enum StoreAuthStatus
    {
        NoAuth =0,
        Audit =1,
        Authed = 100
    }

    public enum StoreType
    {
        Small = 1,
        CreditCard = 2,
        Huge =4,
        All =99,
    }

    public enum PriviegeError
    {
        UnKnow = -1,
        InviteCode =1000,
        QRHuge=1001,
    }

    /* O2O begin */
  
    public enum O2OStepStatus
    {
        NotStart=0,
        Process=5,
        Complete=10,
    }

    public enum O2OOrderStatus
    {
        //商品已选择
        OpenOrder =0,

        //真实订单上传
        WaitingUpload = 2,

        //审核驳回
        OrderRefused = 5,
        //等待订单审核
        OrderReview=6,

        //JD订单需要发送短信给快递员
        WaitingSendSMS = 8,

        //用户确认签收
        ComfirmSign=10,

        //提货柜信息
        SignCodeInfo =12,
         
        //等待到货结算
        Settlement = 14,

        //支付用户
        Payment=18,

       

        UserClose = 45,

        //订单完成
        Complete=50,

        All = 99,

        /* 订单过滤查询使用 */
        //出库商过滤使用，到货确认
        ItemArrival = 100,
        //已结算
        Payment_Complete = 1850,

        Sign_Settle_Payment_Complete = 10141850,
/*
        Sign_Settle_Payment = 101418,
        Payment_UserClose = 1845,
        Settle_Payment_UserClose = 141845,
        */
    }

    public enum O2OOrderType
    {
        Normal = 0,
        //代替用户下单
        ForUser = 10,
    }


    /* O2O end */
    public enum UserAccountType
    {
        /// <summary>
        /// 出库商
        /// </summary>
        O2OShippment = 10,

        /// <summary>
        /// 代理
        /// </summary>
        Agent = 0,

    }

    public enum BuyerType
    {
        /// <summary>
        /// O2O用户
        /// </summary>
        O2O =10,
        /// <summary>
        /// 小额码
        /// </summary>
        QR =1,
    }

    /// <summary>
    /// 支付方式，花呗，白条
    /// </summary>
    public enum PayMethod
    {
        /// <summary>
        /// 花呗
        /// </summary>
        HuaBei=0,
        /// <summary>
        /// 花呗风控
        /// </summary>
        HuaBei_FK =1,
        /// <summary>
        /// 京东白条
        /// </summary>
        BaiTiao=10,
    }


}