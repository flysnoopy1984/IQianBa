using IQBCore.OO.BaseEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.OO.Models.Entity
{
    [Table("UserBehavior")]
    public class EUserBehavior
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public UserBehaviorType Type { get; set; }

        public DateTime? LogDateTime { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }
    }
}
