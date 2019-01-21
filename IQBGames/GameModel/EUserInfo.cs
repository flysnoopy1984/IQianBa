using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel
{
    [Table("UserInfo")]
    public class EUserInfo
    {
       
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Key]
        [MaxLength(32)]
        public string openId { get; set; }

        [MaxLength(40)]
        public string nickName { get; set; }

        [MaxLength(256)]
        public string wxHeadImgUrl { get; set; }

        public int sex { get; set; }
        [MaxLength(20)]
        public string province { get; set; }

        public DateTime RegisterDate { get; set; }

        public DateTime LastLogin { get; set; }

        public int LoginCount { get; set; }
    }
}
