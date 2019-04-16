using GameModel.Enums;

using GameModel.Message.SendData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel
{
    public class EOneGame:BaseSendMsg
    {
        public EOneGame() { }
        public EOneGame(string rc) {
            // RoomCode = rc;
            BasicInfo = new EGameInfo(rc);
            GameCoins = new EGameCoins(rc);
        }

        /// <summary>
        /// 当前大小盲注，当前谁出牌，游戏状态
        /// </summary>
        public EGameInfo BasicInfo { get; set; }

        /// <summary>
        /// 押注币
        /// </summary>
        public EGameCoins GameCoins { get; set; }

        /// <summary>
        /// 当前游戏桌上的牌
        /// </summary>
        public Dictionary<int, ECard> TableCardList { get; set; }

        /// <summary>
        /// 当前游戏玩家信息
        /// </summary>
        public List<ERoomUser> PlayerList { get; set; }

        /// <summary>
        /// 游戏桌面
        /// </summary>
        public override GameActionCode Action
        {
            get
            {
                return GameActionCode.ShowOneGame;
            }          
        }

   
    }
}
