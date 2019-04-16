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
        private string _RoomCode; 
        public GameRedis(string rc)
        {
            _RoomCode = rc;
        }

        #region Game Basic Info
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

        #endregion

        #region TableCard

        public OutAPIResult SetTableCards(List<ECard> tableCards)
        {
            OutAPIResult r = new OutAPIResult();
            try
            {
                string Key = GK.TableCard(_RoomCode);
                int No = 1;
                foreach(ECard card in tableCards)
                {
                    _redis.HashAddT(Key, No, card);
                    No++;
                }
            }
            catch (Exception ex)
            {
                r.ErrorMsg = ex.Message;
            }

            return r;
        }

        public NResult<ECard> GetTableCards()
        {
            NResult<ECard> result = new NResult<ECard>();
            try
            {
                string Key = GK.TableCard(_RoomCode);
                result = _redis.HashFindAllValue<ECard>(Key);
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }

            return result;
        }
        #endregion

        #region GameCoins

        public OutAPIResult AddCoinDetail(string RoomCode,ECoinDetail PileCoin)
        {
            OutAPIResult r = new OutAPIResult();
            try
            {
                if (string.IsNullOrEmpty(RoomCode))
                {
                    r.ErrorMsg = "为获取游戏房间号!";
                    return r;
                }
                var key = GK.CoinDetail(RoomCode);

                r = _redis.HashAddT<ECoinDetail>(key, PileCoin.UserOpenId, PileCoin);
            }
            catch (Exception ex)
            {
                r.ErrorMsg = ex.Message;
            }

            return r;
        }

        //public SResult<EGameCoins> GetGameCoins()
        //{
        //    SResult<EGameCoins> result = new SResult<EGameCoins>();
        //    try
        //    {
               
        //    }
        //    catch(Exception ex)
        //    {
        //        result.ErrorMsg = ex.Message;
        //    }
        //    return result;
        //}
        #endregion



    }
}
