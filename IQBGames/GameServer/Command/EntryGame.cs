
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
        public override bool VerifyCommandData(dataNewConnect InData,GameUserSession session)
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
            {
                //表明是新房间，则初始化新游戏的数据
                if (r.IntMsg == 0)
                    GameDataHandle.GenerateEmptyGame(session.GameServer, r.SuccessMsg);
                else
                {
                    GameDataHandle.ReCoverData(session.GameServer, r.SuccessMsg);
                }               
                   
                //用户进入房间
                r = GameManager.UserEntryRoom(r.SuccessMsg);
            }
            if (r.IsSuccess)
            {
                /*【注】如果服务器宕机，GameDataHandle将空*/
                var DataHandle = GameManager.GameDataHandle;
                var oneGame = DataHandle.GetGameData(true);
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
