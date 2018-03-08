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
        }
        public T resultObj { get; set; }

        public List<T> resultList { get; set; }

        public bool IsSuccess { get; set; }
        public string ErrorMsg { get; set; }

        public string SuccessMsg { get; set; }

        public int IntMsg { get; set; }

    }
}
