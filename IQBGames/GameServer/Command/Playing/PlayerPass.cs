using GameModel.WebSocketData.ReceiveData.Playing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameModel.Message;
using GameServer.Engine;

namespace GameServer.Command.Playing
{
    public class PlayerPass : BaseGameCommand<dataPlayerGiveUp>
    {
        public override List<IGameMessage> HandleData(GameUserSession session, dataPlayerGiveUp Data)
        {

            List<IGameMessage> msgList = new List<IGameMessage>();
            GameManager gm = session.GameManager;
            gm.PlayerPass();

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
                    var passMsg = GameMessageHandle.CreateResultPlayerPassMsg(gm.RoomCode, Data.OpenId, gi.CurBetUserOpenId);
                    msgList.Add(passMsg);
                }
            }
           
            return msgList;
        }

        public override bool VerifyCommandData(dataPlayerGiveUp InData, GameUserSession session)
        {
            GameManager gm = session.GameManager;
            var gi = gm.GameDataHandle.GetGameInfo();
            if(gi.CurRequireCoins>0)
            {
                base.GameMessageHandle.PushErrorMsg($"无法过牌,需要跟注[{gi.CurRequireCoins}]", session);
                return false;
            }
            return true;
        }
    }
}
