using IQBCore.IQBPay.BaseEnum;
using IQBCore.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IQBCore.IQBPay.Models.User
{
    [Table("UserInfo")]
    public class EUserInfo:BasePageModel
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

        [MaxLength(40)]
        public string Name { get; set; }

        public Boolean Isadmin { get; set; }

        /// <summary>
        /// QRUser中的ID
        /// </summary>
        public long QRUserDefaultId { get; set; }

        [MaxLength(256)]
        public string Headimgurl { get; set; }

        /// <summary>
        /// 模板QRID
        /// </summary>

        public long QRAuthId { get; set; }

        /// <summary>
        /// 提现账户
        /// </summary>
        [MaxLength(100)]
        public string AliPayAccount { get; set; }

        public DateTime RegisterDate { get; set; }

        public DateTime LastLoginDate { get; set; }

        /// <summary>
        /// 是否自动转账
        /// </summary>
        public Boolean IsAutoTransfer { get; set; }

        [NotMapped]
        public string UserRoleName
        {
            get
            {
                switch (this.UserRole)
                {
                    case IQBPay.BaseEnum.UserRole.NormalUser:
                        return "普通用户";
                    case IQBPay.BaseEnum.UserRole.StoreMaster:
                        return "高级商户";
                    case IQBPay.BaseEnum.UserRole.StoreVendor:
                        return "商户";

                }
                return "";

            }
        }

        [NotMapped]
        public bool QueryResult { get; set; }

        [NotMapped]
        public float Rate { get; set; }

        [NotMapped]
        public string QRFilePath { get; set; }

       


    }
}