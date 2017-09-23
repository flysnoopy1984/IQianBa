using IQBWX.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IQBWX.Models.User
{
    [Table("UserSMSVerify")]
    public class EUserSMSVerify
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        [MaxLength(10)]
        public string VerifyCode { get; set; }

        [MaxLength(20)]
        public string SMSEvent { get; set; }

        public UserSMSStatus VerifyStatus { get; set; }

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