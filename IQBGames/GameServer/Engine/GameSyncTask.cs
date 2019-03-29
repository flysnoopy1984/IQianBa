using GameModel.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameServer.Engine
{
    public class GameSyncTask
    {
        private List<IGameMessage> _taskMsgList = null;
        private GameMessageHandle _GameMessageHandle = null;

        public delegate void DoAfterTask(string RoomCode);
        public event DoAfterTask afterTaskEvent;
        

        public GameSyncTask(List<IGameMessage> msgList)
        {
            _taskMsgList = msgList;
            
            _GameMessageHandle = new GameMessageHandle();
        }

        public void RunSyncTask(int AfterSec,GameServer GameServer,string RoomCode)
        {
            _GameMessageHandle.Push(_taskMsgList);

            Task SubTask = new Task(() =>
            {
                SpinWait.SpinUntil(() =>
                {
                    return false;
                }, AfterSec * 1000);
                afterTaskEvent?.Invoke(RoomCode);
                _GameMessageHandle.Run(GameServer);

            });
            SubTask.Start();
        }
    }
}
