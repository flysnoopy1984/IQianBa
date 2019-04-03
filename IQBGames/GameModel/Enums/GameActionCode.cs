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
       
       
        /* 0- 100 用户进房到出房，玩游戏的所有操作*/
        ShowOneGame = 0,
        Wait =1,
        ShuffleStart = 2,
        Shuffling =3,
        ShuffleEnd =4,
        //通知下一个押注者
        NextUserBet = 10,

        BackHall =100,
        PlayerSitDown = 101,
        PlayerSitUp = 102,

        //系统错误
        GameError  = 10000,

    }
}
