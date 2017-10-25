using IQBCore.IQBPay.BaseEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.SMS
{
    [Table("SMSVerification")]
    public class ESMSVerification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [MaxLength(64)]
        public string OrderNo { get; set; }

        [MaxLength(10)]
        public string VerifyCode { get; set; }

        [MaxLength(10)]
        public string ReceiveNo { get; set; }

        [MaxLength(20)]
        public string MobilePhone { get; set; }

        public SMSVerifyStatus SMSVerifyStatus { get; set; }

        public SMSEvent SMSEvent { get; set; }

        private DateTime _SendDateTime = DateTime.MaxValue;
        public DateTime SendDateTime
        {
            get
            {
                return _SendDateTime;
            }
            set
            {
                _SendDateTime = value;
            }
        }


    }
}
