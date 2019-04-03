using GameModel.WebSocketData.ReceiveData.Playing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameModel.Message;

namespace GameServer.Command.Playing
{
    public class UserWaitBet : BaseGameCommand<dataUserWaitBet>
    {
        public override List<IGameMessage> HandleData(GameUserSession session, dataUserWaitBet Data)
        {
            List<IGameMessage> result = new List<IGameMessage>();
           

            return result;
        }

        public override bool VerifyCommandData(dataUserWaitBet InData, GameUserSession session)
        {
            return true;
        }
    }
}
