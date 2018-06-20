using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.OO.Models.Entity
{
    [Table("UserTask")]
    public class EUserTask:EBaseRecord
    {
        public long TaskId { get; set; }

        public long UserId { get; set; }

        public TaskStatus TaskStatus { get; set; }
    }
}
