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
       // public static Dictionary<string, List<string>> Room_UserSession { get; set; }
        public Dictionary<string,string> User_OpenIdSession { get; set; }
        public GameServer()
        {
            User_OpenIdSession = new Dictionary<string, string>();
        }

        public void SetOpenIdSession(string openId,string curSessionId)
        {
            if (User_OpenIdSession.ContainsKey(openId))
            {

                var userSession = GetSessionByID(User_OpenIdSession[openId]);
              //  User_OpenIdSession.Remove(openId);
                if (userSession != null)
                    userSession.Close(SuperSocket.SocketBase.CloseReason.InternalError);
            }

            User_OpenIdSession[openId] = curSessionId;
        }
        protected override void OnStarted()
        {
            base.OnStarted();
        }

        protected override void OnStopped()
        {
            
            base.OnStopped();
        }

      
    }
}
