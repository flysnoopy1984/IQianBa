using IQBCore.IQBPay.BaseEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Result
{
    public class RUser_ARQR
    {

        public int userId { get; set; }
        public string HeadImgUrl { get; set; }
        public string UserName { get; set; }

        public float? Rate { get; set; }

        public float? ParentCommissionRate { get; set; }

        public UserStatus UserStatus { get; set; }

        public string ParentOpenId { get; set; }

        public float? MarketRate { get; set; }

        public float? FeeRate { get; set; }

        private double? _MemberTotalAmount;

       
        public double? MemberTotalAmount
        {
            get { return _MemberTotalAmount; }
            set {
                if (value == null)
                    _MemberTotalAmount = 0;
                else
                    _MemberTotalAmount = value;
            }
        }

        public int TotalMember { get; set; }

        public int MaxInviteCount { get; set; }

        public float TotalAmount { get; set; }

        public string OpenId { get; set; }

        public string RegisterDate { get; set; }

        public RUser_ARQR HugeQR { get; set; }

    }
}
