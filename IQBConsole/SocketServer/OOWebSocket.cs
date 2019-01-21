using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fleck;

namespace IQBConsole.SocketServer
{
    public class OOWebSocket
    {
       
        Dictionary<string,IWebSocketConnection> _AllSocket;
        WebSocketServer _WebServer;
        public void Init()
        {

            try

            {
                _AllSocket = new Dictionary<string, IWebSocketConnection>();

                _WebServer = new WebSocketServer("ws://0.0.0.0:8090");
                
                //出错后进行重启
                _WebServer.RestartAfterListenError = true;

                _WebServer.Start(socket =>
                {
                   
                    socket.OnOpen = () =>
                    {
                        string clientAddr = socket.ConnectionInfo.ClientIpAddress + ":" + socket.ConnectionInfo.ClientPort;
                        _AllSocket.Add(clientAddr, socket);

                        Console.WriteLine($"与客户端[{clientAddr}]建立链接");
                    };
                    socket.OnClose = () =>
                    {
                        string clientAddr = socket.ConnectionInfo.ClientIpAddress + ":" + socket.ConnectionInfo.ClientPort;
                        _AllSocket.Remove(clientAddr);
                        Console.WriteLine($"与客户端[{clientAddr}]断开链接");
                    };

                    socket.OnMessage = message =>{
                        string clientAddr = socket.ConnectionInfo.ClientIpAddress + ":" + socket.ConnectionInfo.ClientPort;

                        Console.WriteLine($"收到客户端[{clientAddr}]的消息{message}");
                    };
                    
                });
            }
            catch(Exception ex)
            {
                Console.WriteLine("WebSocket 服务出错:" + ex.Message);
            }
        

            

        }
    }
}
