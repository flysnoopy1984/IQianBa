using IQBCore.OO.BaseEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.OO.Models.Entity
{
    [Table("OrderInfo")]
    public class EOrderInfo: EBaseRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long ItemId { get; set; }

        /// <summary>
        /// 买家UserId
        /// </summary>
        public long BuyerUserId { get; set; }

        /// <summary>
        /// 卖家UserId
        /// </summary>
        public int StoreId { get; set; }

        [MaxLength(150)]
        public string ItemName { get; set; }

        public int ItemQty { get; set; }

        public double ItemPrice { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public DateTime ModifyTime { get; set; }


    }
}
