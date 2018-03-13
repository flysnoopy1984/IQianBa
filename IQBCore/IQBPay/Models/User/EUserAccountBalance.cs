﻿using IQBCore.IQBPay.BaseEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.User
{
    [Table("UserAccountBalance")]
    public class EUserAccountBalance
    {
        public EUserAccountBalance()
        {
          //  TransDate = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string OpenId { get; set; }

        public UserAccountType UserAccountType { get; set; }
 
        [MaxLength(100)]
        public string AliPayAccount { get; set; }

        /// <summary>
        ///针对出库商的余额
        /// </summary>
        public double O2OShipBalance { get; set; }

        ///// <summary>
        ///// 针对出库商的商品被购买中的订单数量
        ///// </summary>
        //public double O2OOnOrderAmount { get; set; }

     //   public DateTime TransDate { get; set; }

       

     
    }
}