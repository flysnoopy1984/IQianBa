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

namespace GameServer
{
    public class GameUserSession: WebSocketSession<GameUserSession>
    {
        public string RoomCode { get; set; }

        public string OpenId { get; set; }

        public string UserName { get; set; }



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
