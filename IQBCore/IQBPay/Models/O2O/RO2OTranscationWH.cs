using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.O2O
{
    [NotMapped()]
    public class RO2OTranscationWH: EO2OTranscationWH
    {
        public string TransDateTimeStr { get; set; }
        private DateTime _TransDateTime;
        public new DateTime TransDateTime
        {
            get { return _TransDateTime; }
            set
            {
                _TransDateTime = value;
                TransDateTimeStr = _TransDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        public string MallName { get; set; }

        public string ItemName { get; set; }
    }
}
