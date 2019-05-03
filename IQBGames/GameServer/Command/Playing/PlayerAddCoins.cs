using GameModel.WebSocketData.ReceiveData.Playing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameModel.Message;
using GameServer.Engine;
using GameModel;

namespace GameServer.Command.Playing
{
    public class PlayerAddCoins : BaseGameCommand<dataPlayerAddCoins>
    {
        public override string Name
        {
            get
            {
                return "AddCoin";
            }
        }

        public override List<IGameMessage> HandleData(GameUserSession session, dataPlayerAddCoins Data)
        {
            List<IGameMessage> msgList = new List<IGameMessage>();
            GameManager gm = session.GameManager;
          
            EGameInfo gi = gm.GameDataHandle.GetGameInfo();
            var nextUser = gm.PlayerAddCoins(gi,Data.OpenId, Data.Coins, 0);
            if (nextUser == null)
            {
                gi = gm.GameEndShowCard(gi, true);
            }

            var msg = gm.WaitNextPlayer(gi); 
            if (msg != null)
                msgList.Add(msg);
            else
            {
                var passMsg = GameMessageHandle.CreateResultPlayerAddCoinsMsg(gm.RoomCode, Data.OpenId, gi.CurBetUserOpenId, Data.Coins);
                msgList.Add(passMsg);
            }
            return msgList;
        }

        public override bool VerifyCommandData(dataPlayerAddCoins InData, GameUserSession session)
        {
            if(InData.Coins<=0)
            {
                base.GameMessageHandle.PushErrorMsg("筹码不能小于0", session);
                return false;
            }
            return true;
        }
    }
}
