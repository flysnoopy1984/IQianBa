using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel.Enums
{
    /// <summary>
    /// 服务端给客户端发送的指令
    /// </summary>
    public enum GameActionCode
    {
        ShowOneGame = 0,
        PlayerSitDown = 101,
        PlayerSitUp = 102,
        Wait =1,
        ShuffleStart = 2,
        Shuffling =3,
        ShuffleEnd =4,
        BackHall =100,

    }
}
