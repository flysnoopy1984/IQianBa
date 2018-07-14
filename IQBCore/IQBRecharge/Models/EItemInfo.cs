using IQBCore.IQBRecharge.BaseEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBRecharge.Models
{
    [Table("ItemInfo")]
    public class EItemInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public CardType CardType { get; set; }

        /// <summary>
        /// 卡类型描述
        /// </summary>
        [MaxLength(50)]
        public string CardTypeName { get; set; }

        /// <summary>
        /// 卡面值
        /// </summary>
        [MaxLength(50)]
        public string CardValue { get; set; }

        /// <summary>
        /// 回收价格
        /// </summary>
        public double RecValue { get; set; }
       
    }
}
