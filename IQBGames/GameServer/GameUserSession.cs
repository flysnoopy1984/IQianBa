using SuperSocket.SocketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.WebSocket;
using SuperSocket.WebSocket.Protocol;
using SuperSocket.WebSocket.SubProtocol;
using GameServer.Engine;
using GameModel;

namespace GameServer
{
    public class GameUserSession: WebSocketSession<GameUserSession>
    {
        

        public GameServer GameServer
        {
            get
            {
                return (GameServer)AppServer;
            }
        }

        private ESessionAttr _GameAttr;
        public ESessionAttr GameAttr
        {
            get
            {
                if (_GameAttr == null) _GameAttr = new ESessionAttr();
                return _GameAttr;
            }
        }

        private GameManager _gameManager;
        public GameManager GameManager
        {
            get
            {
                if (_gameManager == null)
                    _gameManager = new GameManager(GameAttr.OpenId,this);
                return _gameManager;
            }
        }

        public void KeepOneSession(string openId)
        {
            var existSession = GameServer.GetSessions(a => a.GameAttr.OpenId == openId);
            if (existSession.Count()> 0)
            {
               foreach(var session in existSession)
               {
                    if(session.SessionID != this.SessionID)
                        session.Close(SuperSocket.SocketBase.CloseReason.InternalError);
               }
            }
        }

        protected override void OnSessionStarted()
        {
            base.OnSessionStarted();
        }

        protected override void OnSessionClosed(CloseReason reason)
        {

            base.OnSessionClosed(reason);
        }

        protected override void HandleException(Exception e)
        {
            base.HandleException(e);
        }

        protected override void HandleUnknownRequest(IWebSocketFragment requestInfo)
        {
            base.HandleUnknownRequest(requestInfo);
        }

        protected override void HandleUnknownCommand(SubRequestInfo requestInfo)
        {
            base.HandleUnknownCommand(requestInfo);
        }

        

     

    }
}
