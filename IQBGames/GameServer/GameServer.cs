using SuperSocket.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.WebSocket.Protocol;

namespace GameServer
{
    public class GameServer: WebSocketServer<GameUserSession>
    {
        protected override void OnStarted()
        {
            base.OnStarted();
        }

        protected override void OnStopped()
        {
            base.OnStopped();
        }

        protected override void ExecuteCommand(GameUserSession session, IWebSocketFragment requestInfo)
        {
            base.ExecuteCommand(session, requestInfo);
        }
    }
}
