﻿using IQBPay.Models.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IQBPay.DataBase
{
    public class AliPayContent: DbContext
    {
        public AliPayContent() : base("PPConnection")
        {

        }

        public DbSet<EUserInfo> UserInfoDB { get; set; }

        
    }
}