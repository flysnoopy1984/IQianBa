using IQBCore.IQBWX.BaseEnum;
using IQBWX.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQBWX.Models.Results
{
    public class DataMemberCenter
    {
        public decimal Balance { get; set; }
        public decimal TotalIncome { get; set; }
        public int ChildrenNum { get; set; }
        public string HeaderImg { get; set; }
        public MemberType MemberType { get; set; }
        public string NickName { get; set; }

        public string MemberTypeValue { get; set; }
    }
}