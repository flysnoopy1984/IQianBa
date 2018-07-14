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
    [Table("RcOrder")]
    public class ERcOrderInfo
    {

        public ERcOrderInfo()
        {
            CreateDateTime = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long ItemId { get; set; }

        [MaxLength(50)]
        public string No { get; set; }


        [MaxLength(32)]
        public string UserOpenId { get; set; }

        public int Qty { get; set; }

        [MaxLength(100)]
        public string UserCardNo { get; set; }

        [MaxLength(100)]
        public string UserCardPwd { get; set; }

        public RcOrderStatus Status { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
