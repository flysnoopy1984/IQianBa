using GameModel.Message.SendData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameModel.Enums;
using GameModel.Message;

namespace GameModel.WebSocketData.SendData.Playing
{
    public class ResultPlayerAddCoins : BaseSendMsg
    {
        public override GameActionCode Action
        {
            get
            {
                return GameActionCode.PlayerAddCoins;
            }
        }

        public ResultPlayerAddCoins(string roomCode)
        {
            IGameMessage ig = this;
            ig.MessageSendTarget.SendTarget = SendTarget.UserInRoom;
            ig.MessageSendTarget.TargetRoom = roomCode;
        }

        public string AddCoinsUserOpenId { get; set; }

        public string NextUserOpenId { get; set; }

        public decimal AddCoins { get; set; }
    }
}
