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
    public class ResultDealCard : BaseSendMsg
    {

        public List<ECard> DealCardList = null;
        public override GameActionCode Action
        {
            get
            {
                return GameActionCode.DealCard;
            }
        }

        public string NextUserOpenId { get; set; }

        public ResultDealCard(string roomCode)
        {
            IGameMessage ig = this;
            ig.MessageSendTarget.SendTarget = SendTarget.UserInRoom;
            ig.MessageSendTarget.TargetRoom = roomCode;

            DealCardList = new List<ECard>();

        }
    }
}
