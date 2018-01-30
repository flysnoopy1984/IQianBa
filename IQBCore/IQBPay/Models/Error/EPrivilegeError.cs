using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Error
{
    public class EPrivilegeError
    {
        
        public string Title { get; set; }

        public string Content { get; set; }

        public string NeedPoint { get; set; }

        public string CurrentPoint { get; set; }

        public string RemainPoint { get; set; }


    }
}
