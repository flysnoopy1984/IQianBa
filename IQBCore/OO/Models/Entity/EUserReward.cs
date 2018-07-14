using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.OO.Models.Entity
{
    [Table("UserReward")]
    public class EUserReward
    {
        [Key]
        public long UserId { get; set; }

        /// <summary>
        /// 广告回报率
        /// </summary>
        public double ADRewardRate { get; set; }

        /// <summary>
        /// 刷单回报率
        /// </summary>
        public double OrderRewardRate { get; set; }

        /// <summary>
        /// 推广回报率
        /// </summary>
        public double IntroRate { get; set; }
    }
}
