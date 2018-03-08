using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.O2O
{
    [Table("O2OStep")]
    public class EO2OStep
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Code { get; set; }

        public int Seq { get; set; }

        /* Step Content */
        [MaxLength(50)]
        public string LeftName { get; set; }

        [DataType(DataType.Html)]
        public string BeginContent { get; set; }

        [DataType(DataType.Html)]
        public string EndContent { get; set; }

        public void InitFromUpdate(EO2OStep obj)
        {
            this.Code = obj.Code;
            this.Seq = obj.Seq;
            this.LeftName = obj.LeftName;
            this.BeginContent = obj.BeginContent;
            this.EndContent = obj.EndContent;

        }
    }
}
