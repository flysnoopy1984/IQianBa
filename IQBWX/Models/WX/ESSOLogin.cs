using IQBCore.IQBWX.BaseEnum;
using IQBWX.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IQBWX.Models.WX
{
    [Table("SSOLogin")]
    public class ESSOLogin
    {
        [Key]
        [MaxLength(32)]
        public string ssoToken { get; set; }

        [MaxLength(32)]
        public string OpenId { get; set; }

        public bool IsValidate { get; set; }
        public double RequireTime { get; set; }

        public LoginStatus LoginStatus { get; set; }

       

        public DateTime CreatedDate { get; set; }

        [MaxLength(20)]
        public string AppId { get; set; }
    }
}