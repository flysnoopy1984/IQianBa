using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBRecharge.Models
{
    [Table("UserAccountBalance")]
    public class EUserAccountBalance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(32)]
        public string UserOpenId { get; set; }

        /// <summary>
        ///针对出库商的余额
        /// </summary>
        public double Balance { get; set; }
      
    }
}
