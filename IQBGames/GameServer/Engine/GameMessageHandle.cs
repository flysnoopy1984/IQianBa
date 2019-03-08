using GameModel.Enums;
using GameModel.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Engine
{
    public class GameMessageHandle
    {
      
        public GameMessageHandle()
        {
            
        }

        private List<IGameMessage> _MessageQueue;
        public List<IGameMessage> MessageQueue
        {
            get
            {
                if (_MessageQueue == null) _MessageQueue = new List<IGameMessage>();
                return _MessageQueue;
            }
        }

        private List<IGameMessage> _ErrorQueue;
        public List<IGameMessage> ErrorQueue
        {
            get
            {
                if (_ErrorQueue == null) _ErrorQueue = new List<IGameMessage>();
                return _ErrorQueue;
            }
        }

        public void Push(List<IGameMessage> msgList)
        {

            foreach(var msg in msgList)
            {
                Push(msg);
            }
           
        }

        public void Push(IGameMessage msg)
        {

            if (msg.MessageType == GameModel.Enums.MessageType.Error)
                ErrorQueue.Add(msg);
            else
                MessageQueue.Add(msg);
        }

        public void PushErrorMsg(string msgString)
        {
            var msg = new MessageNormalError();
            msg.ErrorMsg = msgString;
            ErrorQueue.Add(msg); 
        }

        private void RunQueue(GameUserSession session,List<IGameMessage> msgList, bool runOnce = true)
        {
            foreach (IGameMessage gameMsg in msgList)
            {
                var msgStr = gameMsg.GetMessage();
                var sendTarget = gameMsg.MessageSendTarget.SendTarget;
                switch (sendTarget)
                {
                    case SendTarget.UserInRoom:
                        var targetList = session.GameServer.GetSessions(a => a.GameAttr.RoomCode == gameMsg.MessageSendTarget.TargetRoom);
                        foreach(var target in targetList)
                        {
                            target.Send(msgStr);
                        }
                        break;
                    case SendTarget.Self:
                        session.Send(msgStr);
                        break;
                }
               
                if (runOnce) break;
            }
        }
        public void Run(GameUserSession session, bool endIfError = true)
        {
            try
            {
                if (_ErrorQueue != null)
                {
                    RunQueue(session, _ErrorQueue, true);
                }

                if (_MessageQueue != null)
                {
                    RunQueue(session, _MessageQueue, false);
                }
                
            }
            catch(Exception ex)
            {
                if (session.Connected)
                    session.Send("错误，请联系管理员！");
            }
            finally
            {
                ErrorQueue.Clear();
                MessageQueue.Clear();
            }
           
        }
    }
}
