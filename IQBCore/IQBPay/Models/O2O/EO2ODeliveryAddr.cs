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
    [Table("O2ODeliveryAddress")]
    public class EO2ODeliveryAddr
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(20)]
        public string Code { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(10)]
        public string City { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }

        public RecordStatus RecordStatus { get; set; }

        public void InitFromUpdate(EO2ODeliveryAddr updateObj)
        {
            this.Name = updateObj.Name;
            this.Code = updateObj.Code;
            this.City = updateObj.City;
            this.Address = updateObj.Address;
            this.RecordStatus = updateObj.RecordStatus;
        }
    }
}
