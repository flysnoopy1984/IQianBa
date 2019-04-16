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
    public class ResultPlayerFollow : BaseSendMsg
    {
        public override GameActionCode Action
        {
            get
            {
                return GameActionCode.PlayerFollow;
            }
        }

        public ResultPlayerFollow(string roomCode)
        {
            IGameMessage ig = this;
            ig.MessageSendTarget.SendTarget = SendTarget.UserInRoom;
            ig.MessageSendTarget.TargetRoom = roomCode;
        }

        public string FollowCoinsUserOpenId { get; set; }

        public string NextUserOpenId { get; set; }

        public decimal FollowCoins { get; set; }
    }
}
