using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.OO.Models.Entity
{
    [Table("UserQRInvite")]
    public class EUserQRInvite:EBaseRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long UserId { get; set; }

        /// <summary>
        /// 二维码对应的地址
        /// </summary>
        public string QRUrl { get; set; }

        /// <summary>
        /// 二维码存放地址
        /// </summary>
        public string QRPath { get; set; }

        /// <summary>
        /// 一般的邀请码
        /// </summary>
        [MaxLength(20)]
        public string InviteCode { get; set; }
    }
}
