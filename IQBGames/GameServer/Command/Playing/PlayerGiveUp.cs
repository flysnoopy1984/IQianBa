using GameModel.WebSocketData.ReceiveData.Playing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameModel.Message;
using GameModel.WebSocketData.SendData.Playing;
using GameServer.Engine;

namespace GameServer.Command.Playing
{
    public class PlayerGiveUp : BaseGameCommand<dataPlayerGiveUp>
    {
        public override List<IGameMessage> HandleData(GameUserSession session, dataPlayerGiveUp Data)
        {
            List<IGameMessage> msgList = new List<IGameMessage>();
            GameManager gm = session.GameManager;

            gm.PlayerGiveUp();

            var gi = gm.PreNextStep(true);
            var dealCards = gm.DealCard(gi);
            if (dealCards != null)
            {
                var cardsMsg = GameMessageHandle.CreateDealCardMsg(gm.RoomCode, dealCards);
                msgList.Add(cardsMsg);
            }
            else
            {
                var msg = gm.WaitNextPlayer(gi);
                if (msg != null)
                    msgList.Add(msg);
                else
                {
                    ResultPlayerGiveUp giveUpMsg = GameMessageHandle.CreateResultPlayerGiveUpMsg(gm.RoomCode, Data.OpenId, gi.CurBetUserOpenId);
                    msgList.Add(giveUpMsg);
                }
            }
          
            return msgList;
        }

        public override bool VerifyCommandData(dataPlayerGiveUp InData, GameUserSession session)
        {
            return true;
        }
    }
}
