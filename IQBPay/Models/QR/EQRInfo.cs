using IQBCore.Model;
using IQBPay.Core.BaseEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IQBPay.Models.QR
{
    [Table("QRInfo")]
    public class EQRInfo: BasePageModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [MaxLength(32)]
        /// <summary>
        /// 拥有者
        /// </summary>
        public string OwnnerOpenId { get; set; }

        /// <summary>
        /// 返点率0-100
        /// </summary>
        public float Rate { get; set; }

        [MaxLength(20)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Remark { get; set; }

        public RecordStatus RecordStatus { get; set; }

        public QRChannel Channel { get; set; }

        public QRType Type { get; set; }

        [MaxLength(128)]
        public string TargetUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(128)]
        public string FilePath { get; set; }

    }
}