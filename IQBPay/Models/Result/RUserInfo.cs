using IQBPay.Controllers;
using IQBPay.Core.BaseEnum;
using IQBPay.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQBPay.Models.Result
{
    public class RUserInfo:EUserInfo
    {
        public string UserRoleName
        {
            get
            {
                return Enum.GetName(typeof(UserRole),this.UserRole);
            }
        }

     

    }
}