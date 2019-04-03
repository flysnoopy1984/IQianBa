using GameModel.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameModel.Enums;

namespace GameModel.WebSocketData.SendData.Playing
{
    public class ResultNextUserBet: BaseNormalMsg
    {
        public string NextUserOpenId { get; set; }

        public int NextSeatNo { get; set; }

        public override GameActionCode Action
        {
            get
            {
                return GameActionCode.NextUserBet;
            }
        }
    }
}
