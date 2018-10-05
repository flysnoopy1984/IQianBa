using System;
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
            SuccessMsg = "成功";
        }
        public bool IsSuccess { get; set; }

        private string _ErrorMsg;
        public string ErrorMsg
        {
            get { return _ErrorMsg; }
            set {
                IsSuccess = false;
                _ErrorMsg = value;
                SuccessMsg = "";
            }
        }

        public string SuccessMsg { get; set; }

        /// <summary>
        /// -10 已经存在
        /// </summary>
        public  int IntMsg { get; set; }

       
    }
}
