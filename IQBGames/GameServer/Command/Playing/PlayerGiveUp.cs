﻿using GameModel.WebSocketData.ReceiveData.Playing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameModel.Message;
using GameModel.WebSocketData.SendData.Playing;
using GameServer.Engine;
using GameServer.Engine.Sync;

namespace GameServer.Command.Playing
{
    public class PlayerGiveUp : BaseGameCommand<dataPlayerGiveUp>
    {
        public override string Name
        {
            get
            {
                return "GiveUp";
            }
        }

        public override List<IGameMessage> HandleData(GameUserSession session, dataPlayerGiveUp Data)
        {
            List<IGameMessage> msgList = new List<IGameMessage>();
            GameManager gm = session.GameManager;

            gm.PlayerGiveUp();

            var gi = gm.PreNextStep(true);
            var dealCards = gm.DealCard(gi);
            if (dealCards != null)
            {
                ResultPlayerGiveUp giveUpMsg = GameMessageHandle.CreateResultPlayerGiveUpMsg(gm.RoomCode, Data.OpenId, "");
                msgList.Add(giveUpMsg);

                gi = gm.PreNextStep(true);
                var cardsMsg = GameMessageHandle.CreateDealCardMsg(gm.RoomCode, dealCards,gi);
                msgList.Add(cardsMsg);

                GameTaskManager.SyncTask_DealCardDone(session, gi);
            }
            else
            {
                var msg = gm.WaitNextPlayer(gi);
                if (msg != null)
                    msgList.Add(msg);
                else
                {
                    ResultPlayerGiveUp giveUpMsg = GameMessageHandle.CreateResultPlayerGiveUpMsg(gm.RoomCode, Data.OpenId, gi.CurBetUserOpenId);
                    msgList.Add(giveUpMsg);
                }
            }
          
            return msgList;
        }

        public override bool VerifyCommandData(dataPlayerGiveUp InData, GameUserSession session)
        {
            return true;
        }
    }
}
