﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.OutParameter
{
    public class OutDoTransfer
    {
        public bool IsSuccess { get; set; }
        public string ErrorMsg { get; set; }
    }
}
