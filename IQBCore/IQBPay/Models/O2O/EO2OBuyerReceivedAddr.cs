using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.O2O
{
    [Table("O2OBuyerReceiveAddr")]
    public class EO2OBuyerReceiveAddr
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public  long Id { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        public int ItemId { get; set; }

        public int AddrId { get; set; }

        public int MallId { get; set; }

      
    }
}
