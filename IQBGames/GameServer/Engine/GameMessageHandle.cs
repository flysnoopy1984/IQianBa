using GameModel;
using GameModel.Enums;
using GameModel.Message;
using GameModel.WebSocketData.SendData.Playing;
using IQBCore.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameServer.Engine
{
    public class GameMessageHandle
    {

        private GameServer _GameServer;
        public GameMessageHandle()
        {
           
        }

        private Queue<IGameMessage> _MessageQueue;
        public Queue<IGameMessage> MessageQueue
        {
            get
            {
                if (_MessageQueue == null) _MessageQueue = new Queue<IGameMessage>();
                return _MessageQueue;
            }
        }

        private Queue<IGameMessage> _ErrorQueue;
        public Queue<IGameMessage> ErrorQueue
        {
            get
            {
                if (_ErrorQueue == null) _ErrorQueue = new Queue<IGameMessage>();
                return _ErrorQueue;
            }
        }

        public void Push(List<IGameMessage> msgList, GameUserSession session=null)
        {
            
            foreach(var msg in msgList)
            {
                if (session != null)
                    msg.SessionId = session.SessionID;

                Push(msg);
            }
        }

      

        private void Push(IGameMessage msg)
        {

            if (msg.MessageType == GameModel.Enums.MessageType.Error)
                ErrorQueue.Enqueue(msg);
            else
                MessageQueue.Enqueue(msg);
        }

        public void PushErrorMsg(string msgString, GameUserSession session)
        {
            var msg = new MessageNormalError();
            if (session != null) msg.SessionId = session.SessionID;
            msg.ErrorMsg = msgString;
            ErrorQueue.Enqueue(msg); 
        }

        /// <summary>
        /// once 给Error Query 使用，如果出错，就不继续发送其他消息
        /// </summary>
        /// <param name="session"></param>
        /// <param name="msgList"></param>
        /// <param name="runOne"></param>
        private void RunQueue(Queue<IGameMessage> msgList, bool runOne = false)
        {
            try
            {
                while(msgList.Count>0)
                {
                    IGameMessage gameMsg = msgList.Dequeue();

                    var msgStr = gameMsg.GetMessage();
                    var sendTarget = gameMsg.MessageSendTarget.SendTarget;
                    switch (sendTarget)
                    {
                        case SendTarget.UserInRoom:
                            var targetList = _GameServer.GetSessions(a => a.GameAttr.RoomCode == gameMsg.MessageSendTarget.TargetRoom);
                            foreach (var target in targetList)
                            {
                                if (target.Connected)
                                    target.Send(msgStr);
                            }
                            break;
                        case SendTarget.Self:
                            if (!string.IsNullOrEmpty(gameMsg.SessionId))
                            {
                                var session = _GameServer.GetSessionByID(gameMsg.SessionId);
                                if (session != null && session.Connected)
                                    session.Send(msgStr);
                            }

                            break;
                    }

                    if (runOne) break;
                }
              
             
            }
            catch(Exception ex)
            {
                msgList.Clear();
                throw ex;
            }
        
        }

        public bool Run(GameServer GameServer)
        {
            _GameServer = GameServer;
            if(_GameServer != null)
            {
                try
                {
                    if (_ErrorQueue != null && _ErrorQueue.Count>0)
                    {
                        RunQueue(_ErrorQueue, true);
                        return false;
                    }

                    if (_MessageQueue != null && _MessageQueue.Count > 0)
                    {
                        RunQueue(_MessageQueue);
                        return true;
                    }

                }
                catch (Exception ex)
                {
                    NLogHelper.GameError("[Message Run] Error:" + ex.Message);
                }
                finally
                {
                    ErrorQueue.Clear();

                }
            }
            return false;
           
        }


        public static ResultGameEndShowCard CreateGameEndShowCardMsg(string RoomCode)
        {
            ResultGameEndShowCard msg = new ResultGameEndShowCard(RoomCode);
            return msg;
        }

        public static ResultPlayerGiveUp CreateResultPlayerGiveUpMsg(string RoomCode,string giveUpOpenId,string nextOpenId)
        {
            ResultPlayerGiveUp msg = new ResultPlayerGiveUp(RoomCode);
            msg.GiveUpUserOpenId = giveUpOpenId;
            msg.NextUserOpenId = nextOpenId;
            return msg;
        }

        public static ResultPlayerPass CreateResultPlayerPassMsg(string RoomCode, string passOpenId, string nextOpenId)
        {
            ResultPlayerPass msg = new ResultPlayerPass(RoomCode);
            msg.PassUserOpenId = passOpenId;
            msg.NextUserOpenId = nextOpenId;
            return msg;
        }

        public static ResultPlayerFollow CreateResultPlayerFollowMsg(string RoomCode, string passOpenId, string nextOpenId,decimal coins)
        {
            ResultPlayerFollow msg = new ResultPlayerFollow(RoomCode);
            msg.FollowCoinsUserOpenId = passOpenId;
            msg.NextUserOpenId = nextOpenId;
            msg.FollowCoins = coins;
            return msg;
        }

        public static ResultPlayerAddCoins CreateResultPlayerAddCoinsMsg(string RoomCode, string addCoinsOpenId, string nextOpenId,decimal addCoins)
        {
            ResultPlayerAddCoins msg = new ResultPlayerAddCoins(RoomCode);
            msg.AddCoinsUserOpenId = addCoinsOpenId;
            msg.NextUserOpenId = nextOpenId;
            msg.AddCoins = addCoins;
            return msg;
        }

        public static ResultDealCard CreateDealCardMsg(string RoomCode,List<ECard> cardList,EGameInfo gi)
        {
            ResultDealCard msg = new ResultDealCard(RoomCode);
            msg.NextUserOpenId = gi.CurBetUserOpenId;
            msg.DealCardList = cardList;
            return msg;
        }

    }
}
