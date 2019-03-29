using GameModel.Enums;
using GameModel.Message;
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

        //public void PushDelayMsg(IGameMessage msg,int AfterSec)
        //{
        //    Task SubTask = new Task(() =>
        //    {
        //        SpinWait.SpinUntil(() =>
        //        {
        //            return false;
        //        }, AfterSec*1000);
        //        Push(msg);
        //        RunQueue(_MessageQueue);
        //        //  session.Send(msg.GetMessage());

        //    });
        //    SubTask.Start();
        //}

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
        public void Run(GameServer GameServer)
        {
            _GameServer = GameServer;
            if(_GameServer != null)
            {
                try
                {
                    if (_ErrorQueue != null && _ErrorQueue.Count>0)
                    {
                        RunQueue(_ErrorQueue, true);
                        return;
                    }

                    if (_MessageQueue != null && _MessageQueue.Count > 0)
                    {
                        RunQueue(_MessageQueue);
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
           
           
        }
    }
}
