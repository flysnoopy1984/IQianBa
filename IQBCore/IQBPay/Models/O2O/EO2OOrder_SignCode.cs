using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.O2O
{
    [Table("O2OOrder_SignCode")]
    public class EO2OOrder_SignCode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(20)]
        public string O2ONo { get; set; }

        [MaxLength(255)]
        public string SignCode { get; set; }
    }
}
