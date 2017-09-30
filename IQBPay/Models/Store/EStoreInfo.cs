using IQBCore.Model;
using IQBPay.Core.BaseEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IQBPay.Models.Store
{
    [Table("StoreInfo")]
    public class EStoreInfo:BasePageModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// 拥有者
        /// </summary>
        [MaxLength(32)]
        public string OwnnerOpenId { get; set; }

        [MaxLength(20)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Remark { get; set; }

        /// <summary>
        /// 可能被禁用
        /// </summary>
        public RecordStatus RecordStatus{ get; set; }

    }
}