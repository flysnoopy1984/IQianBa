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
    [Table("UserInfo")]
    public class EUserInfo: EBaseRecord
    {
        //[Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(20)]
        public string LoginName { get; set; }

        [MaxLength(20)]
        public string Pwd { get; set; }

        [Key, Column(Order = 0)]
        [MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(50)]
        public string NickName { get; set; }

        public UserRole UserRole { get; set; }

        [MaxLength(256)]
        public string HeaderImgUrl { get; set; }

        public RecordStatus RecordStatus { get; set; }

        public DateTime? LastLoginDateTime { get; set; }

        public DateTime? RegisterDateTime { get; set; }
    }
}
