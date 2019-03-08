using GameModel.Enums;
using GameModel.Message;
using GameModel.WebSocketData.ReceiveData;
using GameModel.WebSocketData.SendData;
using GameServer.Engine;
using IQBCore.Common.Helper;
using Newtonsoft.Json;
using SuperSocket.WebSocket.SubProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Command
{
    public class UserSitDown : BaseGameCommand<dataUserSitDown>
    {
        public override string Name
        {
            get
            {
                return "UserSitDown";
            }
        }

        public override bool VerifyCommandData(dataUserSitDown InData)
        {
          
            if (InData.SeatNo!=-1 && InData.SeatNo<=0)
            {
                base.GameMessageHandle.PushErrorMsg("没有获取座位号！");
                return false;
            }
            if (InData.Coins<=0)
            {
                base.GameMessageHandle.PushErrorMsg("没有资金不能继续！");
                return false;
            }
            return true;
        }

        public override List<IGameMessage> HandleData(GameUserSession session, dataUserSitDown Data)
        {
            List<IGameMessage> result = new List<IGameMessage>();
            //用户坐下
            GameManager gameManager = session.GameManager;
            var r = gameManager.UserSitDown(Data.SeatNo, Data.Coins);
            result.Add(r);

            //游戏下一阶段
            if(r.MessageType == MessageType.Normal)
            {
               if (gameManager.GameStatus == GameModel.Enums.GameStatus.NoGame ||
               gameManager.GameStatus == GameModel.Enums.GameStatus.WaitPlayer)
                {
                    var gs = gameManager.MoveNextGameStatus();
                    if (gs == GameStatus.WaitPlayer)
                        result.Add(new ResultGameWait());
                    if (gs == GameStatus.Shuffle)
                    {
                        ResultGameShuffle shuffleMsg = new ResultGameShuffle(gameManager.RoomCode);
                        result.Add(shuffleMsg);
                    }
                }
                session.GameAttr.RoomCode = gameManager.RoomCode;
            }

            return result;
            
        }
    }
}
