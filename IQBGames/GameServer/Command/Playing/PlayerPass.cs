using GameModel.WebSocketData.ReceiveData.Playing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameModel.Message;
using GameServer.Engine;
using GameServer.Engine.Sync;

namespace GameServer.Command.Playing
{
    public class PlayerPass : BaseGameCommand<dataPlayerPass>
    {
        public override string Name
        {
            get
            {
                return "Pass";
            }
        }

        public override List<IGameMessage> HandleData(GameUserSession session, dataPlayerPass Data)
        {

            List<IGameMessage> msgList = new List<IGameMessage>();
            GameManager gm = session.GameManager;
            gm.PlayerPass();

            var gi = gm.PreNextStep(true);
            var dealCards = gm.DealCard(gi);
            if (dealCards != null)
            {
                var passMsg = GameMessageHandle.CreateResultPlayerPassMsg(gm.RoomCode, Data.OpenId,"");
                msgList.Add(passMsg);

                gi = gm.PreNextStep(true);
                var cardsMsg = GameMessageHandle.CreateDealCardMsg(gm.RoomCode, dealCards,gi);
                msgList.Add(cardsMsg);
                GameTaskManager.SyncTask_DealCardDone(session, gi);
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

        public override bool VerifyCommandData(dataPlayerPass InData, GameUserSession session)
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
