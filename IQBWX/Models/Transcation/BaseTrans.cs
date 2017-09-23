using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IQBWX.Models.Transcation
{
    public class BaseTrans
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransId { get; set; }

        [MaxLength(32)]
        public string openId { get; set; }

       
        public decimal Amount { get; set; }
        [MaxLength(10)]
        public string TransDate { get; set; }
        [MaxLength(10)]
        public string TransTime { get; set; }
        private DateTime _TransDateTime = DateTime.MaxValue;
        public DateTime TransDateTime
        {
            get
            {
                if (_TransDateTime == DateTime.MaxValue)
                    _TransDateTime = DateTime.Now;
                return _TransDateTime;
            }
            set
            {
                _TransDateTime = value;
                this.TransDate = _TransDateTime.ToShortDateString();
                this.TransTime = _TransDateTime.ToLongTimeString();
            }
        }

        [MaxLength(100)]
        public string TransRemark { get; set; }
    }
}