﻿using IQBCore.IQBPay.BaseEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Result
{
    [NotMapped]
    public class RUserInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string AliPayAccount { get; set; }

        public UserStatus UserStatus { get; set; }

        public Boolean IsAutoTransfer { get; set; }

        public int TotalCount { get; set; }

        public string CDate { get; set; }

        public string MDate { get; set; }

        public string Headimgurl { get; set; }

        public string QRFilePath { get; set; }

        [DefaultValue(0)]
        public float? Rate { get; set; }

        public float? ParentCommissionRate { get; set; }

        public string ParentAgent { get; set; }

        public UserRole UserRole { get; set; }

       
        public string UserRoleName
        {
            get
            {
                switch (this.UserRole)
                {
                    case UserRole.NormalUser:
                        return "普通用户";
                    case UserRole.StoreMaster:
                        return "高级商户";
                    case UserRole.StoreVendor:
                        return "商户";
                    case UserRole.Agent:
                        return "代理";
                    case UserRole.Administrator:
                        return "管理员";

                }
                return "";

            }
        }

        [NotMapped]
        public bool QueryResult { get; set; }
    }
}