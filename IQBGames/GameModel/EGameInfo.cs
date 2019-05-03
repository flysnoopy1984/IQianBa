using GameModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel
{
    public class EGameInfo
    {
        public EGameInfo(string rc)
        {
            RoomCode = rc;
        }

        public string DotUserOpenId { get; set; }

        public string CurBetUserOpenId { get; set; }

        public string SmallBetUserOpenId { get; set; }

        public string BigBetUserOpenId { get; set; }

        /// <summary>
        /// 当前出牌人至少出多少筹码
        /// </summary>
        public decimal CurRequireCoins { get; set; }

        public int FirstPlayerIndex { get; set; }

        public GameStatus GameStatus { get; set; }

        public GameTurn GameTurn { get; set; }


        public string RoomCode { get; set; }
    }
}
