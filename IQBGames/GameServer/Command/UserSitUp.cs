using GameModel.Message;
using GameModel.WebSocketData.ReceiveData;
using GameServer.Engine;
using SuperSocket.WebSocket.SubProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Command
{
    public class UserSitUp : BaseGameCommand<dataUserSitUp>
    {
        public override string Name
        {
            get
            {
                return "UserSitUp";
            }
        }

        public override List<IGameMessage> HandleData(GameUserSession session, dataUserSitUp Data)
        {
            List<IGameMessage> result = new List<IGameMessage>();
            GameManager gameManager = session.GameManager;
            var r =  gameManager.UserSitUp();
          
            result.Add(r);
            return result;
        }

        public override bool VerifyCommandData(dataUserSitUp InData)
        {
            return true;
        }
    }
}
