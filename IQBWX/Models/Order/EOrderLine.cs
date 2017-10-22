using IQBCore.IQBWX.BaseEnum;
using IQBWX.Common;
using IQBWX.Models.Product;
using IQBWX.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IQBWX.Models.Order
{
    [Table("OrderLine")]
    public class EOrderLine
    {
        [MaxLength(20)]
        public string ItemId { get; set; }

        [MaxLength(40)]
        public string ItemName { get; set; }
        public int UserId { get; set; }

        public int MemberId { get; set; }

        [MaxLength(200)]
        public string OpenId { get; set; }

        [Key]
        [MaxLength(20)]
        public string OrderId { get; set; }
        public decimal LineAmount { get; set; }
        public int Qty { get; set; }
        public PaymentState PaymentState { get; set; }

        private DateTime _CreateDateTime = DateTime.MaxValue;
        public DateTime CreateDateTime
        {
            get
            {
                return _CreateDateTime;
            }
            set
            {
                _CreateDateTime = value;
            }
        }

        public MemberType GetMemberLevel()
        {
            if (ItemId == EItemInfo.Item158)
                return MemberType.Channel;
            else
                return MemberType.City;
        }

        public static string GenerateOrderNo(int userId,int TC)
        {
            return DateTime.Now.ToString("yyyyMMddhh")+"_"+ userId.ToString()+"_"+Convert.ToInt32(TC);
        }
    }
}