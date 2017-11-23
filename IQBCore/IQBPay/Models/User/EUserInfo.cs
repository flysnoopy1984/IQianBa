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
        /// 提现账户,支付宝正常账户song_fuwei@hotmail.com
        /// </summary>
        [MaxLength(100)]
        public string AliPayAccount { get; set; }

        public DateTime RegisterDate { get; set; }

        public DateTime LastLoginDate { get; set; }

        /// <summary>
        /// 是否自动转账
        /// </summary>
        public Boolean IsAutoTransfer { get; set; }

        public void InitRegiser()
        {
            this.UserRole = IQBCore.IQBPay.BaseEnum.UserRole.NormalUser;
            this.UserStatus = IQBCore.IQBPay.BaseEnum.UserStatus.JustRegister;
            this.RegisterDate = DateTime.Now;
            this.LastLoginDate = DateTime.Now;
        }

       


    }
}