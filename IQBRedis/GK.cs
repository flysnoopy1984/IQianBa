﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameRedis
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

        public const string GameBasic = "GameBasicInfo";

        public const string GameStatus = "GameStatus";

   

        public const string GameDotPosition = "Game_Dot_Pos";
     
        

        public const string ALLRoomCode = "ALLRoomCode";

        public const string ALLRoomEntity = "ALLRoomEntity";

        public static int SeatNo(int no)
        {
            return no;
           // return "SN" + no;
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
            return roomCode + "_RoomUser";
        }

        public static string RoomPlayer(string roomCode)
        {
            return  roomCode+"_Player";
        }

        //public static string RoomTable(string roomCode)
        //{
        //    return roomCode + "_Table";
        //}

        public static string UserInfo(string openId)
        {
            return "U_" + openId;
        }

        public static string GameBasicInfo(string roomCode)
        {
            return roomCode + "_GameInfo";
        }

        public static string TableCard(string roomCode)
        {
            return roomCode + "_TableCards";
        }

        public static string CoinDetail(string roomCode)
        {
            return roomCode + "_CoinDetail";
        }

        //public static string AllRoom()
        //{
        //    return "SN" + no;
        //}

        //public static string GameStatus(string roomCode)
        //{
        //    return roomCode + "_GameStatus";
        //}

    }
}
