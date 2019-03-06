
using GameModel;
using GameModel.Message;
using GameModel.WebSocketData;
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
    public abstract class BaseGameCommand<T> : SubCommandBase<GameUserSession> where T : BaseWSJsonData
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

        public abstract bool VerifyCommandData(T InData);

        public abstract List<BaseNormalMsg> HandleData(GameUserSession session,T Data);

        private T Data = null;

        public T InitInData(string Body)
        {
            Console.WriteLine($"{this.Name}:{Body}");

            if (string.IsNullOrEmpty(Body))
            {
                GameMessageHandle.PushErrorMsg("没有获取数据");
                return null;
            }

            var preData = JsonConvert.DeserializeObject<T>(Body);
            var r = VerifyCommandData(preData);

            if (r == false)
                return null;
            else
                Data = preData;

            return Data;
        }

        public override void ExecuteCommand(GameUserSession session, SubRequestInfo requestInfo)
        {
            try
            {
                InitInData(requestInfo.Body);

                if (Data != null)
                {
                    var msgData = HandleData(session, this.Data);

                    GameMessageHandle.Push(msgData);
                }
            }
            catch(Exception ex)
            {
                GameMessageHandle.PushErrorMsg(ex.Message);
            }
           
            if (session.Connected)
                GameMessageHandle.Run(session);

        }
    }
}
