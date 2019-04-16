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
    public class ResultPlayerGiveUp : BaseSendMsg
    {
        public override GameActionCode Action
        {
            get
            {
                return GameActionCode.PlayerGiveUp;
            }
        }

        public ResultPlayerGiveUp(string roomCode)
        {
            IGameMessage ig = this;
            ig.MessageSendTarget.SendTarget = SendTarget.UserInRoom;
            ig.MessageSendTarget.TargetRoom = roomCode;
        }

        public string GiveUpUserOpenId { get; set; }

        public string NextUserOpenId { get; set; }
    }
}
