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
        public static void WaitBetUser(GameServer gameServer, string waitUserId)
        {

            //UserWaitBetTask waitTask = UserWaitBetTask.CreateNewInstance(gameServer, waitUserId);
            //waitTask.Run(GameConfig.Turn_Wait_Server);

        }

        public static void SyncTask_ShuffleEnd(GameUserSession session)
        {

            ShuffleEndTask syncTask = new ShuffleEndTask(session.GameManager);
            syncTask.Run(GameConfig.Game_Shuffle_Sec, session.GameServer,session.GameAttr.Weight);

        }

        public static void SyncTask_DealCardDone(GameUserSession session, EGameInfo gi)
        {
            DealCardDoneTask syncTask = new DealCardDoneTask();
            syncTask.Run(GameConfig.Send_OneCard_Sec, session.GameServer, gi);
        }
    }
}
