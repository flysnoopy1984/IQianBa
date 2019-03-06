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
        static GameMessageHandle()
        {
           
            
        }
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

        public void Push(List<BaseNormalMsg> msgList)
        {

            foreach(var msg in msgList)
            {
                Push(msg);
            }
           
        }

        public void Push(BaseNormalMsg msg)
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

        public void Run(GameUserSession session, bool endIfError = true)
        {
            try
            {
                if (_ErrorQueue != null)
                {
                    foreach (IGameMessage gameMsg in ErrorQueue)
                    {
                        var msgStr = gameMsg.GetMessage();
                        session.Send(msgStr);
                        if (endIfError)
                            return;
                    }
                }

                if (_MessageQueue != null)
                {
                    foreach (IGameMessage gameMsg in MessageQueue)
                    {
                        var msgStr = gameMsg.GetMessage();
                        session.Send(msgStr);
                    }
                }
            }
            catch(Exception ex)
            {
                if (session.Connected)
                    session.Send("错误，请联系管理员！");
            }
           
        }
    }
}
