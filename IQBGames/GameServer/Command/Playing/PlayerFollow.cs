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
    public class PlayerFollow : BaseGameCommand<dataPlayerFollow>
    {
        public override List<IGameMessage> HandleData(GameUserSession session, dataPlayerFollow Data)
        {
            List<IGameMessage> msgList = new List<IGameMessage>();
            GameManager gm = session.GameManager;
            var gi = gm.GameDataHandle.GetGameInfo();

            gm.PlayerFollow(gi,Data.OpenId, Data.FollowCoins);

            gi = gm.PreNextPlayer(true);
            var msg = gm.WaitNextPlayer(gi);
            if (msg != null)
                msgList.Add(msg);
            else
            {
                var passMsg = GameMessageHandle.CreateResultPlayerPassMsg(gm.RoomCode, Data.OpenId, gi.CurBetUserOpenId);
                msgList.Add(passMsg);
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
