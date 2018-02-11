using IQBCore.IQBPay.BaseEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.O2O
{
    [Table("PreOrder")]
    public class EPreOrder
    {
        public EPreOrder()
        {
            CreateDateTime = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }


        public long RefOrderId { get; set; }

        [MaxLength(20)]
        public string UserPhone { get; set; }

        [MaxLength(100)]
        public string UserAliPayAccount { get; set; }

        [MaxLength(50)]
        public string MallAccount { get; set; }

        [MaxLength(50)]
        public string MallPwd { get; set; }

        [MaxLength(20)]
        public string MallSMSVerify { get; set; }

        [MaxLength(255)]
        public string OrderImgUrl { get; set; }

        [MaxLength(50)]
        public  string MallOrderId { get; set; }

        public O2OOrderStatus PreOrderStatus { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
