using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.OutParameter
{
    public class SResult<T> where T:class
    {
        public SResult()
        {
            IsSuccess = true;
            SuccessMsg = "成功";
        }
        public bool IsSuccess { get; set; }

        private string _ErrorMsg;
        public string ErrorMsg
        {
            get { return _ErrorMsg; }
            set
            {
                IsSuccess = false;
                _ErrorMsg = value;
                SuccessMsg = "";
            }
        }

        public T Instance
        {
            get;set;
        }

        public string SuccessMsg { get; set; }


        public int IntMsg { get; set; }

    }
}
