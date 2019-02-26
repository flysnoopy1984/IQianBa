using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel
{
    [Table("Room")]
    public class ERoom
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(40)]
        public string Code { get; set; }

        /// <summary>
        /// 重量（0= 1-10倍 1 = 10-100 2= 100-1000 3=MAX）
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// 进房间的用户数
        /// </summary>
        public int PlayerCount { get; set; }


        public DateTime CreateTime { get; set; }

        
    }
}
