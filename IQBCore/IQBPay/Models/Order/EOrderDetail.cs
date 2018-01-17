using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Order
{
    [Table("OrderDetail")]
    public class EOrderDetail
    {
        [Key]
        public string OrderNo { get; set; }

       
        [MaxLength(512)]
        public  string fund_bill_list { get; set; }


    }
}
