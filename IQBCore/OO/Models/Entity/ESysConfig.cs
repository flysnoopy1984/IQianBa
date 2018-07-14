using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.OO.Models.Entity
{
    [Table("SysConfig")]
    public class ESysConfig
    {
        [Key]
        public string Code { get; set; }

        public double L1RewardRate { get; set; }

        public double L2RewardRate { get; set; }

        public double L3RewardRate { get; set; }

        public double ADRewardRate { get; set; }

        public double IntroRate { get; set; }
        
        public string CurCurrencyCode { get; set; }



        
    }
}
