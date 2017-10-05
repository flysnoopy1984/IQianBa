using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.Model
{
    public abstract class BasePageModel:BaseModel
    {
        [NotMapped]
        [DefaultValue(0)]
        public int TotalCount { get; set; }
    }
}
