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
    [Table("O2OTransAgent")]
    public class EO2OTransAgent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public DateTime TransDateTime { get; set; }

        public double Amount { get; set; }

        public TransactionType TransactionType { get; set; }

        public string AgentOpenId { get; set; }
    }
}
