using IQBWX.Common;
using IQBWX.DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IQBWX.Models.Product
{
    [Table("ItemInfo")]
    public class EItemInfo
    {
        public const string Item158 = "QMMD158";
        public const string Item358 = "QMMD358";
        public const string ItemL1ToL2 = "QMMDL1ToL2";

        [Key]
        [MaxLength(20)]
        public string ItemId { get; set; }
        [MaxLength(40)]
        public string ItemName { get; set; }
        [MaxLength(80)]
        public string Description { get; set; }

        /// <summary>
        /// 交易金额默认为人民币交易，接口中参数支付金额单位为【分】，参数值不能带小数。对账单中的交易金额单位为【元】
        /// </summary>
        public int SalesPrice { get; set; }

        public static EItemInfo get158Item()
        {
            EItemInfo item;
            using (ProductContent db = new ProductContent())
            {
                item = db.ItemInfo.Find(EItemInfo.Item158);
            };

            return item;

        }
        public static EItemInfo getL1ToL2Item()
        {
            EItemInfo item;
            using (ProductContent db = new ProductContent())
            {
                item = db.ItemInfo.Find(EItemInfo.ItemL1ToL2);
            };

            return item;
        }
        public static EItemInfo get358Item()
        {
            EItemInfo item;
            using (ProductContent db = new ProductContent())
            {
                item = db.ItemInfo.Find(EItemInfo.Item358);
            };

            return item;
        }

        public static void InitItem()
        {
            try
            { 
                ProductContent db = new ProductContent();
                EItemInfo item = db.ItemInfo.Find(EItemInfo.Item158);
                if (item == null)
                {
                    item = new EItemInfo();
                    item.ItemId = EItemInfo.Item158;
                    item.ItemName = "城市经理";
                    item.Description = "全民秒贷-城市经理产品";
                    item.SalesPrice = 1;
                    db.ItemInfo.Add(item);
                }

                item = db.ItemInfo.Find(EItemInfo.Item358);
                if (item == null)
                {
                    item = new EItemInfo();
                    item.ItemId = EItemInfo.Item358;
                    item.ItemName = "大区经理";
                    item.Description = "全民秒贷-大区经理产品";
                    item.SalesPrice = 2;
                    db.ItemInfo.Add(item);
                }

                item = db.ItemInfo.Find(EItemInfo.ItemL1ToL2);
                if (item == null)
                {
                    item = new EItemInfo();
                    item.ItemId = EItemInfo.ItemL1ToL2;
                    item.ItemName = "升级大区经理";
                    item.Description = "全民秒贷-升级大区经理产品";
                    item.SalesPrice = 1;
                    db.ItemInfo.Add(item);
                }

                db.SaveChanges();
            }
            catch(Exception ex)
            {
                IQBLog log = new IQBLog();
                log.log("InitItem Error：" + ex.Message);
            }
        }
    }
}