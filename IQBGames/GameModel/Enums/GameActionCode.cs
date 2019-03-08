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
        PlayerSitDown = 1,
        PlayerSitUp = 2,
        Wait =3,
        Shuffle = 4,
        BackHall =100,

    }
}
