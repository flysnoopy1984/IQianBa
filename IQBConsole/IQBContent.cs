using IQBCore.Common.Helper;
using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.BLL;
using IQBCore.IQBPay.Models.AccountPayment;
using IQBCore.IQBPay.Models.O2O;
using IQBCore.IQBPay.Models.Order;
using IQBCore.IQBPay.Models.PayChannel;
using IQBCore.IQBPay.Models.QR;
using IQBCore.IQBPay.Models.Result;
using IQBCore.IQBPay.Models.SMS;
using IQBCore.IQBPay.Models.Store;
using IQBCore.IQBPay.Models.Sys;
using IQBCore.IQBPay.Models.Tool;
using IQBCore.IQBPay.Models.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBConsole
{
    public class IQBContent : DbContext
    {
        public IQBContent() : base("PPConnection")
        {

        }

      

        public DbSet<EAgentCommission> DBAgentCommission { get; set; }



        public DbSet<EStoreInfo> DBStoreInfo { get; set; }

        public DbSet<EQRInfo> DBQRInfo { get; set; }

        public DbSet<EQRUser> DBQRUser { get; set; }
        public DbSet<EAliPayApplication> DBAliPayApp { get; set; }

        public DbSet<EOrderInfo> DBOrder { get; set; }

        public DbSet<ETransferAmount> DBTransferAmount { get; set; }

        public DbSet<EGlobalConfig> DBGlobalConfig { get; set; }

        public DbSet<EBuyerInfo> DBBuyerInfo { get; set; }

        public DbSet<ETool_QR> DBTool_QR { get; set; }

        public DbSet<EQRHuge> DBQRHuge { get; set; }

        public DbSet<EQRHugeTrans> DBQRHugeTrans { get; set; }

        public DbSet<EOrderDetail> DBOrderDetail { get; set; }

        public DbSet<EPayChannel> DBPayChannel { get; set; }

        #region O2O

        public DbSet<EO2OItemInfo> DBO2OItemInfo { get; set; }

        public DbSet<EO2OMall> DBO2OMall { get; set; }

        public DbSet<EO2ORule> DBO2ORule { get; set; }

        public DbSet<EO2OPriceGroup> DBO2OPriceGroup { get; set; }

        public DbSet<EO2OOrder> DBO2OOrder { get; set; }

        public DbSet<EO2ODeliveryAddr> DBO2ODeliveryAddr { get; set; }

        public DbSet<EO2OTranscationWH> DBO2OTranscationWH { get; set; }

        public DbSet<EO2OBuyer> DBO2OBuyer { get; set; }

        public DbSet<EO2OStep> DBO2OStep { get; set; }

        public DbSet<RelRuleStep> DBO2ORelRuleStep { get; set; }

        public DbSet<EO2OAgentFeeRate> DBO2OAgentFeeRate { get; set; }

        public DbSet<EO2ORoleCharge> DBO2ORoleCharge { get; set; }

        public DbSet<EO2OBuyerReceiveAddr> DBO2OBuyerReceiveAddr { get; set; }



        #endregion
    }
}
