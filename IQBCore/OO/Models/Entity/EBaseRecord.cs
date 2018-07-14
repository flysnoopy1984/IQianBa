using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.OO.Models.Entity
{
    public class EBaseRecord
    {
        public EBaseRecord()
        {
            CreatedTime = DateTime.Now;
        }
        public DateTime CreatedTime { get; set; }
    }
}
