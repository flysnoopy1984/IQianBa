using IQBWX.Common;
using IQBWX.Models.WX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IQBWX.Models.User
{
    [Table("UserInfo")]
    public class EUserInfo: WXUserInfo
    {
        [MaxLength(50)]
        public string Password { get; set; }

        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        public bool IsMember { get; set; }

        [MaxLength(20)]
        public string IdentityCard { get; set; }

        [MaxLength(20)]
        public string UserName { get; set; }

        [MaxLength(20)]
        public string FillCity { get; set; }

        [MaxLength(20)]
        public string FillCounty { get; set; }

        public int ProvinceSeleced { get; set; }

        [MaxLength(20)]
        public string ProvinceValue { get; set; }

        [MaxLength(100)]
        public string HomeAddr { get; set; }

        public PaymentState PaymentState { get; set; }

        public int WXMemberSelected { get; set; }

        [MaxLength(20)]
        public string WXMemberRange { get; set; }

        [MaxLength(32)]
        public string ParentOpenId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]       
        public int UserId { get; set; }


        private DateTime _SubscribeDateTime = DateTime.MaxValue;            
        public DateTime SubscribeDateTime
        {
            get
            {
                return _SubscribeDateTime;
            }
            set
            {
                _SubscribeDateTime = value;
                this.SubscribeDate = _SubscribeDateTime.ToShortDateString();
                this.SubscribeTime = _SubscribeDateTime.ToLongTimeString();
            }
        }
        [MaxLength(10)]
        public string SubscribeDate { get; set; }
        [MaxLength(10)]
        public string SubscribeTime { get; set; }

        /// <summary>
        /// 扫码进入用的哪个二维码
        /// </summary>
        [MaxLength(20)]
        public string ScanSceneId { get; set; }

        public EUserInfo()
        {

        }
        public EUserInfo(WXUserInfo wxui)
        {
            this.SetWXUserInfo(wxui);
        }
        public void SetWXUserInfo(WXUserInfo wxui)
        {
            this.subscribe = wxui.subscribe;
            this.openid = wxui.openid;
            this.nickname = wxui.nickname;
            this.sex = wxui.sex;
            this.language = wxui.language;
            this.city = wxui.city;
            this.province = wxui.province;
            this.country = wxui.country;
            this.headimgurl = wxui.headimgurl;
            this.unionid = wxui.unionid;
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public EMemberInfo InitMember()
        {
            EMemberInfo mi = new EMemberInfo()
            {
                RegisterDateTime = DateTime.Now,
                openId = this.openid,
                ParentOpenId = this.ParentOpenId,
                AvailDeposit = 0,
                Balance = 0,
                nickname = this.nickname,                    
                FillCity = this.FillCity,
                FillCounty = this.FillCounty,
                HomeAddr = this.HomeAddr,
                sex = this.sex,
                TotalGainAmt = 0,
                headimgurl = this.headimgurl,
                UserName = this.UserName,
                ProvinceSeleced = this.ProvinceSeleced,
                ProvinceValue = this.ProvinceValue,
                province = this.province,
                PhoneNumber = this.PhoneNumber,
                WXMemberSelected = this.WXMemberSelected,
                WXMemberRange = this.WXMemberRange
              
            };
          
            return mi;
        }
    }
}