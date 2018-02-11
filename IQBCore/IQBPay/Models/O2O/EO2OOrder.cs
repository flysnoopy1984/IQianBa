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
    [Table("O2OOrder")]
    public class EO2OOrder
    {
        public EO2OOrder()
        {
            CreateDateTime = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(20)]
        public string O2ONo { get; set; }
        public long RefOrderId { get; set; }

        [MaxLength(20)]
        public string UserPhone { get; set; }

        [MaxLength(100)]
        public string UserAliPayAccount { get; set; }

        public int MallId { get; set; }

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

        public O2OOrderStatus O2OOrderStatus { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
