using GameModel;
using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.Model;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameRedis.Games
{
    public class GameTableRedis: BaseRedis
    {
        public NResult<int> TableCardList(string RoomCode)
        {
            NResult<int> r = new NResult<int>();
            try
            {
               r.resultList =  _redis.SetGetAll<int>(GK.RoomTable(RoomCode));    
            }
            catch (Exception ex)
            {
                r.ErrorMsg = ex.Message;
            }

            return r;
        }

        public OutAPIResult AddCard(string RoomCode,int CardNo)
        {
            OutAPIResult r = new OutAPIResult();
            try
            {
                RedisValue v = CardNo;
                _redis.SetAdd(GK.RoomTable(RoomCode), v);

            }
            catch (Exception ex)
            {
                r.ErrorMsg = ex.Message;
            }

            return r;
        }

        public OutAPIResult CleanCard(string RoomCode)
        {
            OutAPIResult r = new OutAPIResult();
            try
            {
                r.IsSuccess = _redis.KeyDelete(GK.RoomTable(RoomCode));

            }
            catch (Exception ex)
            {
                r.ErrorMsg = ex.Message;
            }

            return r;
        }

        public OutAPIResult DotPosition(string RoomCode)
        {
            OutAPIResult r = new OutAPIResult();
            try
            {
                r.IntMsg = (int)_redis.HashGet(GK.GameDotPosition, RoomCode);

            }
            catch (Exception ex)
            {
                r.ErrorMsg = ex.Message;
            }

            return r;
        }


    }
}
