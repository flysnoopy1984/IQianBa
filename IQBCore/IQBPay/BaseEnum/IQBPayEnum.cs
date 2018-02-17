﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQBCore.IQBPay.BaseEnum
{
    public class IQBPayEnum
    {
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
                case O2OOrderStatus.Payment:
                    O2OOrderStatusStr = "等待支付用户";
                    break;
                case O2OOrderStatus.Settlement:
                    O2OOrderStatusStr = "等待发货结算";
                    break;
                //case O2OOrderStatus.WaitingDeliver:
                //    O2OOrderStatusStr = "等待发货";
                //    break;
                case O2OOrderStatus.WaitingUpload:
                    O2OOrderStatusStr = "等待用户上传";
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
        User =0,
        
        Mall =5,
        
        Shippment = 10,       
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
        O2OWareHouse =20,
      
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
        All =99,
    }

    public enum PriviegeError
    {
        UnKnow = -1,
        InviteCode =1000,
        QRHuge=1001,
    }

    public enum O2OStepStatus
    {
        NotStart=0,
        Process=5,
        Complete=10,
    }

    public enum O2OOrderStatus
    {
        //初始化
        OpenOrder =0,

        //真实订单上传
        WaitingUpload = 2,

        //审核驳回
        OrderRefused = 5,
        //等待订单审核
        OrderReview=6,

        ////仓库等待到货
        //WaitingDeliver=10,

        //到货结算
        Settlement=14,

        //支付用户
        Payment=18,

        //订单完成
        Complete=50,

        All = 99,

        Settle_Payment=1418,
    }

   

}