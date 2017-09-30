using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.Model
{
    public abstract class BasePageModel:BaseModel
    {
        [DefaultValue(0)]
        public int TotalCount { get; set; }
    }
}
