using IQBCore.IQBWX.BaseEnum;
using IQBWX.Common;
using IQBWX.Models.Order;
using IQBWX.Models.Product;
using IQBWX.Models.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IQBWX.DataBase
{
    public class OrderContent:DbContext
    {
        IQBLog log = new IQBLog();
        public DbSet<EOrderLine> OrderInfo { get; set; }
        public OrderContent() : base("MainDBConnection")
        {

        }
        public void ForInit()
        {
            OrderInfo.FirstOrDefault<EOrderLine>(o => o.OrderId == "1");
        }

        public EOrderLine GetByOrderId(string orderId)
        {
            EOrderLine order = this.OrderInfo.FirstOrDefault<EOrderLine>(o => o.OrderId == orderId);
            return order;
        }

        public EOrderLine GetByMemberId(int mId)
        {
            EOrderLine order = this.OrderInfo.FirstOrDefault<EOrderLine>(o => o.MemberId == mId);
            return order;
        }

        public EOrderLine CreateOrder(EMemberInfo mi, int selTC)
        {
            if (mi == null) return null;//需要报错
            EItemInfo item = null;
            if (selTC == 1)
                item = EItemInfo.get158Item();
            else if(selTC==2)
                item = EItemInfo.get358Item();
            else if (selTC == 3)
                item = EItemInfo.getL1ToL2Item();

            EOrderLine order = this.CreateOrder(mi.UserId, mi.openId, item);
            order.MemberId = mi.MemberId;
            OrderInfo.Add(order);
            return order;
        }

        public EOrderLine UpdateToPaidOrder(EMemberInfo mi)
        {
            IQBLog log = new IQBLog();
            EOrderLine order = null;
            try
            {
                order = OrderInfo
                .OrderByDescending(m=>m.CreateDateTime)
                .FirstOrDefault<EOrderLine>(o => o.OpenId == mi.openId && o.PaymentState == PaymentState.Paying)
                ;
            order.PaymentState = PaymentState.paid;
            order.MemberId = mi.MemberId;
            this.SaveChanges();
            }
            catch(Exception ex)
            {
                log.log("UpdateToPaidOrder Order:" + order.OrderId);

                log.log("UpdateToPaidOrder Error:" + ex.Message);
                log.log("UpdateToPaidOrder InnerException:" + ex.InnerException);
                 
            }
            return order;
        }

        /// <summary>
        /// 先创建订单，支付成功，创建会员
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="openId"></param>
        /// <param name="item"></param>
        public EOrderLine CreateOrder(int userId, string openId,EItemInfo item)
        {
            int tc = 1;
            if (item.ItemId == EItemInfo.Item358) tc = 2;
            if (item.ItemId == EItemInfo.ItemL1ToL2) tc = 3;
            string orderId = EOrderLine.GenerateOrderNo(userId,tc);
            try
            { 
                if (userId ==0) return null;//需要报错

                EOrderLine order = this.GetByOrderId(orderId);

                if (order != null) return order;
                order = new EOrderLine()
                {
                    CreateDateTime = DateTime.Now,
                    ItemId = item.ItemId,
                    ItemName = item.ItemName,
                    OpenId = openId,
                    UserId = userId,
                    Qty = 1,
                    OrderId = orderId,
                    LineAmount = item.SalesPrice,
                    PaymentState = IQBCore.IQBWX.BaseEnum.PaymentState.Paying
                };
                OrderInfo.Add(order);
                return order;
                
            }
            catch(Exception ex)
            {
                log.log("CreateOrder Error:" + ex.Message);
                log.log("CreateOrder Inner Error:" + ex.InnerException.Message);
            }
            return null;
        }

        public EOrderLine CreateOrderForUpdateMember(int userId, string openId, EItemInfo item)
        {
            string orderId = EOrderLine.GenerateOrderNo(userId, 3);
            decimal diffAmt = 1;
            try
            {
                if (userId == 0) return null;//需要报错

                EOrderLine order = this.GetByOrderId(orderId);

                if (order != null) return order;
                order = new EOrderLine()
                {
                    CreateDateTime = DateTime.Now,
                    ItemId = item.ItemId,
                    ItemName = item.ItemName,
                    OpenId = openId,
                    UserId = userId,
                    Qty = 1,
                    OrderId = orderId,
                    LineAmount = diffAmt,
                    PaymentState = IQBCore.IQBWX.BaseEnum.PaymentState.Paying
                };
                OrderInfo.Add(order);
                return order;

            }
            catch (Exception ex)
            {
                log.log("CreateOrderForUpdate Error:" + ex.Message);
                log.log("CreateOrderForUpdate Stack:" + ex.StackTrace);
            }
            return null;
        }


    }
}