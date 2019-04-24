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
        StartShuffle = 2,

        /// <summary>
        /// 洗牌中，允许玩家入座
        /// </summary>
        Shuffling=3,

        /// <summary>
        /// 洗好牌发牌中,不能有人入座
        /// </summary>
        ShuffleEnd = 4,

        /// <summary>
        /// 游戏中
        /// </summary>
        Playing = 10,

        /// <summary>
        /// 一局将结束，在翻牌，
        /// </summary> 
        GameEndShowingCard = 50,

        /// <summary>
        /// 在算账
        /// </summary>
        GameEndSettlement = 51,
    }

    public enum GameTurn
    {
        NotStart = 0,
        FirstTurn =1,
        SecTurn =2,
        ThirdTurn =3,
        FourthTurn =4,
        End = 5,
    }

}
