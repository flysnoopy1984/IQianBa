using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQBCore.IQBPay.BaseEnum
{
    public class IQBPayEnum
    {
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

        WaitingAliPayNotify =0,

        Paid =1,

        Closed= 2,
        
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
        Normal=0,
        Huge = 1,
        UnKnow=-1,
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

    public enum StoreAuthStatus
    {
        NoAuth =0,
        Audit =1,
        Authed = 100
    }

    public enum StoreType
    {
        Small = 0,
        Huge =4,
        All =100,
    }

   

}