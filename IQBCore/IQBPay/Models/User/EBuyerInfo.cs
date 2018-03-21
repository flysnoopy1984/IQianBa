using IQBCore.IQBPay.BaseEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.User
{
    [Table("BuyerInfo")]
    public class EBuyerInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        public DateTime LastTransDate { get; set; }

        public EBuyerInfo()
        {
            LastTransDate = DateTime.Now;
        }

        [NotMapped()]
        public bool HasPhone { get; set; }

        [MaxLength(100)]
        public string AliPayAccount { get; set; }

        public DateTime TransDate { get; set; }

        public BuyerType BuyerType { get; set; }
    }
}
