using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IQBWX.Models.Results
{
    public class RInfoDetail
    {
        public int DetailId { get; set; }

        [MaxLength(60)]
        public string Title { get; set; }

        [DataType(DataType.Text)]
        public string ArticleContent { get; set; }

        [MaxLength(25)]
        public string PublishDate { get; set; }

        public int ReadCount { get; set; }
    }
}