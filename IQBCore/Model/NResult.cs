using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.Model
{
    public class NResult<T>
    {
        public NResult()
        {
            IsSuccess = true;
            SuccessMsg = "成功";
            resultList = new List<T>();
        }
        public T resultObj { get; set; }

        public List<T> resultList { get; set; }

        public Dictionary<string,T> resultDict { get; set; }

        private bool _IsSuccess;
        public bool IsSuccess
        {
            get
            {
                return _IsSuccess;
            }
            set
            {
                _IsSuccess = value;
                if (!_IsSuccess)
                    SuccessMsg = "";
            }
        }
        private string _ErrorMsg;
        public string ErrorMsg
        {
            get { return _ErrorMsg; }
            set {
                _IsSuccess = false;
                _ErrorMsg = value;
                SuccessMsg = "";
            }
        }

        public string SuccessMsg { get; set; }

        public int IntMsg { get; set; }

    }
}
