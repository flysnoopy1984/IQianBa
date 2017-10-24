using IQBCore.IQBPay.BaseEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.AccountPayment
{
    [Table("AgentCommission")]
    public class EAgentCommission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [MaxLength(64)]
        public string OrderNo { get; set; }

        public int Level { get; set; }

        public float CommissionRate { get; set; }

        public float CommissionAmount { get;set;}

        [MaxLength(32)]
        public string ParentOpenId { get; set; }

        [MaxLength(40)]
        public string ParentName { get; set; }

        [MaxLength(32)]
        public string ChildOpenId { get; set; }

        [MaxLength(40)]
        public string ChildName { get; set; }

        public AgentCommissionStatus AgentCommissionStatus { get; set; }



    }
}
