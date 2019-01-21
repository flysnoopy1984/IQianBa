using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IQBConsole
{
    public partial class SocketClient : Form
    {
        private Socket _socketConnect,_SocketReceive;
        private Thread _threadReceive;

        private ClientWebSocket _WebSocketClient;

        //      private string _IP = "172.19.132.74";
        private string _IP = "47.101.130.0";
        public SocketClient()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        private void bn_Connect_Click(object sender, EventArgs e)
        {
            try
            {
                _socketConnect = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Parse(_IP);
                //Bind Socket
                IPEndPoint point = new IPEndPoint(ip, 8090);
                _socketConnect.Connect(point);

                MessageBox.Show("链接成功!");

                _threadReceive = new Thread(new ThreadStart(Receive));

                _threadReceive.IsBackground = true;
                _threadReceive.Start();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void bn_Send_Click(object sender, EventArgs e)
        {
            try
            {
                if(_socketConnect != null && _socketConnect.Connected)
                {
                    string txt = tb_Word.Text.Trim();
                    byte[] buffer = new byte[2048];
                    buffer = Encoding.Default.GetBytes(txt);
                    int r = _socketConnect.Send(buffer);
                    MessageBox.Show("发送成功！");
                }
              
            }
            catch(Exception ex)
            {
                MessageBox.Show("发送消息出错:" + ex.Message);
            }
         
        }

        private void bn_WSConnect_Click(object sender, EventArgs e)
        {

            WebSocketConnect();
        }

        private  void WebSocketConnect()
        {
            if (_WebSocketClient == null)
                _WebSocketClient = new ClientWebSocket();
           
            _WebSocketClient.ConnectAsync(new Uri("ws://47.101.130.0:8090"), CancellationToken.None);
        }

        private void Receive()
        {
            try
            {
                while(true)
                {
                    byte[] buffer = new byte[2048];
                    int r = _socketConnect.Receive(buffer);
                    if(r == 0)
                    {
                        break;
                    }
                    else
                    {
                        string str = Encoding.Default.GetString(buffer);
                        MessageBox.Show($"Get Message:{str}");

                       
                    }
                    
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
