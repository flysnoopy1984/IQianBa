using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCommon.Config
{
    public class GameConfig
    {
        public const int Room_Max_Count = 20;

        public const int Room_Max_PlayerCount = 8;

        /// <summary>
        /// 洗牌时间
        /// </summary>
        public const int Game_Shuffle_Sec = 2;

        public const int Table_Cards_Count = 5;

        /// <summary>
        /// 发牌时间
        /// </summary>
        public const int Send_OneCard_Sec = 3;

        /// <summary>
        /// 每次押注服务端等待时间
        /// </summary>
        public const int Turn_Wait_Server = 20;

        public const int Turn_Wait_Client = 20;

        public static decimal GetSmallBet(int weight)
        {
            switch(weight)
            {
                case 0:
                    return 10;
                case 1:
                    return 50;
                case 2:
                    return 100;
                case 3:
                    return 500;         
                
            }
            return 10;
        }
    }
}
