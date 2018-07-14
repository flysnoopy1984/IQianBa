using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBRecharge.Models
{
    [Table("UserInfo")]
    public class EUserInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(32)]
        public string UserOpenId { get; set; }
        /// <summary>
        /// 用户对应的代理
        /// </summary>
        [MaxLength(32)]
        public string AgentOpenId { get; set; }

        [MaxLength(20)]
        public string LoginPhone { get; set; }

        [MaxLength(20)]
        public string Pwd { get; set; }


        public DateTime RegisterDateTime { get; set; }

        public DateTime LastLoginDateTime { get; set; }

        public EUserInfo()
        {
            RegisterDateTime = DateTime.Now;
            LastLoginDateTime = DateTime.Now;
        }
    }
}
