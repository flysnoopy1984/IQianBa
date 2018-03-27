using IQBCore.IQBPay.BaseEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.O2O
{
    [Table("O2OItemInfo")]
    public class EO2OItemInfo
    {
        public EO2OItemInfo()
        {
            CreateDateTime = DateTime.Now;
            ModifyDateTime = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        /// <summary>
        /// 商品属于哪个供应商
        /// </summary>
        [MaxLength(32)]
        public string OpenId { get; set; }



        [DefaultValue("")]
        [MaxLength(50)]
        public string Name { get; set; }

        public string MallCode { get; set; }

        [DefaultValue("")]
        [MaxLength(500)]
        public string RealAddress { get; set; }

        [DefaultValue("")]
        [MaxLength(255)]
        public string ImgUrl { get; set; }

        public double Amount { get; set; }

        public int Qty { get; set; }

        [MaxLength(20)]
        public string O2ORuleCode { get; set; }

        public RecordStatus RecordStatus { get; set; }

        /// <summary>
        /// 出库商的费率
        /// </summary>
        public double ShipFeeRate { get; set; }

        public int PriceGroupId { get; set; }

        public DateTime CreateDateTime
        { get; set; }

        public DateTime ModifyDateTime { get; set; }

        [DefaultValue("")]
        public string ModifyUser { get; set; }

        


        /// <summary>
        /// 秒到
        /// </summary>
       public bool IsLightReceive { get; set; }


        /// <summary>
        /// 套现方式 花呗，白条
        /// </summary>
       public PayMethod PayMethod { get; set; }

    public void InitFromUpdate(EO2OItemInfo updateObj)
    {
        //this.Name = updateObj.Name;
          
        //this.MallId = updateObj.MallId;
        //this.RealAddress = updateObj.RealAddress;
        //this.ImgUrl = updateObj.ImgUrl;
        //this.Amount = updateObj.Amount;
        //this.Qty = updateObj.Qty;
        //this.O2ORuleId = updateObj.O2ORuleId;
        //this.RecordStatus = updateObj.RecordStatus;
        //this.CreateDateTime = DateTime.Now;
        //this.ModifyDateTime = DateTime.Now;
        //this.PriceGroupId = updateObj.PriceGroupId;
    }
    }
}
