using IQBCore.IQBPay.Models.QR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.Result
{
    [NotMapped()]
    public class RQRHuge:EQRHuge
    {
        
        public string CreateDateStr { get; set; }

        private DateTime _CreateDate;
        public new DateTime CreateDate {
            get { return _CreateDate; }
            set {
                _CreateDate = value;
                CreateDateStr = _CreateDate.ToString("MM/dd HH:mm:ss");
            }
        }

     
    }
}
