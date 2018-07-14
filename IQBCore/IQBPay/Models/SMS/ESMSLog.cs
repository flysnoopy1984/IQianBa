using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.SMS
{
    [Table("SMSLog")]
    public class ESMSLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [MaxLength(30)]
        public string APPName { get; set; }

        [MaxLength(15)]
        public string UserPhone { get; set; }


        public DateTime SendDateTime { get; set; }

        [MaxLength(100)]
        public string RequestMessage { get; set; }

        [MaxLength(200)]
        public string ResponseMessage { get; set; }

        public bool IsSuccess { get; set; }

        [MaxLength(200)]
        public string Exception { get; set; }

       
    }
}
