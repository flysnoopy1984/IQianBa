using GameModel.Message;
using GameModel.WebSocketData.ReceiveData;
using GameServer.Engine;
using SuperSocket.WebSocket.SubProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Command
{
    public class UserSitUp : BaseGameCommand<dataUserSitUp>
    {
        public override string Name
        {
            get
            {
                return "UserSitUp";
            }
        }

        public override List<BaseNormalMsg> HandleData(GameUserSession session, dataUserSitUp Data)
        {
            List<BaseNormalMsg> result = new List<BaseNormalMsg>();
            GameManager gameManager = new GameManager(Data.OpenId);
            var r =  gameManager.UserSitUp();
          
            result.Add(r);
            return result;
        }

        public override bool VerifyCommandData(dataUserSitUp InData)
        {
            if (string.IsNullOrEmpty(InData.OpenId))
            {
                base.GameMessageHandle.PushErrorMsg("错误，没有获取您的身份，请重新登陆");
                return false;
            }
            return true;
        }
    }
}
