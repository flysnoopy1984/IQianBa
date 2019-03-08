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
            if(string.IsNullOrEmpty(InData.OpenId))
            {
                base.GameMessageHandle.PushErrorMsg("错误，没有获取您的身份，请重新登陆");
                return false;
            }
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

        public override List<BaseNormalMsg> HandleData(GameUserSession session, dataUserSitDown Data)
        {
            List<BaseNormalMsg> result = new List<BaseNormalMsg>();
            //用户坐下
            GameManager gameManager = new GameManager(Data.OpenId);
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
                        result.Add(new ResultGameShuffle(gameManager.RoomCode));
                    }
                }
                session.GameAttr.RoomCode = gameManager.RoomCode;

            }
           
               

            return result;
            
        }
    }
}
