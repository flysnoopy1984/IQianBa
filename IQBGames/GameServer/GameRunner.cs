﻿using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class GameRunner
    {
        GameServer _GameServer = new GameServer();

        public void InitSocket()
        {
            ServerConfig serverConfig = new ServerConfig();
            serverConfig.Port = 8090;
            serverConfig.Mode = SocketMode.Tcp;


            if (!_GameServer.Setup(serverConfig))
            {
                Console.WriteLine("Server Serup 失败");
                return;
            }
            Console.WriteLine("Server Serup");

            if (!_GameServer.Start())
            {
                Console.WriteLine("Server Start 失败");
                return;
            }
            Console.WriteLine("Server Start");

            _GameServer.NewSessionConnected += _GameServer_NewSessionConnected;
        //    _GameServer.NewMessageReceived += _GameServer_NewMessageReceived;



        }

        private void _GameServer_NewMessageReceived(GameUserSession session, string value)
        {
            Console.WriteLine("GetMessage:" + value);
        }

        private void _GameServer_NewSessionConnected(GameUserSession session)
        {
            Console.WriteLine("New Session");
           
        }

      
    }
}
