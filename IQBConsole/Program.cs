using GameServer;
using IQBConsole.SocketServer;
using IQBCore.IOS.APNS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace IQBConsole
{
    class Program
    {
        static PPBatchJob job = new PPBatchJob();
        static PayTest payTest = new PayTest();

        static int _OrderDiffMin = 90;
        static int _interval = 60;

        static OOSocketServer _SocketServer = new OOSocketServer();
        static OOWebSocket _WebSocket = new OOWebSocket();

        static GameRunner _websockerRuner = new GameRunner();

        static void Main(string[] args)
        {
          
            try
            {
                _websockerRuner.InitSocket();
               // _WebSocket.Init();
               // _SocketServer.Init();
               //PushSharp ps = new PushSharp();
               //ps.StartServer();

                //ps.SendMsg();

                Console.Read();

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Read();
            }
            finally
            {
              
            }
            
        }



       

      
    }
}
