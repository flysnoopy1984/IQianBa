using GameCommon.Config;
using GameModel;
using GameModel.Enums;
using GameModel.Message;
using GameModel.WebSocketData.SendData;
using IQBCore.IQBPay.Models.OutParameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameServer.Engine.Sync
{
    public class ShuffleEndTask
    {
    //    private List<IGameMessage> _taskMsgList = null;
        private GameMessageHandle _GameMessageHandle = null;
        private GameManager _GameManager;
    
        //public delegate void DoAfterTask(string RoomCode);
        //public event DoAfterTask afterTaskEvent;
        

        public ShuffleEndTask(GameManager gameManger)
        {
            _GameManager = gameManger;
            _GameMessageHandle = new GameMessageHandle();
        }

        private List<IGameMessage> CreateShuffleEndMessage(int weight)
        {
            var msgList = new List<IGameMessage>();
            try
            {
                
                //洗牌，获取每个玩家的牌
                List<ERoomUser> players = _GameManager.DoShuffle();

                //根据座位玩家排序
                players = players.OrderBy(a => a.SeatNo).ToList();

                //初始化Dot,大小盲注
                SResult<EGameInfo> r = _GameManager.GetFirstDotAndBet(players);

                if (r.IsSuccess)
                {
                    var gi = r.Instance;
                    gi.GameStatus = GameStatus.ShuffleEnd;
                    gi.GameTurn = GameTurn.FirstTurn;

                    //保存游戏信息
                    _GameManager.SetGameInfo(gi);

                    //确认并缓存玩家队列
                    _GameManager.GameDataHandle.SetCachePlayer(players,gi);

                    //消息
                    ResultGameShuffleEnd shuffleEndMsg = new ResultGameShuffleEnd(_GameManager.RoomCode);
                    shuffleEndMsg.GameInfo = gi;
                    shuffleEndMsg.PlayerCards.Clear();
                    foreach (var p in players)
                        shuffleEndMsg.PlayerCards.Add(p.UserOpenId, p.CardList);


                    //大小盲注押注
                   // var Room = _GameManager.GetRoom();
                    var smallBet = GameConfig.GetSmallBet(weight);
                    _GameManager.PlayerAddCoins(gi, gi.SmallBetUserOpenId, smallBet, 0);
                    _GameManager.PlayerAddCoins(gi, gi.BigBetUserOpenId, smallBet*2, 0);

                    shuffleEndMsg.SmallBetAmount = smallBet;
                    shuffleEndMsg.BigBetAmount = smallBet * 2;

                    msgList.Add(shuffleEndMsg);
                }
                else
                    throw new Exception(r.ErrorMsg);
               
            }
            catch(Exception ex)
            {
                msgList.Add(new MessageNormalError(ex.Message));
            }
            return msgList;

        }

        public void Run(int AfterSec,GameServer GameServer,EGameInfo gi,int weight)
        {

            Task SubTask = new Task(() =>
            {
               // Thread.Sleep(AfterSec * 1000);
                SpinWait.SpinUntil(() =>
                {
                    return false;
                }, AfterSec * 1000);

                //确定牌的信息，开始位置，出牌玩家
               var msgList = CreateShuffleEndMessage(weight);
               _GameMessageHandle.Push(msgList);
               var r =  _GameMessageHandle.Run(GameServer);
               if (r == false) return;

            
               gi.GameStatus = GameStatus.Playing;
               gi.GameTurn = GameTurn.FirstTurn;
               _GameManager.SetGameInfo(gi);

               AfterShuffleEnd(GameServer, gi);


            });
            SubTask.Start();
        }

        private void AfterShuffleEnd(GameServer gameServer, EGameInfo gi)
        {

            UserWaitBetTask waitTask = UserWaitBetTask.CreateNewInstance(gameServer, gi.CurBetUserOpenId);
            waitTask.Run(GameConfig.Turn_Wait_Server);
          
        }
    }
}
