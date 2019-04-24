using GameModel.WebSocketData.ReceiveData.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameModel.Message;
using GameServer.Engine;
using GameModel;
using GameModel.WebSocketData.SendData;
using GameModel.Enums;
using GameServer.Engine.Sync;

namespace GameServer.Command
{
    public class TestStartGame : BaseGameCommand<dataTestStartGame>
    {
        public override List<IGameMessage> HandleData(GameUserSession session, dataTestStartGame Data)
        {
            List<IGameMessage> result = new List<IGameMessage>();
            GameManager gameManager = session.GameManager;

            ResultGameShuffleStart shuffleStartMsg = new ResultGameShuffleStart(Data.RoomCode);
            result.Add(shuffleStartMsg);

            gameManager.StartNewGame(session, Data.RoomCode);

            return result;
        }

        public override bool VerifyCommandData(dataTestStartGame InData, GameUserSession session)
        {
            return true;
        }
    }
}
