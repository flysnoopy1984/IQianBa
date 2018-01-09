using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.IQBPay.Models.QR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Result
{
    [NotMapped()]
    public class RQRInfo : EQRInfo
    {
        public string ParentName { get; set; }

        public string StoreName { get; set; }

        public UserRole UserRole { get; set; }

        public List<HashStore> StoreList { get; set; }

        public List<HashUser> ParentAgentList { get; set; }
    }
}
