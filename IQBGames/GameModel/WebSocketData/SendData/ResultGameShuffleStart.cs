using GameModel.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameModel.Enums;

namespace GameModel.WebSocketData.SendData
{
    /// <summary>
    /// 通知客户端洗牌,发牌
    /// </summary>
    public class ResultGameShuffleStart : BaseNormalMsg
    {
        public override GameActionCode Action
        {
            get
            {
                return GameActionCode.ShuffleStart;
            }
        }
     
        public  ResultGameShuffleStart(string roomCode)
        {
            IGameMessage ig = this;
            ig.MessageSendTarget.SendTarget = SendTarget.UserInRoom;
            ig.MessageSendTarget.TargetRoom = roomCode;
        }

        public List<ECard> TableCards { get; set; }

        public List<ECard> PlayerCards { get; set; }

       


    }
}
