﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.OutParameter
{
    public class OutAPIResult
    {
        public OutAPIResult()
        {
            IsSuccess = true;
            SuccessMsge = "成功";
        }
        public bool IsSuccess { get; set; }
        public string ErrorMsg { get; set; }

        public string SuccessMsge { get; set; }

       
    }
}