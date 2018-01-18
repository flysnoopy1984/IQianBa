using IQBCore.IQBPay.BaseEnum;
using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.IQBPay.Models.Store;
using IQBCore.IQBPay.Models.User;
using IQBCore.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IQBCore.IQBPay.Models.QR
{
    [Table("QRInfo")]
    public class EQRInfo: BasePageModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [MaxLength(32)]
        /// <summary>
        /// 拥有者
        /// </summary>
        public string OwnnerOpenId { get; set; }

        [MaxLength(32)]
        public string APPId { get; set; }

        [MaxLength(32)]
        /// <summary>
        /// 2级授权码用
        /// </summary>
        public string ParentOpenId { get; set; }


        public float ParentCommissionRate { get; set; }

        /// <summary>
        /// 返点率0-100
        /// </summary>
        public float Rate { get; set; }

        [MaxLength(40)]
        public string Name { get; set; }

        [DefaultValue("")]
        [MaxLength(100)]
        public string Remark { get; set; }

        public RecordStatus RecordStatus { get; set; }

        public Channel Channel { get; set; }

        public QRType Type { get; set; }

        [MaxLength(256)]
        public string TargetUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [MaxLength(128)]
        public string FilePath { get; set; }

        /// <summary>
        /// 纯净版本
        /// </summary>
        [MaxLength(128)]
        public string OrigFilePath { get; set; }

        public int ReceiveStoreId { get; set; }

        [NotMapped()]
        public List<HashStore> HashStoreList { get; set; }

        [NotMapped()]
        public List<HashUser> HashUserList { get; set; }

        public int Level { get; set; }

        public bool NeedFollowUp { get; set; }


        /// <summary>
        /// 是否身份校验
        /// </summary>
        public Boolean NeedVerification { get; set; }


        /// <summary>
        /// 最大可邀请人数
        /// </summary>
        public int MaxInviteCount { get; set; }

        public void InitByStore(EStoreInfo si)
        {
            this.NeedVerification = false;
            this.InitCreate();
            this.InitModify();
            this.Level = 1;
            this.OwnnerOpenId = si.OwnnerOpenId;
            this.Rate = si.Rate;
            this.RecordStatus = RecordStatus.Normal;
            this.Channel = si.Channel;
            this.Type = QRType.StoreAuth;


        }

        public void InitByUser(EUserInfo ui)
        {
            this.InitCreate();
            this.InitModify();
            this.OwnnerOpenId = ui.OpenId;
            this.ParentOpenId = ui.OpenId;

           
            this.ReceiveStoreId = 1;
            this.Channel = IQBCore.IQBPay.BaseEnum.Channel.League;
            this.Type = IQBCore.IQBPay.BaseEnum.QRType.ARAuth;


            this.Name =  ui.Name;
            this.Remark = "[邀请码]" + ui.Name;
            if (ui.Name.Length > 40)
            {
                this.Name = this.Name.Substring(0, 40);
            }


        }


    }
}