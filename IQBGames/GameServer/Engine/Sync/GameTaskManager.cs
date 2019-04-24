using GameCommon.Config;
using GameModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Engine.Sync
{
    public class GameTaskManager
    {
        public static void AfterShuffleEnd(GameServer gameServer, EGameInfo gi)
        {

            UserWaitBetTask waitTask = UserWaitBetTask.CreateNewInstance(gameServer, gi.CurBetUserOpenId);
            waitTask.Run(GameConfig.Turn_Wait_Server);

        }

        public static void SyncTask_ShuffleEnd(GameUserSession session, EGameInfo gi)
        {

            ShuffleEndTask syncTask = new ShuffleEndTask(session.GameManager);
            syncTask.Run(GameConfig.Game_Shuffle_Sec, session.GameServer, gi,session.GameAttr.Weight);

        }
    }
}
