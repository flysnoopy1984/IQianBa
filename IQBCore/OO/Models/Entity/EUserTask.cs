using IQBCore.Common.Constant;
using IQBCore.OO.BaseEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;


namespace IQBCore.OO.Models.Entity
{
    [Table("UserTask")]
    public class EUserTask:EBaseRecord
    {
        public EUserTask()
        {
            CompleteDateTime = DateTime.MaxValue;
            ErrorDateTime = DateTime.MaxValue;

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long Id { get; set; }
        public long TaskId { get; set; }

        public long UserId { get; set; }

        public UserTaskStatus UserTaskStatus { get; set; }

        public DateTime CompleteDateTime { get; set; }

        public DateTime ErrorDateTime { get; set; }
    }
}
