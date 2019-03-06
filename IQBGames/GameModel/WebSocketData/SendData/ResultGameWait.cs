using GameModel.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameModel.Enums;

namespace GameModel.WebSocketData.SendData
{
    public class ResultGameWait : BaseNormalMsg
    {
        public override GameActionCode Action
        {
            get
            {
                return GameActionCode.Wait;
            }
        }
    }
}
