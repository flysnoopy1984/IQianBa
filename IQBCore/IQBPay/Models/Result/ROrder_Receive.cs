﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Result
{
    public class ROrder_Receive
    {
        public string OrderNo { get; set; }
        public string TransDateStr { get; set; }

        
        public float Amount { get; set; }
    }
}
