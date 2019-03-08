using GameModel.WebSocketData.ReceiveData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameModel.Message;
using GameServer.Engine;

namespace GameServer.Command
{
    public class BackHall : BaseGameCommand<dataBackHall>
    {
        public override List<IGameMessage> HandleData(GameUserSession session, dataBackHall Data)
        {
            List<IGameMessage> result = new List<IGameMessage>();
            GameManager gameManager = session.GameManager;
            var r = gameManager.UserBackHall();

            result.Add(r);
            return result;
        }

        public override bool VerifyCommandData(dataBackHall InData)
        {
            return true;
        }
    }
}
