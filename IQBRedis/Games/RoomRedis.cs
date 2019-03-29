using GameCommon.Config;
using GameModel;
using GameModel.Enums;
using IQBCore.Common.Helper;
using IQBCore.IQBPay.Models.OutParameter;
using IQBCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameRedis.Games
{
    public class RoomRedis: BaseRedis
    {
        private readonly Object _LockRoom = new Object();

        /// <summary>
        /// SuccessMsg: RoomCode, IntMsg:0代表新房间
        /// </summary>
        /// <param name="userOpenId"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public OutAPIResult FindOrCreateRoom(string userOpenId, int weight)
        {

            OutAPIResult result = new OutAPIResult();
            try
            {
                lock(_LockRoom)
                {
                    var roomKey = GK.AllRoomScore(weight);
                    string roomCode = "";

                    //获取可用的房间
                    var list = _redis.FindSortedSet<string>(roomKey, 0, GameConfig.Room_Max_PlayerCount);
                    if (list.Count > 0)
                    {
                        roomCode = list[0];
                        result.IntMsg = 1;
                    }
                    else
                    {
                        string createResult = CreateRoom(weight, out roomCode);
                        if (createResult != null)
                        {
                            result.ErrorMsg = createResult;
                            return result;
                        }
                        if (string.IsNullOrWhiteSpace(roomCode))
                        {
                            result.ErrorMsg = "Room 没有创建成功，Code 空";
                            return result;
                        }
                    }
                    result.SuccessMsg = roomCode;
                }
            }
            catch(Exception ex)
            {
                result.ErrorMsg = ex.Message;
            }

            return result;
        }

      

        private string CreateRoom(int weight,out string roomCode)
        {
            string msg = null;
            roomCode = StringHelper.GenerateRoomCode();
            try
            { 
                var AllRoomScoreKey = GK.AllRoomScore(weight);
                
                ERoom room = new ERoom
                {
                    Code = roomCode,
                    PlayerCount = 0,
                    Weight = weight,
                    CreateTime = DateTime.Now,
                };
                //房间Code加入应用全局
                _redis.SetAdd(GK.ALLRoomCode, roomCode);
                _redis.HashAddT<ERoom>(GK.ALLRoomEntity,room.Code, room);
                //创建座位
                var roomSeatKey = GK.Room_Seat(roomCode);
                _redis.KeyDelete(roomSeatKey);
                for (int i=1;i<= GameConfig.Room_Max_PlayerCount;i++)
                {
                    _redis.HashAdd(roomSeatKey, GK.SeatNo(i), "");
                }
                
                //利用score 记录房间玩家数量
                _redis.WriteSortedSet<string>(AllRoomScoreKey, roomCode, 0);

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }  
    }
}
