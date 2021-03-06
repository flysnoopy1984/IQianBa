﻿using GameModel.Enums;
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
        public ERoomUser()
        {
            PlayerStauts = PlayerStauts.NotSeat;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string RoomCode { get; set; }

        public string UserOpenId { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 当前发到的牌
        /// </summary>
        public List<ECard> CardList { get; set; }

        /// <summary>
        /// 当前桌上剩余的资金（坐下后有效）
        /// </summary>
        public decimal RemainCoins { get; set; }

        /// <summary>
        /// 座位号
        /// </summary>
        public int SeatNo { get; set; }

        public PlayerStauts PlayerStauts { get; set; }



    }
}
