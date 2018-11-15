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
    [Table("UserDevice")]
    public class EUserDevice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(20)]
        public string UserPhone { get; set; }

        [MaxLength(100)]
        public string DeviceToken { get; set; }

        [MaxLength(40)]
        public string IDFV { get; set; }

        public int LoginCount { get; set; }

        public DeviceChannel DeviceChannel { get; set; }

        [MaxLength(20)]
        public string AppName { get; set; }

        public DateTime LastLoginDateTime { get; set; }

        public DateTime CreateDateTime { get; set; }

       
    }
}
