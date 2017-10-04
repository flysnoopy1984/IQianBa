using IQBCore.Model;
using IQBPay.Core.BaseEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IQBPay.Models.User
{
    [Table("UserInfo")]
    public class EUserInfo:BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
     
        [MaxLength(32)]
        public string parentOpenId { get; set; }

        [MaxLength(32)]
        public string OpenId { get; set; }

        public UserStatus UserStatus { get; set; }

        public UserRole UserRole { get; set; }

        [MaxLength(20)]
        public string Name { get; set; }

        public Boolean Isadmin { get; set; }

        
        public long QRDefaultId { get; set; }

        [MaxLength(256)]
        public string Headimgurl { get; set; }

    }
}