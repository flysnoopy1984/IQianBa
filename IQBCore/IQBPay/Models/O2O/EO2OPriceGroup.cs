using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.O2O
{
    [Table("O2OPriceGroup")]
    public class EO2OPriceGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(20)]
        public string Code { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public double FromPrice { get; set; }
        public double ToPrice { get; set; }

        public void InitFromUpdate(EO2OPriceGroup updateObj)
        {
            this.Name = updateObj.Name;
            this.Code = updateObj.Code;
            this.FromPrice = updateObj.FromPrice;
            this.ToPrice = updateObj.ToPrice;
          
        }
    }
}
