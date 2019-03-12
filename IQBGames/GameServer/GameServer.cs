using SuperSocket.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.WebSocket.Protocol;
using GameServer.Engine;
using GameModel;
using GameRedis.Games;

namespace GameServer
{
    public class GameServer: WebSocketServer<GameUserSession>
    {

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, GameDataHandle> _GameDataDic;

        public Dictionary<string, GameDataHandle> GameDataDic
        {

            get
            {
                if (_GameDataDic == null)
                    _GameDataDic = new Dictionary<string, GameDataHandle>();
                return _GameDataDic;
            }
        }

       
        

        public GameServer()
        {
            
          
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
