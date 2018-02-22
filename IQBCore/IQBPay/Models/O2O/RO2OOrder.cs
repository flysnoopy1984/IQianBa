using IQBCore.IQBPay.BaseEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.O2O
{
    [NotMapped()]
    public class RO2OOrder:EO2OOrder
    {
        public string CreateDateTimeStr { get; set; }
        private DateTime _CreateDateTime;
        public new DateTime CreateDateTime
        {
            get { return _CreateDateTime; }
            set
            {
                _CreateDateTime = value;
                CreateDateTimeStr = _CreateDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        public string O2OOrderStatusStr { get; set; }

        private O2OOrderStatus _O2OOrderStatus;

        public new O2OOrderStatus O2OOrderStatus
        {
            get { return _O2OOrderStatus; }
            set
            {
                _O2OOrderStatus = value;
                O2OOrderStatusStr =IQBPayEnum.GetO2OName(value);
              
            }
        }

        public string MallName { get; set; }

        public string Address { get; set; }

        public string ItemName { get; set; }

    }
}
