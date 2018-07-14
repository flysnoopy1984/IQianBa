using IQBCore.OO.BaseEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.OO.Models.Query
{
    public class QItem
    {

        public int pageIndex { get; set; }
        
        public int pageSize { get; set; }

        public RecordStatus RecordStatus { get; set; }


    }
}
