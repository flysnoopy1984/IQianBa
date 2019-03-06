using GameCommon.Config;
using GameModel;
using GameModel.Enums;
using IQBCore.Common.Helper;
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
    public class RoomUserRedis: BaseRedis
    {
        private static Object _lockSitDown = new Object();
        private static Object _lockSitUp = new Object();

        public int GetAllPlayerCount(string roomCode)
        {
            return 0;
        }
        /// <summary>
        /// 获取一个房间中所有玩家
        /// </summary>
        /// <param name="roomCode"></param>
        /// <returns></returns>
        public NResult<ERoomUser> GetAllPlayer(string roomCode)
        {
            NResult<ERoomUser> r = new NResult<ERoomUser>();
            try
            {
                _redis.HashFindAllValue<ERoomUser>(GK.RoomPlayer(roomCode));
            }
            catch (Exception ex)
            {
                r.ErrorMsg = ex.Message;
                
            }
            return r;
        }
          

        private OutAPIResult ExitPlayer(string userOpenId,string roomCode)
        {
            OutAPIResult r = new OutAPIResult();
            try
            {
                _redis.HashDelete(GK.RoomPlayer(roomCode), userOpenId);
            }
            catch (Exception ex)
            {
                r.ErrorMsg = ex.Message;
            }

            return r;
        }
      
        private OutAPIResult AddNewPlayer(string userOpenId, string roomCode,int seatNo,decimal coins)
        {
            OutAPIResult r = new OutAPIResult();
            try
            {
                ERoomUser user = new ERoomUser
                {
                    RoomCode = roomCode,
                    UserOpenId = userOpenId,
                    CardList = new List<int>(),
                    RemainCoins = coins,
                    SeatNo = seatNo,
                };

                _redis.HashAdd<ERoomUser>(GK.RoomPlayer(roomCode), userOpenId, user);
            }
            catch(Exception ex)
            {
                r.ErrorMsg = ex.Message;
            }

            return r;
        }

       
        /// <summary>
        /// 一般用户进入房间
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="userOpenId"></param>
        /// <param name="roomCode"></param>
        /// <returns></returns>
        public OutAPIResult UserEntryRoom(int weight,string userOpenId,string roomCode)
        {
            OutAPIResult r = new OutAPIResult();
           
            var oneRoomUserKey = GK.GetOneRoomUser(roomCode);

            var userKey = GK.UserInfo(userOpenId);
            //获取用户信息
            var hasRoomUser = _redis.HashExist(userKey, GK.U_UserOpenId);
            if(hasRoomUser)
                _redis.KeyDelete(userKey);

            //创建Redis用户信息
            _redis.StartTrans();
            _redis.HashAdd(userKey, GK.U_UserOpenId, userOpenId);
            _redis.HashAdd(userKey, GK.U_RoomCode, roomCode);
            _redis.HashAdd(userKey, GK.U_RoomWeight, weight);
            _redis.HashAdd(userKey, GK.U_SeatNo,-1);
            r.IsSuccess = _redis.EndTrans();

        
            if (!r.IsSuccess)
            {
                _redis.KeyDelete(userKey);
                r.ErrorMsg = ($"[RoomUserRedis]UserEntry:用户[{userOpenId}]进入房间[{roomCode}]，获取信息失败");
                return r;
            }
            //加入房间
            r.IsSuccess = _redis.SetUpdate<string>(oneRoomUserKey, userOpenId);
           
            if (r.IsSuccess == false)
            {
                r.ErrorMsg = ($"[RoomUserRedis]UserEntry:用户[{userOpenId}]进入房间[{roomCode}]失败");
                _redis.KeyDelete(userKey);
            }
                
            return r;
        }

        /// <summary>
        /// 用户退出房间
        /// </summary>
        /// <param name="userOpenId"></param>
        /// <returns></returns>
        public OutAPIResult UserExitRoom(string userOpenId)
        {
            
         
            OutAPIResult r = new OutAPIResult();
            try
            {
                //获取用户所在房间Code
                var userKey = GK.UserInfo(userOpenId);

                var roomCode = _redis.HashGet(userKey, GK.U_RoomCode);
                if(string.IsNullOrEmpty(roomCode))
                {
                    r.ErrorMsg = "用户已退出";
                    return r;
                }
                int seatNo;
                var IsInt = int.TryParse(_redis.HashGet(userKey, GK.U_SeatNo), out seatNo);
                if(IsInt)
                {
                    if (seatNo > 0)
                        this.UserSitUp(userOpenId);
                }
                var oneRoomUserKey = GK.GetOneRoomUser(roomCode);
              
                //删除房间-用户关系
                r.IsSuccess = _redis.DeleteSet<string>(oneRoomUserKey, userOpenId);
                if (r.IsSuccess)
                {
                    //更新用户-房间信息
                    r = _redis.HashUpdate(userKey, GK.U_RoomCode, "");
                }
            }
            catch(Exception ex)
            {
                r.ErrorMsg = ex.Message;
            }
            return r;

        }

       /// <summary>
       /// 检查Hash用户信息，获取房间号
       /// </summary>
       /// <param name="userOpenId"></param>
       /// <returns></returns>
        private OutAPIResult CheckRoomUserHash(string userOpenId)
        {
            OutAPIResult result = new OutAPIResult();
            RedisValue rv = _redis.HashGet(userOpenId, GK.U_RoomWeight);
            var userKey = GK.UserInfo(userOpenId);
            if (rv.IsNullOrEmpty)
            {
                result.IntMsg = 1;
                result.ErrorMsg = "获取您的信息错误，请重新进入房间";
                NLogHelper.GameError($"UserSitDown Error: 用户{userOpenId}没有找到Weight");
                return result;
            }
            result.IntMsg = (int)rv;

            RedisValue roomCode = _redis.HashGet(userKey, GK.U_RoomCode);
            if (roomCode.IsNullOrEmpty)
            {
                result.IntMsg = 2;
                result.ErrorMsg = "获取房间信息错误，请重新进入房间";
                NLogHelper.GameError($"UserSitDown Error: 用户{userOpenId}没有找到房间");
                return result;
            }

            result.SuccessMsg = roomCode;
            return result;
        }

        /// <summary>
        /// 用户入座，成为玩家
        /// </summary>
        /// <param name="userOpenId"></param>
        /// <param name="seatNo">-1 代表由系统分配</param>
        /// <param name="coins"></param>
        /// <returns>IntMsg:SeatNo</returns>
        public OutAPIResult UserSitDown(string userOpenId,int seatNo,decimal coins)
        {
           
            OutAPIResult result = new OutAPIResult();
            var userKey = GK.UserInfo(userOpenId);
            try
            {
                result = CheckRoomUserHash(userOpenId);
                int weight = (int)result.IntMsg;
                var roomCode = result.SuccessMsg;

                var AllRoomScoreKey = GK.AllRoomScore(weight);
                var roomSeatKey = GK.Room_Seat(roomCode);
                lock (_lockSitDown)
                {
                    //获取座位信息
                    if(seatNo == -1)
                    {
                        seatNo = sysAssignSeat(roomCode);
                        if(seatNo == 0)
                        {
                            result.ErrorMsg = "没有位置可坐";
                            return result;
                        }
                    }
                    string rv = _redis.HashGet(roomSeatKey, GK.SeatNo(seatNo));

                    if(string.IsNullOrEmpty(rv))
                    {
                        _redis.StartTrans();
                        _redis.HashUpdate(roomSeatKey, GK.SeatNo(seatNo), userOpenId);
                        _redis.HashUpdate(userKey, GK.U_SeatNo, seatNo);

                        var r = _redis.EndTrans();

                        if (r == true)
                        {
                            double? dr = _redis.AdjustScore(AllRoomScoreKey, roomCode, 1);
                            if (dr == null)
                            {
                                _redis.StartTrans();
                                _redis.HashUpdate(roomSeatKey, GK.SeatNo(seatNo), "");
                                _redis.HashUpdate(userKey, GK.U_SeatNo, -1);
                                _redis.EndTrans();
                            }
                            else
                                AddNewPlayer(userOpenId, roomCode, seatNo, coins);


                        }
                        else
                        {
                            _redis.HashUpdate(roomSeatKey, GK.SeatNo(seatNo), "");

                            result.ErrorMsg = "坐下失败，请尝试重进房间！";
                            NLogHelper.GameError($"UserSitDown Error: 用户{userOpenId}坐下失败。");
                            return result;
                        }
                        result.IntMsg = seatNo;
                    }
                    else
                    {
                        result.ErrorMsg = "已有人坐下";
                    }
                  
                }
            }
            catch (Exception ex)
            {
                result.ErrorMsg = "坐下失败";

                NLogHelper.GameError("UserSitDown Error:" + ex.Message);
            }


            return result;
        }

        public static int GetSeatNo(string seat)
        {
            if(!string.IsNullOrEmpty(seat))
            {
                return int.Parse(seat.Substring(2));
            }
            return -1;
        }

        public int sysAssignSeat(string roomCode)
        {
            var roomSeatKey = GK.Room_Seat(roomCode);
            var dicSeat = _redis.HashFindAll<string>(roomSeatKey);
            if(dicSeat.resultDict !=null)
            {
                foreach (KeyValuePair<string,string> seat in dicSeat.resultDict)
                {
                    if(string.IsNullOrEmpty(seat.Value))
                    {
                        return GetSeatNo(seat.Key);
                    }
                }
                return 0;
            }
            return -1;

        }

        /// <summary>
        /// 玩家站起，从玩家变为用户
        /// </summary>
        /// <param name="userOpenId"></param>
        /// <returns></returns>
        public OutAPIResult UserSitUp(string userOpenId)
        {
            OutAPIResult result = new OutAPIResult();
            var userKey = GK.UserInfo(userOpenId);
            try
            {
                result = CheckRoomUserHash(userOpenId);
                int weight = (int)result.IntMsg;
                var roomCode = result.SuccessMsg;

                var AllRoomScoreKey = GK.AllRoomScore(weight);
                var roomSeatKey = GK.Room_Seat(roomCode);

                int seatNo;
                lock (_lockSitDown)
                {
                    var r = int.TryParse(_redis.HashGet(userKey, GK.U_SeatNo), out seatNo);
                    if (r && seatNo > 0)
                    {
                        _redis.StartTrans();
                        _redis.HashUpdate(roomSeatKey, GK.SeatNo(seatNo), "");
                        _redis.HashUpdate(userKey, GK.U_SeatNo, -1);
                        r = _redis.EndTrans();
                        if (r == true)
                        {
                            double? dr = _redis.AdjustScore(AllRoomScoreKey, roomCode, -1);
                            if (dr == null)
                            {
                                _redis.StartTrans();
                                _redis.HashUpdate(roomSeatKey, GK.SeatNo(seatNo), userOpenId);
                                _redis.HashUpdate(userKey, GK.U_SeatNo, seatNo);
                                _redis.EndTrans();
                            }
                            else
                                ExitPlayer(userOpenId, roomCode);
                                
                        }
                        else
                        {
                            _redis.HashUpdate(userKey, GK.U_SeatNo, seatNo);

                            result.ErrorMsg = "坐下失败，请尝试重进房间！";
                            NLogHelper.GameError($"UserSitDown Error: 用户{userOpenId}坐下失败.");
                            return result;
                        }

                    }
                        else
                    {
                        result.ErrorMsg = "您已站起"; 
                        return result;
                    }
                }
               
              
            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
               
            }
            return result;

        }
    }
}
