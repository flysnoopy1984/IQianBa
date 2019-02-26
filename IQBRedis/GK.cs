using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQBRedis
{
    /// <summary>
    /// Game_Key
    /// </summary>
    public class GK
    {
        
        public const string U_UserOpenId = "U_UserOpenId";
        public const string U_SeatNo = "U_SeatNo";
        public const string U_RoomCode = "U_RoomCode";
        public const string U_RoomWeight = "U_RoomWeight";

       

        public const string ALLRoomCode = "ALLRoomCode";

        public static string SeatNo(int no)
        {
            return "SN" + no;
        }
        public static string Room_Seat(string roomCode)
        {
            return "Seat_" + roomCode;
        }


        public static string AllRoomScore(int weight)
        {
            return "AllRoomScore_" + weight;
        }

        public static string GetOneRoomUser(string roomCode)
        {
            return roomCode + "_One";
        }

        public static string GetRoomUser(string roomCode, string openId)
        {
            return roomCode + "_" + openId;
        }
       
    }
}
