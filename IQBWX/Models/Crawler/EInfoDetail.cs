using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IQBWX.Models.Crawler
{
    [Table("InfoDetail")]
    public class EInfoDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DetailId { get; set; }
        
        [Column("InfoId")]
        public int EInfoSummeryId { get; set; }

        [MaxLength(60)]
        public string Title { get; set; }

        [MaxLength(400)]
        public string OrigUrl { get; set; }

        [DataType(DataType.Text)]
        public string ArticleContent { get; set; }

        public virtual EInfoSummery InfoSummery { get; set; }

    }
}