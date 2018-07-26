using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.Models.Store;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.IQBPay.Models.QR
{
    [Table("QRStoreAuth")]
    public class EQRStoreAuth
    {
        public EQRStoreAuth()
        {
            MaxLimitAmount = 499;
            MinLimitAmount = 0;
            DayIncome = 10000;
            RemainAmount = 10000;
            CreateDateTime = DateTime.Now;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        
        public long StoreId { get; set; }

        [MaxLength(32)]
        public string APPId { get; set; }

        [MaxLength(32)]
        /// <summary>
        /// 拥有者
        /// </summary>
        public string OwnnerOpenId { get; set; }

        /// <summary>
        /// 店铺码名称
        /// </summary>
        [MaxLength(50)]
        public string StoreName { get; set; }

        /// <summary>
        /// 平台码还是加盟店码
        /// </summary>
        public Channel Channel { get; set; }

        public float Rate { get; set; }

        public StoreType StoreType { get; set; }


      

        /// <summary>
        /// 备注
        /// </summary>
        [DefaultValue("")]
        [MaxLength(100)]
        public string Remark { get; set; }

        /// <summary>
        /// 扫码后的实际地址
        /// </summary>
        [MaxLength(256)]
        public string TargetUrl { get; set; }

        /// <summary>
        /// 二维码存放地址
        /// </summary>
        [MaxLength(128)]
        public string FilePath { get; set; }

        public DateTime CreateDateTime { get; set; }

        public RecordStatus RecordStatus { get; set; }

        public float MaxLimitAmount { get; set; }

      
        public float MinLimitAmount { get; set; }

        public float RemainAmount { get; set; }

        public float DayIncome { get; set; }


        public void InitByStore(EStoreInfo si)
        {

            this.StoreId = si.ID;
            this.OwnnerOpenId = si.OwnnerOpenId;
            this.Rate = si.Rate;
            this.RecordStatus = RecordStatus.Normal;
            this.Channel = si.Channel;
            this.StoreType = StoreType.Small;
            this.StoreName = si.Name;
            MaxLimitAmount = si.MaxLimitAmount;
            MinLimitAmount = si.MinLimitAmount;
            DayIncome = si.DayIncome;
            RemainAmount = 0;


        }
    }
}
