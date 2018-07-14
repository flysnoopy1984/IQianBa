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
    [Table("UserTaskOrder")]
    public class EUserTaskOrder : EBaseRecord
    {
        [Key]
        public string  OrderId { get; set; }

        public long UserTaskId { get; set; }

        public int Qty { get; set; }

    }
}
