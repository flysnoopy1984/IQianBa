using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IQBConsole.SocketServer
{
    public class OOSocketServer
    {
        private Socket _ListenSocket;
        private Socket _ReceiveSocket;
        private string _IP = "172.19.132.74";
        private int _Port = 8090;
        private Dictionary<string, Socket> _ClientList = new Dictionary<string, Socket>();

       
        private Thread _ThreadListen;
        private Thread _ThreadReceive;

        public void Init()
        {
            try
            {
                _ListenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //获取ip地址
                IPAddress ip = IPAddress.Parse(_IP);

                //创建端口号
                IPEndPoint point = new IPEndPoint(ip, _Port);

                _ListenSocket.Bind(point);
                //开始监听:设置最大可以同时连接多少个请求
                _ListenSocket.Listen(10);
                Console.WriteLine($"Socket 绑定成功，正在监听端口【{point}】");

                _ThreadListen = new Thread(new ParameterizedThreadStart(StartListen));
                _ThreadListen.IsBackground = true;
                _ThreadListen.Start(_ListenSocket);

              
            }
            catch(Exception ex)
            {
                Console.WriteLine("初始化Socket 报错：" + ex.Message);
            }
        }

        private void StartListen(object obj)
        {
            Socket wSocket = obj as Socket;
            while(true)
            {
                try
                {
                   
                    //等待客户端的连接，并且创建一个用于通信的Socket
                    _ReceiveSocket = wSocket.Accept();

                    EndPoint ep = _ReceiveSocket.RemoteEndPoint;

                    string remoteIP = ep.ToString().Split(':')[0];

                    _ClientList.Add(remoteIP, _ReceiveSocket);

                   
                    

                    Console.WriteLine($"【{remoteIP}】链接成功!");
                    PushToClient(remoteIP);

                    _ThreadReceive = new Thread(new ParameterizedThreadStart(ReceiveMsg));
                    _ThreadReceive.IsBackground = true;
                    _ThreadReceive.Start(_ReceiveSocket);

                }
                catch(Exception ex)
                {

                }
            } 
        }

        private void ReceiveMsg(object obj)
        {
            try
            {
                Socket socketReceive = obj as Socket;
                var IP = socketReceive.RemoteEndPoint;
                while (true)
                {
                    //客户端连接成功后，服务器接收客户端发送的消息
                    byte[] buffer = new byte[2048];
                    int count = socketReceive.Receive(buffer);
                    if (count == 0)//count 表示客户端关闭，要退出循环
                    {
                        Console.WriteLine($"[{IP}] 没有消息;");
                        break;
                    }
                    else
                    {
                        string str = Encoding.Default.GetString(buffer, 0, count);
                        string strReceiveMsg = $"接收：{IP}. 发送的消息:{str}";
                        Console.WriteLine(strReceiveMsg);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"接受消息错误：{ex.Message};");
            }
        }

        private void PushToClient(string IP)
        {
            try

            {
                string msg = "欢迎您，访客";
                byte[] buffer = new byte[2048];
                buffer = Encoding.Default.GetBytes(msg);

                _ClientList[IP].Send(buffer);

                Console.WriteLine($"发送消息给IP【{IP}】成功");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"发送消息给IP 【{IP}】 失败！");
            }
           


        }
    }
}
