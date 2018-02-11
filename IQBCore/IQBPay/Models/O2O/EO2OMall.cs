using IQBCore.IQBPay.BaseEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.O2O
{
    [Table("O2OMall")]
    public class EO2OMall
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DefaultValue("")]
        [MaxLength(50)]
        public string Name { get; set; }

        [DefaultValue("")]
        [MaxLength(20)]
        public string Code { get; set; }

        [DefaultValue("")]
        [MaxLength(100)]
        public string Description { get; set; }

        public int O2ORuleId { get; set; }

        public RecordStatus RecordStatus { get; set; }

        public void InitFromUpdate(EO2OMall updateObj)
        {
            this.Name = updateObj.Name;
            this.Code = updateObj.Code;
            this.Description = updateObj.Description;
            this.O2ORuleId = updateObj.O2ORuleId;
            this.RecordStatus = updateObj.RecordStatus;
        }
    }
}
