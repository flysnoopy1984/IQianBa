using IQBWX.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IQBWX.Models.User
{
    [Table("MemberInfo")]
    public class EMemberInfo
    {
        /// <summary>
        /// 1 城市经理 2大区经理
        /// </summary>
        public MemberType MemberType { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MemberId { get; set; }
          
        [MaxLength(32)]
        public string openId { get; set; }        
        public int UserId { get; set; }
        [MaxLength(32)]
        public string ParentOpenId { get; set; }

        [MaxLength(40)]
        public string nickname { get; set; }
        public int sex { get; set; }

        [MaxLength(20)]
        public string FillCity { get; set; }      

        [MaxLength(20)]
        public string FillCounty { get; set; }

        [MaxLength(100)]
        public string HomeAddr { get; set; }

        [MaxLength(200)]
        public string headimgurl { get; set; }

        public int WXMemberSelected { get; set; }

        [MaxLength(20)]
        public string WXMemberRange { get; set; }

        public int ProvinceSeleced { get; set; }

        [MaxLength(20)]
        public string ProvinceValue { get; set; }

        [MaxLength(20)]
        public string province { get; set; }

        [MaxLength(20)]
        public string UserName { get; set; }

        [MaxLength(20)]
        public string PhoneNumber { get; set; }


        [MaxLength(20)]
        public string SceneId { get; set; }

        /// <summary>
        /// 总的余额
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// 已提现金额
        /// </summary>
        public decimal TotalGainAmt { get; set; }

        /// <summary>
        /// 可提现金额
        /// </summary>
        public decimal AvailDeposit { get; set; }

        /// <summary>
        /// 总收益
        /// </summary>
        public decimal TotalIncome { get; set; }

        private DateTime _RegisterDateTime = DateTime.MaxValue;
        /// <summary>
        /// 会员加入时间
        /// </summary>
        ///   
        public DateTime RegisterDateTime
        {
            get
            {
                return _RegisterDateTime;
            }
            set
            {
                _RegisterDateTime = value;

                this.RegisterDate = _RegisterDateTime.ToShortDateString();
                this.RegisterTime = _RegisterDateTime.ToLongTimeString();
            }
        }
        [MaxLength(10)]
        public string RegisterDate { get; set; }
        [MaxLength(10)]
        public string RegisterTime { get; set; }

        /// <summary>
        /// 会员汇总数据
        /// </summary>
        /// <param name="amount"></param>
        public void InComeAmount(decimal amount)
        {
            TotalIncome += amount;
            Balance += amount;
            AvailDeposit += amount;
        }

        public void OutComeAmount()
        {
            Balance -= AvailDeposit;
            TotalGainAmt += AvailDeposit;
            AvailDeposit = 0;
        }

    }
}