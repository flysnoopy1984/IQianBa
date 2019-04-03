using GameCommon.Config;
using GameModel;
using GameModel.Enums;
using GameModel.Message;
using GameModel.WebSocketData.ReceiveData;
using GameModel.WebSocketData.SendData;
using GameServer.Engine;
using GameServer.Engine.Sync;
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
        private readonly Object _lockSitDown = new Object();
        public override string Name
        {
            get
            {
                return "UserSitDown";
            }
        }

        public override bool VerifyCommandData(dataUserSitDown InData, GameUserSession session)
        {
          
            if (InData.SeatNo!=-1 && InData.SeatNo<=0)
            {
                base.GameMessageHandle.PushErrorMsg("没有获取座位号！", session);
                return false;
            }
            if (InData.Coins<=0)
            {
                base.GameMessageHandle.PushErrorMsg("没有资金不能继续！", session);
                return false;
            }
            return true;
        }

        public override List<IGameMessage> HandleData(GameUserSession session, dataUserSitDown Data)
        {
            List<IGameMessage> result = new List<IGameMessage>();
           
            //检测用户状态
            GameManager gameManager = session.GameManager;
            if (!gameManager.CanSitDown())
            {
                result.Add(new MessageNormalError("当前不能入座！请等待"));
                return result;
            }
            //用户坐下
            lock (_lockSitDown)
            {
                var r = gameManager.UserSitDown(Data.SeatNo, Data.Coins);
                result.Add(r);

                //游戏下一阶段
                if (r.MessageType == MessageType.Normal)
                {
                    var gameInfo = gameManager.GetGameBasic();
                    var gs = gameInfo.GameStatus;
                    if (gs == GameStatus.NoGame ||
                    gs == GameStatus.WaitPlayer ||
                    gs == GameStatus.Shuffling)
                    {
                        var nextGs = gameManager.GetNextGameStatus(gs);

                        if (nextGs == GameStatus.WaitPlayer)
                            result.Add(new ResultGameWait());

                        if (nextGs == GameStatus.StartShuffle)
                        {
                            //开始洗牌,并分发所有牌
                            ResultGameShuffleStart shuffleStartMsg = gameManager.DoShuffle();
                            result.Add(shuffleStartMsg);

                            if(shuffleStartMsg.MessageType == MessageType.Normal)
                            {
                                gameManager.PrePareNewGame(gameInfo);
                             
                            }

                            //洗牌异步指令,洗牌结束需要通知前端，开始游戏
                            SyncTask_ShuffleEnd(gameManager,gameInfo, session.GameServer);
                        }
                        else if (nextGs == GameStatus.Shuffling)
                        {
                            ResultGameShuffling shuffleMsg = gameManager.WhileShuffling();
                            result.Add(shuffleMsg);
                        }
                    }
                    session.GameAttr.RoomCode = gameManager.RoomCode;
                }
            }
           
            return result;
            
        }

        private void SyncTask_ShuffleEnd(GameManager gm,EGameInfo gi, GameServer gameServer)
        {
            var msgList = new List<IGameMessage>();
            ResultGameShuffleEnd shuffleEndMsg = new ResultGameShuffleEnd(gm.RoomCode);

            gi = gm.GetFirstSeatAndBet(gi);
            if(gi.CurD == -1)
            {
                msgList.Add(new ResultError("系统错误！",gm.RoomCode));
            }
            else
            {
                shuffleEndMsg.CurBetSeatNo = gi.BetSeat;
          
                msgList.Add(shuffleEndMsg);
            }
            ShuffleEndTask syncTask = new ShuffleEndTask(msgList);
            syncTask.RunSyncTask(GameConfig.Game_Shuffle_Sec,gameServer,gm,gi);

         

        }

       

    }
}
