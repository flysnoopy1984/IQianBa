
using IQBCore.IQBPay.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IQBCore.IQBPay.Models.Result
{
    public class RUserInfo:EUserInfo
    {
        public string UserRoleName
        {
            get
            {
                switch(this.UserRole)
                {
                    case IQBPay.BaseEnum.UserRole.NormalUser:
                        return "普通用户";
                    case IQBPay.BaseEnum.UserRole.StoreMaster:
                        return "高级商户";
                    case IQBPay.BaseEnum.UserRole.StoreVendor:
                        return "商户";
                     
                }
                return "";
               
            }
        }

        [NotMapped]
        public bool QueryResult { get; set; }

        [NotMapped]
        public float Rate { get; set; }

        [NotMapped]
        public string QRFilePath { get; set; }

        public void InitFromChild(EUserInfo ui)
        {
            this.CDate = ui.CDate;
            this.CreateDate = ui.CreateDate;
            this.CreateUser = ui.CreateUser;
            this.CTime = ui.CTime;
            this.Headimgurl = ui.Headimgurl;
            this.Isadmin = ui.Isadmin;
            this.MDate = ui.MDate;
            this.ModifyDate = ui.ModifyDate;
            this.ModifyUser = ui.ModifyUser;
            this.MTime = ui.MTime;
            this.Name = ui.Name;
            this.parentOpenId = ui.parentOpenId;
            this.QRUserDefaultId = ui.QRUserDefaultId;
            this.UserRole = ui.UserRole;
            this.UserStatus = ui.UserStatus;
            this.Id = ui.Id;
        }


     

    }
}