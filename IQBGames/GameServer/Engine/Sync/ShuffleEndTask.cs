﻿using GameCommon.Config;
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
                SResult<EGameInfo> r = _GameManager.GetFirstDotAndBet(_GameManager.RoomCode,players);

                if (r.IsSuccess)
                {
                    var gi = r.Instance;
                    gi.GameStatus = GameStatus.Playing;
                    gi.GameTurn = GameTurn.FirstTurn;

                    //确认并缓存玩家队列
                    _GameManager.GameDataHandle.SetCachePlayer(players,gi);

                    //消息
                    ResultGameShuffleEnd shuffleEndMsg = new ResultGameShuffleEnd(_GameManager.RoomCode);
                    shuffleEndMsg.GameInfo = gi;
                    shuffleEndMsg.PlayerCards.Clear();
                    foreach (var p in players)
                        shuffleEndMsg.PlayerCards.Add(p.UserOpenId, p.CardList);


                  //  var GameCoins = 
                    //大小盲注押注
                   // var Room = _GameManager.GetRoom();
                    var smallBet = GameConfig.GetSmallBet(weight);
                    _GameManager.PlayerAddCoins(gi, gi.SmallBetUserOpenId, smallBet, 0);
                    _GameManager.PlayerAddCoins(gi, gi.BigBetUserOpenId, smallBet*2, 0);

                    shuffleEndMsg.SmallBetAmount = smallBet;
                //    shuffleEndMsg.SmallBetOpenId = gi.SmallBetUserOpenId;
                    shuffleEndMsg.BigBetAmount = smallBet * 2;

                    gi.CurRequireCoins = smallBet*2;

                    shuffleEndMsg.GameCoins = _GameManager.GameDataHandle.GetGameData().GameCoins;
                    //保存游戏信息
                    _GameManager.SetGameInfo(gi);

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

        public void Run(int AfterSec,GameServer GameServer,int weight)
        {
            //洗牌，并获取洗牌结束消息
            var msgList = CreateShuffleEndMessage(weight);
          

            Task SubTask = new Task(() =>
            {
               // Thread.Sleep(AfterSec * 1000);
                SpinWait.SpinUntil(() =>
                {
                    return false;
                }, AfterSec * 1000);

               //将洗牌的消息发送出去
                _GameMessageHandle.Push(msgList);
                var r = _GameMessageHandle.Run(GameServer);
                if (r == false) return;

                //EGameInfo gi = null; 
                //if (msgList[0] is ResultGameShuffleEnd)
                //{
                //    var rgseMsg = msgList[0] as ResultGameShuffleEnd;
                //    rgseMsg.GameInfo = gi;
                //} 
                var gi = _GameManager.GetGameBasic();
               // gi.GameStatus = GameStatus.Playing;
               //gi.GameTurn = GameTurn.FirstTurn;
               //_GameManager.SetGameInfo(gi);

               GameTaskManager.WaitBetUser(GameServer, gi.CurBetUserOpenId);


            });
            SubTask.Start();
        }

        //private void AfterShuffleEnd(GameServer gameServer, EGameInfo gi)
        //{

        //    UserWaitBetTask waitTask = UserWaitBetTask.CreateNewInstance(gameServer, gi.CurBetUserOpenId);
        //    waitTask.Run(GameConfig.Turn_Wait_Server);
          
        //}
    }
}
