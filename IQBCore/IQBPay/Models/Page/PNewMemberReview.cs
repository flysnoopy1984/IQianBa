using IQBCore.IQBPay.BaseEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Page
{
    public class PNewMemberReview
    {
        public string NewOpenId { get; set; }

        public string NewName { get; set; }

        public UserStatus UserStatus { get; set; }
    }
}
