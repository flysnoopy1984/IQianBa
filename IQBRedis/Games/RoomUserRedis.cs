using GameCommon.Config;
using GameModel;
using GameModel.Enums;
using IQBCore.Common.Helper;
using IQBCore.IQBPay.Models.OutParameter;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBRedis.Games
{
    public class RoomUserRedis: BaseRedis
    {
        private static Object _lockSitDown = new Object();
        private static Object _lockSitUp = new Object();
      

        public OutAPIResult UserEntryRoom(int weight,string userOpenId,string roomCode)
        {
            OutAPIResult r = new OutAPIResult();
           
            var oneRoomUserKey = GK.GetOneRoomUser(roomCode);

            //获取用户信息
            var hasRoomUser = _redis.HashExist(userOpenId, GK.U_UserOpenId);
            if(hasRoomUser)
                _redis.KeyDelete(userOpenId);

            //创建Redis用户信息
            _redis.StartTrans();
            _redis.HashAdd(userOpenId, GK.U_UserOpenId, userOpenId);
            _redis.HashAdd(userOpenId, GK.U_RoomCode, roomCode);
            _redis.HashAdd(userOpenId, GK.U_RoomWeight, weight);
            _redis.HashAdd(userOpenId, GK.U_SeatNo,-1);
            r.IsSuccess = _redis.EndTrans();

        
            if (!r.IsSuccess)
            {
                _redis.KeyDelete(userOpenId);
                r.ErrorMsg = ($"[RoomUserRedis]UserEntry:用户[{userOpenId}]进入房间[{roomCode}]，获取信息失败");
                return r;
            }
            //加入房间
            r.IsSuccess = _redis.SetUpdate<string>(oneRoomUserKey, userOpenId);
           
            if (r.IsSuccess == false)
            {
                r.ErrorMsg = ($"[RoomUserRedis]UserEntry:用户[{userOpenId}]进入房间[{roomCode}]失败");
                _redis.KeyDelete(userOpenId);
            }
                
            return r;
        }

        public OutAPIResult UserExitRoom(string userOpenId)
        {
            
         
            OutAPIResult r = new OutAPIResult();
            try
            {
               //获取用户所在房间Code
                var roomCode = _redis.HashGet(userOpenId, GK.U_RoomCode);
                if(string.IsNullOrEmpty(roomCode))
                {
                    r.ErrorMsg = "用户已退出";
                    return r;
                }
                int seatNo;
                var IsInt = int.TryParse(_redis.HashGet(userOpenId, GK.U_SeatNo), out seatNo);
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
                    r = _redis.HashUpdate(userOpenId, GK.U_RoomCode, "");
                }
            }
            catch(Exception ex)
            {
                r.ErrorMsg = ex.Message;
            }
            return r;

        }

        //public RoomUserStatus GetUserStatus(string userOpenId)
        //{
        //    RedisValue rv = _redis.HashGet(userOpenId, ERoomUser.HF_UserStatus);
        //    if (rv.IsNullOrEmpty)
        //        return RoomUserStatus.Standup;
        //    else
        //        return (RoomUserStatus)Enum.Parse(typeof(RoomUserStatus), rv);
        //}
        private OutAPIResult CheckRoomUserHash(string userOpenId)
        {
            OutAPIResult result = new OutAPIResult();
            RedisValue rv = _redis.HashGet(userOpenId, GK.U_RoomWeight);
            if (rv.IsNullOrEmpty)
            {
                result.IntMsg = 1;
                result.ErrorMsg = "获取您的信息错误，请重新进入房间";
                NLogHelper.GameError($"UserSitDown Error: 用户{userOpenId}没有找到Weight");
                return result;
            }
            result.IntMsg = (int)rv;

            RedisValue roomCode = _redis.HashGet(userOpenId, GK.U_RoomCode);
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

        public OutAPIResult UserSitDown(string userOpenId,int seatNo)
        {
           
            OutAPIResult result = new OutAPIResult();
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
                    string rv = _redis.HashGet(roomSeatKey, GK.SeatNo(seatNo));

                    if(string.IsNullOrEmpty(rv))
                    {
                        _redis.StartTrans();
                        _redis.HashUpdate(roomSeatKey, GK.SeatNo(seatNo), userOpenId);
                        _redis.HashUpdate(userOpenId,GK.U_SeatNo, seatNo);

                        var r = _redis.EndTrans();

                        if (r == true)
                        {
                            double? dr = _redis.AdjustScore(AllRoomScoreKey, roomCode, 1);
                            if (dr == null)
                            {
                                _redis.StartTrans();
                                _redis.HashUpdate(roomSeatKey, GK.SeatNo(seatNo), "");
                                _redis.HashUpdate(userOpenId, GK.U_SeatNo, -1);
                                _redis.EndTrans();
                            }
                                
                        }
                        else
                        {
                            _redis.HashUpdate(roomSeatKey, GK.SeatNo(seatNo), "");

                            result.ErrorMsg = "坐下失败，请尝试重进房间！";
                            NLogHelper.GameError($"UserSitDown Error: 用户{userOpenId}坐下失败。");
                            return result;
                        }
                    }
                    else
                    {
                        result.ErrorMsg = "已有人坐下";
                    }
                    ////获取房间玩家数
                    //var score = _redis.GetSortedSetScore(roomKey, roomCode);
                    //if (score >= GameConfig.Room_Max_PlayerCount)
                    //{
                    //    result.ErrorMsg = "没有空位，已坐满！";
                    //    return result;
                    //}
                    //获取座位状态信息
                    //RedisValue rv = _redis.HashGet(userOpenId, ERoomUser.HF_UserStatus);
                    //if(rv.IsNullOrEmpty)
                    //{
                    //    result.ErrorMsg = "坐下失败，没有获取用户状态，请尝试重进房间！";
                    //    return result;
                    //}
                    //RoomUserStatus userStatus = (RoomUserStatus)Enum.Parse(typeof(RoomUserStatus),rv);

                    ////确认玩家状态
                    //if (userStatus == RoomUserStatus.Standup)
                    //{
                    //    var r = _redis.HashUpdate(userOpenId, ERoomUser.HF_UserStatus, (int)RoomUserStatus.SitDown);
                    //    if (r.IsSuccess == true){
                    //        double? dr = _redis.AdjustScore(roomKey, roomCode, 1);
                    //        if (dr == null)
                    //            r = _redis.HashUpdate(userOpenId, ERoomUser.HF_UserStatus, (int)RoomUserStatus.Standup);
                    //    }
                    //    else
                    //    {
                    //        result.ErrorMsg = "坐下失败，请尝试重进房间！";
                    //        NLogHelper.GameError($"UserSitDown Error: 用户{userOpenId}坐下失败。错误：{r.ErrorMsg}");
                    //        return result;
                    //    }
                    //}
                    //else
                    //{
                    //    result.ErrorMsg = "已经坐下..";
                    //    return result;
                    //}
                }
            }
            catch (Exception ex)
            {
                result.ErrorMsg = "坐下失败";

                NLogHelper.GameError("UserSitDown Error:" + ex.Message);
            }


            return result;
        }

        public OutAPIResult UserSitUp(string userOpenId)
        {
            OutAPIResult result = new OutAPIResult();            
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
                    var r = int.TryParse(_redis.HashGet(userOpenId, GK.U_SeatNo), out seatNo);
                    if (r && seatNo > 0)
                    {
                        _redis.StartTrans();
                        _redis.HashUpdate(roomSeatKey, GK.SeatNo(seatNo), "");
                        _redis.HashUpdate(userOpenId, GK.U_SeatNo, -1);
                        r = _redis.EndTrans();
                        if (r == true)
                        {
                            double? dr = _redis.AdjustScore(AllRoomScoreKey, roomCode, -1);
                            if (dr == null)
                            {
                                _redis.StartTrans();
                                _redis.HashUpdate(roomSeatKey, GK.SeatNo(seatNo), userOpenId);
                                _redis.HashUpdate(userOpenId, GK.U_SeatNo, seatNo);
                                _redis.EndTrans();
                            }
                                
                        }
                        else
                        {
                            _redis.HashUpdate(userOpenId, GK.U_SeatNo, seatNo);

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
               
                // string rv = _redis.HashGet(roomSeatKey, GK.SeatNo(seatNo));

                ////获取用户状态
                //RedisValue rv = _redis.HashGet(userOpenId, ERoomUser.HF_UserStatus);
                //if (rv.IsNullOrEmpty)
                //{
                //    result.ErrorMsg = "站起失败，没有获取用户状态，请尝试重进房间！";
                //    return result;
                //}
                //RoomUserStatus userStatus = (RoomUserStatus)Enum.Parse(typeof(RoomUserStatus), rv);
                //if (userStatus == RoomUserStatus.SitDown)
                //{
                //    lock (_lockSitDown)
                //    {
                //        var r = _redis.HashUpdate(userOpenId, ERoomUser.HF_UserStatus, (int)RoomUserStatus.Standup);
                //        if (r.IsSuccess == true)
                //        {
                //            double? dr = _redis.AdjustScore(roomKey, roomCode, -1);
                //            if (dr == null)
                //                r = _redis.HashUpdate(userOpenId, ERoomUser.HF_UserStatus, (int)RoomUserStatus.SitDown);
                //        }
                //        else
                //        {
                //            result.ErrorMsg = "站起失败，请重进游戏尝试！";
                //            NLogHelper.GameError($"UserSitDown Error: 用户{userOpenId}站起失败。错误：{r.ErrorMsg}");
                //            return result;
                //        }
                //    }
                //}
                //else
                //{
                //    result.ErrorMsg = "您已站起";
                //    return result;
                //}

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
               
            }
            return result;

        }
    }
}
