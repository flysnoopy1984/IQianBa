using GameRedis.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBConsole.Test
{
    public class RedisTest
    {
        static GameTableRedis gameRedis = new GameTableRedis();
        public static void TestTable()
        {
            //string roomCode = "TestRoom";
            ////   gameRedis.AddCard(roomCode, 3);
            //gameRedis.CleanCard(roomCode);
            //  var r =  gameRedis.TableCardList(roomCode);
            //foreach(int no in r.resultList)
            //{
            //    Console.WriteLine(no);
            //}
        }
    }
}
