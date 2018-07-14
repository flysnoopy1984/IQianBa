using IQBCore.OO.BaseEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace IQBCore.OO.Models.Query
{
    public class QTask
    {

        public long TaskId { get; set; }
        /// <summary>
        /// 正在接受的用户Id
        /// </summary>
        public long AcceptUserId { get; set; }

        public UserTaskStatus TaskStatus { get; set; }

        public RecordStatus RecordStatus { get; set; }

        public TaskType TaskType { get; set; }


        public int pageIndex { get; set; }

        public int pageSize { get; set; }
    }
}
