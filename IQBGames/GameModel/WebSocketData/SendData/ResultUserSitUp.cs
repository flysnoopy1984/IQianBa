using GameModel.Message.SendData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameModel.Enums;

namespace GameModel.WebSocketData.SendData
{
    public class ResultUserSitUp : BaseSendMsg
    {
        public override GameActionCode Action
        {
            get
            {
                return GameActionCode.PlayerSitUp;
            }
        }
    }
}
