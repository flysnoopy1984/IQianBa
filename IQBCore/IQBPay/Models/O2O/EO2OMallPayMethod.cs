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
    /// <summary>
    ///暂时写死在js中
    /// </summary>
    [Table("O2OMallPayMethod")]
    public class EO2OMallPayMethod
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string MallCode { get; set; }

        public PayMethod PayMethod { get; set; }
    }
}
