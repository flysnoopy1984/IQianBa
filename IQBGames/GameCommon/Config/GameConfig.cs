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
        public const int Game_Shuffle_Sec = 10;

        public const int Table_Cards_Count = 5;

        /// <summary>
        /// 每次押注服务端等待时间
        /// </summary>
        public const int Turn_Wait_Server = 18;

        public const int Turn_Wait_Client = 15;
    }
}
