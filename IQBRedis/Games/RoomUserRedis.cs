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
 
       // private readonly Object _lockSitUp = new Object();

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
               r = _redis.HashFindAllValue<ERoomUser>(GK.RoomPlayer(roomCode));
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
                    CardList = new List<ECard>(),
                    RemainCoins = coins,
                    SeatNo = seatNo,
                };

                _redis.HashAddT<ERoomUser>(GK.RoomPlayer(roomCode), userOpenId, user);
            }
            catch(Exception ex)
            {
                r.ErrorMsg = ex.Message;
            }

            return r;
        }

        public OutAPIResult UserLogin(string userOpenId, string roomCode)
        {
            var userKey = GK.UserInfo(userOpenId);

            OutAPIResult r = new OutAPIResult();
            try
            {
                _redis.StartTrans();
                _redis.HashAdd(userKey, GK.U_UserOpenId, userOpenId);
                _redis.HashAdd(userKey, GK.U_RoomCode, roomCode);
               // _redis.HashAdd(userKey, GK.U_RoomWeight, weight);
                _redis.HashAdd(userKey, GK.U_SeatNo, 0);
                r.IsSuccess = _redis.EndTrans();

                if (!r.IsSuccess)
                {
                    _redis.KeyDelete(userKey);
                    r.ErrorMsg = ($"用户信息登陆失败");
                    return r;
                }
            }
            catch(Exception ex)
            {
                r.ErrorMsg = ($"用户信息登陆失败");
                NLogHelper.GameError($"[RoomUserRedis]UserLogin:用户[{userOpenId}]进入房间[{roomCode}]失败:{ex.Message}");
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
        public OutAPIResult UserEntryRoom(string userOpenId,string roomCode)
        {
            OutAPIResult r = new OutAPIResult();
            var oneRoomUserKey = GK.GetOneRoomUser(roomCode);
        
            //登陆用户信息
         
            r = _redis.HashAdd(oneRoomUserKey, userOpenId,roomCode);
          
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
                //  r.IsSuccess = _redis.DeleteSet<string>(oneRoomUserKey, userOpenId);
                r = _redis.HashDelete(oneRoomUserKey, userOpenId);
                if (r.IsSuccess)
                {
                    //更新用户-房间信息
                    r = _redis.HashAdd(userKey, GK.U_RoomCode, "");
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

            var userKey = GK.UserInfo(userOpenId);

            RedisValue roomCode = _redis.HashGet(userKey, GK.U_RoomCode);
            if (roomCode.IsNullOrEmpty)
            {
                result.IntMsg = 2;
                result.ErrorMsg = "获取房间信息错误，请重新进入房间";
                NLogHelper.GameError($"UserSitDown Error: 用户{userOpenId}没有找到房间");
                return result;
            }

            ERoom room = _redis.HashGet<ERoom>(GK.ALLRoomEntity,roomCode);
         //   RedisValue rv = _redis.HashGet(userKey, GK.U_RoomWeight);
            if (room == null)
            {
                result.IntMsg = 1;
                result.ErrorMsg = "获取您的信息错误，请重新进入房间";
                NLogHelper.GameError($"UserSitDown Error: 用户{userOpenId}没有找到Weight");
                return result;
            }

            result.IntMsg = room.Weight;
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
                if (!result.IsSuccess) return result;

                int weight = (int)result.IntMsg;
                var roomCode = result.SuccessMsg;
                var AllRoomScoreKey = GK.AllRoomScore(weight);
                var roomSeatKey = GK.Room_Seat(roomCode);
               
               
                    //系统分配入座
                if(seatNo == -1)
                {
                    var userSeat = (int)_redis.HashGet(userKey, GK.U_SeatNo);
                    if (userSeat > 0)
                    {
                        result.ErrorMsg = "您已入座";
                        return result;
                    }
                    seatNo = sysAssignSeat(roomCode);
                    if(seatNo == 0)
                    {
                        result.ErrorMsg = "没有位置可坐";
                        return result;
                    }
                }
                //选定入座
                else
                {
                    var seatUser = _redis.HashGet(roomSeatKey, seatNo);
                    if (!string.IsNullOrEmpty(seatUser))
                    {
                        result.ErrorMsg = "已有人坐下";
                        return result;
                    }
                }

                _redis.StartTrans();
                _redis.HashAdd(roomSeatKey, GK.SeatNo(seatNo), userOpenId);
                _redis.HashAdd(userKey, GK.U_SeatNo, seatNo);

                var r = _redis.EndTrans();

                if (r == true)
                {
                    double? dr = _redis.AdjustScore(AllRoomScoreKey, roomCode, 1);
                    if (dr == null)
                    {
                        _redis.StartTrans();
                        _redis.HashAdd(roomSeatKey, GK.SeatNo(seatNo), "");
                        _redis.HashAdd(userKey, GK.U_SeatNo, -1);
                        _redis.EndTrans();
                    }
                    else
                        AddNewPlayer(userOpenId, roomCode, seatNo, coins);
                }
                else
                {
                    _redis.HashAdd(roomSeatKey, GK.SeatNo(seatNo), "");

                    result.ErrorMsg = "坐下失败，请尝试重进房间！";
                    NLogHelper.GameError($"UserSitDown Error: 用户{userOpenId}坐下失败。");
                    return result;
                }
                result.SuccessMsg = roomCode;
                result.IntMsg = seatNo;

            }
            catch (Exception ex)
            {
                result.ErrorMsg = "坐下失败";

                NLogHelper.GameError("UserSitDown Error:" + ex.Message);
            }


            return result;
        }

        //public static int GetSeatNo(string seat)
        //{
        //    if(!string.IsNullOrEmpty(seat))
        //    {
        //        return int.Parse(seat.Substring(2));
        //    }
        //    return -1;
        //}

        public int sysAssignSeat(string roomCode)
        {
            var roomSeatKey = GK.Room_Seat(roomCode);
            try
            {
                var dicSeat = _redis.HashFindAll(roomSeatKey);
                if (dicSeat != null)
                {
                    foreach(var seat in dicSeat)
                    {
                        if (string.IsNullOrEmpty(seat.Value))
                        {
                            return (int)seat.Name;
                        }
                    }
                    //foreach (KeyValuePair<int, string> seat in dicSeat.resultDict)
                    //{
                    //    if (string.IsNullOrEmpty(seat.Value))
                    //    {
                    //        return seat.Key;
                    //       // return GetSeatNo(seat.Key);
                    //    }
                    //}
                    return 0;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
          
            return -1;

        }

        public static string GetUserRoomCode(string userOpenId)
        {
            try
            {
                RedisManager _redis = new RedisManager();
                return _redis.HashGet(GK.UserInfo(userOpenId), GK.U_RoomCode);
            }
            catch
            {
                return null;
            }
        }

        public static int GetUserSeatNo(string userOpenId)
        {
            try
            {
                var userKey = GK.UserInfo(userOpenId);
                RedisManager _redis = new RedisManager();
                var userSeat = (int)_redis.HashGet(userKey, GK.U_SeatNo);
                return userSeat;
            }
            catch
            {
                return -1;
            }
          
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
                var r = int.TryParse(_redis.HashGet(userKey, GK.U_SeatNo), out seatNo);
                if (r && seatNo > 0)
                {
                    _redis.StartTrans();
                    _redis.HashAdd(roomSeatKey, GK.SeatNo(seatNo), "");
                    _redis.HashAdd(userKey, GK.U_SeatNo, 0);
                    r = _redis.EndTrans();
                    if (r == true)
                    {
                        double? dr = _redis.AdjustScore(AllRoomScoreKey, roomCode, -1);
                        if (dr == null)
                        {
                            _redis.StartTrans();
                            _redis.HashAdd(roomSeatKey, GK.SeatNo(seatNo), userOpenId);
                            _redis.HashAdd(userKey, GK.U_SeatNo, seatNo);
                            _redis.EndTrans();
                        }
                        else
                            ExitPlayer(userOpenId, roomCode);

                    }
                    else
                    {
                        _redis.HashAdd(userKey, GK.U_SeatNo, seatNo);

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
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
               
            }
            return result;

        }

        /// <summary>
        /// 找到下一个可用的桌上指针位置
        /// </summary>
        /// <param name="RoomCode"></param>
        /// <returns></returns>
        public DicResult<int,string> FindAllSeatNo(string roomCode)
        {
            DicResult<int, string> r = new DicResult<int, string>();
            try
            {
                var roomSeatKey = GK.Room_Seat(roomCode);
                var dicSeat = _redis.HashFindAll(roomSeatKey);
                foreach(var s in dicSeat)
                {
                    r.resultDic.Add((int)s.Name, s.Value);
                }
            }
            catch (Exception ex)
            {
                r.ErrorMsg = ex.Message;
            }
            return r;
        }
    }
}
