using IQBCore.IQBPay.Models.Store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Result
{
    [NotMapped()]
    public class RStoreInfo:EStoreInfo
    {
        public bool IsAuth { get; set; }

     

    }
}
