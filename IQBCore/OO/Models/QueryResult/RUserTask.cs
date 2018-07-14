using IQBCore.OO.BaseEnum;
using IQBCore.OO.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.OO.Models.QueryResult
{
    [NotMapped()]
    public class RUserTask:EUserTask
    {
        public string ItemName { get; set; }

        public string TaskDescription { get; set; }

        public string RealUrl { get; set; }

        public double Price { get; set; }

        public TaskType TaskType { get; set; }
    }
}
