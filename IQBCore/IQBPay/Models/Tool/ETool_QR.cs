using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Tool
{
    [Table("Tool_QR")]
    public class ETool_QR
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public float CouponAmt { get; set; }

        public float InputAmt { get; set; }

        public float OrderAmt { get; set; }

        [MaxLength(128)]
        public string FilePath { get; set; }
    }
}
