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
    public class DealCardDone : BaseGameCommand<dataDealCardDone>
    {
        public override List<IGameMessage> HandleData(GameUserSession session, dataDealCardDone Data)
        {
            List<IGameMessage> result = new List<IGameMessage>();
            GameManager gm = session.GameManager;

            var gi = gm.PreNextStep(true);


            return result;
        }

        public override bool VerifyCommandData(dataDealCardDone InData, GameUserSession session)
        {
            return true;
        }
    }
}
