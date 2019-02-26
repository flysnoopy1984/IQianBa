using GameModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel
{
    [Table("RoomUser")]
    public class ERoomUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string RoomCode { get; set; }

        public string UserOpenId { get; set; }

        public RoomUserStatus UserStatus { get; set; }

        /// <summary>
        /// 座位号
        /// </summary>
        public int SeatNo { get; set; }

       

    }
}
