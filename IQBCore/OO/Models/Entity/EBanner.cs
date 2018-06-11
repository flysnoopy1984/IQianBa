using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBCore.OO.Models.Entity
{
    [Table("Banner")]
    public class EBanner
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(255)]
        public string BannerImg { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }


        public int Position { get; set; }


        public DateTime CreateDateTime { get; set; }
    }
}
