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
        public int CurD { get; set; }

        public int BetSeat { get; set; }

        public GameStatus GameStatus { get; set; }

        public GameTurn GameTurn { get; set; }

      

        public string RoomCode { get; set; }
    }
}
