using GameModel;
using GameModel.Enums;
using GameRedis;
using GameRedis.Games;
using GameServer;
using IQBConsole.SocketServer;
using IQBConsole.Test;
using IQBCore.IOS.APNS;
using IQBCore.IQBPay.Models.OutParameter;
using Newtonsoft.Json;
using StackExchange.Redis;
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
        static TestRunner _testRuner = new TestRunner();
        static RedisManager redis = new RedisManager();
        static RoomUserRedis _roomUserRedis = new RoomUserRedis();
        static GameRedis.Games.GameRedis _gameRedis = new GameRedis.Games.GameRedis();
        static void Main(string[] args)
        {
          
            try
            {
                var r = _gameRedis.SetGameStatus("test", GameStatus.WaitPlayer);
                _gameRedis.SetGameStatus("test2", GameStatus.Settlement);
                //Console.WriteLine(no);
                //  RedisTest.TestTable();
                // _websockerRuner.InitSocket();
                // _WebSocket.Init();
                // _SocketServer.Init();
                //PushSharp ps = new PushSharp();
                //ps.StartServer();

                //ps.SendMsg();
                //
                //ConnectionMultiplexer conn = redis.NewConnection();
                //  _RedisClient = new StackExchangeRedisCacheClient(conn, new NewtonsoftSerializer());
                //   redis.WriteSortedSet2<string>("test1", "abc", 0);
                //  var r = redis.FindSortedSet<string>("test1");
                // RedisValue rv = JsonConvert.SerializeObject("abc");


                //var s = redis.AdjustScore("Room_AllRoom_0", "02190154", -1);
                //Console.WriteLine(s);


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
