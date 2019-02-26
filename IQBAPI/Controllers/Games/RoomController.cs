using GameCommon.Config;
using GameCommon.DataBase;
using GameModel;
using GameModel.Enums;
using IQBCore.Common.Helper;
using IQBCore.IQBPay.Models.OutParameter;
using IQBRedis;
using IQBRedis.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;

namespace IQBAPI.Controllers.Games
{
    public class RoomController : BaseAPIController
    {
        private static Object _LockRoom = new Object();
        private static Object _EntryRoom = new Object();
        private static Object _ExitRoom = new Object();

        #region Sql Sever
        //[HttpPost]
        //public OutAPIResult UserStandUp(string roomCode,string UserOpenId)
        //{
        //    OutAPIResult result = new OutAPIResult();
        //    try
        //    {
        //        using (TransactionScope trans = new TransactionScope())
        //        {
        //            using (GameContent db = new GameContent())
        //            {

        //                var sql = $"update RoomUser set UserStatus = {(int)RoomUserStatus.Standup} where RoomCode='{roomCode}' and UserOpenId='{UserOpenId}'";
                       
        //                int c = db.Database.ExecuteSqlCommand(sql);
        //                //的确有用户站起
        //                if(c>0)
        //                {
        //                    //Room 表玩家总数-1
        //                    sql = $"update Room set PlayerCount = PlayerCount - 1 where Code='{roomCode}'";

        //                    db.Database.ExecuteSqlCommand(sql);
        //                }
        //            }
        //            trans.Complete();
        //        }

                  

        //    }
        //    catch (Exception ex)
        //    {
        //        result.ErrorMsg = ex.Message;
        //    }
        //    return result;
        //}
        //[HttpPost]
        //public OutAPIResult UserSitDown(string roomCode,string UserOpenId)
        //{
        //    OutAPIResult result = new OutAPIResult();
        //    try
        //    {
        //        lock(_LockRoom)
        //        {
        //            using (TransactionScope trans = new TransactionScope())
        //            {

        //                using (GameContent db = new GameContent())
        //                {
        //                    int playerInCount = db.DBRoomUser.Count(a => a.RoomCode == roomCode && a.UserStatus == RoomUserStatus.Standup);
        //                    if(playerInCount> GameConfig.Room_Max_PlayerCount)
        //                    {
        //                        result.ErrorMsg = "没有位置可以坐下";
        //                        return result;
        //                    }

        //                    var sql = $"update RoomUser set UserStatus = {(int)RoomUserStatus.SitDown} where RoomCode='{roomCode}' and UserOpenId='{UserOpenId}'";
        //                    int c = db.Database.ExecuteSqlCommand(sql);
        //                    if(c>0)
        //                    {
        //                        sql = $"update Room set PlayerCount = PlayerCount + 1 where Code='{roomCode}'";
        //                        db.Database.ExecuteSqlCommand(sql);
        //                    }
        //                }
        //                trans.Complete();
        //            }
        //        }
                   
        //    }
        //    catch (Exception ex)
        //    {
        //        result.ErrorMsg = ex.Message;
        //    }
        //    return result;
        //}

        //[HttpPost]
        ///// <summary>
        ///// 先找有没有房间 ，没有尝试创建
        ///// -1 没有可用的房间，系统允许最大房间数量已满
        ///// </summary>
        ///// <returns></returns>
        //public OutAPIResult EntryAvailableRoom(string userOpenId,int weight)
        //{
        //    OutAPIResult result = new OutAPIResult();
        
        //    try
        //    {
        //        lock (_EntryRoom)
        //        {
        //            using (TransactionScope trans = new TransactionScope())
        //            {
        //                string sql = $@"select Code from Room 
        //                        where PlayerCount<{GameConfig.Room_Max_PlayerCount}
        //                          and Weight={weight}";
        //                using (GameContent db = new GameContent())
        //                {
        //                    string availcode = db.Database.SqlQuery<string>(sql).FirstOrDefault();
        //                    if (availcode == null)
        //                    {
        //                        //暂时不控制开房的最大数量

        //                        //创建Room
        //                        var Room = db.DBRoom.Add(new ERoom
        //                        {
        //                            Code = StringHelper.GenerateRoomCode(),
        //                            PlayerCount = 0,
        //                            Weight = weight,
        //                            CreateTime = DateTime.Now,

        //                        });
        //                        db.SaveChanges();

        //                        availcode = Room.Code;
        //                    }
        //                    //同时更改房间用户

        //                    db.DBRoomUser.Add(new ERoomUser
        //                    {
        //                        UserOpenId = userOpenId,
        //                        RoomCode = availcode,
        //                        UserStatus = RoomUserStatus.Standup,
        //                    });
        //                    db.SaveChanges();

        //                    result.SuccessMsg = availcode;
        //                }
        //                trans.Complete();
        //            }
                   
        //        }
                
        //    }
        //    catch(Exception ex)
        //    {
        //        result.ErrorMsg = ex.Message;
        //    }
        //    return result;  
        //}

        #endregion

        #region Redis
        [HttpPost]
        public OutAPIResult UserSitDown_Redis(string UserOpenId,int SeatNo)
        {
            OutAPIResult result = new OutAPIResult();
            RoomUserRedis redis = new RoomUserRedis();
            try
            {
                result = redis.UserSitDown(UserOpenId, SeatNo);

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        [HttpPost]
        public OutAPIResult UserStandUp_Redis(string UserOpenId)
        {
            OutAPIResult result = new OutAPIResult();
            RoomUserRedis redis = new RoomUserRedis();
            try
            {
                result = redis.UserSitUp(UserOpenId);

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        [HttpPost]
        public OutAPIResult EntryAvailableRoom_Redis(string userOpenId, int weight)
        {
            OutAPIResult result = new OutAPIResult();
            RoomRedis roomRedis = new RoomRedis();
            RoomUserRedis roomUserRedis = new RoomUserRedis();
            try
            {
                lock (_EntryRoom)
                {
                    result = roomRedis.FindOrCreateRoom(userOpenId, weight);
                    if(result.IsSuccess)
                    {
                        result = roomUserRedis.UserEntryRoom(weight,userOpenId, result.SuccessMsg);
                    }
                }

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }

        [HttpPost]
        public OutAPIResult ExitRoom_Redis(string userOpenId)
        {
            OutAPIResult result = new OutAPIResult();
         
            RoomUserRedis roomUserRedis = new RoomUserRedis();
            try
            {
                lock (_ExitRoom)
                {
                 
                    result = roomUserRedis.UserExitRoom(userOpenId);
                }

            }
            catch (Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }
            return result;
        }


        #endregion
    }
}
