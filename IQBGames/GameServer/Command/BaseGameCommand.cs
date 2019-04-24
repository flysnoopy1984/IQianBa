
using GameModel;
using GameModel.Message;
using GameModel.WebSocketData.ReceiveData;
using GameServer.Engine;
using Newtonsoft.Json;
using SuperSocket.WebSocket.SubProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Command
{
    public abstract class BaseGameCommand<T> : SubCommandBase<GameUserSession> where T : BaseReceiveData
    {
        private GameMessageHandle _GameMessageHandle;
        public GameMessageHandle GameMessageHandle
        {
            get
            {
                if (_GameMessageHandle == null)
                    _GameMessageHandle = new GameMessageHandle();
                return _GameMessageHandle;
            }
        }

       

        public abstract bool VerifyCommandData(T InData, GameUserSession session);

        public abstract List<IGameMessage> HandleData(GameUserSession session,T Data);

        private T Data = null;

        public T InitInData(string Body,GameUserSession session)
        {
            Console.WriteLine($"{this.Name}:{Body}");

            if (string.IsNullOrEmpty(Body))
            {
                GameMessageHandle.PushErrorMsg("没有获取数据", session);
                return null;
            }

            var preData = JsonConvert.DeserializeObject<T>(Body);

            if (!VerifyBaseData(preData, session)) return null;
           
            if (!VerifyCommandData(preData, session)) return null;
           
            Data = preData;

            return Data;
        }

        private bool VerifyBaseData(T preData,GameUserSession session)
        {
            if (string.IsNullOrEmpty(preData.OpenId))
            {
                GameMessageHandle.PushErrorMsg("错误，没有获取您的身份，请重新登陆", session);
                return false;
            }
            return true;
        } 

        public override void ExecuteCommand(GameUserSession session, SubRequestInfo requestInfo)
        {
            try
            {
                InitInData(requestInfo.Body,session);

                if (Data != null)
                {
                    session.GameAttr.UserOpenId = this.Data.OpenId;

                    var msgData = HandleData(session, this.Data);
                    if(msgData!=null)
                        GameMessageHandle.Push(msgData,session);
                }
            }
            catch(Exception ex)
            {
                GameMessageHandle.PushErrorMsg(ex.Message,session);
            }
           
            if (session.Connected)
                GameMessageHandle.Run(session.GameServer);

        }

        
    }
}
