using GameModel;
using GameModel.Enums;
using GameRedis;
using GameRedis.Games;
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
    public class GameRedis: BaseRedis
    {

        public OutAPIResult SetGameBasic(EGameInfo gameInfo)
        {
            OutAPIResult r = new OutAPIResult();
            try
            {
                if(string.IsNullOrEmpty(gameInfo.RoomCode))
                {
                    r.ErrorMsg = "为获取游戏房间号!";
                    return r;
                }
                var key = GK.GameBasic;

                r =_redis.HashAddT<EGameInfo>(key, gameInfo.RoomCode, gameInfo);
            }
            catch (Exception ex)
            {
                r.ErrorMsg = ex.Message;
            }

            return r;
        }

        public OutAPIResult SetGameBasic(string RoomCode,GameStatus newStatus,GameTurn newTurn)
        {
            OutAPIResult r = new OutAPIResult();
            try
            {
                if (string.IsNullOrEmpty(RoomCode))
                {
                    r.ErrorMsg = "为获取游戏房间号!";
                    return r;
                }
                var key = GK.GameBasic;

                var gi = _redis.HashGet<EGameInfo>(key, RoomCode);
                gi.GameStatus = newStatus;
                gi.GameTurn = newTurn;

                r = _redis.HashAddT<EGameInfo>(key, RoomCode, gi);
            }
            catch (Exception ex)
            {
                r.ErrorMsg = ex.Message;
            }

            return r;
        }

        public NResult<EGameInfo> GetGameBasic(string RoomCode)
        {
            NResult<EGameInfo> r = new NResult<EGameInfo>();
            try
            {
               
                var key = GK.GameBasic;

                r.resultObj  = _redis.HashGet<EGameInfo>(key, RoomCode);
            }
            catch (Exception ex)
            {
                r.ErrorMsg = ex.Message;
            }

            return r;
        }


        public OutAPIResult SetGameStatus(string RoomCode,GameStatus status)
        {
            OutAPIResult r = new OutAPIResult();
            try
            {
                r = _redis.HashAdd(GK.GameStatus, RoomCode, (int)status);
            
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



        public OutAPIResult GetGameTurn(string RoomCode)
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

        #region Table
        public NResult<int> TableCardList(string RoomCode)
        {
            NResult<int> r = new NResult<int>();
            try
            {
                r.resultList = _redis.SetGetAll<int>(GK.RoomTable(RoomCode));
            }
            catch (Exception ex)
            {
                r.ErrorMsg = ex.Message;
            }

            return r;
        }

        public OutAPIResult AddCard(string RoomCode, int CardNo)
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
        #endregion

       

    }
}
