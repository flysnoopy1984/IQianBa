using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.User
{
    [Table("UserStore")]
    public class EUserStore
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(32)]
        public string OpenId { get; set; }

        /// <summary>
        /// 码主费率
        /// </summary>
        public float OwnerRate { get; set; }

        /// <summary>
        /// 当前返佣
        /// </summary>
        public float Rate { get; set; }

        /// <summary>
        /// 下级固定返佣
        /// </summary>
        public float FixComm { get; set; }

      
    }
}
