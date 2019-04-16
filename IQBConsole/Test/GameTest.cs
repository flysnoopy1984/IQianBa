using GameModel;
using GameRedis.Games;
using GameServer.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBConsole.Test
{
    public class GameTest
    {
        public void DoSomethine()
        {
            RoomUserRedis redis = new RoomUserRedis();
            CardDataManager CardDataManager = new CardDataManager();
            List<ERoomUser> Players = redis.GetAllPlayer("04023135").resultList;

        //    ResultGameShuffleStart result = new ResultGameShuffleStart(RoomCode);

            CardDataManager.ShuffleCard(Players);

            foreach (ERoomUser p in Players)
            {
                var cl = p.CardList;
                //RoomUserRedis.SetPlayer(RoomCode, p);
            }
        }

        public ERoom GetRoom()
        {
            RoomRedis redis = new RoomRedis();
            return redis.GetRoom("04023135").Instance;
        }
    }
}
