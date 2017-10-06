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

        public long QRId { get; set; }

        /// <summary>
        /// 返点率0-100
        /// </summary>
        public float Rate { get; set; }

        [MaxLength(128)]
        public string TargetUrl { get; set; }

        [MaxLength(128)]
        public string FilePath { get; set; }

       
          

    }
}