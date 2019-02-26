using GameModel.WSJsonData;
using GameServer.Command;
using Newtonsoft.Json;

using SuperSocket.WebSocket.SubProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Command
{
    public class NEWC : SubCommandBase<GameUserSession>
    {
      
        public override void ExecuteCommand(GameUserSession session, SubRequestInfo requestInfo)
        {
            var jsonStr = requestInfo.Body;
            if (!string.IsNullOrEmpty(jsonStr))
            {
                jdNewConnect data = JsonConvert.DeserializeObject<jdNewConnect>(requestInfo.Body);
                session.RoomCode = data.RoomCode;
                session.OpenId = data.OpenId;
                session.Send($"Hello！你好{session.OpenId},欢迎您来到房间{session.RoomCode}");
            }
        }
    }
}
