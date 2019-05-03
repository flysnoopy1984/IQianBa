using GameModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameServer.Engine.Sync
{
    public class DealCardDoneTask
    {
      

        public DealCardDoneTask()
        {
           
        }

        public void Run(int AfterSec, GameServer GameServer,EGameInfo gi)
        {
            if (gi.GameTurn == GameModel.Enums.GameTurn.Send3Card)
                AfterSec =  AfterSec * 3;

            Task Task = new Task(() =>
            {
                // Thread.Sleep(AfterSec * 1000);
                SpinWait.SpinUntil(() =>
                {
                    return false;
                }, AfterSec * 1000);

                GameTaskManager.WaitBetUser(GameServer, gi.CurBetUserOpenId);
            });
            Task.Start();
        }
    }
}
