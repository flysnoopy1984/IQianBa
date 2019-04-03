using GameModel.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameModel.Enums;

namespace GameModel.WebSocketData.SendData
{
    public class ResultGameShuffleEnd : BaseNormalMsg
    {
        public override GameActionCode Action
        {
            get
            {
                return GameActionCode.ShuffleEnd;
            }
        }

        public int CurBetSeatNo { get; set; }

        public ResultGameShuffleEnd(string roomCode)
        {
            IGameMessage ig = this;
            ig.MessageSendTarget.SendTarget = SendTarget.UserInRoom;
            ig.MessageSendTarget.TargetRoom = roomCode;
        }
    }
}
