using GameCommon.Config;
using GameModel.Message;
using GameModel.WebSocketData.SendData.Playing;
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
        //  private int _WaitSeatNo = -1;

        private GameServer _GameServer = null;
        private GameMessageHandle _GameMessageHandle = null;
        private string _waitUserOpenId;
        public UserWaitBetTask(GameServer gameServer,string waitUserOpenId)
        {
            _GameServer = gameServer;
            _waitUserOpenId = waitUserOpenId;

            _GameMessageHandle = new GameMessageHandle();

        }

        public void Run(int WaitSec = GameConfig.Turn_Wait_Server)
        {

            Task SubTask = new Task(() =>
            {
             
                SpinWait.SpinUntil(() =>
                {
                    return false;
                }, WaitSec * 1000);

                //【有Bug 风险，用户突然离线】
                var betSession = _GameServer.GetSessions(s => s.GameAttr.UserOpenId == _waitUserOpenId).FirstOrDefault();
                GameManager gm = betSession.GameManager;

                var gi = gm.GetGameBasic();
                //时间到了 还是此用户下注，说明超时，则弃牌
                if (gi.CurBetUserOpenId == gm.UserOpenId)
                {
                    var list = new List<IGameMessage>();

                    //系统主动弃牌
                    gm.PlayerGiveUp();
                    var giveUpMsg = GameMessageHandle.CreateResultPlayerGiveUpMsg(gm.RoomCode,
                                                                                  gm.UserOpenId,
                                                                                  gi.CurBetUserOpenId);
                    list.Add(giveUpMsg);

                    gi = gm.PreNextPlayer(true);

                    var msg = gm.WaitNextPlayer(gi);
                    if(msg !=null)
                    {
                        list.Add(msg);    
                    }
                    _GameMessageHandle.Push(list);
                    _GameMessageHandle.Run(_GameServer);


                }
            });
            SubTask.Start();
        }

        public static UserWaitBetTask CreateNewInstance(GameServer gameServer,string waitUserOpenId)
        {
            UserWaitBetTask waitTask = new UserWaitBetTask(gameServer, waitUserOpenId);
           
            return waitTask;
        }

      
    }
}
