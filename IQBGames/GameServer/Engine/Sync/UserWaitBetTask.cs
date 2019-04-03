using GameCommon.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameServer.Engine.Sync
{
    public class UserWaitBetTask
    {
        private int _WaitSeatNo = -1;
        public UserWaitBetTask(int waitSeatNo)
        {
            _WaitSeatNo = waitSeatNo;
        }

        public void Run(int WaitSec = GameConfig.Turn_Wait_Server)
        {
            Task SubTask = new Task(() =>
            {
               // Thread.Sleep(AfterSec * 1000);
                SpinWait.SpinUntil(() =>
                {
                    return false;
                }, WaitSec * 1000);
         

            });
            SubTask.Start();
        }

      
    }
}
