using IQBCore.OO.BaseEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.OO.Models.Entity
{
    [Table("ItemInfo")]
    public class EItemInfo:EBaseRecord
    {

        public EItemInfo()
        {
            Channel = OOChannel.Normal;
            RecordStatus = RecordStatus.Normal;
            Price = 0;

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long StoreId { get; set; }

        public RecordStatus RecordStatus { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public OOChannel Channel { get; set; }

       
        public string RealUrl { get; set; }

        public double Price { get; set; }

        [MaxLength(50)]
        public string ExtId { get; set; }


    }
}
