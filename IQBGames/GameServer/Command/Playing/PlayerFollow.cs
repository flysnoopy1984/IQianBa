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
    public class PlayerFollow : BaseGameCommand<dataPlayerFollow>
    {
        public override string Name
        {
            get
            {
                return "Follow";
            }
        }
        public override List<IGameMessage> HandleData(GameUserSession session, dataPlayerFollow Data)
        {
            List<IGameMessage> msgList = new List<IGameMessage>();
            GameManager gm = session.GameManager;
            var gi = gm.GameDataHandle.GetGameInfo();

            gm.PlayerFollow(gi,Data.OpenId, Data.FollowCoins);

            gi = gm.PreNextStep(true);
         
            var dealCards = gm.DealCard(gi);
            if(dealCards !=null)
            {
                var followMsg = GameMessageHandle.CreateResultPlayerFollowMsg(gm.RoomCode, Data.OpenId, "", Data.FollowCoins);
                msgList.Add(followMsg);

                gi = gm.PreNextStep(true);
                var cardsMsg = GameMessageHandle.CreateDealCardMsg(gm.RoomCode, dealCards,gi);
                msgList.Add(cardsMsg);

                GameTaskManager.SyncTask_DealCardDone(session,gi);
            }
            else
            {
                var msg = gm.WaitNextPlayer(gi);
                if (msg != null)
                    msgList.Add(msg);
                else
                {
                    var followMsg = GameMessageHandle.CreateResultPlayerFollowMsg(gm.RoomCode, Data.OpenId, gi.CurBetUserOpenId,Data.FollowCoins);
                    msgList.Add(followMsg);
                }
            }
          
            return msgList;
        }

        public override bool VerifyCommandData(dataPlayerFollow InData, GameUserSession session)
        {
            if (InData.FollowCoins <= 0)
            {
                base.GameMessageHandle.PushErrorMsg("跟注筹码不能小于0", session);
                return false;
            }
            return true;
        }
    }
}
