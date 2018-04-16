using IQBCore.IQBPay.BaseEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.O2O
{
    public class RO2OOrder_ForAgent
    {

        public string O2ONo { get; set; }

        public string ItemName { get; set; }



       
        public  string CreateDateTime { get; set; }
       

        public string O2OOrderStatusStr { get; set; }

        private O2OOrderStatus _O2OOrderStatus;

        public O2OOrderStatus O2OOrderStatus
        {
            get { return _O2OOrderStatus; }
            set
            {
                _O2OOrderStatus = value;
                O2OOrderStatusStr = IQBPayEnum.GetO2OName(value);

            }
        }

        public double OrderAmount { get; set; }

        public string User { get; set; }

        public O2OOrderType O2OOrderType { get; set; }
    }
}
