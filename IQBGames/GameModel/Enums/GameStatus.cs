using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel.Enums
{
    public enum GameStatus
    {
        NoGame = 0,
        /// <summary>
        /// 等待用户
        /// </summary>
        WaitPlayer = 1,
        /// <summary>
        /// 洗牌
        /// </summary>
        Shuffle = 2,

        /// <summary>
        /// 洗好牌发牌中,不能有人进入
        /// </summary>
        ShuffleEnd = 3,

        /// <summary>
        /// 游戏中
        /// </summary>
        Playing = 10,

        /// <summary>
        /// 一局将结束，在算账
        /// </summary>
        Settlement = 4,
    }
}
