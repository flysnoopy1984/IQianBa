using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.O2O
{
    [Table("O2OStepTemplate")]
    public class EO2OStepTemplate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [MaxLength(20)]
        public int Code { get; set; }

        [MaxLength(50)]
        public string LeftName { get; set; }

        [DataType(DataType.Text)]
        public string BeginContent { get; set; }

        [DataType(DataType.Text)]
        public string EndContent { get; set; }
    }
}
