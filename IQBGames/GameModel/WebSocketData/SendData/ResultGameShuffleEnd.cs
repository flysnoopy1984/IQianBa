using GameModel.Message.SendData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameModel.Enums;
using GameModel.Message;

namespace GameModel.WebSocketData.SendData
{
    public class ResultGameShuffleEnd : BaseSendMsg
    {
        public override GameActionCode Action
        {
            get
            {
                return GameActionCode.ShuffleEnd;
            }
        }

       public EGameInfo GameInfo { get; set; }

        public decimal SmallBetAmount { get; set; }

        public decimal BigBetAmount { get; set; }

        public ResultGameShuffleEnd(string roomCode)
        {
            IGameMessage ig = this;
            ig.MessageSendTarget.SendTarget = SendTarget.UserInRoom;
            ig.MessageSendTarget.TargetRoom = roomCode;
        }

        private Dictionary<string, List<ECard>> _PlayerCards;
        public Dictionary<string, List<ECard>> PlayerCards
        {
            get
            {
                if (_PlayerCards == null) _PlayerCards = new Dictionary<string, List<ECard>>();
                return _PlayerCards;
            }
        }
    }
}
