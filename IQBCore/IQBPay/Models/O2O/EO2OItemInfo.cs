﻿using IQBCore.IQBPay.BaseEnum;
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

        [DefaultValue("")]
        [MaxLength(50)]
        public string Name { get; set; }

        public int MallId { get; set; }

        [DefaultValue("")]
        [MaxLength(500)]
        public string RealAddress { get; set; }

        [DefaultValue("")]
        [MaxLength(255)]
        public string ImgUrl { get; set; }

        public double Amount { get; set; }

        public int Qty { get; set; }

        public int O2ORuleId { get; set; }

        public RecordStatus RecordStatus { get; set; }

        public int PriceGroupId { get; set; }

        public DateTime CreateDateTime
        { get; set; }

        public DateTime ModifyDateTime { get; set; }

        [DefaultValue("")]
        public string ModifyUser { get; set; }

        public void InitFromUpdate(EO2OItemInfo updateObj)
        {
            this.Name = updateObj.Name;
          
            this.MallId = updateObj.MallId;
            this.RealAddress = updateObj.RealAddress;
            this.ImgUrl = updateObj.ImgUrl;
            this.Amount = updateObj.Amount;
            this.Qty = updateObj.Qty;
            this.O2ORuleId = updateObj.O2ORuleId;
            this.RecordStatus = updateObj.RecordStatus;
            this.CreateDateTime = DateTime.Now;
            this.ModifyDateTime = DateTime.Now;
            this.PriceGroupId = updateObj.PriceGroupId;
        }
    }
}
