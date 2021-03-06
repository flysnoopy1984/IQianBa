﻿using IQBCore.IQBPay.BaseEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IQBCore.IQBPay.Models.QR
{
    [Table("QRUser")]
    public class EQRUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [MaxLength(32)]
        public string OpenId { get; set; }

        [MaxLength(40)]
        public string ParentOpenId { get; set; }

        [MaxLength(32)]

        public string ParentName { get; set; }

        public float ParentCommissionRate { get; set; }

        [MaxLength(40)]
        public string UserName { get; set; }

        public long QRId { get; set; }


        /// <summary>
        /// 返点率0-100
        /// </summary>
        private float _Rate;
        public float Rate
        {
            get
            {
                return _Rate;
            }
            set
            {
                _Rate = float.Parse(value.ToString("0.00"));
            }
        }

        /// <summary>
        /// 市场扣点，用户支付后，默认扣除这些点率
        /// </summary>
        private float _MarketRate { get; set; }
        public float MarketRate {
            get
            {
                return _MarketRate;
            }
            set
            {
                _MarketRate = float.Parse(value.ToString("0.00"));
            }
        }

        [MaxLength(128)]
        public string TargetUrl { get; set; }

        [MaxLength(128)]
        public string FilePath { get; set; }

        [MaxLength(128)]
        public string OrigQRFilePath { get; set; }


        public int ReceiveStoreId { get; set; }

        /// <summary>
        /// 是否当前收款码
        /// </summary>
        public bool IsCurrent { get; set; }

        public QRReceiveType QRType { get; set; }

        public RecordStatus RecordStatus { get; set; }


        /// <summary>
        /// 大额码是否自助
        /// </summary>
        public bool IsQRHugeAuto { get; set; }

        public static EQRUser CopyToQRUserForHuge(EQRUser qrSmall)
        {
            EQRUser QRhuge = new EQRUser
            {
                OpenId = qrSmall.OpenId,
                ParentOpenId = qrSmall.ParentOpenId,
                ParentName = qrSmall.ParentName,
                ParentCommissionRate = qrSmall.ParentCommissionRate,
                ReceiveStoreId = qrSmall.ReceiveStoreId,
                UserName = qrSmall.UserName,
                QRType = QRReceiveType.Huge,
            };
            return QRhuge;
        }

        public static EQRUser CopyToQRUserForCC(EQRUser qrSmall)
        {
            EQRUser QRhuge = new EQRUser
            {
                OpenId = qrSmall.OpenId,
                ParentOpenId = qrSmall.ParentOpenId,
                ParentName = qrSmall.ParentName,
                ParentCommissionRate = qrSmall.ParentCommissionRate,
                ReceiveStoreId = qrSmall.ReceiveStoreId,
                UserName = qrSmall.UserName,
                QRType = QRReceiveType.CreditCard,
            };
            return QRhuge;
        }

        public static EQRUser CopyToQRUserForO2O(EQRUser qrSmall)
        {
            EQRUser QRhuge = CopyToQRUserForHuge(qrSmall);
            QRhuge.QRType = QRReceiveType.O2O;
         
            return QRhuge;
        }




    }
}