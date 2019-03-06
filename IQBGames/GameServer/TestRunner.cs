using SuperSocket.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class TestRunner
    {
        WebSocketServer wsApp;
        public void Start()
        {
            wsApp = new WebSocketServer();
            wsApp.NewSessionConnected += WsApp_NewSessionConnected;
            //下面是通过事件响应信息的代码，如果想用命令的话，要注释掉事件，增加上命令相关的类！
            //wsApp.NewMessageReceived +=new SessionHandler<WebSocketSession, string>( WsApp_NewMessageReceived);
            //private void WsApp_NewMessageReceived(WebSocketSession session, string value)
            //{
            //    session.Send("you just send msg=" + value.ToString());
            //}
            if (!wsApp.Setup(8090))
            {
                Console.WriteLine("未能Set up ws");
            }
            if (!wsApp.Start()) //Setup with listening port
            {
                Console.WriteLine("未能启动ws");
            }
        }
        public void Stop()
        {
            if (wsApp.State == SuperSocket.SocketBase.ServerState.Running)
                wsApp.Stop();
        }


        private void WsApp_NewSessionConnected(WebSocketSession session)
        {
            session.Send("you are connected!" + session.ToString());
        }
    }
}
