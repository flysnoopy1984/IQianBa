using Aop.Api.Response;
using IQBCore.IQBPay.BLL;
using IQBCore.IQBPay.Models.AccountPayment;
using IQBCore.IQBPay.Models.InParameter;
using IQBCore.IQBPay.Models.Order;
using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.IQBPay.Models.Result;
using IQBPay.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IQBPay.Controllers.ExternalAPI
{
    public class TransferAPIController : ApiController
    {
       

        [HttpPost]
        public OutDoTransfer TransferToAgent([FromBody]InDoTransfer transfer)
        {
            string TransferId;
            float OrderAmt =0 ;
            float CommAmt =0 ;
            AliPayManager payManager = new AliPayManager();
            OutDoTransfer result = new OutDoTransfer();
            result.IsSuccess = true;

            using (AliPayContent db = new AliPayContent())
            {
                List<ROrderInfo> orderlist = db.DBOrder.Where(s => s.OrderType == IQBCore.IQBPay.BaseEnum.OrderType.Normal
                                  && s.OrderStatus == IQBCore.IQBPay.BaseEnum.OrderStatus.Paid
                                  && s.AgentOpenId == transfer.OpenId).Select(o=>new ROrderInfo {
                                      RealTotalAmount = o.RealTotalAmount,

                                  }).ToList();

                OrderAmt = orderlist.Sum(a => a.RealTotalAmount);
                if(OrderAmt != transfer.OrderAmount)
                {
                    result.ErrorMsg = "订单总金额不等于提交的总金额，刷新页面，重新提交！";
                    result.IsSuccess = false;
                    return result;
                }
               

                if (transfer.CommissionAmount>0)
                {
                    List<RAgentCommission> commlist = db.DBAgentCommission.Where(s => s.ParentOpenId == transfer.OpenId
                                                                              && s.AgentCommissionStatus == IQBCore.IQBPay.BaseEnum.AgentCommissionStatus.Paid)
                                                     .Select(a => new RAgentCommission
                                                     {
                                                         CommissionAmount = a.CommissionAmount,
                                                     }).ToList();
                    CommAmt = commlist.Sum(a => a.CommissionAmount);

                    if (CommAmt != transfer.CommissionAmount)
                    {
                        result.ErrorMsg = "佣金总金额不等于提交的总金额，刷新页面，重新提交！";
                        result.IsSuccess = false;
                        return result;
                    }
                }

                float transferTotalAmt = transfer.OrderAmount + transfer.CommissionAmount;

                AlipayFundTransToaccountTransferResponse res2 = payManager.TransferAmount(BaseController.App, transfer.AliPayAccount, transferTotalAmt.ToString("0.00"), out TransferId);
                if (res2.Code == "10000")
                {
                    return result;
                }
                else
                {
                    result.ErrorMsg = string.Format("转账失败，代码;{0},原因{1}，请联系管理员。", res2.Code, res2.Msg);
                    result.IsSuccess = false;
                    return result;

                }


            }

            //AlipayFundTransToaccountTransferResponse res2 = payManager.TransferAmount(BaseController.App, transfer.AliPayAccount, transfer.Amount.ToString("0.00"), out TransferId);
            //if (res2.Code == "10000")
            //{
            //    ////通知开始
            //    //string accessToken = this.getAccessToken(true);
            //    //PPOrderPayNT notice = new PPOrderPayNT(accessToken, ui.OpenId, order);
            //    //notice.Push();
            //    ////通知结束
            //    using (AliPayContent db = new AliPayContent())
            //    {
                   
            //    }
            //        //转账记录开始
            //    ETransferAmount tranfer = ETransferAmount.Init(TransferId, transfer.OpenId, transfer.AliPayAccount, order);

            //    //tranfer.Buyer_AliPayId = order.BuyerAliPayId;
            //    //tranfer.Buyer_AliPayLoginId = order.BuyerAliPayLoginId;
            //    //db.DBTransferAmount.Add(tranfer);
            //    //order.OrderStatus = IQBCore.IQBPay.BaseEnum.OrderStatus.Closed;
            //    //order.TransferId = TransferId;
            //    //order.TransferAmount = tranfer.TransferAmount;

            //    ////转装记录结束
            //    //order.LogRemark += string.Format("[Transfer] Code:{0};msg:{1}", res2.Code, res2.Msg);
            //}
            
        }
    }
}
