using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.OO.Models.Entity
{
    [Table("UserRelation")]
    public class EUserRelation:EBaseRecord
    {
        public EUserRelation()
        {
            PId = 0;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long UserId { get; set; }

        [MaxLength(50)]
        public string UserName { get; set; }

        public long PId { get; set; }

        [MaxLength(50)]
        public string PName { get; set; }

    }
}
