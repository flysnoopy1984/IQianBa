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

            var gi = gm.PreNextPlayer(true);
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

        public override bool VerifyCommandData(dataPlayerGiveUp InData, GameUserSession session)
        {

            return true;
        }
    }
}
