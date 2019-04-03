using GameModel;
using GameModel.Enums;
using GameModel.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameServer.Engine.Sync
{
    public class ShuffleEndTask
    {
        private List<IGameMessage> _taskMsgList = null;
        private GameMessageHandle _GameMessageHandle = null;

        //public delegate void DoAfterTask(string RoomCode);
        //public event DoAfterTask afterTaskEvent;
        

        public ShuffleEndTask(List<IGameMessage> msgList)
        {
            _taskMsgList = msgList;
            
            _GameMessageHandle = new GameMessageHandle();
        }

        public void RunSyncTask(int AfterSec,GameServer GameServer,GameManager gm, EGameInfo gi)
        {
            _GameMessageHandle.Push(_taskMsgList);

            Task SubTask = new Task(() =>
            {
               // Thread.Sleep(AfterSec * 1000);
                SpinWait.SpinUntil(() =>
                {
                    return false;
                }, AfterSec * 1000);

                //afterTaskEvent?.Invoke(RoomCode);
                _GameMessageHandle.Run(GameServer);

                AfterShuffleEnd(gm,gi);

            });
            SubTask.Start();
        }

        private void AfterShuffleEnd(GameManager gm, EGameInfo gi)
        {
            gi.GameStatus = GameStatus.ShuffleEnd;
            gi.GameTurn = GameTurn.FirstTurn;
            gm.SetGameInfo(gi);

            UserWaitBetTask waitTask = new UserWaitBetTask(gi.BetSeat);
            waitTask.Run(20);
        }
    }
}
