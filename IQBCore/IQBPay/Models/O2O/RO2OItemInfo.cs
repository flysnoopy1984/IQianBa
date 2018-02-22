using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.O2O
{
    [NotMapped()]
    public class RO2OItemInfo:EO2OItemInfo
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

        public string ModifyDateTimeStr { get; set; }
        private DateTime _ModifyDateTime;
        public new DateTime ModifyDateTime
        {
            get { return _ModifyDateTime; }
            set
            {
                _ModifyDateTime = value;
                ModifyDateTimeStr = _ModifyDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

       
       




    }
}
