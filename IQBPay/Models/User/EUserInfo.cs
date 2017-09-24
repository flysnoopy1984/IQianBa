using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IQBPay.Models.User
{
    [Table("UserInfo")]
    public class EUserInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(32)]
        public string OpenId { get; set; }

        [MaxLength(20)]
        public string Name { get; set; }

        public string VendorPid { get; set; }
        public string VendorAppId { get; set; }

        


        
    }
}