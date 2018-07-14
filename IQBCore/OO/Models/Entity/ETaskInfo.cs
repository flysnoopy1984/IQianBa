using IQBCore.OO.BaseEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;


namespace IQBCore.OO.Models.Entity
{

    [Table("TaskInfo")]
    public class ETaskInfo:EBaseRecord
    {

       
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 不同类型的任务对应的实际项目（可能是刷单 可能是广告或者其他）
        /// </summary>
        public long RefItemId { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }


        public TaskType TaskType { get; set; }

        public long CreatedUser { get; set; }

        public double RewardAmount { get; set; }

        public RecordStatus RecordStatus { get; set; }



    }
}
