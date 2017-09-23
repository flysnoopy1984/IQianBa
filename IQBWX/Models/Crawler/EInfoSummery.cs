using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IQBWX.Models.Crawler
{
    [Table("InforSummery")]
    public class EInfoSummery
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InfoId { get; set; }
        [MaxLength(60)]
        public string Title { get; set; }
        [MaxLength(200)]
        public string Summery { get; set; }
        [MaxLength(200)]
        public string CoverImg { get; set; }

        public int ReadCount { get; set; }

        [MaxLength(25)]
        public string PublishDate { get; set; }

        [MaxLength(20)]
        public string OrigInfoId { get; set; }

        private DateTime _CreateDateTime = DateTime.MaxValue;
        public DateTime CreateDateTime
        {
            get
            {
                return _CreateDateTime;
            }
            set
            {
                _CreateDateTime = value;
            }
        }

        public int TotalCount { get; set; }
    }
}