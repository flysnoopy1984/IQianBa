using IQBCore.IQBPay.BaseEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Report
{
    [Table("Report_Order")]
    public class EReport_Order
    {
        [Key]
        [MaxLength(64)]
        public string OrderNo { get; set; }

        [MaxLength(32)]
        public string AgentOpenId { get; set; }

        [MaxLength(32)]
        public string A2OpenId { get; set; }

        [MaxLength(32)]
        public string A3OpenId { get; set; }

        [MaxLength(32)]
        public string StoreOpenId { get; set; }

        [MaxLength(32)]
        public string S2OpenId { get; set; }

        [MaxLength(32)]
        public string S3OpenId { get; set; }


        [MaxLength(20)]
        public string BuyerPhone { get; set; }

        public float OrderAmount { get; set; }

        public float AgentInCome { get; set; }

        public float A2InCome { get; set; }

        public float A3InCome { get; set; }

        public float StoreInCome { get; set; }

        public float S2InCome { get; set; }

        public float S3InCome { get; set; }

        public float StoreOwnerRateAmount { get; set; }

        public float BuyerInCome { get; set; }

        public float PPInCome { get; set; }

        public DateTime TransDate { get; set; }

        public string QRUserId { get; set; }

        public QRReceiveType QRType { get; set; }

       

        public void CaluPPInCome()
        {
            PPInCome = OrderAmount - AgentInCome - A2InCome - A3InCome - StoreOwnerRateAmount - StoreInCome - S2InCome - S3InCome - BuyerInCome;
            PPInCome = (float)Math.Round(PPInCome, 2, MidpointRounding.AwayFromZero);

        }

        public void CaluPPInCome_WX()
        {
            PPInCome = OrderAmount - AgentInCome - BuyerInCome;
            PPInCome = (float)Math.Round(PPInCome, 2, MidpointRounding.AwayFromZero);

        }
    }
}
