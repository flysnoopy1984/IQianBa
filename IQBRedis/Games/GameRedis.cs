using GameModel.Enums;
using GameRedis;
using GameRedis.Games;
using IQBCore.IQBPay.Models.OutParameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameRedis.Games
{
    public class GameRedis: BaseRedis
    {
        public OutAPIResult SetGameStatus(string RoomCode,GameStatus status)
        {
            OutAPIResult r = new OutAPIResult();
            try
            {
                r = _redis.HashAdd(GK.GameStatus, RoomCode, status);
            
            }
            catch (Exception ex)
            {
                r.ErrorMsg = ex.Message;
            }

            return r;
        }

        public OutAPIResult GetGameStatus(string RoomCode)
        {
            OutAPIResult r = new OutAPIResult();
            try
            {
                r.IntMsg = (int)_redis.HashGet(GK.GameStatus, RoomCode);
                 
            }
            catch (Exception ex)
            {
                r.ErrorMsg = ex.Message;
            }

            return r;
        }
    }
}
