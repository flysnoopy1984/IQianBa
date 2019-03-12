﻿
using Newtonsoft.Json;
using SuperSocket.SocketBase.Command;
using SuperSocket.WebSocket.SubProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.WebSocket;
using GameServer.Engine;
using GameModel.WebSocketData.ReceiveData;
using IQBCore.Common.Helper;
using GameServer.Command;
using GameModel;
using GameModel.Message;

namespace GameServer.Commond
{
    public class EntryGame : BaseGameCommand<dataNewConnect>
    {
        public override bool VerifyCommandData(dataNewConnect InData)
        {
            
            return true;
        }

        public override List<IGameMessage> HandleData(GameUserSession session, dataNewConnect data)
        {

            session.GameAttr.OpenId = data.OpenId;
            session.GameAttr.UserName = data.UserName;
            session.GameAttr.Weight = data.Weight;

            session.KeepOneSession(data.OpenId);

            List<IGameMessage> msgList = new List<IGameMessage>();
            var GameManager = session.GameManager;
            var r = GameManager.FindAvailableRoom(data.Weight);
            if (r.IsSuccess)
                r = GameManager.UserEntryRoom(r.SuccessMsg);

            if (r.IsSuccess)
            {

                var DataHandle = GameManager.GameDataHandle;
                var oneGame = DataHandle.GetGameData();
                msgList.Add(oneGame);
            }
            else
                msgList.Add(new MessageNormalError(r.ErrorMsg));
          
            return msgList;
        
        }

      
        public override string Name
        {
            get
            {
                return "EntryGame";
            }
        }


        

       
    }
}
