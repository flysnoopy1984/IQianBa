using IQBCore.OO.BaseEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;


namespace IQBCore.OO.Models.Entity
{
    [Table("StoreInfo")]
    public class EStoreInfo: EBaseRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// 属于哪个用户
        /// </summary>
        public long UserId { get; set; }

        public RecordStatus RecordStatus { get; set; }


    }
}
