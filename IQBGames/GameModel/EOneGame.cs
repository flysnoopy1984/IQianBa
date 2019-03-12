using GameModel.Enums;
using GameModel.Message;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel
{
    public class EOneGame:BaseNormalMsg
    {
        public EOneGame() { }
        public EOneGame(string rc) {
            RoomCode = rc;
        }
        public GameStatus GameStatus { get; set; }
        /// <summary>
        /// 当前游戏所在房间Code
        /// </summary>
        public string RoomCode { get; set; }

        /// <summary>
        /// 当前游戏桌上的牌
        /// </summary>
        public Dictionary<int, ECard> TableCardList { get; set; }

        /// <summary>
        /// 当前游戏玩家信息
        /// </summary>
        public List<ERoomUser> PlayerList { get; set; }

        public Dictionary<int,ECard> RemainCardList { get; set; }

        /// <summary>
        /// 桌上指针(用户大小盲注)
        /// </summary>
        //当前按钮位置
        public int CurD { get; set; }

        public override GameActionCode Action
        {
            get
            {
                return GameActionCode.ShowOneGame;
            }          
        }
    }
}
