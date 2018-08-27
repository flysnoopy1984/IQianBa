using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.BLL;
using IQBCore.IQBPay.Models.AccountPayment;
using IQBCore.IQBPay.Models.InParameter;
using IQBCore.IQBPay.Models.Order;
using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.IQBPay.Models.Report;
using IQBPay.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IQBPay.Controllers.ExternalAPI
{
    public class BuyerController : ApiController
    {
        [HttpPost]
        /// <summary>
        /// 打款给买家（终端用户）
        /// </summary>
        /// <returns></returns>
        public OutAPIResult TransferAmountToBuyer(InTransferAmountToBuyer InParameter)
        {
            OutAPIResult result = new OutAPIResult();
            AliPayManager payManager = new AliPayManager();
            ETransferAmount tranfer = null;
            EOrderInfo order = null;
            EReport_Order _ReportOrder = new EReport_Order();
           
            try
            {
              
               // base.ActionContext.Response.Headers.Server["Access-Control-Allow-Origin"] = "http:localhost:8080/";
                using (AliPayContent db = new AliPayContent())
                {
                    order = db.DBOrder.Where(a => a.OrderNo == InParameter.OrderNo).FirstOrDefault();
                    if (order == null) result.ErrorMsg = "订单没有找到，请联系客服";
                    else if(order.OrderStatus != OrderStatus.Paid) result.ErrorMsg = "订单状态不正确，无法收款，请联系客服";
                    else
                    {
                        order.BuyerAliPayAccount = InParameter.AliAccount;
                       
                        tranfer = payManager.TransferHandler(TransferTarget.User, BaseController.App, BaseController.SubApp, null, ref order, 0, null, BaseController.GlobalConfig);
                        if (tranfer.TransferStatus == TransferStatus.Success)
                        {
                            order.OrderStatus = OrderStatus.Closed;
                            db.DBTransferAmount.Add(tranfer);

                            AliPayController payController = new AliPayController();
                            payController.UpdateUserBalance(db, order.AgentOpenId, order.RateAmount, TransactionType.Agent_Order_Comm,ref _ReportOrder);

                            _ReportOrder.BuyerInCome = tranfer.TransferAmount;
                            _ReportOrder.BuyerPhone = order.BuyerMobilePhone;

                            _ReportOrder.QRUserId = order.QRUserId.ToString();
                            _ReportOrder.QRType = QRReceiveType.CreditCard; 
                            _ReportOrder.TransDate = order.TransDate;
                            _ReportOrder.OrderNo = order.OrderNo;
                            _ReportOrder.OrderAmount = order.TotalAmount;
                            _ReportOrder.CaluPPInCome();
                            
                            db.DBReportOrder.Add(_ReportOrder);

                            db.SaveChanges();
                        }
                        else
                            result.ErrorMsg = "收款失败,请联系客服";
                    }
                }      
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.ErrorMsg = ex.Message;
            }
            return result;
        }
    }
}
