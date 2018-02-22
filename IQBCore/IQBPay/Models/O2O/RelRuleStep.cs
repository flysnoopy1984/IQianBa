using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.O2O
{
    [Table("RelRuleStep")]
    public class RelRuleStep
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public  int Id { get; set; }

        [MaxLength(20)]
        public string RuleCode { get; set; }

        [MaxLength(50)]
        public string StepCode { get; set; }

        public int Seq { get; set; }

        public void InitFromUpdate(RelRuleStep obj)
        {
            this.RuleCode = obj.RuleCode;
            this.StepCode = obj.StepCode;
            this.Seq = obj.Seq;

        }
    }
}
